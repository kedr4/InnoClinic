using Business.DTOs;
using MediatR;

namespace Business.Offices.Queries.GetOfficeById;

public record GetOfficeByIdQuery(Guid Id) : IRequest<OfficeDTO>;