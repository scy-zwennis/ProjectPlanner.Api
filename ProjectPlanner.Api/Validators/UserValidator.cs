using FluentValidation;
using ProjectPlanner.Api.Models;

namespace ProjectPlanner.Api.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly int _minUsernameLength = 4;
        private readonly int _maxUsernameLength = 20;

        private readonly int _minNameLength = 3;
        private readonly int _maxNameLength = 15;

        public UserValidator()
        {
            RuleFor(user => user.Username)
                .NotNull().WithMessage("Username may not be null")
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(_minUsernameLength).WithMessage(string.Format("Username may not be shorter than {0} characters", _minUsernameLength))
                .MaximumLength(_maxUsernameLength).WithMessage(string.Format("Username may not be longer than {0} characters", _maxUsernameLength));

            RuleFor(user => user.EmailAddress)
                .NotNull().WithMessage("Email address may not be null")
                .NotEmpty().WithMessage("Email address is required")
                .EmailAddress().WithMessage("Email address is not valid");

            /*RuleFor(user => user.FirstName)
                .NotNull().WithMessage("First name may not be null")
                .NotEmpty().WithMessage("First name is required")
                .MinimumLength(_minNameLength).WithMessage(string.Format("First name may not be shorter than {0} characters", _minNameLength))
                .MaximumLength(_maxNameLength).WithMessage(string.Format("First name may not be longer than {0} characters", _maxNameLength));

            RuleFor(user => user.LastName)
                .NotNull().WithMessage("Last name may not be null")
                .NotEmpty().WithMessage("Last name is required")
                .MinimumLength(_minNameLength).WithMessage(string.Format("First name may not be shorter than {0} characters", _minNameLength))
                .MaximumLength(_maxNameLength).WithMessage(string.Format("First name may not be longer than {0} characters", _maxNameLength));*/

            RuleFor(user => user.Password)
                .NotNull().NotEmpty().WithMessage("Password may not be null");
        }
    }
}
