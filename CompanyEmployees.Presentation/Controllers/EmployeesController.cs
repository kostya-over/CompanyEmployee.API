using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[ApiController]
[Route("api/companies/{companyId}/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _services;

    public EmployeesController(IServiceManager services)
    {
        _services = services;
    }

    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        var employees = _services.EmployeeService.GetAllEmployees(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetEmployee(Guid companyId, Guid id)
    {
        var employee = _services.EmployeeService.GetEmployee(companyId, id, trackChanges: false);
        return Ok(employee);
    }
}