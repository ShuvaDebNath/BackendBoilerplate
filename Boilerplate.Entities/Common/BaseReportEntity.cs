namespace Boilerplate.Entities.Common
{
    public abstract class BaseReportEntity
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? RerportType { get; set; }
    }
}
