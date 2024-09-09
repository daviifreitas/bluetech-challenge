namespace Schedule.Domain.Entities;

public class Schedule : IEntity
{
    #region Base

    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public StatusDefault StatusDefault { get; set; }

    #endregion
    
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    
    //public AppUser Requester { get; set; }
}
