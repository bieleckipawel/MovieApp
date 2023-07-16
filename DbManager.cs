using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static ObservableCollection<dynamic> MovieList()
        {
            var list = (from m in db.filmy
                        select new
                        {
                            movieID = m.id,
                            movieName = m.nazwa
                        ,
                            movieDir = (from n in db.rezyserowie where m.id_rezyser == n.id select n.imie + " " + n.nazwisko).FirstOrDefault()
                        ,
                            movieGen = (from n in db.gatunki where m.id_gatunek == n.id select n.nazwa).FirstOrDefault()
                        ,
                            movieDesc = m.opis
                        ,
                            movieRat = Math.Round((from n in db.oceny where m.id == n.id_film select (double)n.ocena).Average(), 2)
                        }) ;
            ObservableCollection<dynamic> observableList = new ObservableCollection<dynamic>(list);
            return observableList;
        }
    }
}
