using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Services;
using System.Text.RegularExpressions;

namespace MonitoringTheProgressOfForeignStudents.Application.Services
{
    public class OfficeService : IOfficeService
    {
        public byte[] ExportExcel<T>(IEnumerable<T> lst, IEnumerable<string> names)
        {
            using (var mem = new MemoryStream())
            {
                using (var spreadsheet = SpreadsheetDocument.Create(mem, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var wbPart = spreadsheet.AddWorkbookPart();
                    wbPart.Workbook = new Workbook();

                    var wsPart = wbPart.AddNewPart<WorksheetPart>();
                    wsPart.Worksheet = new Worksheet(new SheetData());

                    var sheets = spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());
                    var sheet = new Sheet
                    {
                        Id = spreadsheet.WorkbookPart.GetIdOfPart(wsPart),
                        SheetId = 1,
                        Name = "Result"
                    };

                    sheets.Append(sheet);
                    var worksheet = wsPart.Worksheet;
                    var sheetData = worksheet.GetFirstChild<SheetData>();

                    AddNames(sheetData, names);

                    foreach (var item in lst)
                    {
                        var row = new Row();
                        foreach (var prop in item.GetType().GetProperties())
                        {
                            var cell = new Cell()
                            {
                                CellValue = new CellValue(prop.GetValue(item, null)?.ToString() ?? string.Empty),
                                DataType = CellValues.String
                            };
                            row.Append(cell);
                        }

                        sheetData.Append(row);
                    }

                    wsPart.Worksheet.Save();
                }
                mem.Position = 0;
                return mem.ToArray();
            }
        }

        private void AddNames(SheetData sd, IEnumerable<string> names)
        {
            var row = new Row();

            foreach (var item in names)
            {
                var cell = new Cell()
                {
                    CellValue = new CellValue(item),
                    DataType = CellValues.String
                };
                row.Append(cell);
            }

            sd.Append(row);
        }

        public byte[] ReplaceTemplate(string templateDist, Dictionary<string, string> parameters)
        {
            string docText = null;

            byte[] byteArray = File.ReadAllBytes(templateDist);
            using (var stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
                using (var wordDoc = WordprocessingDocument.Open(stream, true))
                {
                    using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }

                    foreach (var item in parameters)
                    {
                        Regex regexText = new Regex(item.Key);
                        docText = regexText.Replace(docText, item.Value);
                    }

                    using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }

                    wordDoc.MainDocumentPart.Document.Save();
                }

                stream.Position = 0;
                return stream.ToArray();
            }
        }
    }
}
