using System;
using System.Collections.Generic;
using System.Linq;
using Seed.Items;
using Seed.Locations;
using Seed.Scenarios;

namespace Seed.Characters
{
    public sealed class Player : Character
    {
        private int _Energy;
        public int Energy
        {
            get { return _Energy; }
            set
            {
                _Energy = value;
                if (_Energy < 0)
                {
                    _Energy = 0;
                }
                else if (_Energy > MaxEnergy)
                {
                    _Energy = MaxEnergy;
                }
            }
        }
        public int MaxEnergy { get; private set; }
        public uint CarryingCapacity { get; private set; }
        public uint Burden { get; private set; }
        public List<Item> WornItems { get; private set; }
        private Player FamiliarSpirit;
        private Location PrisonForFamiliars;

        private List<string> RestSamples = new List<string>()
            {
                "Drapiesz się po głowie.",
                "Wzdychasz do siebie.",
                "Rozglądasz się niespokojnie.",
                "Dumasz nad sensem istnienia.",
                "Nasłuchujesz szczurów.",
                "Marzysz o niebieskich migdałach.",
                "Puszczasz bąki.",
                "Bekasz z cicha."
            };

        public Player(string name = "Nullator", string description = "jest, ale... jak to możliwe?",
                string overview = "Wygląda zupełnie jak ty.", int hp = 50, int energy = 25, uint strength = 5,
                uint armor = 5, Location presentLocation = null, Player familiarSpirit = null) :
            base(name, description, overview, hp, strength, armor, presentLocation)
        {
            if (name == String.Empty)
                Name = "Bezimienny";
            else
                Name = name;

            Energy = MaxEnergy = energy;
            CarryingCapacity = (uint)strength * 3;
            WornItems = new List<Item>();
            FamiliarSpirit = familiarSpirit;

            Burden = 0;
            foreach (var item in Inventory)
                Burden += item.Weight;
            foreach (var item in WornItems)
                Burden += item.Weight;

            if (Burden > CarryingCapacity)
                throw new Exception("Za ciężko!");
        }

        public void Move(Direction direction)
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (Energy == 0)
            {
                Console.WriteLine("Nie pochodzisz sobie. Musisz odpocząć");
                return;
            }

            Energy -= 3;

            switch (direction)
            {
                case Direction.North:
                    {
                        if (presentLocation.North.DoorState != DoorState.Closed &&
                            presentLocation.North.Location != null)
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            presentLocation = presentLocation.North.Location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść.");
                        }
                    }
                    break;
                case Direction.South:
                    {
                        if (presentLocation.South.DoorState != DoorState.Closed &&
                            presentLocation.South.Location != null)
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            presentLocation = presentLocation.South.Location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść.");
                        }
                    }
                    break;
                case Direction.East:
                    {
                        if (presentLocation.East.DoorState != DoorState.Closed &&
                            presentLocation.East.Location != null)
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            presentLocation = presentLocation.East.Location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść.");
                        }
                    }
                    break;
                case Direction.West:
                    {
                        if (presentLocation.West.DoorState != DoorState.Closed &&
                            presentLocation.West.Location != null)
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            presentLocation = presentLocation.West.Location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść.");
                        }
                    }
                    break;
                case Direction.Up:
                    {
                        if (presentLocation.Up.DoorState != DoorState.Closed &&
                            presentLocation.Up.Location != null)
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            presentLocation = presentLocation.Up.Location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść.");
                        }
                    }
                    break;
                case Direction.Down:
                    {
                        if (presentLocation.Down.DoorState != DoorState.Closed &&
                            presentLocation.Down.Location != null)
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            presentLocation = presentLocation.Down.Location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść.");
                        }
                    }
                    break;
                default:
                    {
                        Console.WriteLine("Nie możesz tam iść.");
                    }
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public bool MoveFamiliar(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    {
                        if (presentLocation.North.DoorState != DoorState.Open) return false;
                        presentLocation = presentLocation.North.Location;
                        return true;

                    }
                case Direction.South:
                    {
                        if (presentLocation.South.DoorState != DoorState.Open) return false;
                        presentLocation = presentLocation.South.Location;
                        return true;

                    }
                case Direction.East:
                    {
                        if (presentLocation.East.DoorState != DoorState.Open) return false;
                        presentLocation = presentLocation.East.Location;
                        return true;

                    }
                case Direction.West:
                    {
                        if (presentLocation.West.DoorState != DoorState.Open) return false;
                        presentLocation = presentLocation.West.Location;
                        return true;

                    }
                case Direction.Up:
                    {
                        if (presentLocation.Up.DoorState != DoorState.Open) return false;
                        presentLocation = presentLocation.Up.Location;
                        return true;

                    }
                case Direction.Down:
                    {
                        if (presentLocation.Down.DoorState != DoorState.Open) return false;
                        presentLocation = presentLocation.Down.Location;
                        return true;

                    }
                default:
                    return false;
            }
        }

        public void PickUp(Item item)
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (!(Burden + item.Weight > CarryingCapacity))
            {
                Inventory.Add(item);
                presentLocation.Stack.Remove(item);
                Burden += item.Weight;
                Console.WriteLine($"Podnosisz {item.Name}.");
            }
            else
            {
                Console.WriteLine("To jest za ciężkie.");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void PutAway(Item item)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            presentLocation.Stack.Add(item);
            Inventory.Remove(item);
            Burden -= item.Weight;
            Console.WriteLine($"Wyrzucasz {item.Name}.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Wear(Item item)
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (!(item is Armor) && !(item is Weapon) && !(item is Bag))
            {
                Console.WriteLine("Nie możesz tego na siebie założyć.");
                return;
            }

            if (item is Armor)
            {
                foreach (var i in WornItems)
                    if (i is Armor && CheckIfArmorTypeIsTheSame((Armor)item, (Armor)i))
                    {
                        Console.WriteLine($"Już masz na sobie {WhichTypeOfArmor((Armor)i)}.");
                        return;
                    }

                IncreaseArmor((Armor)item);
            }
            else if (item is Bag)
            {
                foreach (var i in WornItems)
                    if (i is Bag)
                    {
                        Console.WriteLine("Już masz na sobie torbę.");
                        return;
                    }
                this.IncreaseCarryingCapacity((Bag)item);
            }
            else
            {
                foreach (var i in WornItems)
                    if (i is Weapon)
                    {
                        Console.WriteLine("Już trzymasz broń.");
                        return;
                    }
                IncreaseDamage((Weapon)item);
            }

            WornItems.Add(item);
            Inventory.Remove(item);
            Burden -= item.Weight;
            Console.WriteLine($"Zakładasz na siebie {item.Name}.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void IncreaseDamage(Weapon weapon)
        {
            Damage += weapon.Damage;
        }

        private void IncreaseArmor(Armor armor)
        {
            Armor += armor.Toughness;
        }

        private void IncreaseCarryingCapacity(Bag bag)
        {
            CarryingCapacity += bag.CarryingCapacity;
        }

        private static bool CheckIfArmorTypeIsTheSame(Armor itemToWear, Armor itemWorn)
        {
            return itemToWear.Type == itemWorn.Type;
        }

        private static string WhichTypeOfArmor(Armor itemWorn)
        {
            var type = "";
            switch (itemWorn.Type)
            {
                case ArmorType.Boots:
                    type = "buty";
                    break;
                case ArmorType.Gloves:
                    type = "rękawice";
                    break;
                case ArmorType.Helmet:
                    type = "hełm";
                    break;
                case ArmorType.Jacket:
                    type = "zbroję";
                    break;
                case ArmorType.Shield:
                    type = "tarczę";
                    break;
                case ArmorType.Trousers:
                    type = "spodnie";
                    break;
            }

            return type;
        }

        public void TakeOff(Item item)
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (item is Bag)
            {
                ReduceCarryingCapacity((Bag)item);
                while (Burden > CarryingCapacity)
                {
                    var _lighest = from i in Inventory
                                   orderby i.Weight
                                   select i;
                    var lighest = _lighest.First();
                    Console.WriteLine("Jest ci ciężko, musisz coś zrzucić.");
                    this.PutAway(lighest);
                }
            }
            else if (item is Armor)
            {
                ReduceArmor((Armor)item);
            }
            else if (item is Weapon)
            {
                ReduceDamage((Weapon)item);
            }

            if (item.Weight + Burden <= CarryingCapacity)
            {
                Inventory.Add(item);
                WornItems.Remove(item);
                Burden += item.Weight;
                Console.WriteLine($"Zdejmujesz z siebie {item.Name}.");
            }
            else
            {
                presentLocation.Stack.Add(item);
                WornItems.Remove(item);
                Console.WriteLine($"To jest zbyt ciężkie! Upuszczasz na ziemię {item.Name}.");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void ReduceDamage(Weapon weapon)
        {
            Damage -= weapon.Damage;
        }

        private void ReduceArmor(Armor armor)
        {
            Armor -= armor.Toughness;
        }

        private void ReduceCarryingCapacity(Bag bag)
        {
            CarryingCapacity -= bag.CarryingCapacity;
        }

        public void Rest()
        {
            if (Energy == MaxEnergy)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nJesteś wypoczęty");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.WriteLine("\nOdpoczywasz sobie");
                while (Energy != MaxEnergy)
                {
                    System.Threading.Thread.Sleep(700);
                    Console.WriteLine(RestSamples[new Random().Next(0, 8)]);
                    Energy += 2;
                    if (Energy >= MaxEnergy)
                    {
                        Energy = MaxEnergy;
                    }
                }
            }
        }

        public void Read(Book book)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Service.DisplayLongString("\n\"" + book.SomeLettersOnPaper + "\"\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Watch(Location location)
        {
            Service.DisplayLongString("\n" + location.Overview + "\n");
        }

        public void Watch(Character character)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Service.DisplayLongString("\n" + character.Overview + "\n");

            Console.Write($"{character.Name} jest ");
            if (character.HP == character.MaxHP)
                Console.WriteLine("w pełni sił.");
            else if (character.HP > 0.75 * character.MaxHP)
                Console.WriteLine("lekko ranny.");
            else if (character.HP > 0.5 * character.MaxHP)
                Console.WriteLine("ciężko ranny.");
            else
                Console.WriteLine("umierający.");

            Console.WriteLine();
            var strengthDifference = (int)character.Strength - (int)Strength;

            if (strengthDifference >= 10)
                Console.WriteLine("Nie masz szans w walce.");
            else if (strengthDifference >= 5)
                Console.WriteLine("Jesteś wyraźnie słabszy.");
            else if (strengthDifference >= 2)
                Console.WriteLine("Chyba masz jakieś szanse.");
            else if (strengthDifference >= -2)
                Console.WriteLine("Szanse w walce są wyrównane.");
            else if (strengthDifference >= -5)
                Console.WriteLine("Jesteś silniejszy.");
            else
                Console.WriteLine("Wygrasz jednym strzałem.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Lookout()
        {
            PrisonForFamiliars = FamiliarSpirit.presentLocation;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nRozglądasz się i widzisz:");
            Console.WriteLine("Na północy");
            LookAtDirection(Direction.North);
            Console.WriteLine("Na południu");
            LookAtDirection(Direction.South);
            Console.WriteLine("Na wschodzie");
            LookAtDirection(Direction.East);
            Console.WriteLine("Na zachodzie");
            LookAtDirection(Direction.West);
            Console.ForegroundColor = ConsoleColor.Gray;

            FamiliarSpirit.presentLocation = PrisonForFamiliars;
        }

        private void LookAtDirection(Direction direction)
        {
            FamiliarSpirit.presentLocation = presentLocation;

            if (FamiliarSpirit.MoveFamiliar(direction))
            {
                ShowNPCsInLocation(FamiliarSpirit, "blisko");

                if (FamiliarSpirit.MoveFamiliar(direction))
                {
                    ShowNPCsInLocation(FamiliarSpirit, "niedaleko");

                    if (FamiliarSpirit.MoveFamiliar(direction))
                    {
                        ShowNPCsInLocation(FamiliarSpirit, "daleko");
                    }
                }
            }

        }

        private void ShowNPCsInLocation(Player doppelganger, string howFar)
        {
            foreach (var c in doppelganger.presentLocation.CharactersInLocation)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{c.Name} jest {howFar}.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void ShowItems()
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (Inventory.Count > 0)
            {
                Console.WriteLine("\nW swoim ekwpipunku posiadasz:");
                foreach (var i in this.Inventory)
                {
                    Console.WriteLine(i.Name);
                }
            }
            else
            {
                Console.WriteLine("\nTwój ekwipunek jest pusty");
            }

            if (this.WornItems.Count > 0)
            {
                Console.WriteLine("\nNa sobie nosisz:");
                foreach (var i in WornItems)
                {
                    Console.WriteLine(i.Name);
                }
            }
            else
            {
                Console.WriteLine("\nNie masz na sobie nic... ciekawego.");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }

        public void Attack(Character character)
        {
            character.HP -= (int)(Strength - character.Armor);

            if (character.HP > 0)
            {
                Battle.Fight(this, character);
            }
            else
            {
                Console.WriteLine($"Zabiłeś {character.Name}!");
                Battle.CleanTheMess(character);
            }

        }

        public void Eat(Food food)
        {
            Console.ForegroundColor = ConsoleColor.White;
            HP += (int)food.RestoredHP;
            Inventory.Remove(food);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Drink(Drink drink)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Energy += (int)drink.RestoredEnergy;
            Inventory.Remove(drink);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}
