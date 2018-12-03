using System;
using System.Collections.Generic;
using System.Text;
using Seed.Locations;

namespace Seed.Items
{
    public class Book : Item
    {
        public string SomeLettersOnPaper { get; private set; }

        public Book(string name = "Jakiś świstek", string description = "oczekuje na czytelnika.", uint weight = 0,
            string text = "Jakieś bazgroły.", Location location = null) : base(name, weight, description, location)
        {
            this.SomeLettersOnPaper = text;
        }


    }
}
