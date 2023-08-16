namespace Boilerplate.Entities.DTOs
{
    public class GetReportParams : GetDataTableParams
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CompanyId { get; set; }
    }
}
