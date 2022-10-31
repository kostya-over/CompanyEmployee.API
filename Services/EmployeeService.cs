using System.Dynamic;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DTO;
using Shared.RequestFeatuers;

namespace Services;

public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDataShaper<EmployeeDto> _dataShaper;

    public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper,
        IDataShaper<EmployeeDto> dataShaper)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
        _mapper = mapper;
        _dataShaper = dataShaper;
    }

    public async Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId,
        EmployeeParameters employeeParameters, bool trackChanges)
    {
        if (!employeeParameters.ValidAgeRange)
            throw new MaxRangeBadRequestException();
        
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeesWithMetaData = await _repositoryManager.Employee
            .GetAllEmployeesAsync(companyId, employeeParameters, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

        var shapedData = _dataShaper.ShapedData(employeesDto, employeeParameters.Fields);
        
        return (employees: shapedData, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeFromDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);
        
        var employeeDto = _mapper.Map<EmployeeDto>(employeeFromDb);
        return employeeDto;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId,
        EmployeeForCreationDto employee, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeEntity = _mapper.Map<Employee>(employee);
        
        _repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repositoryManager.SaveAsync();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeToReturn;

    }

    public async Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employee = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);
        
        _repositoryManager.Employee.DeleteEmployee(employee);
        await _repositoryManager.SaveAsync();
    }

    public async Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdateDto,
        bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);

        var employee = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

        _mapper.Map(employeeForUpdateDto, employee);
        await _repositoryManager.SaveAsync();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatch(Guid companyId, Guid id,
        bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeDb);
        return (employeeToPatch, employeeDb);
    }

    public async Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        await _repositoryManager.SaveAsync();
    }

    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
    {
        var employeeEntity = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeEntity is null)
            throw new EmployeeNotFoundException(id);

        return employeeEntity;
    }
}