namespace Application.Exceptions;
public class UserNotFoundException : AppException
{
    public UserNotFoundException(Guid userId)
        : base($"User with id {userId} not found.", 404)
    { }

}

