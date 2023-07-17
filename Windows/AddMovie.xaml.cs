using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    /// Logika interakcji dla klasy AddMovie.xaml
    /// </summary>
    public partial class AddMovie : Window
    {
        public AddMovie()
        {
            InitializeComponent();
            var genList = DbManager.GetGenreData();
            foreach (var gen in genList)
            {
                movieGen.Items.Add(gen);
            }
            var dirList = DbManager.GetDirectorData();
            foreach (var dir in dirList)
            {
                movieDir.Items.Add(dir);
            }
            this.Show();
        }
        private void Add_Movie()
        {
            string name = movieName.Text.Trim();
            string desc = movieDesc.Text.Trim();
            if(short.TryParse(movieRelYear.Text.Trim(),out short year) && Int32.TryParse(movieBud.Text.Trim(), out int budget))
            {
                //bo liczymy od 0
                int genID = movieGen.SelectedIndex+1;
                int dirID = movieDir.SelectedIndex+1;
                if (genID == -1 || dirID == -1)
                {
                    Error();
                    return;
                }
                if (name.Length > 50)
                {
                    Error();
                    return;
                }
                if(DbManager.AddMovie(name, desc, year, budget, genID, dirID))
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wystąpił błąd podczas próby dodania filmu. Spróbuj ponownie lub skontaktuj się z twórcą."
                  , "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Error();
                return;
            }
        }
        /// <summary>
        /// Zwraca ten sam błąd za każdym razem.
        /// </summary>
        private void Error()
        {
            MessageBox.Show("Nieprawidłowe dane lub zbyt długi tytuł - jego limit to 50 znaków. Wprowadź poprawne dane i spróbuj ponownie."
                                      , "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Add_Movie();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add_Movie();
        }
    }
}
