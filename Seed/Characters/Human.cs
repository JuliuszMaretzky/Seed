using System;
using Seed.Locations;
using Seed.Scenarios;

namespace Seed.Characters
{
    public class Human : Character, IMove, IAttack
    {
        public bool IsLazy { get; private set; }
        public bool IsAggressive { get; private set; }

        public Human(string name = "Jakiś człowiek", string description = "tu jest.",
            string overview = "Nic szczególnego.", int hp = 30, uint strength = 1, uint armor = 1,
            Location presentLocation = null,
            bool isLazy = true, bool isAggressive = false) :
            base(name, description, overview, hp, strength, armor, presentLocation)
        {
            this.IsAggressive = isAggressive;
            this.IsLazy = isLazy;
        }

        public void Move(Direction direction)
        {
            if (direction == Direction.Unknown)
            {
                if (new Random().Next(0, 2) == 1)
                {
                    var directions = Enum.GetValues(typeof(Direction));
                    direction = (Direction)directions.GetValue(new Random().Next(directions.Length));
                }
            }

            switch (direction)
            {
                case Direction.North:
                    if (presentLocation.North.isOpen == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        this.presentLocation = presentLocation.North.location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                case Direction.South:
                    if (presentLocation.South.isOpen == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        this.presentLocation = presentLocation.South.location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                case Direction.East:
                    if (presentLocation.East.isOpen == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        this.presentLocation = presentLocation.East.location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                case Direction.West:
                    if (presentLocation.West.isOpen == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        this.presentLocation = presentLocation.West.location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                case Direction.Up:
                    if (presentLocation.Up.isOpen == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        this.presentLocation = presentLocation.Up.location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                case Direction.Down:
                    if (presentLocation.Down.isOpen == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        this.presentLocation = presentLocation.Down.location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Attack(Character character, World world)
        {
            if (character is Player)
            {
                if (new Random().Next(0, 2) > 0)
                {
                    character.HP -= (int)(this.Strength - character.Armor);
                }

                if (character.HP > 0)
                {
                    Battle.Fight((Player)character, this, world);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("NIE ŻYJESZ!");
                    Console.Read();
                    Environment.Exit(0);
                }

            }
            else
            {
                Battle.Fight(this, character, world);
            }
        }

    }
}
