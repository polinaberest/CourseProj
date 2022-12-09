using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CourseProj
{
    // клас, що описує злочинця
    internal class Criminal:ICloneable
    {
        public bool IsInBand;
        private string name;
        private string surname;
        private string nickname;
        private int height;
        private string eyeColor;
        private string hairColor;
        private string specialFeatures;
        private string citizenship;
        private string dateOfBirth;
        private string placeOfBirth;
        private string lastAccomodation;
        private string languages;
        private string criminalJob;
        private string affairType;
        //private int affairTypeID;
        private string lastAffair;
        private string? bandName;
        private CrimeBand band;

        // конструктор без параметрів
        public Criminal()
        {

        }

        // конструктор з параметрами
        public Criminal(
            string name,
            string surname,
            string nickname,
            int height,
            string eyeColor,
            string hairColor,
            string specialFeatures,
            string citizenship, 
            string dateOfBirth,
            string placeOfBirth,
            string lastAccomodation,
            string languages,
            string criminalJob,
            string affairType,
            string lastAffair,
            bool isInBand,
            string? bandName)
        {
            Name = name;
            Surname = surname;
            Nickname = nickname;
            Height = height;
            EyeColor = eyeColor;
            HairColor = hairColor;
            SpecialFeatures = specialFeatures;
            Citizenship = citizenship;
            DateOfBirth = dateOfBirth;
            PlaceOfBirth = placeOfBirth;
            LastAccomodation = lastAccomodation;
            Languages = languages;
            CriminalJob = criminalJob;
            AffairType = affairType;
            LastAffair = lastAffair;
            if (isInBand)
            {
                IsInBand = true;

                BandName = CrimeBand.AdoptBandName(bandName);
                //band = SearchBand(BandName);
            }
            
        }

        // властивість 'ім'я злочинця'
        public string Name
        {
            get { return name; }
            set
            {
                if (value.Length >= 1)
                    name = (value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower()).Trim();
                else
                    name = "";
            }
        }

        // властивість 'прізвище злочинця'
        public string Surname
        {
            get { return surname; }
            set
            {
                if (value.Length >= 1)
                    surname = (value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower()).Trim();
                else
                    surname = "";
            }
        }

        // властивість 'позивний злочинця'
        public string Nickname
        {
            get { return nickname; }
            set
            {
                if (value.Length >= 1)
                    nickname = (value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower()).Trim();
                else
                    nickname = "";
            }
        }

        // властивість 'зріст злочинця'
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        // властивість 'колір очей злочинця'
        public string EyeColor
        {
            get { return eyeColor; }
            set { eyeColor = value; }
        }

        // властивість 'колір волосся злочинця'
        public string HairColor
        {
            get { return hairColor; }
            set { hairColor = value; }
        }

        // властивість 'особливі прикмети злочинця'
        public string SpecialFeatures
        {
            get { return specialFeatures; }
            set { specialFeatures = value; }
        }

        // властивість 'громадянство злочинця'
        public string Citizenship
        {
            get { return citizenship; }
            set { citizenship = value; }
        }

        // властивість 'дата народження злочинця'
        public string DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }

        // властивість 'місце народження злочинця'
        public string PlaceOfBirth
        {
            get { return placeOfBirth; }
            set { placeOfBirth = value; }
        }

        // властивість 'останнє місце проживання злочинця'
        public string LastAccomodation
        {
            get { return lastAccomodation; }
            set { lastAccomodation = value; }
        }

        // властивість 'мови, якими володіє злочинець'
        public string Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        // властивість 'кримінальний фах злочинця'
        public string CriminalJob
        {
            get { return criminalJob; }
            set { criminalJob = value; }
        }

        // властивість 'вид останньої справи злочинця'
        public string AffairType
        {
            get { return affairType; }
            set { affairType = value; }
        }

        // властивість 'остання справа злочинця'
        public string LastAffair
        {
            get { return lastAffair; }
            set { lastAffair = value; }
        }

        // властивість 'банда, до якої належить злочинець'
        public CrimeBand Band
        {
            get { return band; }
            set { band = value; }
        }

        // властивість 'назва банди, до якої належить злочинець'
        public string BandName
        {
            get { return bandName; }
            set { bandName = value; }
        }

        // методи:

        // копіювання об'єкта злочинця
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        // перегрузка методу ToString() для виведення ПІ та позивного
        public override string ToString()
        {
            return this.Name + " " + this.Surname + " (" + this.Nickname + ") ";
        }

        // логіка визначення банди (за назвою), членом якої є злочинець
        /*private CrimeBand SearchBand(string bandName)
        {
            if(PoliceCardIndex.AllBands != null)
            { 
                foreach (CrimeBand band in PoliceCardIndex.AllBands)
                {
                    if (band.BandName == bandName)
                    {
                        band.AddMember(this);
                        return band;
                    }
                }
            }

            CrimeBand newBand = new CrimeBand(bandName, new List<Criminal> { this });
            return newBand;
        }    */
    }
}
