using Microsoft.AspNetCore.Identity;

namespace TrainingPlan.API.Application.Common.Services
{
    public class InitialDataService(IServiceProvider serviceProvider) : IHostedService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "ADMIN", "INSTRUCTOR", "ATHLETE" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var userExists = await userService.UserExistsAsync("anderson@andersonteles.com");

            if (!userExists)
            {
                await userService.CreateAdminAsync("Anderson Teles", "anderson@andersonteles.com", "@Anderson2024", new DateTime(1991, 01, 01), "11943839026", cancellationToken);
            }

        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}
