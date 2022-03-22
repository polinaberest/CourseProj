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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InterpolCardIndex interpolCardIndex = new InterpolCardIndex();
        }

        private void EditAffair_Click(object sender, RoutedEventArgs e)
        {
            SearchAffair searchAffair = new SearchAffair();
            searchAffair.Show();
            this.Close();
        }

        private void AddAffair_Click(object sender, RoutedEventArgs e)
        {
            AddAffair addAffair = new AddAffair();
            addAffair.Show();
            this.Close();
        }
    }
}
