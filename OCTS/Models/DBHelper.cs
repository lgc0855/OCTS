using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCTS.Models
{
    public class DBHelper
    {
        public static DBHelper myHelper = null;

        public static DBHelper getMyHelper()
        {
            myHelper = new DBHelper();
            return myHelper;
        }

        public List<User> getUsers()
        {
            UserDBContext userdb = new UserDBContext();
            List<User> users = userdb.users.ToList();
            userdb = null;
            return users;
        }
    }

   


}