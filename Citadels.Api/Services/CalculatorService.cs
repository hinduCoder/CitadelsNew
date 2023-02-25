using Grpc.Core;

namespace Citadels.Api.Services;

public sealed class CalculatorService : Calculator.CalculatorBase
{
    public override Task<AddOneResponse> AddOne(AddOneRequest request, ServerCallContext context)
    {
        return Task.FromResult(new AddOneResponse()
        {
            Number = request.Number + 1
        });
    }
}