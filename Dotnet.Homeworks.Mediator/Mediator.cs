using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator;

public class Mediator : IMediator
{
    private static readonly Type RequestHandlerWithResponseType = typeof(IRequestHandler<,>);
    private static readonly Type RequestHandlerType = typeof(IRequestHandler<,>);
    private static readonly Type PipelineBehaviorType = typeof(IPipelineBehavior<,>);

    private static readonly ConcurrentDictionary<Type, (Type handler, MethodInfo handle)> RequestToHandler = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> PipelineToHandle = new();
    
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        object? requestHandler = null;
        var (genericRequestHandlerType, handle) = RequestToHandler.GetOrAdd(request.GetType(), req =>
        {
            var genericRequestHandlerType =
                RequestHandlerWithResponseType.MakeGenericType(req, typeof(TResponse));
            requestHandler = _serviceProvider.GetService(genericRequestHandlerType)!;
            var handle = requestHandler.GetType()
                .GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle))!;
            return (genericRequestHandlerType, handle);
        });

        requestHandler ??= _serviceProvider.GetService(genericRequestHandlerType)!;
        
        // pipeline behaviors
        var pipelineBehaviorType = PipelineBehaviorType.MakeGenericType(request.GetType(), typeof(TResponse));
        var pipelines = _serviceProvider.GetServices(pipelineBehaviorType).ToArray();

        RequestHandlerDelegate<TResponse> agg = () =>
            (handle.Invoke(requestHandler, new object[] {request, cancellationToken}) as Task<TResponse>)!;

        var result = pipelines
            .Reverse()
            .Aggregate(agg,
                (next, pipeline) =>
                {
                    var pipelineHandle = PipelineToHandle.GetOrAdd(pipeline!.GetType(), p =>
                        p.GetMethod(nameof(IPipelineBehavior<IRequest<TResponse>, TResponse>.Handle))!);
                    return () =>
                        (pipelineHandle.Invoke(pipeline, new object[] {request, next, cancellationToken}) as Task<TResponse>)!;
                });

        // var handlerResultTask = handle.Invoke(requestHandler, new object[] { request, cancellationToken }) as Task<TResponse>;
        return await result();
    }

    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        // pipeline behaviors
        
        object? requestHandler = null;
        var (genericRequestHandlerType, handle) = RequestToHandler.GetOrAdd(request.GetType(), req =>
        {
            var genericRequestHandlerType = RequestHandlerType.MakeGenericType(req);
            requestHandler = _serviceProvider.GetService(genericRequestHandlerType)!;
            var handle = requestHandler.GetType()
                .GetMethod(nameof(IRequestHandler<IRequest>.Handle))!;
            return (genericRequestHandlerType, handle);
        });
        
        requestHandler ??= _serviceProvider.GetService(genericRequestHandlerType)!;
        
        var handleTask = handle.Invoke(requestHandler, new object[] { request, cancellationToken }) as Task;
        await handleTask!;
    }

    public Task<dynamic?> Send(dynamic request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}