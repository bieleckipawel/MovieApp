using MovieApp.Windows;
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
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// Konstruktor okna logowania.
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Logowanie do aplikacji. Przekazuje parametry do funkcji logowania.
        /// Funkcja logowania sprawdza czy użytkownik o podanej nazwie i haśle istnieje w bazie danych i zwraca userID użytkownika
        /// oraz zwraca true jeśli logowanie jest poprawne i false jeśli wystąpił błąd.
        /// W przypadku poprawnego logowania zapisuje ID użytkownika w klasie Session i zamyka okno logowania.
        /// W przypadku błędnego logowania wyświetla komunikat o błędzie.
        /// </summary>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if(DbManager.Login(this.userName.Text,this.password.Password, out int userID, out string userFirstName))
            {
                Session.userID = userID;
                Session.userFirstName = userFirstName;
                this.Close();
            }
            else
            {
                MessageBox.Show("Niepoprawna nazwa użytkownika lub hasło. Spróbuj ponownie."
                    ,"Błąd logowania",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Wywoływane po wciśnięciu klawisza w boxach okna logowania.
        /// Jeśli klawisz to enter funkcja wywołuje funkcję Login_Click
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Login_Click(sender, e);
            }
        }
        // TODO: Dodać rejestrację użytkownika
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow _registerWindow = new RegisterWindow();
        }
    }
}
