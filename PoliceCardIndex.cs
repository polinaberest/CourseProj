using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CourseProj
{
    // клас картотеки
    internal static class PoliceCardIndex
    {
        // перелік усіх банд, що зареєстровані в базі
        public static List<CrimeBand>? AllBands;

        private static SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-SH7CLGN\SQLEXPRESS;Initial Catalog=Interpol;Integrated Security=True");

        // перелік усіх злочинців, анкети яких зберігаються в основній базі
        public static List<Criminal>? Criminals;

        // перелік злочинців, знайдених за поточним запитом
        public static List<Criminal>? CriminalsFoundByRequest;

        // перелік злочинців, справи яких збережено в архіві
        public static List<Criminal>? Archived;

        // перелік злочинців, справи яких треба занести до файлу-витягу
        public static List<Criminal>? FoundToWrite;

        public static bool IsDetective = false;

        public static int DetectiveID = 0;

        // конструктор без параметрів
        static PoliceCardIndex()
        {
            AllBands = new List<CrimeBand>();
            Criminals = new List<Criminal>();
            CriminalsFoundByRequest = new List<Criminal>();
            Archived = new List<Criminal>();
            FoundToWrite = new List<Criminal>();
        }

        // метод додавання злочинця до переліку картотеки
        public static void AddCriminal(Criminal criminal)
        {
            Criminals.Add(criminal);
            SortByNames(Criminals);
        }

        // метод додавання банди до переліку картотеки
        public static void AddBand(CrimeBand band)
        {
            AllBands.Add(band);
        }

        public static void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            { 
                 sqlConnection.Open();
            }
        }

        public static void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public static SqlConnection GetSqlConnection()
        {
            return sqlConnection;
        }

        // метод запису переліку картотеки в файл для збереження
        public static void WriteToFile(string p)
        {
            string path = p;
            string str = "";
            string bandName = "-1";

            if (path == "criminals.txt")
            {
                foreach (var item in Criminals)
                {
                    if (item.IsInBand)
                    {
                        bandName = item.BandName;
                    }
                    str += item.Name + ";" + item.Surname + ";" + item.Nickname + ";" + item.Height + ";"
                        + item.EyeColor + ";" + item.HairColor + ";" + item.SpecialFeatures + ";" + item.Citizenship + ";"
                        + item.DateOfBirth + ";" + item.PlaceOfBirth + ";" + item.LastAccomodation + ";"
                        + item.Languages + ";" + item.CriminalJob + ";" + item.LastAffair + ";" + item.IsInBand + ";" + bandName + "\n";
                }
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(str);
                    writer.Close();
                }
            }
            else if (path == "archived.txt")
            {
                foreach (var item in Archived)
                {
                    if (item.IsInBand)
                    {
                        bandName = item.BandName;
                    }
                    str += item.Name + ";" + item.Surname + ";" + item.Nickname + ";" + item.Height + ";"
                        + item.EyeColor + ";" + item.HairColor + ";" + item.SpecialFeatures + ";" + item.Citizenship + ";"
                        + item.DateOfBirth + ";" + item.PlaceOfBirth + ";" + item.LastAccomodation + ";"
                        + item.Languages + ";" + item.CriminalJob + ";" + item.LastAffair + ";" + item.IsInBand + ";" + bandName + ";" + "\n";
                }
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(str);
                    writer.Close();
                }
            }
        }

        // метод зчитування даних картотеки, збережених у текстовому файлі
        /*public static void ReadFromFile(string p)
        {
            string path = p;

            using (StreamReader reader = new StreamReader(path))
            {
                string? str;
                
                while ((str = reader.ReadLine()) != null) 
                {
                    string[] propArr = str.Split(";", StringSplitOptions.RemoveEmptyEntries);
                    string name = propArr[0];
                    string surname = propArr[1];
                    string nickname = propArr[2];
                    int height = int.Parse(propArr[3]);
                    string eyeColor = propArr[4];
                    string hairColor = propArr[5];
                    string specialFeatures = propArr[6];
                    string citizenship = propArr[7];
                    string dateOfBirth = propArr[8];
                    string placeOfBirth = propArr[9];
                    string lastAcc = propArr[10];
                    string languages = propArr[11];
                    string job = propArr[12];
                    string lastAffair = propArr[13];
                    bool isInBand = Convert.ToBoolean(propArr[14]);
                    string bandName = propArr[15];

                    if (bandName == "-1")
                    {
                        bandName = "";
                    }

                    Criminal criminal = new Criminal(name, surname, nickname, height, eyeColor, hairColor, specialFeatures, citizenship, dateOfBirth, placeOfBirth, lastAcc, languages, job, lastAffair, isInBand, bandName);

                    if (p == "criminals.txt")
                        Criminals.Add(criminal);
                    else if (p == "archived.txt")
                    {
                        Archived.Add(criminal);
                        if (criminal.IsInBand)
                        {
                            foreach (var band in AllBands)
                            {
                                if (band.BandName == criminal.BandName)
                                {
                                    band.members.Remove(criminal);
                                }
                            }
                        }
                    }
                }
                reader.Close();
            }
            
        }*/

        // метод пошуку справи в картотеці (за запитом незрозуміло, чи злочинець є членом банди)
        public static void SearchNotinBand(Criminal prototype, int[] hRange) 
        {
            if (Criminals.Count==0)
            {
                return;
            }
            foreach (Criminal criminal in Criminals)
            {
                    var processedProto = (Criminal)prototype.Clone();

                    MakeNullsEquivalent(processedProto, criminal);
                    if (CompareCriminals(processedProto, criminal, hRange))
                    {
                        CriminalsFoundByRequest.Add(criminal);
                    }
            }
        }

        //метод додавання детектива до картотеки
        public static void AddDetective(string name, string surname, int badge_num, int department_id, string reg_date, string last_visit_date, string pass, int type_id, out int id) 
        {
            SqlCommand command = new SqlCommand($"INSERT INTO Detectives(first_name, surname, badge_num, department_id, reg_date, last_visit_date, pass, type_id) VALUES('{name}', '{surname}', {badge_num}, {department_id},  '{reg_date}', '{last_visit_date}', '{pass}', {type_id});", PoliceCardIndex.GetSqlConnection());
            PoliceCardIndex.OpenConnection();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT detective_id FROM Detectives WHERE badge_num = {badge_num};", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            id = (int)table.Rows[0]["detective_id"];

        }

        public static int FindIDbyBadge(int badge_num)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT detective_id FROM Detectives WHERE badge_num = {badge_num};", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            int id = (int)table.Rows[0]["detective_id"];
            return id;
        }


        // метод пошуку справи в картотеці (злочинець є членом банди)
        public static void SearchInBand(Criminal prototype, int[] hRange)
        {
            if (Criminals.Count == 0 || AllBands.Count == 0)
            {
                return;
            }
            if (prototype.BandName == "")
            {
                AllBands.RemoveAt(AllBands.Count-1);
                foreach (Criminal criminal in Criminals)
                {
                    if (criminal.IsInBand)
                    {
                        var processedProto = (Criminal)prototype.Clone();

                        MakeNullsEquivalent(processedProto, criminal);
                        if (CompareCriminals(processedProto, criminal, hRange))
                        {
                            CriminalsFoundByRequest.Add(criminal);
                        }
                    }
                }
                return;
            }
            foreach (CrimeBand band in AllBands)
            {
                if (prototype.BandName == band.BandName && band.members.Count!=0)
                {
                    band.members.Remove(prototype);
                    foreach (Criminal criminal in band.members)
                    {
                        var processedProto = (Criminal)prototype.Clone();
                        MakeNullsEquivalent(processedProto, criminal);

                        if (CompareCriminals(processedProto, criminal, hRange))
                        {
                            CriminalsFoundByRequest.Add(criminal);
                        }
                    }
                }
            }
        }

        // метод підготовки прототипу для пошуку справи в картотеці
        public static void MakeNullsEquivalent(Criminal prototype, Criminal criminal)
        { 
            var protoProps = prototype.GetType().GetProperties();
            var crimProps = criminal.GetType().GetProperties();
            int i = 0;
            foreach (var prop in crimProps)
            {
                 if (protoProps[i].GetValue(prototype)?.ToString() == "" || 
                    protoProps[i].GetValue(prototype)?.ToString() == "0" || 
                    protoProps[i].GetValue(prototype)?.ToString() == null)
                 { 
                    protoProps[i].SetValue(prototype, prop.GetValue(criminal));
                 }
                i++;
            }

        }

        // метод порівняння справ (прототипу та н-ного злочинця з картотеки)
        public static bool CompareCriminals(Criminal prototype, Criminal criminal, int[] hRange)
        {
            if (prototype.Name == criminal.Name &&
                prototype.Surname == criminal.Surname &&
                prototype.Nickname == criminal.Nickname &&
                prototype.EyeColor == criminal.EyeColor &&
                prototype.HairColor == criminal.HairColor &&
                prototype.SpecialFeatures == criminal.SpecialFeatures &&
                prototype.Citizenship == criminal.Citizenship &&
                prototype.DateOfBirth == criminal.DateOfBirth &&
                prototype.PlaceOfBirth == criminal.PlaceOfBirth &&
                prototype.LastAccomodation == criminal.LastAccomodation &&
                prototype.CriminalJob == criminal.CriminalJob &&
                prototype.Languages == criminal.Languages &&
                prototype.LastAffair == criminal.LastAffair &&
                prototype.BandName == criminal.BandName)
            {
                if (hRange[0] == -1)
                    return true;
                else if (HeightEquals(criminal.Height, hRange))
                    return true;
            }
                    
            return false;
        }

        // метод порівняння зросту (для пошуку за зростом в діапазонах)
        public static bool HeightEquals(int crHeight, int[] range)
        {
            if (crHeight >= range[0] && crHeight <= range[1])
                return true;
            return false;
        }

        // метод перевірки унікальності особистого номера детектива
        public static bool IsUniqueDetectiveNumber(int personalNumber)
        {
            bool isUnique = true;

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT badge_num FROM Detectives;", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if ((int)table.Rows[i]["badge_num"] == personalNumber)
                {
                    isUnique = false;
                    break;
                }
            }

            return isUnique;
        }

        public static bool IsInDetectives(int num)
        {
            bool isIn = false;

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT badge_num FROM Detectives;", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if ((int)table.Rows[i]["badge_num"] == num)
                {
                    isIn = true;
                    break;
                }
            }

            return isIn;
        }

        // метод сортування переліку злочинців за ім'ям та прізвищем
        public static void SortByNames(List<Criminal> list)
        {
            list.Sort(delegate (Criminal x, Criminal y)
            {
                if (x.Name + x.Surname == null && y.Name + y.Surname == null) return 0;
                else if (x.Name + x.Surname == null) return -1;
                else if (y.Name + y.Surname == null) return 1;
                else return (x.Name + x.Surname).CompareTo(y.Name + y.Surname);
            });
        }

        // метод видалення анкети
        public static void DeleteAffair(Criminal processed)
        {
            CriminalsFoundByRequest.Remove(processed);
            Criminals.Remove(processed);

            if (processed.IsInBand)
            {
                foreach (var band in AllBands)
                {
                    if (band.BandName == processed.BandName)
                    {
                        band.members.Remove(processed);
                    }
                }
            }
        }

        // метод архівування анкети
        public static void ArchiveAffair(Criminal processed)
        {
            Archived.Add(processed);
            CriminalsFoundByRequest.Remove(processed);
            Criminals.Remove(processed);
            if (processed.IsInBand)
            {
                foreach (var band in AllBands)
                {
                    if (band.BandName == processed.BandName)
                    {
                        band.members.Remove(processed);
                    }
                }
            }
        }

        // метод розархівування справи
        public static void Unarchive(Criminal criminal)
        {
            PoliceCardIndex.AddCriminal(criminal);
            if (criminal.IsInBand)
            {
                if (PoliceCardIndex.AllBands != null)
                {
                    foreach (CrimeBand band in PoliceCardIndex.AllBands)
                    {
                        if (band.BandName == criminal.BandName)
                        {
                            band.AddMember(criminal);
                        }
                    }
                }

                else
                {
                    CrimeBand newBand = new CrimeBand(criminal.BandName, new List<Criminal> { criminal });
                }
            }
            PoliceCardIndex.Archived.Remove(criminal);
        }

        // метод формування списку унікальних справ для подальшого збереження у файл
        public static void FormListToPrint(Criminal criminal, out bool isIncluded)
        {
            isIncluded = false;
            if (FoundToWrite.Count == 0)
            {
                FoundToWrite.Add(criminal);
                isIncluded = true;
                return;
            }
            if (FoundToWrite.Contains(criminal))
            {
                FoundToWrite.Remove(criminal);
                return;
            }
            else {
                FoundToWrite.Add(criminal);
                SortByNames(FoundToWrite);
                isIncluded = true;
                return;
            }
        }

        // метод запису переліку справ у витяг з картотеки
        public static void WriteResults()
        {
            string path = "";
            string header = "\n\t\t\t\t\tВитяг з картотеки Інтерполу\n\n";
            string str = "";
            string band = "";
            string footer = "\n\n\t\t\tДата та час звернення до картотеки Інтерполу: " + DateTime.Now;
            var dialog = new Microsoft.Win32.SaveFileDialog();

            dialog.FileName = "results"; 
            dialog.DefaultExt = ".pdf"; 
            dialog.Filter = "Text documents (.txt)|*.txt"; 

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                path = dialog.FileName;
            }
            foreach (Criminal criminal in FoundToWrite)
            {
                if (criminal.IsInBand)
                {
                    band = "\nЗлочинець є членом банди " + criminal.BandName;
                }
                str += (FoundToWrite.IndexOf(criminal) + 1) + ". "
                    + criminal.ToString() + "\nДата народження: " + criminal.DateOfBirth
                    + "\nЗріст: " + criminal.Height
                    + "\nКолір очей: " + criminal.EyeColor
                    + "\tКолір волосся: " + criminal.HairColor
                    + "\nОсобливі прикмети: " + criminal.SpecialFeatures
                    + "\nГромадянство: " + criminal.Citizenship
                    + "\nМісце народження: " + criminal.PlaceOfBirth
                    + "\tМісце останнього проживання: " + criminal.LastAccomodation
                    + "\nВолодіє мовами: " + criminal.Languages
                    + "\nКримінальний фах: " + criminal.CriminalJob
                    + "\nОстання справа: " + criminal.LastAffair
                    + band + "\n\n";
            }
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(header + str + footer);
                writer.Close();
            }
        }

    }
}
