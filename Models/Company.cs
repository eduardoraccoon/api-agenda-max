namespace api_iso_med_pg.Models;

public class Company : BaseModel
{
    public string NameComertial { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Logo { get; set; } = null!;
    public string Rfc { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
}
