﻿using Entities.Models;
using Shared.RequestFeatuers;

namespace Contracts;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetAllEmployeesAsync(Guid companyId,
        EmployeeParameters employeeParameters, bool trackChanges);
    Task<Employee?> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployee(Employee employee);
}