namespace MonitoringTheProgressOfForeignStudents.Application.Interfaces.Services
{
    public interface IOfficeService
    {
        public byte[] ReplaceTemplate(string templateDist, Dictionary<string, string> parameters);
        public byte[] ExportExcel<T>(IEnumerable<T> lst, IEnumerable<string> names);
    }
}
