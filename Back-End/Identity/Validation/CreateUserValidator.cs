using FluentValidation;
using library.DTOs;

namespace Identity.Validation
{
    public class CreateUserValidator : AbstractValidator<DtoCreateUser>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name Is Required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name Is Required");
           
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Is Required")
                .EmailAddress().WithMessage("Invalid Email Format");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password Is Required")
                .MinimumLength(8).WithMessage("Password Length must be not less than 8 characters")
                .Matches(@"[A-Z]").WithMessage("Must Contain at least one upper case character")
                .Matches(@"[a-z]").WithMessage("Must Contain at least one lower case character")
                .Matches(@"[\d]").WithMessage("Must Contain at least one number");

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm Password Is Required")
                .Equal(x => x.Password).WithMessage("Passwords are not equal");


        }
    }
}
