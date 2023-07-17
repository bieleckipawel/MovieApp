using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
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
                        //jak nie mam ocen to głupioby było z nich liczyć średnią
                            movieRat = (from n in db.oceny where m.id == n.id_film select (double)n.ocena).Any() ? Math.Round((from n in db.oceny where m.id == n.id_film select (double)n.ocena).Average(), 2) : 0
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
                            //jak nie mam ocen to głupioby było z nich liczyć średnią
                            movieRat = (from n in db.oceny where m.id == n.id_film select (double)n.ocena).Any() ? Math.Round((from n in db.oceny where m.id == n.id_film select (double)n.ocena).Average(), 2) : 0
                        });
            ObservableCollection<dynamic> observableList = new ObservableCollection<dynamic>(list);
            return observableList;
        }
        /// <summary>
        /// Pobiera oceny oraz dane o ocenach z tabeli oceny przy użyciu LINQ.
        /// W bieżącej implementacji nie zajmujemy się obsługą błędów bo przejmuje to funkcja w ViewModelu.
        /// </summary>
        /// <returns>
        /// ObservableCollection z danymi ocen dla konkretnego filmu.
        /// Średnią ocenę dla filmu
        /// Nazwę filmu
        /// </returns>
        public static bool RateList(int movieID, out ObservableCollection<dynamic> observableList, out double avgRate, out string movieName)
        {
            avgRate = 0;
            movieName = "";
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
            if (movieData.Any())
            {
                foreach (dynamic movie in movieData)
                {
                    avgRate = movie.movieRat;
                    movieName = movie.movieName;
                }
                return true;
            }
            else return false;
        }
        public static bool AddRate(int movieID, short rate, string rateDesc)
        {
            int userID = Session.userID;
            oceny nowaOcena = new oceny
            {
                id_film = movieID,
                id_uzytkownik = userID,
                ocena = rate,
                opis = rateDesc
            };
            db.oceny.Add(nowaOcena);
            if (db.SaveChanges() > 0) return true;
            else return false;
        }
        public static bool UpdateRate(int rateID, short rate, string rateDesc)
        {
            int userID = Session.userID;
            var row = db.oceny.Find(rateID);
            if (row != null)
            {
                row.ocena = rate;
                row.opis = rateDesc;
                db.oceny.AddOrUpdate(row);
                if (db.SaveChanges()>0) return true;
                else return false;
            }
            return false;
        }
        public static List<int>[] GetUsersRatedMovies(int userID)
        {
            var movies = from m in db.oceny
                         where m.id_uzytkownik == userID
                         select new
                         {
                             movieID = m.id_film,
                             rateID = m.id
                         };
            List<int>[] data = new List<int>[2];
            data[0] = new List<int>(); // Inicjalizacja id oceny
            data[1] = new List<int>(); // Inicjalizacja id filmu
            foreach (var movie in movies)
            {
                data[0].Add(movie.rateID);
                data[1].Add(movie.movieID);
            }
            return data;
        }
        public static bool DidUserAlreadyRateThisMovie(int userID, int movieID, out int rateID)
        {
            rateID = 0;
            var info = GetUsersRatedMovies(userID);
            if (info[1].Contains(movieID))
            {
                int index = info[1].IndexOf(movieID);
                rateID = info[0][index];
                return true;
            }
            else return false;
        }
        public static List<string> GetGenreData()
        {
            var genres = from m in db.gatunki
                         select m.nazwa;
            return genres.ToList();
        }
        public static List<string> GetDirectorData()
        {
            var genres = from m in db.rezyserowie
                         select m.imie+" "+m.nazwisko;
            return genres.ToList();
        }
        public static bool AddMovie(string name, string desc, short rel, int bud, int genID, int dirID)
        {
            filmy newMovie = new filmy();
            {
                newMovie.nazwa = name;
                newMovie.opis = desc;
                newMovie.rok_premiery = rel;
                newMovie.budzet = bud;
                newMovie.id_gatunek = genID;
                newMovie.id_rezyser = dirID;
            };
            db.filmy.Add(newMovie);
            if (db.SaveChanges() > 0) return true;
            else return false;
        }
    }
}
