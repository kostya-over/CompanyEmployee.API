using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTO;

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

    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployee(Guid companyId, Guid id)
    {
        var employee = _services.EmployeeService.GetEmployee(companyId, id, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeForCreationDto object is null.");
        
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        
        var employeeToReturn = _services.EmployeeService
            .CreateEmployeeForCompany(companyId, employee, trackChanges: false);
        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.id },
            employeeToReturn);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        _services.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges:false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] EmployeeForUpdateDto employeeForUpdateDto)
    {
        if (employeeForUpdateDto is null)
            return BadRequest("EmployeeForUpdateDto object is null");

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        
        _services.EmployeeService.UpdateEmployeeForCompany(companyId, id, employeeForUpdateDto,
            compTrackChanges:false, empTrackChanges:true);
        
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto> pathDoc)
    {
        if (pathDoc is null)
            return BadRequest("pathDoc object is null");

        var result = _services.EmployeeService.GetEmployeeForPatch
            (companyId, id, compTrackChanges: false, empTrackChanges: true);
        
        pathDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);
        
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        _services.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);
        
        return NoContent();
    }
}