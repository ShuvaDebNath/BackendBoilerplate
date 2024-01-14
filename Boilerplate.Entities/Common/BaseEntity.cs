namespace Boilerplate.Entities.Common;

public abstract class BaseEntity
{
    protected string? Id { get; set; }

    public string? MakeBy { get; set; }

    public DateTime? MakeDate { get; set; }

    public DateTime? InsertTime { get; private set; } = DateTime.Now;

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime? UpdateTime { get; private set; } = DateTime.Now;
}
