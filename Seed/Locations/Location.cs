using System;
using System.Collections.Generic;
using System.Text;

namespace Seed
{
    public class Location
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Overview { get; private set; }
        public Door North { get; private set; }
        public Door South { get; private set; }
        public Door East { get; private set; }
        public Door West { get; private set; }
        public Door Up { get; private set; }
        public Door Down { get; private set; }
        public List<Item> Stack { get; private set; }
        public List<Character> CharactersInLocation { get; private set; }

        public Location(string name = "", string description = "",
            Direction parentDirection = Direction.Unknown, Location parentLocation = null,
            DoorState fromParentDoorState = DoorState.Open, DoorState fromHereDoorState = DoorState.Open
            , string overview = "Nie ma tu nic ciekawego.")
        {
            this.Name = name;
            this.Description = description;
            this.Overview = overview;
            Stack = new List<Item>();
            CharactersInLocation = new List<Character>();
            this.North = this.South = this.East = this.West = this.Up = this.Down =
                    new Door(DoorState.Closed, null);

            if (!(parentLocation == null || parentDirection == Direction.Unknown))
            {
                switch (parentDirection)
                {
                    case Direction.North:
                        {
                            this.North = new Door(fromHereDoorState, parentLocation);
                            parentLocation.South = new Door(fromParentDoorState, this);
                            break;
                        }
                    case Direction.South:
                        {
                            this.South = new Door(fromHereDoorState, parentLocation);
                            parentLocation.North = new Door(fromParentDoorState, this);
                            break;
                        }
                    case Direction.East:
                        {
                            this.East = new Door(fromHereDoorState, parentLocation);
                            parentLocation.West = new Door(fromParentDoorState, this);
                            break;
                        }
                    case Direction.West:
                        {
                            this.West = new Door(fromHereDoorState, parentLocation);
                            parentLocation.East = new Door(fromParentDoorState, this);
                            break;
                        }
                    case Direction.Up:
                        {
                            this.Up = new Door(fromHereDoorState, parentLocation);
                            parentLocation.Down = new Door(fromParentDoorState, this);
                            break;
                        }
                    case Direction.Down:
                        {
                            this.Down = new Door(fromHereDoorState, parentLocation);
                            parentLocation.Up = new Door(fromParentDoorState, this);
                            break;
                        }
                    default:
                        break;

                }
            }
        }

        public void ChancheDoorState(Direction direction, DoorState wantedState)
        {
            switch (direction)
            {
                case Direction.North:
                    this.North = new Door(wantedState, this.North.location);
                    break;
                case Direction.South:
                    this.South = new Door(wantedState, this.South.location);
                    break;
                case Direction.East:
                    this.East = new Door(wantedState, this.East.location);
                    break;
                case Direction.West:
                    this.West = new Door(wantedState, this.West.location);
                    break;
                case Direction.Up:
                    this.Up = new Door(wantedState, this.Up.location);
                    break;
                case Direction.Down:
                    this.Down = new Door(wantedState, this.Down.location);
                    break;

            }
        }

        public void CreateDoor(Location targetLocation, Direction targetLocationDirection,
            DoorState fromHereDoorState, DoorState fromThereDoorState)
        {
            switch (targetLocationDirection)
            {
                case Direction.North:
                    {
                        this.North = new Door(fromHereDoorState, targetLocation);
                        targetLocation.South = new Door(fromThereDoorState, this);
                        break;
                    }
                case Direction.South:
                    {
                        this.South = new Door(fromHereDoorState, targetLocation);
                        targetLocation.North = new Door(fromThereDoorState, this);
                        break;
                    }
                case Direction.East:
                    {
                        this.East = new Door(fromHereDoorState, targetLocation);
                        targetLocation.West = new Door(fromThereDoorState, this);
                        break;
                    }
                case Direction.West:
                    {
                        this.West = new Door(fromHereDoorState, targetLocation);
                        targetLocation.East = new Door(fromThereDoorState, this);
                        break;
                    }
                case Direction.Up:
                    {
                        this.Up = new Door(fromHereDoorState, targetLocation);
                        targetLocation.Down = new Door(fromThereDoorState, this);
                        break;
                    }
                case Direction.Down:
                    {
                        this.Down = new Door(fromHereDoorState, targetLocation);
                        targetLocation.Up = new Door(fromThereDoorState, this);
                        break;
                    }
            }
        }



    }
}
