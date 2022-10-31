using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProj
{
    internal class Detective
    {
        public int DetectiveId { get; set; }

        private string name;

        private string surname;

        private string password;

        public int BadgeNumber { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int? AffairId { get; set; }

        public int DepartmentId { get; set; }

        public Detective(string name, string surname, string password, int badgeNumber, DateTime regDate, int depId)
        {
            this.name = name;
            this.surname = surname;
            this.password = password;
            BadgeNumber = badgeNumber;
            RegistrationDate = regDate;
            DepartmentId = depId;
        }


        // властивість 'ім'я детектива'
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

        // властивість 'прізвище детектива'
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

        // властивість 'пароль акаунту детектива'
        public string Password
        {
            get { return password; }
            set
            {
                if (value.Length >= 1)
                    password = value.Trim();
                else
                    password = "";
            }
        }

    }
}
