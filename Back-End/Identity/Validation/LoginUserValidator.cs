using FluentValidation;
using library.DTOs;

namespace Identity.Validation
{
    public class LoginUserValidator : AbstractValidator<DtoLoginUser>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid Email format");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");

        }
    }
}
