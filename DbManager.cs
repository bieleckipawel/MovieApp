using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp
{
    public class DbManager
    {
        static MovieDBEntities db = new MovieDBEntities();
        //Not hashing password at the moment.
        //TODO: hash and salt passwords
        public static bool Login(string userName, string password, out int userID)
        {
            userID = -1;
            var pass = (from u in db.uzytkownicy where u.nickname == userName select u.password).FirstOrDefault();
            if (pass == password)
            {
                userID=(from u in db.uzytkownicy where u.nickname == userName select u.id).First();
                return true;
            }
            return false;
        }
    }
}
