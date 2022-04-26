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
                return;
            }
            int i = 1;
            string link = "\t\tнатисніть, щоб редагувати / побачити більше\n";
            foreach (Criminal criminal in InterpolCardIndex.criminalsFoundByRequest)
            { 
                Button button = new Button();
                button.Content = i + ". " + criminal.ToString() + link;
/*                if (textBlock.IsMouseOver)
                    textBlock.Foreground = Brushes.Blue;*/
                button.FontSize = 20;
                button.Margin = new Thickness(3);
                Color color = Color.FromRgb(0, 118, 214);
                button.Background = new SolidColorBrush(color);
                button.MaxWidth = 1100;
                button.Tag = i-1;
                button.ToolTip = "Переглянути, редагувати, архівувати, видалити анкету " + criminal.ToString();
                button.Click += Button_Click;
                Results.Children.Add(button);
                i++;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int count = (int)((Control)sender).Tag;

            //MessageBox.Show(count.ToString());
        }
    }
}
