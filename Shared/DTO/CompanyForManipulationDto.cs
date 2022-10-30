using System.ComponentModel.DataAnnotations;

namespace Shared.DTO;

public abstract record CompanyForManipulationDto
{
    [Required(ErrorMessage = "Employee name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Employee name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string? Address { get; init; }

    [Required(ErrorMessage = "Employee name is a required field.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters.")]
    public string? Country { get; init; }

    public IEnumerable<EmployeeForCreationDto>? Employees { get; }
}