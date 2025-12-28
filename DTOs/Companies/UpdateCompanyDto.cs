namespace api_iso_med_pg.DTOs.Companies;

public class UpdateCompanyDto
{
    public int Id { get; set; }
    public string NameComertial { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Logo { get; set; } = null!;
    public string Rfc { get; set; } = null!;
    public string BusinessName { get; set; } = null!;
    public int UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}
