namespace Boilerplate.Contracts;

public class DataTableParams
{
    public string SearchText { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string voucherType { get; set; }
    public string roleId { get; set; }
    public string fromDate { get; set; }
    public string toDate { get; set; }
    public string DuePaymentType { get; set; }
    public string MemberTypeId { get; set; }
    public string MemberId { get; set; }
    public string V_Type_Id { get; set; }
    public string smsStatus { get; set; }
    public string Status { get; set; }
    public bool IsPaymentSMSData { get; set; }
}
