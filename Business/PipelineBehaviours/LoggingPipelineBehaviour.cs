using MediatR;
using Serilog;
using System.Diagnostics;

namespace Business.PipelineBehaviours;

public class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger _logger;

    public LoggingPipelineBehaviour(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.Information($"Handling {typeof(TRequest).Name}");

        var response = await next();

        stopwatch.Stop();
        _logger.Information($"Handled {typeof(TRequest).Name}, execution time: {stopwatch.ElapsedMilliseconds}ms");

        return response;
    }
}

