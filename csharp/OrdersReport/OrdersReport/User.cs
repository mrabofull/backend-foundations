using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersReport
{
    internal class User
    {
        private int id;
        private string name;
        private string city;

        public User (int id, string name, string city)
        {
            this.id = id;
            this.name = name;
            this.city = city;
        }

        public int getId() { return id; }
        public string getName() { return name; }
        public string getCity() { return city; }

        public override string ToString()
        {
            return $"Id: {id}, name: {name}, city: {city}";
        }
    }
}
