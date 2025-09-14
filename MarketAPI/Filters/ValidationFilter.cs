namespace MarketAPI.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    private readonly IValidator<T> _validator = validator;

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<T>().First();

        var result = await _validator.ValidateAsync(request, context.HttpContext.RequestAborted);

        if (!result.IsValid)
        {
            return TypedResults.ValidationProblem(result.ToDictionary());
        }

        return await next(context);
    }
}
