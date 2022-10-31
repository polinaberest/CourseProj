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
        // конструктор класу
        public SearchResults()
        {
            InitializeComponent();
            DisplayFound(); 
        }

        private void DisplayFound()
        {
            if (PoliceCardIndex.CriminalsFoundByRequest.Count == 0)
            {
                NothingFound.Visibility = Visibility.Visible;
                TextHint.Visibility = Visibility.Hidden;
                Write.Visibility = Visibility.Collapsed;
                return;
            }
            int i = 1;
            string link = "\t\tнатисніть, щоб редагувати / побачити більше\n";

            foreach (Criminal criminal in PoliceCardIndex.CriminalsFoundByRequest)
            {
                Button button = new Button();
                button.Content = i + ". " + criminal.ToString() + link;
                button.FontSize = 20;
                button.HorizontalContentAlignment = HorizontalAlignment.Left;
                button.Margin = new Thickness(3);
                Color color = Color.FromRgb(0, 118, 214);
                button.Background = new SolidColorBrush(color);
                button.MaxWidth = 1100;
                button.Tag = i - 1;
                button.ToolTip = "Переглянути, редагувати, архівувати, видалити анкету " +
                    criminal.ToString() + " - натисніть ліву клавішу миші.";
                button.MouseRightButtonDown += Button_RMBdown;
                button.Click += Button_Click;
                ResultsInner.Children.Add(button);
                i++;
            }
        }

        private void HighlightAdded(bool isIn, Button button)
        {
            Color initColor = Color.FromRgb(0, 118, 214);
            Color newColor = Color.FromRgb(34, 139, 34);
            if (isIn)
                button.Background = new SolidColorBrush(newColor);
            else
                button.Background = new SolidColorBrush(initColor);
        }

        private void BackInSearchResults_Click(object sender, RoutedEventArgs e)
        {
            SearchAffair search = new SearchAffair();
            search.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int count = (int)((Control)sender).Tag;
            EditAffair edit = new EditAffair(PoliceCardIndex.CriminalsFoundByRequest[count]);
           
            edit.Show();
            this.Close();
        }

        private void Button_RMBdown(object sender, RoutedEventArgs e)
        {
            int count = (int)((Control)sender).Tag;
            bool isIncluded;
            
            Criminal proto = PoliceCardIndex.CriminalsFoundByRequest[count];
            PoliceCardIndex.FormListToPrint(proto, out isIncluded);
            HighlightAdded(isIncluded, (sender as Button));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PoliceCardIndex.FoundToWrite.Clear();
        }

        private void Write_Click(object sender, RoutedEventArgs e)
        {
            if (PoliceCardIndex.FoundToWrite.Count == 0)
            {
                MessageBox.Show("Ви не обрали жодної справи!");
                return;
            }
            PoliceCardIndex.WriteResults();
        }
    }
}
