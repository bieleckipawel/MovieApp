using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public MovieRate(int movieID)
        {   
            InitializeComponent();
            this.WelcomeLabel.Content = "Witaj " + Session.userFirstName + "!";
            //Pobieram listę ocen z bazy danych i przypisuję ją do DataGrid.
            //Jak nie mam takiego filmu to musi być baza uszkodzona, innej opcji ATM nie widzę.
            ObservableCollection<dynamic> rateList = new ObservableCollection<dynamic>();
            if(DbManager.RateList(movieID,out rateList)){
                this.Show();
                this.MovieGrid.ItemsSource = rateList;
            }
            else
            {
                MessageBox.Show("Błąd spójności bazy danych. Spróbuj ponownie lub skontaktuj się z twórcą."
                    , "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }
}
