
namespace Boilerplate.Contracts;

public class DoubleMasterEntryModel
{
    public string? TableNameMaster { get; set; }
    public string? TableNameChild { get; set; }
    public string? ColumnNamePrimary { get; set; }
    public string? ColumnNameForign { get; set; }
    public string? SerialType { get; set; }
    public string? ColumnNameSerialNo { get; set; }
    public string? IsFlag { get; set; }//Member_Check_for_Sales
    public object? Data { get; set; }
    public object? DetailsData { get; set; }
    public object? WhereParams { get; set; }
}
