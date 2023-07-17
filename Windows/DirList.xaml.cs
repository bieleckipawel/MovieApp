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
    /// Logika interakcji dla klasy DirList.xaml
    /// </summary>
    public partial class DirList : Window
    {
        public DirList()
        {
            InitializeComponent();
            Refresh();
            this.Show();
        }
        private void Refresh()
        {
            MovieGrid.ItemsSource = DbManager.GetDirectors();
        }
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddDirButton_Click(object sender, RoutedEventArgs e)
        {
            //AddRate _addRate = new AddRate();
            //_addRate.Closed += (s, args) => Refresh();
        }
    }
}
