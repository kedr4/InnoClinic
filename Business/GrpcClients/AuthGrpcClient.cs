using ContractsLib;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Business.GrpcClients;

public class AuthGrpcClient
{
    private readonly AuthService.AuthServiceClient _authServiceClient;
    public AuthGrpcClient(IConfiguration configuration)
    {
        var authServiceUrl = configuration["GrpcServices:AuthServiceUrl"];
        var channel = GrpcChannel.ForAddress(authServiceUrl);
        _authServiceClient = new AuthService.AuthServiceClient(channel);
    }

    public async Task<bool> IsUserAReceptionist(Guid userId)
    {
        var request = new IsUserAReceptionistRequest { UserId = userId.ToString() };
        var response = await _authServiceClient.IsUserAReceptionistAsync(request);

        return response.IsReceptionist;
    }
}
