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
        public LoginWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Logowanie do aplikacji. Sprawdza czy użytkownik o podanej nazwie i haśle istnieje w bazie danych.
        /// W przypadku poprawnego logowania zapisuje ID użytkownika w klasie Session i zamyka okno logowania.
        /// W przypadku błędnego logowania wyświetla komunikat o błędzie.
        /// </summary>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if(DbManager.Login(this.userName.Text,this.password.Password, out int userID))
            {
                Session.userID = userID;
                this.Close();
            }
            else
            {
                MessageBox.Show("Niepoprawna nazwa użytkownika lub hasło. Spróbuj ponownie.","Błąd logowania",MessageBoxButton.OK,MessageBoxImage.Error);
            }
                
        }
    }
}
