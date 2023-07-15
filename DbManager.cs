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
        //Nie haszujemy haseł w tym momencie.
        //TODO: hash i salt
        /// <summary>
        /// Funkcja logowania sprawdza czy użytkownik o podanej nazwie i haśle istnieje w bazie danych i zwraca userID użytkownika oraz imie
        /// oraz zwraca true jeśli logowanie jest poprawne i false jeśli wystąpił błąd.
        /// </summary>

        public static bool Login(string userName, string password, out int userID, out string userFirstName)
        {
            userID = -1;
            userFirstName = "";
            var pass = (from u in db.uzytkownicy where u.nickname == userName select u.password).FirstOrDefault();
            if (pass == password)
            {
                userID=(from u in db.uzytkownicy where u.nickname == userName select u.id).First();
                userFirstName = (from u in db.uzytkownicy where u.nickname == userName select u.imie).First();
                return true;
            }
            return false;
        }
        //public List<MovieDBEntities> MovieList()
        //{
            
        //}
    }
}
