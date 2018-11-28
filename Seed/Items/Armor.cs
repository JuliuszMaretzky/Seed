using System;
using System.Collections.Generic;
using System.Text;
using Seed.Locations;

namespace Seed.Items
{
    public class Armor : Item
    {
        public uint Toughness { get; private set; }
        public ArmorType Type { get; private set; }

        public Armor(string name = "Coś, co wygląda na mocne", string description = "czeka na ziemi.",
            uint weight = 1, uint toughness = 1, ArmorType type = ArmorType.Shield, Location location = null) :
            base(name, weight, description, location)
        {
            this.Toughness = toughness;
            this.Type = type;
        }
    }
}
