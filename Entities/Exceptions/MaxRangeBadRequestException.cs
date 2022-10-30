namespace Entities.Exceptions;

public class MaxRangeBadRequestException : BadRequestException
{
    public MaxRangeBadRequestException() 
        : base("max age can't be less than min age.")
    {
    }
}