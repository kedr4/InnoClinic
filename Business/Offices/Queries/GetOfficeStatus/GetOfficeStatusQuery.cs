using Business.DTOs;
using MediatR;

namespace Business.Offices.Queries.GetOfficeStatus;

public record GetOfficeStatusQuery(Guid Id) : IRequest<StatusDTO>;