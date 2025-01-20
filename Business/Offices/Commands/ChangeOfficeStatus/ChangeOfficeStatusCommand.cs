using MediatR;

namespace Business.Offices.Commands.ChangeOfficeStatus;

public record ChangeOfficeStatusCommand(
    Guid Id,
    bool IsActive
) : IRequest<Guid>;
