﻿using Entities.Models;
using Shared.DTO;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllEmployees(Guid companyId, bool trackChanges);
    Task<EmployeeDto> GetEmployee(Guid companyId, Guid id, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
    Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);
    Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto,
        bool compTrackChanges, bool empTrackChanges);
    Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatch
        (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);
    Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
}