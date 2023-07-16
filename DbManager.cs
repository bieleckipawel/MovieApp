using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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
        /// <summary>
        /// Pobiera listę filmów oraz dane o filmach z tabeli filmy przy użyciu LINQ.
        /// </summary>
        /// <returns>
        /// ObservableCollection z danymi filmu.
        /// </returns>
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
        public static ObservableCollection<dynamic> MovieList(int id)
        {
            var list = (from m in db.filmy
                        where m.id == id
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
                        });
            ObservableCollection<dynamic> observableList = new ObservableCollection<dynamic>(list);
            return observableList;
        }
        /// <summary>
        /// Pobiera oceny oraz dane o ocenach z tabeli oceny przy użyciu LINQ.
        /// Nie obsługujemy wyjątków ponieważ w przypadku braku ocen dla filmu 
        /// (lub braku filmu, co raczej nie powinno się zdarzyć) zwracana jest pusta lista.
        // TODO: napisać jednak wyjątek bo co jeśli chcemy dodać ocenę do filmu którego nie ma?
        /// </summary>
        /// <returns>
        /// ObservableCollection z danymi ocen dla konkretnego filmu.
        /// </returns>
        public static bool RateList(int movieID, out ObservableCollection<dynamic> observableList)
        {
            var rates = (from m in db.oceny
                         where m.id_film == movieID
                         select new
                         {
                             userName = (from n in db.uzytkownicy where m.id_uzytkownik == n.id select n.nickname).FirstOrDefault()
                             ,
                             movieRate = m.ocena
                             ,
                             rateDesc = m.opis

                         });
            var movieData = MovieList(movieID);
            observableList = new ObservableCollection<dynamic>(rates);
            if (movieData.Any()) return true;
            else return false;
            
        }
    }
}
