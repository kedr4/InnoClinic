namespace Application.Exceptions;

public class UserNotFoundException : AppException
{
    public UserNotFoundException(Guid userId)
        : base($"User with id {userId} not found.", 404)
    { }

    public UserNotFoundException(string email)
       : base($"User with email {email} not found.", 404)
    { }

}

