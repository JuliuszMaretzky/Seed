namespace Seed.Locations
{
    public struct Door
    {
        public readonly DoorState doorState;
        public readonly Location location;

        public bool IsOpen => doorState == DoorState.Open;

        public Door(DoorState state, Location location)
        {
            this.doorState = state;
            this.location = location;
        }

    }
}
