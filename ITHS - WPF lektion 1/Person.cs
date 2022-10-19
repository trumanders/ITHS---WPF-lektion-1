using System;
using System.Collections.Generic;
using System.Text;

namespace ITHS___WPF_lektion_1
{
    class Person
    {
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public string Email { get; set; }

        public Person(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
