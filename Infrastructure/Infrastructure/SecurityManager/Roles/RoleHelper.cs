using Infrastructure.SecurityManager.NavigationMenu;

namespace Infrastructure.SecurityManager.Roles;

public class RoleHelper
{
    public static List<string> GetAllRoles()
    {
        var roles = new List<string>();
        roles = NavigationTreeStructure.GetCompleteFirstMenuNavigationSegment();
        return roles;
    }

    //make sure or cross check with NavigationTreeStructure
    public static string GetProfileRole()
    {
        return "Profiles";
    }

    public static List<string> GetTenantRoles()
    {
        var roles = GetAllRoles();
        return roles.Where(role => role != "Tenants" && role != "TenantMembers" && role != "SystemUsers").ToList();
    }

    public static List<string> GetSaaSManagerRoles()
    {
        var roles = new List<string>
        {
            "Tenants",
            "TenantMembers",
            "Profiles",
            "SystemUsers",
        };

        return roles;
    }
}
