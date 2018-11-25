using System;
using System.Collections.Generic;
using System.Text;

namespace Seed
{
    public struct Door
    {
        public readonly DoorState isOpen;
        public readonly Location location;

        public Door(DoorState state, Location location)
        {
            this.isOpen = state;
            this.location = location;
        }

    }
}
