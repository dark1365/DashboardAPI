using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DashboardAPI.Models;
using DashboardAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DashboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly MenuContext _menuContext;
        public MenuItemController(MenuContext menuContext)
        {
            _menuContext = menuContext;
        }

        /* GET - User's Menu Items */
        [HttpGet]
        public JsonResult GetUserMenu(string userId)
        {
            /* Mock all the data for now.. */
            _menuContext.Users.Add(new User("Raju", 1));
            _menuContext.UserRoles.Add(new UserRole(1, "Raju", "1,2,3,4"));

            //Get the User first (or return 404 Not Found)
            User? user = _menuContext.Users?.Find(userId);
            if (user is null)
                return new JsonResult(NotFound());

            //Get the roles for the found user (or return 403 Forbidden)
            UserRole? role = _menuContext.UserRoles.Find(user.UserRoleId);
            if(role?.Roles is null)
                return new JsonResult(Forbid());

            //Roles are in CSV format 
            if (!string.IsNullOrEmpty(role.Roles))
            { 
                var roles = role.Roles.Split(',').ToList();
                MenuItem userMenu = BuildTopLevelMenuItem(roles);
                return new JsonResult(Ok(userMenu));
            }
            else
                return new JsonResult(NotFound());
        }

        private MenuItem BuildTopLevelMenuItem(List<string> roles)
        {
            List<MenuItem> allMenuItems = _menuContext.MenuItem.ToList();
            //TODO: Figure out why ToList() isn't working... in meantime, mock it out

            allMenuItems.Add(new MenuItem(1, "Dashboard", "dashboard", 0, 0, null));
            allMenuItems.Add(new MenuItem(2, "Operations", "operations", 1, 0, null));
            allMenuItems.Add(new MenuItem(3, "Monthly", "monthly", 0, 1, 2));
            allMenuItems.Add(new MenuItem(4, "Summary Reports", "summary", 0, 2, 3));
            allMenuItems.Add(new MenuItem(5, "Total Reports", "total", 1, 2, 3));
            
            MenuItem topLevelMenu = new MenuItem();
            topLevelMenu.subMenu = new List<MenuItem>();
            topLevelMenu.Title = "Menu";

            //Get all level 0 menu items
            foreach (var role in roles)
                topLevelMenu.subMenu.Add(allMenuItems.Where(x => x.Id.ToString().Equals(role) && x.MenuLevel == 0)?.FirstOrDefault());

            //Find highest nested level of menus (0 = lowest)
            int maxLevelIndex = allMenuItems.Max(x => x.MenuLevel);

            //Populate all sub menus using parent references and max menu nest index
            for (int menuIndex = 1; menuIndex <= maxLevelIndex; menuIndex++)
            {
                //Get parent menu items
                List<MenuItem> parents =  allMenuItems.Where(x => x.MenuLevel == menuIndex - 1).ToList();

                //Get the menu items at index level (logical parents would always be index level-1) 
                List<MenuItem> indexedMenuItems = allMenuItems.Where(x => x.MenuLevel == menuIndex).ToList();

                //Populate depending on parentId 
                foreach (var menuItem in parents)
                {
                    menuItem.subMenu = new List<MenuItem>();
                    foreach (var indexedMenuItem in indexedMenuItems)
                    { 
                        if(indexedMenuItem.parentId.Equals(menuItem.Id) && roles.Contains(indexedMenuItem.Id.ToString()))
                            menuItem.subMenu.Add(indexedMenuItem);
                    }

                    if (menuItem.subMenu.Count == 0)
                        menuItem.subMenu = null;
                    else
                        menuItem.subMenu.OrderBy(i => i.Order);

                    //Order each sub menu found according to menu order property

                }
            }

            return topLevelMenu;
        }
    }
}
