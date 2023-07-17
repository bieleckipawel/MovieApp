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
    /// <summary>
    /// Klasa menedżera bazy danych. To tu się dzieje obsługa bazy danych.
    /// </summary>
    public class DbManager
    {
        static MovieDBEntities db = new MovieDBEntities();
        //Nie haszujemy haseł w tym momencie.
        //TODO: hash i salt
        /// <summary>
        /// Logowanie użytkownika.
        /// </summary>
        /// <param name="userName">
        /// Nazwa użytkownika wprowadzona w oknie logowania.
        /// </param>
        /// <param name="password">
        /// Hasło wprowadzone w oknie logowania
        /// </param>
        /// <param name="userID">
        /// Informacja zwrotna o ID użytkownika. Prawdopodobnie zostanie później zapisana w Session.UserID.
        /// </param>
        /// <param name="userFirstName">
        /// Informacja zwrotna o imieniu użytkownika. Prawdopodobnie zostanie później zapisana w Session.UserFirstName.
        /// </param>
        /// <returns>
        /// true, jeśli logowanie jest udane
        /// false, jeśli logowanie nie jest udane
        /// </returns>

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
        /// <summary>
        /// Pobiera film o konkretnym ID z tabeli filmy przy użyciu LINQ.
        /// </summary>
        /// <param name="id">
        /// ID filmu, którego parametry chcemy pobrać.
        /// </param>
        /// <returns>
        /// ObservableCollection z danymi filmu
        /// </returns>
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
        /// Pobiera oceny dla konkretnego filmu.
        /// </summary>
        /// <param name="movieID">
        /// ID filmu którego oceny chcemy pobrać
        /// </param>
        /// <param name="observableList">
        /// Lista ocen dla tego filmu do wyświetlenia w oknie.
        /// </param>
        /// <param name="avgRate">
        /// Obliczona średnia ocena filmu
        /// </param>
        /// <param name="movieName">
        /// Nazwa filmu z bazy danych
        /// </param>
        /// <returns>
        /// true jeśli film został znaleziony w bazie
        /// false jeśli film nie został znaleziony w bazie
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
        /// <summary>
        /// Dodawanie oceny do filmu.
        /// </summary>
        /// <param name="movieID">
        /// ID filmu do którego chcemy dodać ocenę
        /// </param>
        /// <param name="rate">
        /// Ocena sama w sobie.
        /// </param>
        /// <param name="rateDesc">
        /// Opis oceny
        /// </param>
        /// <returns>
        /// true, jesli udało się dodać ocenę,
        /// false jeśli nie udało sie dodać oceny.
        /// </returns>
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
        /// <summary>
        /// aktualizaja oceny
        /// </summary>
        /// <param name="rateID">
        /// ID oceny którą zmieniamy
        /// </param>
        /// <param name="rate">
        /// Ocena sama w sobie.
        /// </param>
        /// <param name="rateDesc">
        /// Opis oceny
        /// </param>
        /// <returns>
        /// true, jesli udało się zmienić ocenę,
        /// false jeśli nie udało sie zmienić oceny.
        /// </returns>
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
        /// <summary>
        /// Pobranie ocen użytkownika.
        /// </summary>
        /// <param name="userID">
        /// ID użytkownika, którego oceny chcemy sprawdzić
        /// </param>
        /// <returns>
        /// Zwraca listę ocen użytkownika w formacie listy, [0] to id oceny, [1] to id filmu
        /// </returns>
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
        /// <summary>
        /// Sprawdzenie czy użytkownik już ocenił dany film.
        /// </summary>
        /// <param name="userID">
        /// ID użytkownika któego chcemy sprawdzić
        /// </param>
        /// <param name="movieID">
        /// ID filmu który chcemy sprawdzić
        /// </param>
        /// <param name="rateID">
        /// Informacja zwrotna z ID oceny którą udało się znaleźć, jeśli nie ma oceny to zwraca 0
        /// </param>
        /// <returns>
        /// true, jeśli ocena już istnieje
        /// false, jeśli ocena użytkownika tego filmu nie istniała.
        /// </returns>
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
        /// <summary>
        /// Pobiera listę gatunków filmowych
        /// </summary>
        /// <returns>
        /// Lista gatunków filmowych
        /// </returns>
        public static List<string> GetGenreData()
        {
            var genres = from m in db.gatunki
                         select m.nazwa;
            return genres.ToList();
        }
        /// <summary>
        /// Pobiera listę reżyserów
        /// </summary>
        /// <returns>
        /// Listę reżyserów
        /// </returns>
        public static List<string> GetDirectorData()
        {
            var genres = from m in db.rezyserowie
                         select m.imie+" "+m.nazwisko;
            return genres.ToList();
        }
        /// <summary>
        /// Dodawanie filmu do bazy danych
        /// </summary>
        /// <param name="name">
        /// Nazwa filmu
        /// </param>
        /// <param name="desc">
        /// Opis filmu
        /// </param>
        /// <param name="rel">
        /// Rok premiery
        /// </param>
        /// <param name="bud">
        /// Budżet filmu
        /// </param>
        /// <param name="genID">
        /// ID gatunku
        /// </param>
        /// <param name="dirID">
        /// ID reżysera
        /// </param>
        /// <returns>
        /// true, jeśli udało się dodać wpis,
        /// false jeśli nie.
        /// </returns>
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
        /// <summary>
        /// Pobiera listę reżyserów
        /// </summary>
        /// <returns>Listę reżyserów</returns>
        public static ObservableCollection<dynamic> GetDirectors()
        {
            var directors = from m in db.rezyserowie
                            select new
                            {
                                dirFName = m.imie,
                                dirLName = m.nazwisko,
                                dirCountry = m.kraj,
                                dirBDate = m.data_urodzenia
                            };
            dynamic observableList = new ObservableCollection<dynamic>(directors);
            return observableList;
        }
        /// <summary>
        /// Dodawanie reżysera do bazy danych
        /// </summary>
        /// <param name="firstName"> Imię reżysera </param>
        /// <param name="lastName">Nazwisko reżysera</param>
        /// <param name="country">Kraj pochodzenia w formacie ISO 3166 ALPHA-2</param>
        /// <param name="birthDate"> Data urodzenia w formacie dd.MM.yyyy </param>
        /// <returns>
        /// true jeśli udało się dodać film,
        /// false jeśli nie udało się.
        /// </returns>
        public static bool AddDirector(string firstName, string lastName, string country, DateTime birthDate) 
        {
            rezyserowie newDirector = new rezyserowie();
            {
                newDirector.imie = firstName;
                newDirector.nazwisko = lastName;
                newDirector.kraj = country;
                newDirector.data_urodzenia = birthDate;
            };
            db.rezyserowie.Add(newDirector);
            if(db.SaveChanges() > 0) return true; 
            else return false;
        }
        /// <summary>
        /// Rejestracja użytkownika. Dodajemy do bazy danych nowego usera
        /// </summary>
        /// <param name="username">
        /// Nazwa użytkownika
        /// </param>
        /// <param name="fName">
        /// Imię
        /// </param>
        /// <param name="lName">
        /// Nazwisko
        /// </param>
        /// <param name="favGen">
        /// Ulubiony gatunek do wyboru z listy rozwijanej
        /// </param>
        /// <param name="pass">
        /// Hasło.
        /// </param>
        /// <returns>
        /// true jeśli udało się utworzyć użytkownika,
        /// false jeśli to nie zadziałało.
        /// </returns>
        //Todo: to samo co w logowaniu. przydałoby się nie przechowywać haseł w DB.
        public static bool UserRegister(string username, string fName, string lName, int favGen, string pass)
        {
            uzytkownicy newUser = new uzytkownicy();
            {
                newUser.nickname = username;
                newUser.imie = fName;
                newUser.nazwisko = lName;
                newUser.ulubiony_gatunek = favGen;
                newUser.password = pass;
            };
            db.uzytkownicy.Add(newUser);
            if(db.SaveChanges() > 0 ) return true;
            else return false;
        }
        /// <summary>
        /// Sprawdza czy użytkownik o podanym loginie przypadkiem już nie istnieje
        /// </summary>
        /// <param name="username">
        /// Nazwa użytkownika do sprawdzenia
        /// </param>
        /// <returns>
        /// true, jeśli użytkownik już istnieje
        /// false, jeśli użytkownik nie istnieje.
        /// </returns>
        public static bool UserExists(string username)
        {
            var userlist = from m in db.uzytkownicy select m.nickname;
            if (userlist.Contains(username)) return true;
            else return false;
        }
    }
}
