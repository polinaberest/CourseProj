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
    /// Interaction logic for SearchAffair.xaml
    /// </summary>
    public partial class SearchAffair : Window
    {
        public SearchAffair()
        {
            InitializeComponent();
            checkBoxIsInBand.Checked += checkBoxIsInBand_Checked;
            checkBoxIsInBand.Unchecked += checkBoxIsInBand_Checked;
        }

        private void checkBoxIsInBand_Checked(object sender, RoutedEventArgs e)
        {
            textBoxBandName.Visibility = Visibility.Visible;
        }

        private void checkBoxIsInBand_Unchecked(object sender, RoutedEventArgs e)
        {
            textBoxBandName.Visibility = Visibility.Hidden;
        }

        private void SearchData_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
