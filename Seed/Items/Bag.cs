using System;
using System.Collections.Generic;
using System.Text;
using Seed.Locations;

namespace Seed.Items
{
    public class Bag : Item
    {
        public uint CarryingCapacity;

        public Bag(string name = "Reklamówka", string description = "wala się po kątach.", uint weight = 0,
            uint capacity = 2, Location location = null) :
            base(name, weight, description, location)
        {
            this.CarryingCapacity = capacity;
        }
    }
}
