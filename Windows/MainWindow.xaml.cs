using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MovieApp.Windows;

namespace MovieApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Przed inicjalizacją głównego okna otwieram okno logowania.
            //Jeśli nie zostanie zwrócone poprawne ID użytkownika to zamykam aplikację.
            LoginWindow _loginWindow = new LoginWindow();
            _loginWindow.ShowDialog();
            if(Session.userID == -1)
            {
                this.Close();
            }
            
            InitializeComponent();
            //Ustawiam powitanie zalogowanego użytkownika.
            this.WelcomeLabel.Content = "Witaj " + Session.userFirstName + "!";
            //Pobieram listę filmów z bazy danych i przypisuję ją do DataGrid.
            Refresh();
        }
        private void MovieGrid_OpenRate(object sender, MouseButtonEventArgs e)
        {
            dynamic selectedMovie = MovieGrid.SelectedItem;
            //po co to liczyć dwa razy skoro mogę przekazać to jako parametr?
            int movieID = selectedMovie.movieID;
            MovieRate _movieRate = new MovieRate(movieID);    
            _movieRate.Closed += (s, args) => Refresh();
        }
        private void Refresh()
        {
            this.MovieGrid.ItemsSource = DbManager.MovieList();
        }

        private void AddMovieButton_Click(object sender, RoutedEventArgs e)
        {
            AddMovie _addMovie = new AddMovie();
            _addMovie.Closed += (s, args) => Refresh();
        }
        private void DirList_Click(object sender, RoutedEventArgs e)
        {
            DirList _dirList = new DirList();
        }
    }
}
