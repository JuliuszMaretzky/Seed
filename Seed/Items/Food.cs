using System;
using System.Collections.Generic;
using System.Text;
using Seed.Locations;

namespace Seed.Items
{
    public class Food : Item
    {
        public uint RestoredHP { get; private set; }

        public Food(string name = "Jedzonko", string description = "chce być zjedzone.", uint weight = 0,
            uint restoredHP = 1, Location location = null) :
            base(name, weight, description, location)
        {
            this.RestoredHP = restoredHP;
        }
    }
}
