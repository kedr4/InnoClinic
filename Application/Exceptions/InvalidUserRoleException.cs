namespace Application.Exceptions;

public class InvalidUserRoleException : AppException
{
    public InvalidUserRoleException()
       : base($"UserRole is not suitable", 401)
    { }

}
