using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using FinalProject.DTO;
using FinalProject.Filters;

namespace FinalProject.Utils
{
    [AuthFilter]
    public class MenuUtils
    {

//------------------------------------------------------------ method for get menu based on role id of user login --------------------------------
        public List<MenuDTO> GetMenus()
        {
            using(DBEntities db = new DBEntities())
            {
                //get data user in user dto format by session user login
                var Context = HttpContext.Current;
                UserDTO UserLogin = (UserDTO)Context.Session["UserLogin"];

                //get data menus from table menu and change to menudto
                //get data menu from tb_menu joined with tb_access menu and where tb_access_menu.role id equal userlogin.role_id
                List<MenuDTO> ListMenu = (from Menu in db.TB_MENU
                                         join AccessMenu in db.TB_ACCESS_MENU
                                         on Menu.MENU_ID equals AccessMenu.MENU_ID
                                         where AccessMenu.ROLE_ID.Equals(UserLogin.ROLE_ID)
                                         select new MenuDTO
                                         {
                                             MENU_ID = Menu.MENU_ID,
                                             TITLE_MENU = Menu.TITLE_MENU,
                                             LOGO_MENU = Menu.LOGO_MENU,
                                         }).ToList();
                return ListMenu;
            }

        }

//----------------------------------------------method for get sub menu based on menu id is based on role id user login-----------------------------
        public List<SubMenuDTO> GetSubmenu(MenuDTO Menu)
        {
            using(DBEntities db = new DBEntities())
            {
                List<SubMenuDTO> ListSubMenuDTO = (from SubMenu in db.TB_SUBMENU where SubMenu.MENU_ID.Equals(Menu.MENU_ID)
                                                   select new SubMenuDTO{
                                                       SUB_MENU_ID = SubMenu.SUB_MENU_ID,
                                                       MENU_ID = SubMenu.MENU_ID,
                                                       TITLE_SUBMENU = SubMenu.TITLE_SUBMENU,
                                                       LOGO_SUBMENU = SubMenu.LOGO_SUBMENU,
                                                       URL = SubMenu.URL
                                                   } ).ToList();
                return ListSubMenuDTO;
            }
        }
    }
}