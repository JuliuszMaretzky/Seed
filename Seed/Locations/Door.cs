using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Locations
{
    public struct Door
    {
        public readonly DoorState DoorState;
        public readonly Location Location;

        public Door(DoorState state, Location location)
        {
            this.DoorState = state;
            this.Location = location;
        }

    }
}
