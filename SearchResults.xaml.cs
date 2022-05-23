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
using System.IO;
using Microsoft.Win32;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResults : Window
    {
        public SearchResults()
        {
            InitializeComponent();
            DisplayFound(); 
        }

        private void BackInSearchResults_Click(object sender, RoutedEventArgs e)
        {
            SearchAffair search = new SearchAffair();
            search.Show();
            this.Close();
        }

        private void DisplayFound() 
        {
            if (InterpolCardIndex.criminalsFoundByRequest.Count==0) {
                NothingFound.Visibility = Visibility.Visible;
                TextHint.Visibility = Visibility.Hidden;
                Write.Visibility = Visibility.Collapsed;
                return;
            }
            int i = 1;
            string link = "\t\tнатисніть, щоб редагувати / побачити більше\n";

            foreach (Criminal criminal in InterpolCardIndex.criminalsFoundByRequest)
            { 
                Button button = new Button();
                button.Content = i + ". " + criminal.ToString() + link;
                button.FontSize = 20;
                button.HorizontalContentAlignment = HorizontalAlignment.Left;
                button.Margin = new Thickness(3);
                Color color = Color.FromRgb(0, 118, 214);
                button.Background = new SolidColorBrush(color);
                button.MaxWidth = 1100;
                button.Tag = i-1;
                button.ToolTip = "Переглянути, редагувати, архівувати, видалити анкету " + criminal.ToString() + " - натисніть ліву клавішу миші.";
                button.MouseRightButtonDown += Button_RMBdown;
                button.Click += Button_Click;
                ResultsInner.Children.Add(button);
                i++;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int count = (int)((Control)sender).Tag;
            EditAffair edit = new EditAffair(InterpolCardIndex.criminalsFoundByRequest[count]);
           
            edit.Show();
            this.Close();
        }

        private void Button_RMBdown(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("правая");
            int count = (int)((Control)sender).Tag;
            Criminal proto = InterpolCardIndex.criminalsFoundByRequest[count];
            InterpolCardIndex.FormListToPrint(proto);      
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            InterpolCardIndex.foundToWrite.Clear();
        }

        private void Write_Click(object sender, RoutedEventArgs e)
        {
            if (InterpolCardIndex.foundToWrite.Count == 0)
            {
                MessageBox.Show("Ви не обрали жодної справи!");
                return;
            }
            InterpolCardIndex.WriteResults();
            /*str += item.Name + ";" + item.Surname + ";" + item.Nickname + ";" + item.Height + ";"
                        + item.EyeColor + ";" + item.HairColor + ";" + item.SpecialFeatures + ";" + item.Citizenship + ";"
                        + item.DateOfBirth + ";" + item.PlaceOfBirth + ";" + item.LastAccomodation + ";"
                        + item.Languages + ";" + item.CriminalJob + ";" + item.LastAffair + ";" + item.IsInBand + ";" + bandName + "\n";*/

        }
    }
}
