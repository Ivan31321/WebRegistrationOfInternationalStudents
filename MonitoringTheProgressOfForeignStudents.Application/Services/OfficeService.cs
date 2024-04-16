using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MonitoringTheProgressOfForeignStudents.Application.Interfaces.Services;
using System.Data;
using System.Text.RegularExpressions;

namespace MonitoringTheProgressOfForeignStudents.Application.Services
{
    public class OfficeService : IOfficeService
    {
        public byte[] ExportExcel<T>(IEnumerable<T> lst, IEnumerable<string> names)
        {
            using (var mem = new MemoryStream())
            {
                using (var spreadsheet = SpreadsheetDocument.Create(mem, SpreadsheetDocumentType.Workbook))
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
        public byte[] ReadDataFromExcel(string sourceFilePath, string sheetName, Dictionary<string, string> parameters)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Создание нового Excel файла в памяти
                using (SpreadsheetDocument destinationDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart destinationWorkbookPart = destinationDocument.AddWorkbookPart();
                    WorksheetPart destinationWorksheetPart = destinationWorkbookPart.AddNewPart<WorksheetPart>();

                    destinationWorkbookPart.Workbook = new Workbook();
                    Sheet destinationSheet = new Sheet { Id = destinationDocument.WorkbookPart.GetIdOfPart(destinationWorksheetPart), SheetId = 1, Name = sheetName };
                    destinationWorkbookPart.Workbook.Append(new Sheets());
                    destinationWorkbookPart.Workbook.GetFirstChild<Sheets>().Append(destinationSheet);

                    // Копирование данных из исходного листа в новый лист в памяти
                    using (SpreadsheetDocument sourceDocument = SpreadsheetDocument.Open(sourceFilePath, false))
                    {
                        WorkbookPart sourceWorkbookPart = sourceDocument.WorkbookPart;
                        Sheet sheet = sourceWorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);

                        if (sheet == null)
                        {
                            Console.WriteLine($"Sheet '{sheetName}' not found.");
                            return null;
                        }

                        WorksheetPart sourceWorksheetPart = (WorksheetPart)sourceWorkbookPart.GetPartById(sheet.Id);
                        destinationWorksheetPart.Worksheet = new Worksheet(sourceWorksheetPart.Worksheet.OuterXml);
                    }

                    destinationWorkbookPart.Workbook.Save();
                }

                memoryStream.Position = 0; // Сброс позиции памяти

                // Модификация данных в Excel в памяти 
                byte[] modifiedData = ModifyCellsInExcel(memoryStream, sheetName, parameters);

                return modifiedData;
            }
        }

        public byte[] ModifyCellsInExcel(MemoryStream memoryStream, string sheetName, Dictionary<string, string> cellChanges)
        {
            byte[] resultData;

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(memoryStream, true))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);

                if (sheet == null)
                {
                    Console.WriteLine($"Sheet '{sheetName}' not found.");
                    return null;
                }

                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault();

                foreach (Row row in sheetData.Elements<Row>())
                {
                    foreach (Cell cell in row.Elements<Cell>())
                    {
                        string cellReference = cell.CellReference.InnerText;
                        if (cellChanges.ContainsKey(cellReference))
                        {
                            cell.CellValue = new CellValue(cellChanges[cellReference]);
                            cell.DataType = new EnumValue<CellValues>(CellValues.String);
                        }
                    }
                }

                worksheetPart.Worksheet.Save();
                workbookPart.Workbook.Save();

                resultData = memoryStream.ToArray(); // Получение обновленных данных из MemoryStream
            }

            return resultData;
        }
    }
}
