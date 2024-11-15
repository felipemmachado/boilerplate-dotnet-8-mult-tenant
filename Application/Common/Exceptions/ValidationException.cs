using Application.Common.Constants;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ValidationException() : Exception(ApiResponseMessages.ValidationFails)
    {
        public ValidationException(string message) : this()
        {
            Errors.Add(new ErrorModel(message));
        }

        public ValidationException(IEnumerable<ValidationFailure> notifications)
            : this()
        {
            var errors = notifications.ToList()
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToList())
                .Select(p => new ErrorModel(p.Value)).ToList();

            Errors = errors;
        }

        public List<ErrorModel> Errors { get; } = [];
    }


    [ExcludeFromCodeCoverage]
    public class ErrorModel
    {

        public ErrorModel(string error)
        {

            Errors = [error];
        }

        public ErrorModel(List<string> errors)
        {

            Errors = errors;
        }
        public List<string> Errors { get; set; }
    }
}
