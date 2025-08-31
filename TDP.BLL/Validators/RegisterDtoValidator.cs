using FluentValidation;
using TDP.BLL.DTOs;

namespace TDP.BLL.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters and spaces");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required")
                .LessThan(DateTime.Today).WithMessage("Birth date must be in the past")
                .GreaterThan(DateTime.Today.AddYears(-120)).WithMessage("Birth date seems invalid");

            RuleFor(x => x.DepartmentID)
                .GreaterThan(0).WithMessage("Department is required");
        }
    }
} 