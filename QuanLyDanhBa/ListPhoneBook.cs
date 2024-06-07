using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuanLyDanhBa
{
    public class ListPhoneBook
    {
        private static ListPhoneBook instance;
        private List<PhoneBook> listNumberPhone;

        public List<PhoneBook> ListNumberPhone
        {
            get => listNumberPhone;
            set => listNumberPhone = value;
        }

        private ListPhoneBook()
        {
            listNumberPhone = new List<PhoneBook>();
            
        }

        public static ListPhoneBook Instance
        {
            get
            {
                if (instance == null)
                    instance = new ListPhoneBook();
                return instance;
            }
        }

        
    }
}
