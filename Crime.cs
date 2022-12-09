using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProj
{
    internal class Crime
    {
        public string Title { get; set; }
        public int CrimeNumber { get; set; }

        public Crime(string title, int count)
        {
            Title = title;
            CrimeNumber = count;
        }
    }
}
