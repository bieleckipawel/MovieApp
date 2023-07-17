using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace MovieApp
{
    /// <summary>
    /// Logika interakcji dla klasy MovieRate.xaml
    /// </summary>
    public partial class MovieRate : Window
    {
        private static int globalMovieID;
        private static string globalMovieName;
        /// <summary>
        /// Okno wyświetlające oceny filmu.
        /// Pozwala również na dodanie oceny.
        /// </summary>
        /// <param name="movieID">
        /// ID filmu, którego oceny chcemy wyświetlić.
        /// </param>
        public MovieRate(int movieID)
        {
            MovieRate.globalMovieID = movieID;
            InitializeComponent();
            if (this.Refresh()) this.Show();
            else this.Close();
        }
        //Pobieram listę ocen z bazy danych i przypisuję ją do DataGrid.
        //Jak nie mam takiego filmu to muszą być niespójne tabele filmy i oceny, innej opcji ATM nie widzę.
        private bool Refresh()
        {
            ObservableCollection<dynamic> rateList = new ObservableCollection<dynamic>();
            if (DbManager.RateList(globalMovieID, out rateList, out double avgRate, out string movieName))
            {
                this.MovieGrid.ItemsSource = rateList;
                MovieRate.globalMovieName = movieName;
                this.TaskLabel.Content = "Oceny filmu " + movieName + "; średnia ocena: " + avgRate;
                return true;
            }
            else
            {
                MessageBox.Show("Błąd spójności bazy danych. Spróbuj ponownie lub skontaktuj się z twórcą."
                    , "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        /// <summary>
        /// Zamykam okno przyciskiem "Powrót".
        /// </summary>
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddRateButton_Click(object sender, RoutedEventArgs e)
        {
            AddRate _addRate = new AddRate(globalMovieID, globalMovieName);
            _addRate.Closed += (s, args) => Refresh();
        }
    }
}
