using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading;
using TrainingPlan.API.Application.Features.AthleteFeatures.CreateAthlete;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Infrastructure.Repositories;
using TrainingPlan.Shared.User;

namespace TrainingPlan.API.Application.Common.Services
{
    public interface IUserService
    {
        Task CreateAthleteAsync(string name, string email, string password, DateTime birth, string phone, CancellationToken cancellationToken);

        Task CreateInstructorAsync(string name, string email, string password, DateTime birth, string phone, CancellationToken cancellationToken);

        Task CreateAdminAsync(string name, string email, string password, DateTime birth, string phone, CancellationToken cancellationToken);

        Task<bool> UserExistsAsync(string email);
    }

    public class UserService(IUnitOfWork unitOfWork, IPersonRepository personRepository, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, IEmailSender<ApplicationUser> emailSender) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPersonRepository _personRepository = personRepository;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserStore<ApplicationUser> _userStore = userStore;
        private readonly IEmailSender<ApplicationUser> _emailSender = emailSender;

        private async Task<string> CreateUser(string email, string password, string[] roles, CancellationToken cancellationToken)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            var emailStore = GetEmailStore();
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}");
            }

            foreach (var role in roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //var callbackUrl = _navigationManager.GetUriWithQueryParameters(
            //    _navigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
            //    new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = "/Athlete/Home" });

            //await _emailSender.SendConfirmationLinkAsync(user, request.Email, HtmlEncoder.Default.Encode(callbackUrl));

            return userId;
        }

        public async Task CreateAthleteAsync(string name, string email, string password, DateTime birth, string phone, CancellationToken cancellationToken)
        {
            var userId = await CreateUser(email, password, [PersonType.ATHLETE], cancellationToken);

            var athlete = new Athlete(Guid.Parse(userId), name, birth, email, phone);

            _personRepository.Create(athlete);
            await _unitOfWork.Save(cancellationToken);
        }

        public async Task CreateInstructorAsync(string name, string email, string password, DateTime birth, string phone, CancellationToken cancellationToken)
        {
            var userId = await CreateUser(email, password, [PersonType.INSTRUCTOR], cancellationToken);

            var instructor = new Instructor(Guid.Parse(userId), name, birth, email, phone);

            _personRepository.Create(instructor);
            await _unitOfWork.Save(cancellationToken);
        }

        public async Task CreateAdminAsync(string name, string email, string password, DateTime birth, string phone, CancellationToken cancellationToken)
        {
            var userId = await CreateUser(email, password, ["ADMIN", PersonType.INSTRUCTOR], cancellationToken);

            var admin = new Instructor(Guid.Parse(userId), name, birth, email, phone);

            _personRepository.Create(admin);
            await _unitOfWork.Save(cancellationToken);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user == null)
            {
                return false;
            }   

            return true;    
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
}
