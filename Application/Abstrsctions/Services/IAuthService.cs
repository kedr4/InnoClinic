using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstrsctions.Services
{
    public interface IAuthService
    {
        public Task RegisterPatientAsync(CancellationToken cancellationToken = default);
        public Task RegisterDoctorAsync(CancellationToken cancellationToken = default);
        public Task RegisterReceptionstAsync(CancellationToken cancellationToken = default);

        public Task LoginAsync(CancellationToken cancellationToken = default);
        public Task LogoutAsync(CancellationToken cancellationToken = default);
        public Task DeleteProfileAsync(CancellationToken cancellationToken = default);
        public Task RefreshAccessTokenAsync(CancellationToken cancellationToken = default);

    }
}
