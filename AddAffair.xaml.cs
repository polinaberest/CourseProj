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
    /// Interaction logic for AddAffair.xaml
    /// </summary>
    public partial class AddAffair : Window
    {

        public AddAffair()
        {
            InitializeComponent();
            checkBoxIsInBand.Checked += checkBox_Checked;
            
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("отмечен");
            CheckBox pressed = (CheckBox)sender;
            
        }

        private void AddData_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
