using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Shared.User;
using TrainingPlan.App.Application.Common.Behaviors;

namespace TrainingPlan.App.Application.Features.AthleteFeatures.CreateAthlete
{
    public sealed class CreateAthleteHandler : IRequestHandler<CreateAthleteRequest, CreateAthleteResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _personRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IEmailSender<ApplicationUser> _emailSender;
        private readonly NavigationManager _navigationManager;

        public CreateAthleteHandler(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IEmailSender<ApplicationUser> emailSender,
            NavigationManager navigationManager,
            IUnitOfWork unitOfWork, 
            IPersonRepository personRepository)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailSender = emailSender;
            _navigationManager = navigationManager;
            _unitOfWork = unitOfWork;
            _personRepository = personRepository;
        }

        public async Task<CreateAthleteResponse> Handle(CreateAthleteRequest request, CancellationToken cancellationToken)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, request.Email, CancellationToken.None);
            var emailStore = GetEmailStore();
            await emailStore.SetEmailAsync(user, request.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new BadRequestException($"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}");
            }

            // Logger.LogInformation("User created a new account with password.");

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = _navigationManager.GetUriWithQueryParameters(
                _navigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
                new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = "/Athlete/Home" });

            await _emailSender.SendConfirmationLinkAsync(user, request.Email, HtmlEncoder.Default.Encode(callbackUrl));


            var athlete = new Athlete(Guid.Parse(userId), request.Name, request.Birth, request.Email, request.Phone);
            
            _personRepository.Create(athlete);
            await _unitOfWork.Save(cancellationToken);

            return new CreateAthleteResponse();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }

    public sealed record CreateAthleteRequest(string Email, string Password, string Name, DateTime Birth, string Phone) : IRequest<CreateAthleteResponse>
    {
    }

    public sealed record CreateAthleteResponse
    {

    }

    public class CreateAthleteValidator : AbstractValidator<CreateAthleteRequest>
    {
        public CreateAthleteValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(20);
            RuleFor(x => x.Birth).NotEmpty();
        }
    }

}
