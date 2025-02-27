namespace EcoinverGMAO_api.Utils
{
    public class AlreadyExistsException(string message) : Exception(message)
    {
    }

    public class NotFoundException(string message) : Exception(message)
    {
    }

    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }

    public class ForbiddenException(string message) : Exception(message)
    {
    }
}
