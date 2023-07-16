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
using System.Windows.Shapes;

namespace MovieApp
{
    /// <summary>
    /// Logika interakcji dla klasy AddRate.xaml
    /// </summary>
    public partial class AddRate : Window
    {
        public static int globalMovieID;
        public AddRate(int movieID, string movieName)
        {
            InitializeComponent();
            this.TitleBox.Text+=movieName;
            globalMovieID = movieID;
            this.Show();
        }
        private void Add_Rate(object sender, RoutedEventArgs e)
        {
            short rateInt;
            if (!short.TryParse(this.rate.Text, out rateInt) || rateInt > 5 || rateInt < 1)
            {
                MessageBox.Show("Podano nieprawidłową ocenę. Poprawna ocena to liczba całkowita z zakresu 1-5."
                   , "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string rateDesc = this.rateDesc.Text.Trim();
            if (DbManager.DidUserAlreadyRateThisMovie(Session.userID, globalMovieID))
            {
                MessageBoxResult result = MessageBox.Show("Dodawałeś już ocenę dla tego filmu, czy zaktualizować?\nMożesz dodać jedną ocenę dla filmu.", "Potwierdzenie", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    this.Close();
                    return;
                }
                else DbManager.UpdateRate(globalMovieID, rateInt, rateDesc);
            }
            if (DbManager.AddRate(globalMovieID, rateInt, rateDesc))
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Wystąpił błąd podczas próby dodania oceny. Spróbuj ponownie lub skontaktuj się z twórcą."
                  , "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Add_Rate(sender, e);
            }
        }
    }
}
