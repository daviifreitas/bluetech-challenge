using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using ValidationException = FluentValidation.ValidationException;

namespace BuildingBlocks.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);

            FluentValidation.Results.ValidationResult[] validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            List<FluentValidation.Results.ValidationFailure> failures = validationResults.Where(v => v.Errors.Any()).SelectMany(err => err.Errors).ToList();

            if(failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
