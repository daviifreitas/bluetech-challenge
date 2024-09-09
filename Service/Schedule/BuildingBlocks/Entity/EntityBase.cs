namespace BuildingBlocks.Entity;

public interface IEntity
{

    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public StatusDefault StatusDefault { get; set; }

}

public enum StatusDefault
{
    Inactive,
    Active,
    Deleted
}
