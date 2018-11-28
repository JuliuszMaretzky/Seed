using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Seed
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
                uint armor = 5, Location presentLocation = null, Player familiarSpirit=null) :
            base(name, description, overview, hp, strength, armor, presentLocation)
        {
            if (name == String.Empty)
                this.Name = "Bezimienny";
            else
                this.Name = name;

            this.Energy = this.MaxEnergy = energy;
            this.CarryingCapacity = (uint)strength * 3;
            this.WornItems = new List<Item>();
            this.FamiliarSpirit = familiarSpirit;

            this.Burden = 0;
            foreach (var item in this.Inventory)
                this.Burden += item.Weight;
            foreach (var item in this.WornItems)
                this.Burden += item.Weight;

            if (this.Burden > CarryingCapacity)
                throw new Exception("Za ciężko!");
        }

        public void Move(Direction direction)
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (this.Energy == 0)
            {
                Console.WriteLine("Nie pochodzisz sobie. Musisz odpocząć");
                return;
            }

            this.Energy -= 3;

            switch (direction)
            {
                case Direction.North:
                    {
                        if (!(this.presentLocation.North.isOpen == DoorState.Closed))
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            this.presentLocation = this.presentLocation.North.location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść");
                        }
                    }
                    break;
                case Direction.South:
                    {
                        if (!(this.presentLocation.South.isOpen == DoorState.Closed))
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            this.presentLocation = this.presentLocation.South.location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść");
                        }
                    }
                    break;
                case Direction.East:
                    {
                        if (!(this.presentLocation.East.isOpen == DoorState.Closed))
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            this.presentLocation = this.presentLocation.East.location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść");
                        }
                    }
                    break;
                case Direction.West:
                    {
                        if (!(this.presentLocation.West.isOpen == DoorState.Closed))
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            this.presentLocation = this.presentLocation.West.location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść");
                        }
                    }
                    break;
                case Direction.Up:
                    {
                        if (!(this.presentLocation.Up.isOpen == DoorState.Closed))
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            this.presentLocation = this.presentLocation.Up.location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść");
                        }
                    }
                    break;
                case Direction.Down:
                    {
                        if (!(this.presentLocation.Down.isOpen == DoorState.Closed))
                        {
                            presentLocation.CharactersInLocation.Remove(this);
                            this.presentLocation = this.presentLocation.Down.location;
                            presentLocation.CharactersInLocation.Add(this);
                        }
                        else
                        {
                            Console.WriteLine("Nie możesz tam iść");
                        }
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
                        if (this.presentLocation.North.isOpen == DoorState.Open)
                        {
                            this.presentLocation = this.presentLocation.North.location;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                case Direction.South:
                    {
                        if (this.presentLocation.South.isOpen == DoorState.Open)
                        {
                            this.presentLocation = this.presentLocation.South.location;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                case Direction.East:
                    {
                        if (this.presentLocation.East.isOpen == DoorState.Open)
                        {
                            this.presentLocation = this.presentLocation.East.location;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                case Direction.West:
                    {
                        if (this.presentLocation.West.isOpen == DoorState.Open)
                        {
                            this.presentLocation = this.presentLocation.West.location;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                case Direction.Up:
                    {
                        if (this.presentLocation.Up.isOpen == DoorState.Open)
                        {
                            this.presentLocation = this.presentLocation.Up.location;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                case Direction.Down:
                    {
                        if (this.presentLocation.Down.isOpen == DoorState.Open)
                        {
                            this.presentLocation = this.presentLocation.Down.location;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                default:
                    return false;
            }
        }

        public void PickUp(Item item)
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (!(this.Burden + item.Weight > this.CarryingCapacity))
            {
                this.Inventory.Add(item);
                this.presentLocation.Stack.Remove(item);
                this.Burden += item.Weight;
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
            this.presentLocation.Stack.Add(item);
            this.Inventory.Remove(item);
            this.Burden -= item.Weight;
            Console.WriteLine($"Wyrzucasz {item.Name}.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Wear(Item item)
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (item.GetType() != typeof(Armor) && item.GetType() != typeof(Weapon) &&
                item.GetType() != typeof(Bag))
            {
                Console.WriteLine("Nie możesz tego na siebie założyć.");
                return;
            }
            else if (item.GetType() == typeof(Armor))
            {
                string type = "";
                foreach (var i in this.WornItems)
                    if (i.GetType() == typeof(Armor) && CheckIfArmorTypeIsTheSame((Armor)item, (Armor)i, ref type))
                    {
                        Console.WriteLine($"Już masz na sobie {type}.");
                        return;
                    }
                this.IncreaseArmor((Armor)item);
            }
            else if (item.GetType() == typeof(Bag))
            {
                foreach (var i in this.WornItems)
                    if (i.GetType() == typeof(Bag))
                    {
                        Console.WriteLine("Już masz na sobie torbę.");
                        return;
                    }
                this.IncreaseCarryingCapacity((Bag)item);
            }
            else
            {
                foreach (var i in this.WornItems)
                    if (i.GetType() == typeof(Weapon))
                    {
                        Console.WriteLine("Już trzymasz broń.");
                        return;
                    }
                this.IncreaseDamage((Weapon)item);
            }

            this.WornItems.Add(item);
            this.Inventory.Remove(item);
            this.Burden -= item.Weight;
            Console.WriteLine($"Zakładasz na siebie {item.Name}.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void IncreaseDamage(Weapon weapon)
        {
            this.Damage += weapon.Damage;
        }

        private void IncreaseArmor(Armor armor)
        {
            this.Armor += armor.Toughness;
        }

        private void IncreaseCarryingCapacity(Bag bag)
        {
            this.CarryingCapacity += bag.CarryingCapacity;
        }

        private bool CheckIfArmorTypeIsTheSame(Armor itemToWear, Armor itemWorn, ref string type)
        {
            if (itemToWear.Type == itemWorn.Type)
            {
                switch (itemToWear.Type)
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

                return true;
            }
            else
                return false;
        }

        public void TakeOff(Item item)
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (item.GetType() == typeof(Bag))
            {
                ReduceCarryingCapacity((Bag)item);
                while (this.Burden > this.CarryingCapacity)
                {
                    var _lighest = from i in this.Inventory
                                   orderby i.Weight ascending
                                   select i;
                    var lighest = _lighest.First();
                    Console.WriteLine("Jest ci ciężko, musisz coś zrzucić.");
                    this.PutAway(lighest);
                }
            }
            else if (item.GetType() == typeof(Armor))
            {
                this.ReduceArmor((Armor)item);
            }
            else if (item.GetType() == typeof(Weapon))
            {
                this.ReduceDamage((Weapon)item);
            }

            if (item.Weight + this.Burden <= this.CarryingCapacity)
            {
                this.Inventory.Add(item);
                this.WornItems.Remove(item);
                this.Burden += item.Weight;
                Console.WriteLine($"Zdejmujesz z siebie {item.Name}.");
            }
            else
            {
                this.presentLocation.Stack.Add(item);
                this.WornItems.Remove(item);
                Console.WriteLine($"To jest zbyt ciężkie! Upuszczasz na ziemię {item.Name}.");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void ReduceDamage(Weapon weapon)
        {
            this.Damage -= weapon.Damage;
        }

        private void ReduceArmor(Armor armor)
        {
            this.Armor -= armor.Toughness;
        }

        private void ReduceCarryingCapacity(Bag bag)
        {
            this.CarryingCapacity -= bag.CarryingCapacity;
        }

        public void Rest()
        {
            if (this.Energy == this.MaxEnergy)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nJesteś wypoczęty");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.WriteLine("\nOdpoczywasz sobie");
                while (this.Energy != this.MaxEnergy)
                {
                    System.Threading.Thread.Sleep(700);
                    Console.WriteLine(RestSamples[new Random().Next(0, 8)]);
                    this.Energy += 2;
                    if (this.Energy >= this.MaxEnergy)
                    {
                        this.Energy = this.MaxEnergy;
                    }
                }
            }
        }

        public void Read(Book book)
        {
            Service.DisplayLongString("\n\"" + book.SomeLettersOnPaper + "\"\n");
        }

        public void Watch(Location location)
        {
            Service.DisplayLongString("\n" + location.Overview + "\n");
        }

        public void Watch(Character character)
        {
            Service.DisplayLongString("\n" + character.Overview + "\n");
        }

        public void Lookout()
        {
            this.PrisonForFamiliars = this.FamiliarSpirit.presentLocation;
            
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

            this.FamiliarSpirit.presentLocation = PrisonForFamiliars;
        }

        private void LookAtDirection(Direction direction)
        {
            this.FamiliarSpirit.presentLocation = this.presentLocation;

            if (this.FamiliarSpirit.MoveFamiliar(direction))
            {
                ShowNPCsInLocation(this.FamiliarSpirit, "blisko");

                if (this.FamiliarSpirit.MoveFamiliar(direction))
                {
                    ShowNPCsInLocation(this.FamiliarSpirit, "niedaleko");

                    if (this.FamiliarSpirit.MoveFamiliar(direction))
                    {
                        ShowNPCsInLocation(this.FamiliarSpirit, "daleko");
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

            if (this.Inventory.Count > 0)
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
                foreach (var i in this.WornItems)
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

        public void Attack(Character character, World world)
        {
            character.HP -= (int)(this.Strength - character.Armor);

            if (character.HP > 0)
            {
                Battle.Fight(this, character, world);
            }
            else
            {
                Console.WriteLine($"Zabiłeś {character.Name}!");
                character.presentLocation.Stack.AddRange(character.Inventory);
                character.Inventory.Clear();
                character.presentLocation.CharactersInLocation.Remove(character);
                world.NPCs.Remove(character);
            }

        }

        public void Eat(Food food)
        {
            Console.ForegroundColor = ConsoleColor.White;
            this.HP += (int)food.RestoredHP;
            this.Inventory.Remove(food);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Drink(Drink drink)
        {
            Console.ForegroundColor = ConsoleColor.White;
            this.Energy += (int)drink.RestoredEnergy;
            this.Inventory.Remove(drink);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}
