using System;
using System.Collections.Generic;
using System.Text;

namespace Seed
{
    public class Item
    {
        public string Name { get; protected set; }
        public uint Weight { get; protected set; }
        public string Description { get; protected set; }

        public Item(string name = "Jakiś syf", uint weight = 0, string description = "tu leży.",
            Location location = null)
        {
            this.Name = name;
            this.Weight = weight;
            this.Description = description;

            if (location != null)
            {
                location.Stack.Add(this);
            }
        }


    }
}
