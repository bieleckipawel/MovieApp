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

namespace MovieApp.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        /// <summary>
        /// Konstruktor okna rejestracji użytkownika
        /// </summary>
        public RegisterWindow()
        {
            InitializeComponent();
            var genList = DbManager.GetGenreData();
            foreach (var gen in genList)
            {
                movieGen.Items.Add(gen);
            }
            this.Show();
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string login = userName.Text.Trim();
            string fName = userFName.Text.Trim();
            string lName = userLName.Text.Trim();
            int favGen = movieGen.SelectedIndex + 1;
            string pass = password.Password.Trim();
            if (!DbManager.UserExists(login))
            {
                if (DbManager.UserRegister(login, fName, lName, favGen, pass))
                {
                    MessageBox.Show("Rejestracja zakończona pomyślnie. Teraz możesz się zalogować.", "Rejestracja", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Błąd rejestracji. Sprawdź wszystkie dane i spróbuj ponownie.", "Rejestracja", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Użytkownik o podanej nazwie już istnieje. Spróbuj ponownie używając innej nazwy", "Rejestracja", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Register_Click(sender, e);
            }
        }
    }
}
