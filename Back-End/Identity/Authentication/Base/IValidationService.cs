using FluentValidation;
using library.DTOs;

namespace Identity.Authentication.Base
{
    public interface IValidationService
    {
        Task<DtoResponse> ValidationAsync<T>(T model, IValidator<T> validator);
    }
}
