using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Logika interakcji dla klasy AddDir.xaml
    /// </summary>
    public partial class AddDir : Window
    {
        public AddDir()
        {
            InitializeComponent();
            this.Show();
        }
        private void Add_Dir(object sender, RoutedEventArgs e)
        {
            string fName = dirName.Text.Trim();
            string lName = dirLName.Text.Trim();
            string country = dirCT.Text.Trim();
            string bDate = dirBDate.Text.Trim();
            if (fName.Length > 30 || lName.Length > 50 || country.Length != 2)
            {
                Error();
                return;
            }
            else
            {
                if (DateTime.TryParseExact(bDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    if (DbManager.AddDirector(fName,lName,country,date))
                    {
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd podczas próby dodania reżysera. Spróbuj ponownie lub skontaktuj się z twórcą."
                  , "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    Error();
                    return;
                }
            }
        }
        private void Error()
        {
            MessageBox.Show("Nieprawidłowe lub zbyt długie dane. Wprowadź poprawne dane i spróbuj ponownie."
                                      , "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Add_Dir(sender, e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add_Dir(sender,e);
        }
    }
}
