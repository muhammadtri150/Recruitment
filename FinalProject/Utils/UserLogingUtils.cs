using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public static class UserLogingUtils
    {
        //----------------------------------------------------------------------------------------------------------
        // for add data to table loging and save any activity of user, return int is value ov save cnhage data, 
        //if value is more then 0 equals insert data is success
        //----------------------------------------------------------------------------------------------------------
        public static int SaveLoggingUserActivity(string notes)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    //prepare data for tb log user activity
                    //user id gained from session[UserLogin]
                    //note gained frm parameter is fill in each action method

                    UserDTO DataUserLogin = (UserDTO)HttpContext.Current.Session["UserLogin"];

                    db.TB_LOG_USER_ACTIVITY.Add(
                        new TB_LOG_USER_ACTIVITY
                        {
                            USER_ID = DataUserLogin.USER_ID,
                            Notes = notes,
                            Time = DateTime.Now
                        }
                    );

                    return db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static UserDTO GetDataUserLogin()
        {
            return (UserDTO)HttpContext.Current.Session["UserLogin"];
        }
    }
}