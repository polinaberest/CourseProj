using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProj
{
    // клас злочинного угруповання
    internal class CrimeBand
    {
        // перелік членів угруповання
        public List<Criminal> members;

        // конструктор класу кримінальної банди
        public CrimeBand(string bandName, List<Criminal> members)
        {
            BandName = bandName;
            this.members = members;
            PoliceCardIndex.AddBand(this);
        }

        // назва угруповання
        public string BandName { get; set; }

        // метод виведення всіх членів банди
        public string ShowMembers()
        {
            string str = "";
            foreach (var member in members)
            {
                str += member.Name + " " + member.Surname + "\n";
            }
            return str; 
        }

        // метод додавання нового члена до банди
        public void AddMember(Criminal criminal) 
        {
            members.Add(criminal);
        }

        // метод видалення особи з переліку членів банди
        public void RemoveCriminal(Criminal criminal)
        {
            members.Remove(criminal);
        }

        // логіка підведення під шаблон назви банди (для пошуку за назвою)
        public static string AdoptBandName(string bandName)
        {
            bandName = bandName.Replace('-', ' ');
            string[] wordArr = bandName.Split(" ");
            if (bandName != "" && bandName != null)
            {
                for (int i = 0; i < wordArr.Length; i++)
                {
                    wordArr[i] = wordArr[i].Trim();
                    wordArr[i] = (wordArr[i].Substring(0, 1).ToUpper() + wordArr[i].Substring(1).ToLower()).Trim();
                }
            }
            bandName = String.Join(" ", wordArr);
            return bandName;
        }
    }
}
