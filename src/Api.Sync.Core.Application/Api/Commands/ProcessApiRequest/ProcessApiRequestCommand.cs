using Api.Sync.Core.Application.Api.Commands.SendApiResponse;
using ARSoftware.Contpaqi.Api.Common.Domain;
using MediatR;

namespace Api.Sync.Core.Application.Api.Commands.ProcessApiRequest;

public sealed record ProcessApiRequestCommand(ApiRequest ApiRequest) : IRequest;

public sealed class ProcessApiRequestCommandHandler : IRequestHandler<ProcessApiRequestCommand>
{
    private readonly IMediator _mediator;

    public ProcessApiRequestCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(ProcessApiRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _mediator.Send(request.ApiRequest.ContpaqiRequest, cancellationToken) as ContpaqiResponse;

            await _mediator.Send(new SendApiResponseCommand(request.ApiRequest.Id, ApiResponse.CreateSuccessfull(response!)),
                cancellationToken);
        }
        catch (Exception e)
        {
            await _mediator.Send(new SendApiResponseCommand(request.ApiRequest.Id, ApiResponse.CreateFailed(e.Message)), cancellationToken);
        }
    }
}
