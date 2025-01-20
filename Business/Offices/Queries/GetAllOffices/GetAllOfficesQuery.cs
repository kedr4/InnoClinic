using Business.DTOs;
using MediatR;

namespace Business.Offices.Queries.GetAllOffices;

public record GetAllOfficesQuery(int Page, int PageSize, bool? IsActive) : IRequest<List<OfficeDTO>>;