namespace api_iso_med_pg.DTOs.Respuesta;

public class GetEvaluationsDto
{
    public string? NoEvaluacion { get; set; }
    public double AvgType1 { get; set; }
    public double AvgType2 { get; set; }
    public double AvgType3 { get; set; }
    public double AvgType4 { get; set; }
    public double AvgType5 { get; set; }
    public double AvgType6 { get; set; }
    public string? Names { get; set; }
}
