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
            foreach (Criminal criminal in InterpolCardIndex.criminalsFoundByRequest)
            { 
                TextBlock textBlock = new TextBlock();
                textBlock.Text = i +". "+ criminal.ToString();
                textBlock.FontSize = 22;
                Results.Children.Add(textBlock);
                i++;
            }
        }
    }
}
