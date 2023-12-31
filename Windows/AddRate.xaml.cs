﻿using System;
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
        private static int globalMovieID;
        /// <summary>
        /// Konstruktor okna, w którym użytkownik może dodać ocenę dla filmu.
        /// </summary>
        /// <param name="movieID">
        /// ID filmu dla którego dodajemy ocenę.
        /// </param>
        /// <param name="movieName">
        /// Nazwa filmu dla którego dodajemy ocenę - służy do wyświetlania w tytule :)
        /// </param>
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
            if (DbManager.DidUserAlreadyRateThisMovie(Session.userID, globalMovieID, out int rateID))
            {
                MessageBoxResult result = MessageBox.Show("Dodawałeś już ocenę dla tego filmu, czy zaktualizować?\nMożesz dodać jedną ocenę dla filmu.", "Potwierdzenie", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    this.Close();
                    return;
                }
                else
                {
                    if (!DbManager.UpdateRate(rateID, rateInt, rateDesc)) MessageBox.Show("Wystąpił błąd podczas próby aktualizacji oceny. Spróbuj ponownie lub skontaktuj się z twórcą."
                  , "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    return;
                }
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
