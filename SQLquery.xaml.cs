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
using System.Data;
using System.Data.SqlClient;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for SQLquery.xaml
    /// </summary>
    public partial class SQLquery : Window
    {
        //new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd).Text;

        DataTable resultTable;

        public SQLquery()
        {
            InitializeComponent();

            resultTable = new DataTable();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TopText2.Visibility = Visibility.Collapsed;
            SelectResults.Visibility = Visibility.Collapsed;
            SetText(selectRichBox, "SELECT");
        }

        private void BackInMain_Click(object sender, RoutedEventArgs e)
        {
            DetAffair da = new DetAffair();
            da.Show();
            this.Close();
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            resultTable.Clear();

            try
            {
                string query = GetText(selectRichBox);

                SqlDataAdapter adapter = new SqlDataAdapter(query, PoliceCardIndex.GetSqlConnection());

                adapter.Fill(resultTable);

                SelectResults.ItemsSource = resultTable.AsDataView();
                SelectResults.AutoGenerateColumns = true;
                SelectResults.IsReadOnly = true;

                
            }
            catch (Exception ex)
            {
                
            }
            finally
            { 
                TopText2.Visibility=Visibility.Visible;
                SelectResults.Visibility = Visibility.Visible;
               
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            SetText(selectRichBox, "SELECT");
            SelectResults.ItemsSource = null;
        }

        private void SetText(RichTextBox richTextBox, string text)
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        private string GetText(RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd).Text;
        }
    }
}
