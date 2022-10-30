namespace Shared.RequestFeatuers;

public class EmployeeParameters : RequestParameters
{
    public EmployeeParameters() => OrderBy = "name";
    public uint MinAge { get; set; } = 0;

    public uint MaxAge { get; set; } = int.MaxValue;

    public bool ValidAgeRange => MaxAge > MinAge;
    
    public string? SearchTerm { get; set; }
}