using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Employee
{
    [Column("EmployeeId")]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Company name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the field is 60 characters.")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Age is a required field.")]
    public int Age { get; set; }
    
    [Required(ErrorMessage = "Company name is a required field.")]
    [MaxLength(20, ErrorMessage = "Maximum length for the field is 60 characters.")]
    public string? Position { get; set; }
    
    [ForeignKey(nameof(Company))]
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
}