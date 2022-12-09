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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        DataTable table = new DataTable();
        DataTable tableDep = new DataTable();
        DataTable tableCrimes = new DataTable();
        DataTable tableArchive = new DataTable();

        List<Crime> crList = new List<Crime>();

        public SeriesCollection Series { get; set; }


        public Statistics()
        {
            InitializeComponent();

            Series = new SeriesCollection();

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter adapterD = new SqlDataAdapter($"SELECT TOP 10 WITH TIES first_name + ' ' + surname AS 'ПІ', badge_num AS 'Номер значка', a.affair_type AS 'Спеціалізація', dep.department_name AS 'Відділ міста', FORMAT(last_visit_date, 'dd.MM.yyyy    hh:mm:ss') AS 'Останній вхід' FROM Detectives d, Departments dep, Affair_Types a WHERE d.department_id = dep.department_id AND d.type_id = a.type_id ORDER BY last_visit_date DESC; ", PoliceCardIndex.GetSqlConnection());
            adapterD.Fill(table);

            SqlDataAdapter adapterDep = new SqlDataAdapter("SELECT TOP 3  WITH TIES dep.department_name AS 'Відділ міста', dep.post_index AS 'Поштовий індект відділу', COUNT(distinct d.detective_id) AS 'Кількість детективів', COUNT(c.detective_id) AS 'Кількість розслідуваних злочинів' FROM Departments dep, Detectives d, Crimes c WHERE dep.department_id = d.department_id AND d.detective_id = c.detective_id  GROUP BY dep.department_name, dep.post_index ORDER BY COUNT(c.detective_id) desc, COUNT(distinct d.detective_id) desc", PoliceCardIndex.GetSqlConnection());
            adapterDep.Fill(tableDep);

            SqlDataAdapter adapterCr = new SqlDataAdapter("SELECT a.affair_type AS 'Title', COUNT(c.crime_id) AS 'Count' FROM Crimes c, Affair_Types a  WHERE c.type_id = a.type_id  GROUP BY a.affair_type ORDER BY COUNT(c.crime_id) desc; ", PoliceCardIndex.GetSqlConnection());
            adapterCr.Fill(tableCrimes);

            SqlDataAdapter adapterA= new SqlDataAdapter(PoliceCardIndex.Statsquery, PoliceCardIndex.GetSqlConnection());
            adapterA.Fill(tableArchive);

            Det_LastVisit.ItemsSource = table.AsDataView();
            Det_LastVisit.AutoGenerateColumns = true;

            Det_Top3.ItemsSource = tableDep.AsDataView();
            Det_LastVisit.AutoGenerateColumns = true;

            Archive_Stats.ItemsSource = tableArchive.AsDataView();
            Archive_Stats.AutoGenerateColumns = true;

            foreach (DataRow dr in tableCrimes.Rows)
            {
                string title = dr["Title"].ToString();
                int count = Convert.ToInt32(dr["Count"]);
                Crime cr = new Crime(title, count);
                crList.Add(cr);
            }

            foreach (Crime crime in crList)
            {
                Series.Add(new PieSeries { Title = crime.Title, Values = new ChartValues<ObservableValue> { new ObservableValue(crime.CrimeNumber) }, DataLabels = true });
            }
        }

        private void BackInMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void Refresh_Det_Click(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter adapterD = new SqlDataAdapter($"SELECT TOP 10 WITH TIES first_name + ' ' + surname AS 'ПІ', badge_num AS 'Номер значка', a.affair_type AS 'Спеціалізація', dep.department_name AS 'Відділ міста', FORMAT(last_visit_date, 'dd.MM.yyyy    hh:mm:ss') AS 'Останній вхід' FROM Detectives d, Departments dep, Affair_Types a WHERE d.department_id = dep.department_id AND d.type_id = a.type_id ORDER BY last_visit_date DESC; ", PoliceCardIndex.GetSqlConnection());
            table.Clear();
            adapterD.Fill(table);

            Det_LastVisit.ItemsSource = table.AsDataView();
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
