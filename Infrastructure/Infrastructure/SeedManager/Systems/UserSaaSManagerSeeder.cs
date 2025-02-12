using Infrastructure.SecurityManager.AspNetIdentity;
using Infrastructure.SecurityManager.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Infrastructure.SeedManager.Systems;

public class UserSaaSManagerSeeder
{
    private readonly IdentitySettings _identitySettings;
    private readonly UserManager<ApplicationUser> _userManager;
    public UserSaaSManagerSeeder(
        IOptions<IdentitySettings> identitySettings,
        UserManager<ApplicationUser> userManager
        )
    {
        _identitySettings = identitySettings.Value;
        _userManager = userManager;

    }

    public async Task GenerateDataAsync()
    {
        var adminEmail = _identitySettings.DefaultAdmin.Email;
        var adminPassword = _identitySettings.DefaultAdmin.Password;

        if (await _userManager.FindByEmailAsync(adminEmail) == null)
        {
            var applicationUser = new ApplicationUser(
                adminEmail,
                "Root",
                "Admin"
                );

            applicationUser.EmailConfirmed = true;

            await _userManager.CreateAsync(applicationUser, adminPassword);

            var roles = RoleHelper.GetSaaSManagerRoles();
            foreach (var role in roles)
            {
                if (!await _userManager.IsInRoleAsync(applicationUser, role))
                {
                    await _userManager.AddToRoleAsync(applicationUser, role);
                }

            }

        }
    }
}
