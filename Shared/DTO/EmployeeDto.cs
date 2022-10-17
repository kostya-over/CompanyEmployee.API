namespace Shared.DTO;

public record EmployeeDto(
    Guid id,
    string Name,
    int Age,
    string Position
    );