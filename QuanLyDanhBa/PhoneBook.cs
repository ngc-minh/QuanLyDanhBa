using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDanhBa
{
    public class PhoneBook
    {
        private string name;
        private string numberPhone;
        private string organization;
        private string note;

        public string Name { get => name; set => name = value; }
        public string NumberPhone { get => numberPhone; set => numberPhone = value; }
        public string Organization { get => organization; set => organization = value; }
        public string Note { get => note; set => note = value; }
        

        public PhoneBook(string name, string numberPhone, string organization, string note)
        {
            Name = name;
            NumberPhone = numberPhone;
            Organization = organization;
            Note = note;
        }
    }
}
