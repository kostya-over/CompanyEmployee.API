using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Contracts;
using Shared.DTO;
using Shared.RequestFeatuers;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,
        [FromQuery] EmployeeParameters employeeParameters)
    {
        var pagedResult = await _services.EmployeeService.GetEmployeesAsync(companyId,
            employeeParameters, trackChanges: false);
        
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(pagedResult.metaData));
        
        return Ok(pagedResult.Item1);
    }

    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployee(Guid companyId, Guid id)
    {
        var employee = await _services.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        var employeeToReturn = await _services.EmployeeService
            .CreateEmployeeForCompany(companyId, employee, trackChanges: false);
        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, employeeToReturn.id },
            employeeToReturn);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        await _services.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges:false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] EmployeeForUpdateDto employeeForUpdateDto)
    {
        await _services.EmployeeService.UpdateEmployeeForCompany(companyId, id, employeeForUpdateDto,
            compTrackChanges:false, empTrackChanges:true);
        
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto>? pathDoc)
    {
        if (pathDoc is null)
            return BadRequest("pathDoc object is null");

        var result = await _services.EmployeeService.GetEmployeeForPatch
            (companyId, id, compTrackChanges: false, empTrackChanges: true);
        
        pathDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);
        
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _services.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);
        
        return NoContent();
    }
}