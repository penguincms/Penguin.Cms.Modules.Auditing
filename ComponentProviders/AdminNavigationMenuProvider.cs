using Penguin.Cms.Modules.Core.ComponentProviders;
using Penguin.Cms.Modules.Core.Navigation;
using Penguin.Navigation.Abstractions;
using Penguin.Security.Abstractions;
using Penguin.Security.Abstractions.Interfaces;
using System.Collections.Generic;
using RoleNames = Penguin.Security.Abstractions.Constants.RoleNames;

namespace Penguin.Cms.Modules.Auditing.ComponentProviders
{
    public class AdminNavigationMenuProvider : NavigationMenuProvider
    {
        public override INavigationMenu GenerateMenuTree()
        {
            return new NavigationMenu()
            {
                Name = "Admin",
                Text = "Admin",
                Children = new List<INavigationMenu>() {
                           new NavigationMenu()
                           {
                               Text = "Audit History",
                               Icon = "list",
                               Href = "/Admin/Audit/ListEntries",
                               Permissions = new List<ISecurityGroupPermission>()
                               {
                                   CreatePermission(RoleNames.SYS_ADMIN, PermissionTypes.Read | PermissionTypes.Write)
                               }
                           },
                    }
            };
        }
    }
}