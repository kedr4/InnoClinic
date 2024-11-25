using Application.Abstrsctions.Persistance.Repositories;
using Application.Abstrsctions.Services;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class AuthService(IDoctorsRepository doctorsRepository) : IAuthService
{
    public Task DeleteProfileAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task LoginAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RefreshAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RegisterDoctorAsync(CancellationToken cancellationToken = default)
    {
       var o = new Doctor();
    public Task RegisterPatientAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RegisterReceptionstAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
