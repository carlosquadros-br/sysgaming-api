using System;
using FluentValidation;

namespace SysgamingApi.Src.Application.Users.Command.RegisterUser;

public class RegisterUserValidator : AbstractValidator<RegisterRequest>
{
    public RegisterUserValidator() : base()

    {
        NameIsNotEmpty();
        EmailIsNotEmpty();
        PasswordIsNotEmpty();
    }

    private void NameIsNotEmpty()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be informed");
    }

    private void EmailIsNotEmpty()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email must be informed");
    }

    private void PasswordIsNotEmpty()
    {
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password must be informed");
    }

}
