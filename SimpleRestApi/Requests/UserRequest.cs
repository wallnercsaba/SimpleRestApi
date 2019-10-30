using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRestApi.Requests
{
    public class UserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailVisibility { get; set; }
        public bool PhoneNumberVisibility { get; set; }
    }

    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(request => request.FirstName).NotEmpty().WithMessage("FirstName is empty").MaximumLength(200).WithMessage("FirstName is too long (max 200)");
            RuleFor(request => request.LastName).NotEmpty().WithMessage("LastName is empty").MaximumLength(200).WithMessage("LastName is too long (max 200)");
            RuleFor(request => request.Email).NotEmpty().WithMessage("Email is empty").MaximumLength(200).WithMessage("Email is too long (max 200)");
            RuleFor(request => request.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is empty").MaximumLength(200).WithMessage("PhoneNumber is too long (max 200)");
        }
    }
}
