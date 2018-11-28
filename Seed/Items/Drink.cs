using System;
using System.Collections.Generic;
using System.Text;

namespace Seed
{
    public class Drink : Item
    {
        public uint RestoredEnergy { get; private set; }

        public Drink(string name = "Pitku", string description = "czeka na wypitku.", uint weight = 0,
            uint restoredEnergy = 1, Location location = null) : base(name, weight, description, location)
        {
            this.RestoredEnergy = restoredEnergy;
        }
    }
}
