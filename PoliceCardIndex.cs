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

        // перелік первинних ключів анкет злочинців, знайдених за запитом
        public static List<int>? IdxsFoundByRequest;

        // перелік первинних ключів анкет - справ злочинців в архіві
        public static List<int>? IdxsArchived;

        // перелік злочинців, справи яких збережено в архіві
        public static List<Criminal>? Archived;

        // перелік злочинців, справи яких треба занести до файлу-витягу
        public static List<int>? FoundToWrite;

        public static bool IsDetective = false;

        public static int DetectiveID = 0;

        // конструктор без параметрів
        static PoliceCardIndex()
        {
            AllBands = new List<CrimeBand>();
            Criminals = new List<Criminal>();
            IdxsFoundByRequest = new List<int>();
            CriminalsFoundByRequest = new List<Criminal>();
            Archived = new List<Criminal>();
            FoundToWrite = new List<int>();
            IdxsArchived = new List<int>();
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

        public static void AddCriminal(string first_name, string surname, string nickname, string add_date, int height, string eye_color, string hair_color, string special_feature, string citizenship, string birth_date, string birth_place, string last_accomodation, string criminal_job, bool is_in_band, out int id, int band_id = -1)
        {
            string query;

            if (is_in_band)
            {
                query = $"INSERT INTO Criminals(first_name, surname, nickname, add_date, height, eye_color, hair_color, special_feature, citizenship, birth_date, birth_place, last_accomodation, criminal_job, is_in_band, band_id, is_archived) VALUES('{first_name}', '{surname}', '{nickname}', '{add_date}',  {height}, '{eye_color}', '{hair_color}', '{special_feature}',  '{citizenship}', '{birth_date}', '{birth_place}', '{last_accomodation}', '{criminal_job}',  '{is_in_band}', {band_id}, '{false}');";
            }
            else 
            {
                query = $"INSERT INTO Criminals(first_name, surname, nickname, add_date, height, eye_color, hair_color, special_feature, citizenship, birth_date, birth_place, last_accomodation, criminal_job, is_in_band, is_archived) VALUES('{first_name}', '{surname}', '{nickname}', '{add_date}',  {height}, '{eye_color}', '{hair_color}', '{special_feature}',  '{citizenship}', '{birth_date}', '{birth_place}', '{last_accomodation}', '{criminal_job}',  '{is_in_band}', '{false}');";
            }

            SqlCommand command = new SqlCommand(query, PoliceCardIndex.GetSqlConnection());
            PoliceCardIndex.OpenConnection();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT criminal_id FROM Criminals WHERE surname = '{surname}' AND special_feature = '{special_feature}' AND add_date = '{add_date}' AND birth_date='{birth_date}' AND nickname = '{nickname}';", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            id = (int)table.Rows[0]["criminal_id"];
        }

        // метод додавання злочину
        public static void AddCrime(out int crime_id, int type_id, string title, string commit_date, int detective_id = -1)
        {
            string query;
            
            if (detective_id > 0)
            {
                query = $"INSERT INTO Crimes(type_id, title, commit_date, detective_id) VALUES({type_id}, '{title}', '{commit_date}', {detective_id});";
            }
            else
            {
                query = $"INSERT INTO Crimes(type_id, title, commit_date) VALUES({type_id}, '{title}', '{commit_date}');";
            }

            SqlCommand command = new SqlCommand(query, PoliceCardIndex.GetSqlConnection());
            PoliceCardIndex.OpenConnection();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT crime_id FROM Crimes WHERE type_id = {type_id} AND title = '{title}' AND commit_date = '{commit_date}';", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            crime_id = (int)table.Rows[0]["crime_id"];
        }

        // метод додавання учасника злочину
        public static void AddParticipant(int criminal_id, int crime_id, string crime_role)
        {
            string query;

            query = $"INSERT INTO Participants(criminal_id, crime_id, crime_role) VALUES({criminal_id}, {crime_id}, '{crime_role}')";
            
            SqlCommand command = new SqlCommand(query, PoliceCardIndex.GetSqlConnection());
            PoliceCardIndex.OpenConnection();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();
        }

        //метод вибору детектива за ІD
        public static void SelectDetectiveProps(int id, out string name, out string surname, out int badge_num, out int department_id, out string reg_date, out string last_visit_date, out string pass, out int type_id, out string department_name, out string affair_type)
        {
            /*SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM(SELECT d.*, dep.department_name, a.affair_type FROM Detectives d, Departments dep, Affair_Types a WHERE d.department_id = dep.department_id AND d.type_id = a.type_id) s WHERE detective_id = {id}; ", PoliceCardIndex.GetSqlConnection());*/
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM Detectives WHERE detective_id = {id}; ", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            name = table.Rows[0]["first_name"].ToString();
            surname = table.Rows[0]["surname"].ToString();
            badge_num = (int)table.Rows[0]["badge_num"];

            var di = table.Rows[0]["department_id"];
            try
            {
                department_id = (int)di;
            }
            catch (Exception ex)
            {
                department_id = 0;
            }

            var dt = table.Rows[0]["type_id"];
            try
            {
                type_id = (int)dt;
            }
            catch (Exception ex)
            {
                type_id = 0;
            }

            reg_date = table.Rows[0]["reg_date"].ToString();
            last_visit_date = table.Rows[0]["last_visit_date"].ToString();
            pass = table.Rows[0]["pass"].ToString();
            //type_id = (int)table.Rows[0]["type_id"];

            if (department_id != 0)
            {
                SqlDataAdapter adapterD = new SqlDataAdapter($"SELECT department_name FROM Departments WHERE department_id = {department_id}; ", PoliceCardIndex.GetSqlConnection());
                DataTable tableD = new DataTable();
                adapterD.Fill(tableD);
                department_name = tableD.Rows[0]["department_name"].ToString();
            }

            else
            {
                department_name = "";
            }

            if (type_id != 0)
            {
                SqlDataAdapter adapterT = new SqlDataAdapter($"SELECT affair_type FROM Affair_Types WHERE type_id = {type_id}; ", PoliceCardIndex.GetSqlConnection());
                DataTable tableT = new DataTable();
                adapterT.Fill(tableT);
                affair_type = tableT.Rows[0]["affair_type"].ToString();
            }

            else
            {
                affair_type = "";
            }

            //department_name = table.Rows[0]["department_name"].ToString();
            //affair_type = table.Rows[0]["affair_type"].ToString();

        }

        public static void SelectCriminalProps(int id, out string name, out string surname, out string nickname, out string add_date, out int height, out string eye_color, out string hair_color, out string special_feature, out string citizenship, out string birth_date, out string birth_place, out string last_accomodation, out string criminal_job, out bool is_in_band, out int band_id, out string band_name)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM Criminals WHERE criminal_id = {id}; ", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);

            name = table.Rows[0]["first_name"].ToString();
            surname = table.Rows[0]["surname"].ToString();
            nickname = table.Rows[0]["nickname"].ToString();
            add_date = table.Rows[0]["add_date"].ToString();
            height = (int)table.Rows[0]["height"];
            eye_color = table.Rows[0]["eye_color"].ToString();
            hair_color = table.Rows[0]["hair_color"].ToString();
            special_feature = table.Rows[0]["special_feature"].ToString();
            citizenship = table.Rows[0]["citizenship"].ToString();
            birth_date = table.Rows[0]["birth_date"].ToString();
            birth_place = table.Rows[0]["birth_place"].ToString();
            last_accomodation = table.Rows[0]["last_accomodation"].ToString();
            criminal_job = table.Rows[0]["criminal_job"].ToString();
            is_in_band = (bool)table.Rows[0]["is_in_band"];
            var bi = table.Rows[0]["band_id"];
            try
            {
                band_id = (int)bi;
            }
            catch (Exception ex)
            {
                band_id = 0;
            }

            if (band_id != 0)
            {
                SqlDataAdapter adapterB = new SqlDataAdapter($"SELECT band_name FROM Bands WHERE band_id = {band_id}; ", PoliceCardIndex.GetSqlConnection());
                DataTable tableB = new DataTable();
                adapterB.Fill(tableB);
                band_name = tableB.Rows[0]["band_name"].ToString();
            }

            else
            {
                band_name = "";
            }
        }

        public static string GetDetSpeciality()
        {
            int type_id;
            string affair_type;

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM Detectives WHERE detective_id = {PoliceCardIndex.DetectiveID}; ", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);

            var dt = table.Rows[0]["type_id"];
            try
            {
                type_id = (int)dt;
            }
            catch (Exception ex)
            {
                type_id = 0;
            }

            if (type_id != 0)
            {
                SqlDataAdapter adapterT = new SqlDataAdapter($"SELECT affair_type FROM Affair_Types WHERE type_id = {type_id}; ", PoliceCardIndex.GetSqlConnection());
                DataTable tableT = new DataTable();
                adapterT.Fill(tableT);
                affair_type = tableT.Rows[0]["affair_type"].ToString();
            }

            else
            {
                affair_type = "";
            }

            return affair_type;
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
        public static void DeleteAffair(int id)
        {
            IdxsFoundByRequest.Remove(id);

            SqlCommand commandDelP = new SqlCommand($"DELETE FROM Participants WHERE criminal_id = {id};", PoliceCardIndex.GetSqlConnection());
            SqlCommand commandDelC = new SqlCommand($"DELETE FROM Criminals WHERE criminal_id = {id};", PoliceCardIndex.GetSqlConnection());

            PoliceCardIndex.OpenConnection();
            commandDelP.ExecuteNonQuery();
            commandDelC.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();
        }

        // метод архівування анкети
        public static void ArchiveAffair(int id)
        {
            DateTime aDate = DateTime.Now;
            string aDateSqlFormatted = aDate.ToString("yyyy-MM-dd");

            SqlCommand command = new SqlCommand($"UPDATE Criminals SET is_archived='{true}', archivation_date='{aDateSqlFormatted}' WHERE criminal_id = {id};", PoliceCardIndex.GetSqlConnection());
            PoliceCardIndex.OpenConnection();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();
        }

        // метод розархівування справи
        public static void Unarchive(int id)
        {
            SqlCommand command = new SqlCommand($"UPDATE Criminals SET is_archived='{false}', archivation_date = NULL WHERE criminal_id = {id};", PoliceCardIndex.GetSqlConnection());
            PoliceCardIndex.OpenConnection();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();
        }

        // метод формування списку унікальних справ для подальшого збереження у файл
        public static void FormListToPrint(int id, out bool isIncluded)
        {
            isIncluded = false;
            if (FoundToWrite.Count == 0)
            {
                FoundToWrite.Add(id);
                isIncluded = true;
                return;
            }
            if (FoundToWrite.Contains(id))
            {
                FoundToWrite.Remove(id);
                return;
            }
            else {
                FoundToWrite.Add(id);
               // SortByNames(FoundToWrite);
                isIncluded = true;
                return;
            }
        }

        // метод запису переліку справ у витяг з картотеки
        public static void WriteResults()
        {
            int i = 1;
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
            foreach (int id in FoundToWrite)
            {
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT c.*, band_name, c.band_id, crime_role, cr.crime_id, cr.type_id, t.affair_type, title, commit_date, cr.detective_id, CONCAT(d.first_name, ' ', d.surname) as d_name FROM((((Criminals c LEFT JOIN Bands b ON(c.band_id = b.band_id)) LEFT JOIN Participants p ON(c.criminal_id = p.criminal_id)) LEFT JOIN Crimes cr ON(cr.crime_id = p.crime_id)) LEFT JOIN Affair_Types t ON(cr.type_id = t.type_id)) LEFT JOIN Detectives d ON(cr.detective_id = d.detective_id) WHERE c.criminal_id = {id }; ", PoliceCardIndex.GetSqlConnection());
                DataTable tableCr = new DataTable();
                adapter.Fill(tableCr);

                if ((bool)tableCr.Rows[0]["is_in_band"])
                {
                    band = "\nЗлочинець є членом банди " + tableCr.Rows[0]["band_name"] ;
                }
                str += "\n\n" + (i) + ". "
                    + tableCr.Rows[0]["first_name"] + " " 
                    + tableCr.Rows[0]["surname"] + " (" 
                    + tableCr.Rows[0]["nickname"] + ") " 
                    + "\n  -- Особисті дані --"
                    + "\nДата народження: " + ((DateTime)tableCr.Rows[0]["birth_date"]).ToString("dd.MM.yyyy")
                    + "\nЗріст: " + tableCr.Rows[0]["height"]
                    + "\nКолір очей: " + tableCr.Rows[0]["eye_color"]
                    + "\tКолір волосся: " + tableCr.Rows[0]["hair_color"]
                    + "\nОсобливі прикмети: " + tableCr.Rows[0]["special_feature"]
                    + "\nГромадянство: " + tableCr.Rows[0]["citizenship"]
                    + "\nМісце народження: " + tableCr.Rows[0]["birth_place"]
                    + "\tМісце останнього проживання: " + tableCr.Rows[0]["last_accomodation"]
                    + "\nКримінальний фах: " + tableCr.Rows[0]["criminal_job"]
                    + "\nДата внесення анкети до картотеки: " + ((DateTime)tableCr.Rows[0]["add_date"]).ToString("dd.MM.yyyy")
                    + band + "\n"
                    + "\n  -- Відомості про злочини --";
                //тут додати перевірки на null!
                foreach (DataRow dr in tableCr.Rows)
                {
                    if (Convert.ToString(dr["title"]) != "")
                    {
                        str += "\nНазва злочину: " + dr["title"];
                    }
                    if (Convert.ToString(dr["affair_type"]) != "")
                    {
                        str += "\nТип злочину: " + dr["affair_type"];
                    }
                    if (Convert.ToString(dr["crime_role"]) != "")
                    {
                        str += "\nРоль особи в злочині: " + dr["crime_role"];
                    }
                    if (Convert.ToString(dr["affair_type"]) != "")
                    {
                        str += "\nДата та час скоєння злочину: " + ((DateTime)dr["commit_date"]).ToString("dd.MM.yyyy  hh:mm");
                    }
                    string name = Convert.ToString(dr["d_name"]);
                    if (name != " ")
                    {
                        str += "\nДетектив, відповідальний за розслідування: " + dr["d_name"] + "\n";
                    }
                }

                i++;
            }
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(header + str + footer);
                writer.Close();
            }
        }

        public static void PrintDetectiveInfo()
        {
            
            string path = "";
            string header = "\n\t\t\t\t\tОСОБОВА СПРАВА ДЕТЕКТИВА ІНТЕРПОЛУ\n\n";
            string str = "";
            string crimes = "";
            string dep = "";
            string footer = "\n\n\t\t\t\tВідомості актуальні на  " + DateTime.Now;
            var dialog = new Microsoft.Win32.SaveFileDialog();

            dialog.FileName = "detective_pers_affair";
            dialog.DefaultExt = ".pdf";
            dialog.Filter = "Text documents (.txt)|*.txt";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                path = dialog.FileName;
            }

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT d.*, dep.department_name, a.affair_type, dep.post_index FROM Detectives d, Departments dep, Affair_Types a WHERE detective_id = {PoliceCardIndex.DetectiveID} AND d.department_id = dep.department_id AND d.type_id = a.type_id; ", PoliceCardIndex.GetSqlConnection());
            DataTable tableDet = new DataTable();
            adapter.Fill(tableDet);

            int department_id;
            int type_id;
            string department_name = "";
            string affair_type = "";
            int p_idx;
            string post_index = "-";

            var di = tableDet.Rows[0]["department_id"];
            try
            {
                department_id = (int)di;
            }
            catch (Exception ex)
            {
                department_id = 0;
            }

            var dt = tableDet.Rows[0]["type_id"];
            try
            {
                type_id = (int)dt;
            }
            catch (Exception ex)
            {
                type_id = 0;
            }

            var pi = tableDet.Rows[0]["post_index"];
            try
            {
                p_idx = (int)pi;
            }
            catch (Exception ex)
            {
                p_idx = 0;
            }


            if (department_id != 0)
            {
                department_name = tableDet.Rows[0]["department_name"].ToString();
            }

            if (department_name.Length < 1 || department_id == 0)
                department_name = "-";


            if (type_id != 0)
            {
                affair_type = tableDet.Rows[0]["affair_type"].ToString();
            }

            if (affair_type.Length < 1 || type_id == 0)
                affair_type = "-";

            if (p_idx != 0)
            {
                post_index = tableDet.Rows[0]["post_index"].ToString();
            }

            if (post_index.Length < 1 || p_idx == 0)
                post_index = "-";


            str += "\n\n\tДетектив "
                + tableDet.Rows[0]["first_name"] + " "
                + tableDet.Rows[0]["surname"] 
                + "\n  -- Особисті дані --" 
                + "\nНомер поліцейського значка: " + tableDet.Rows[0]["badge_num"].ToString()
                + "\nВідділ Інтерполу: " + department_name
                + "\tПоштовий індекс відділу: " + post_index
                + "\nСпеціалізація детектива: " + affair_type
                + "\n\nДата реєстрації в системі Інтерполу: " + ((DateTime)tableDet.Rows[0]["reg_date"]).ToString("dd.MM.yyyy")
                + "\nОстанній вхід до системи: " + ((DateTime)tableDet.Rows[0]["last_visit_date"]).ToString("dd.MM.yyyy hh:mm:ss")
                + "\n\n\n  -- Відомості про злочини --";


            SqlDataAdapter adapterCr = new SqlDataAdapter($"SELECT c.*, COUNT(p.participant_id) as p_count FROM Crimes c, Participants p WHERE detective_id = {PoliceCardIndex.DetectiveID} AND c.crime_id = p.crime_id GROUP BY c.crime_id, type_id, title, commit_date, detective_id, is_unseen; ", PoliceCardIndex.GetSqlConnection());
            DataTable tableCr = new DataTable();
            adapterCr.Fill(tableCr);

            foreach (DataRow dr in tableCr.Rows)
            {
                if (Convert.ToString(dr["title"]) != "")
                {
                    crimes += "\nНазва злочину: " + dr["title"];
                }

                crimes += "\nТип злочину: " + affair_type;
                
                if (Convert.ToString(dr["p_count"]) != "")
                {
                    crimes += "\nКількість виявлених учасників: " + dr["p_count"];
                }
                if (Convert.ToString(dr["commit_date"]) != "")
                {
                    crimes += "\nДата та час скоєння злочину: " + ((DateTime)dr["commit_date"]).ToString("dd.MM.yyyy  hh:mm") + "\n\n";
                }
            }

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(header + str + crimes + footer);
                writer.Close();
            }

        }

    }
}
