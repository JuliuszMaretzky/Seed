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
            IsAggressive = isAggressive;
            IsLazy = isLazy;
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
                    if (presentLocation.North.DoorState == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        presentLocation = presentLocation.North.Location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                case Direction.South:
                    if (presentLocation.South.DoorState == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        presentLocation = presentLocation.South.Location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                case Direction.East:
                    if (presentLocation.East.DoorState == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        presentLocation = presentLocation.East.Location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                case Direction.West:
                    if (presentLocation.West.DoorState == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        presentLocation = presentLocation.West.Location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                case Direction.Up:
                    if (presentLocation.Up.DoorState == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        presentLocation = presentLocation.Up.Location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
                case Direction.Down:
                    if (presentLocation.Down.DoorState == DoorState.Open)
                    {
                        presentLocation.CharactersInLocation.Remove(this);
                        presentLocation = presentLocation.Down.Location;
                        presentLocation.CharactersInLocation.Add(this);
                    }
                    break;
            }
        }

        public void Attack(Character character)
        {
            if (character is Player)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{Name} atakuje cię!");
                System.Threading.Thread.Sleep(500);

                if (new Random().Next(0, 2) > 0)
                {
                    character.HP -= (int)(Strength - character.Armor);
                }

                if (character.HP > 0)
                {
                    Battle.Fight((Player)character, this);
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
                Battle.Fight(this, character);
            }
        }

    }
}
