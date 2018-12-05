using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Seed.Characters;
using Seed.Items;
using Seed.Locations;
using Seed.Scenarios;

namespace Seed
{
    public class Service
    {
        private static readonly ProConsole _pc = new ProConsole();

        public static void Navigate()
        {
            var name=Welcome();
            var world = new World();
            var player = new Player(name, presentLocation: world.Locations[0], familiarSpirit:(Player)world.NPCs[3]);
            var scrap = new Book("Świstek", "zaraz odleci.", 0, "Napisz: pomoc");
            player.Inventory.Add(scrap);
            Command(player, world);
        }

        private static string Welcome()
        {
            _pc.White().Print("Podaj swe imię: ");

            var characterName = _pc.ReadLine();
            characterName = string.IsNullOrEmpty(characterName) ? "Bezimienny" : characterName;

            DisplayLongString("Jeśli nie jesteś facetem, to... no, już nim jesteś. Bardzo mi przykro " +
                "(a tak naprawdę, to nie).");
            PressSomething();
            DisplayLongString($"Otóż, drogi {characterName}, sytuacja wygląda tak..." +
                $"Napchałeś się jakichś dziwnych, tanich chipsów z pobliskiego spożywczaka, i popiłeś " +
                $"solidną porcją gazowanego, słodzonego rakotwórczym słodzikiem napoju. Nic więc dziwnego, " +
                $"że od godziny majtasz wesoło nóżkami siedząc na porcelanowym tronie.");
            PressSomething();
            DisplayLongString("Przed tobą przygoda. Może największa, a może masz już za sobą większe. Tak czy " +
                "inaczej, jeśli poczujesz, że nie wiesz, co robić, możesz przeczytać wskazówki zapisane na tym " +
                              "świstku, który masz w kieszeni.");
            DisplayLongString("No tak, ciekawe, skąd on się tam wziął... napisz: \"czytaj świstek\" i wszystko " +
                "stanie się jasne.");
            _pc.Print("Powodzenia...");
            PressSomething();

            return characterName;
        }

        public static void DisplayLongString(string longstring)
        {
            var substrings = longstring.Split('\n');
            for (var i = 0; i < substrings.Length; i++)
            {
                if (substrings[i].Length <= 80)
                {
                    Console.WriteLine(substrings[i]);
                }
                else
                {
                    string newString;
                    byte pointer = 79;
                    while (substrings[i].Length > 80)
                    {
                        if (substrings[i][pointer] == ' ')
                        {
                            newString = substrings[i].Remove(pointer);
                            substrings[i] = substrings[i].Remove(0, pointer + 1);
                            Console.WriteLine(newString);
                            pointer = 79;
                        }
                        else if (pointer == 0)
                        {
                            newString = substrings[i].Remove(80);
                            substrings[i] = substrings[i].Remove(0, 80);
                            Console.WriteLine(newString);
                            pointer = 79;
                        }
                        else
                            pointer--;
                    }
                    Console.WriteLine(substrings[i]);
                }
            }
        }

        private static void PressSomething()
        {
            Console.WriteLine("\n                                                   Naciśnij coś\n");
            Console.ReadKey();
        }

        public static void DisplayStats(Player player)
        {
            _pc.DarkCyan().Print($"\n<HP: {player.HP} EN: {player.Energy}>");
        }

        public static void DisplayDoors(Location location)
        {
            var gates = "";

            if (location.North.IsOpen)
                gates += "North ";
            if (location.South.IsOpen)
                gates += "South ";
            if (location.East.IsOpen)
                gates += "East ";
            if (location.West.IsOpen)
                gates += "West ";
            if (location.Up.IsOpen)
                gates += "Up ";
            if (location.Down.IsOpen)
                gates += "Down ";

            _pc.DarkYellow().Print(gates);
        }

        public static void DisplayNPCsInLocation(List<Character> Characters)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            foreach (var character in Characters)
            {
                if (character is Player playerCharacter)
                {
                    DisplayLongString($"{playerCharacter.Name} {playerCharacter.Description}");
                }
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void DisplayItemsInLocation(List<Item> Items)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            foreach (var i in Items)
            {
                DisplayLongString($"{i.Name} {i.Description}");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void MoveCharacter(Character character)
        {
            var movingCharacter = (IMove) character;
            if (movingCharacter.IsLazy == false)
            {
                movingCharacter.Move(Direction.Unknown);
            }
        }
        
        private static void LetCharacterAttack(Character character, World world)
        {
            var attacker = (IAttack) character;

            if (attacker.IsAggressive)
            {
                foreach (var defender in character.presentLocation.CharactersInLocation)
                {
                    var isAlive = defender.HP > 0;
                    var requiredAttackerStrength = 3 * defender.Armor;
                    var isStrongEnough = character.Strength >= requiredAttackerStrength;
                    var isAttacker = character != defender;

                    if (isAlive && isStrongEnough && isAttacker)
                    {
                        attacker.Attack(defender, world); //TODO PM: why are you passing world here, seems wrong
                        break;
                    }

                }

                foreach (var defender in character.presentLocation.CharactersInLocation)
                {
                    var isAlive = defender.HP > 0;
                    var isAttacker = character != defender;

                    if (isAlive && isAttacker)
                    {
                        attacker.Attack(defender, world);
                        break;
                    }
                }
            }
        }

        public static void Help()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            DisplayLongString(
                "Wpisane komendy zatwierdzaj klawiszem ENTER. Wielkość liter nie ma znaczenia.\n" +
                "North/South/East/West/Up/Down - kierunki poruszania się.\n" +
                "Atakuj [NPC] - atakowanie napotkanego w lokacji NPCa.\n" +
                "Odpocznij - odpoczywasz, aż odpoczniesz.\n" +
                "Rozejrzyj - rozglądanie się na cztery strony świata.\n" +
                "Obejrzyj [NPC] - oglądanie wskazanego NPCa.\n" +
                "Podnies [ITEM] - zabranie wskazanego przedmiotu z lokacji.\n" +
                "Wyrzuc [ITEM] - wyrzucenie wskazanego przedmiotu z ekwipunku.\n" +
                "Zaloz [ITEM] - ubranie na siebie przedmiotu z ekwipunku.\n" +
                "Zdejmij [ITEM] - ściągnięcie przedmiotu z siebie do ekwipunku.\n" +
                "Ekwipunek - wyświetla informacje o stanie ekwipunku oraz założonych przedmiotach.\n" +
                "Dane - wyświetla dane na temat postaci.\n" +
                "Czytaj [ITEM] - pozwala ci czytać książki, notatki, etc.\n" +
                "Zjedz [ITEM] - jesz coś i regenerujesz zdrowie\n" +
                "Wypij [ITEM] - pijesz coś i regenerujesz energię\n" +
                "Pomoc - wyświetla pomoc.\n" +
                "Exit - wyjście z gry."
                );
            PressSomething();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void TakeActionOnItem(Player player, string[] command, List<Item> listOfItems,
            string ifCommandTooShort, string ifItemAbsent, Action<Item> action = null, Action<Book> read = null,
            Action<Food> eat = null, Action<Drink> drink = null)
        {
            if (action == null && read == null && eat == null && drink == null)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Powiedz programiście, że TakeActionOnItem ma złe parametry XD");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            var isItemInList = false;
            var itemName = "";
            if (command.Length == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n" + ifCommandTooShort + "\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                if (command.Length == 2)
                {
                    itemName = command[1];
                }
                else
                {
                    itemName = command[1] + " " + command[2];
                }

                var _wantedItem = from i in listOfItems
                                  where i.Name.ToLower() == itemName.ToLower()
                                  orderby i.Name ascending
                                  select i;

                if (_wantedItem.Any())
                {
                    var wantedItem = _wantedItem.First();

                    if (wantedItem.GetType() == typeof(Book) && read!=null)
                    {
                        read((Book)wantedItem);
                    }
                    else if (wantedItem.GetType() == typeof(Food) && eat!=null)
                    {
                        eat((Food)wantedItem);
                    }
                    else if (wantedItem.GetType() == typeof(Drink) && drink!=null)
                    {
                        drink((Drink)wantedItem);
                    }
                    else
                    {
                        action(wantedItem);
                    }
                    isItemInList = true;
                }

                if (isItemInList == false)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n" + ifItemAbsent + "\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        public static void Watch(Player player, string[] command)
        {
            if (command.Length == 1)
            {
                player.Watch(player.presentLocation);
            }
            else
            {
                var charName = "";
                if (command.Length == 2)
                {
                    charName = command[1];
                }
                else
                {
                    charName = command[1] + " " + command[2];
                }

                var _wantedChar = from c in player.presentLocation.CharactersInLocation
                                  where c.Name.ToLower() == charName.ToLower()
                                  orderby c.Name ascending
                                  select c;

                if (_wantedChar.Any())
                {
                    var wantedChar = _wantedChar.First();
                    player.Watch(wantedChar);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nNie ma tu nikogo takiego.\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        private static void Attack(Player player, string[] command, World world)
        {
            if (command.Length == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Kogo chcesz atakować?");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                var charName = "";
                if (command.Length == 2)
                {
                    charName = command[1];
                }
                else
                {
                    charName = command[1] + " " + command[2];
                }

                var _wantedChar = from c in player.presentLocation.CharactersInLocation
                                  where c.Name.ToLower() == charName.ToLower()
                                  orderby c.Name ascending
                                  select c;

                if (_wantedChar.Any())
                {
                    var wantedChar = _wantedChar.First();
                    player.Attack(wantedChar, world);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nNie ma tu nikogo takiego.\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        private static void PlayerData(Player player)
        {
            Console.ForegroundColor = ConsoleColor.White;
            DisplayLongString(
                $"Imię: {player.Name}\n" +
                $"Maksymalny poziom zdrowia: {player.MaxHP}\n" +
                $"Aktualny poziom zdrowia: {player.HP}\n" +
                $"Maksymalny poziom energii: {player.MaxEnergy}\n" +
                $"Aktualny poziom energii: {player.Energy}\n" +
                $"Siła: {player.Strength}\n" +
                $"Zadawane obrażenia: {player.Damage}\n" +
                $"Pancerz: {player.Armor}" +
                $"Udźwig: {player.CarryingCapacity}\n" +
                $"Obciążenie: {player.Burden}\n" +
                $"\n\nWięcej danych pojawi się wkrótce."
                );
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Command(Player player, World world)
        {
            for (; ; )
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(player.presentLocation.Name);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Wyjścia: ");
                DisplayDoors(player.presentLocation);
                Console.ForegroundColor = ConsoleColor.Gray;
                DisplayLongString(player.presentLocation.Description);
                DisplayNPCsInLocation(player.presentLocation.CharactersInLocation);
                DisplayItemsInLocation(player.presentLocation.Stack);

                DisplayStats(player);
                Console.WriteLine();

                var command = Console.ReadLine().Split(' ');
                switch (command[0].ToLower())
                {
                    case "north":
                        player.Move(Direction.North);
                        break;
                    case "east":
                        player.Move(Direction.East);
                        break;
                    case "south":
                        player.Move(Direction.South);
                        break;
                    case "west":
                        player.Move(Direction.West);
                        break;
                    case "up":
                        player.Move(Direction.Up);
                        break;
                    case "down":
                        player.Move(Direction.Down);
                        break;
                    case "atakuj":
                        {
                            Attack(player, command, world);
                        }
                        break;
                    case "odpocznij":
                        {
                            player.Rest();
                        }
                        break;
                    case "pomoc":
                        {
                            Help();
                        }
                        break;
                    case "rozejrzyj":
                        {
                            player.Lookout();
                        }
                        break;
                    case "podnies":
                        {
                            TakeActionOnItem(player, command, player.presentLocation.Stack,
                               "Co chcesz podnieść?", "Nie ma tu takiego przedmiotu.", player.PickUp);
                        }
                        break;
                    case "wyrzuc":
                        {
                            TakeActionOnItem(player, command, player.Inventory,
                               "Co chcesz wyrzucić?", "Nie posiadasz takiego przedmiotu.", player.PutAway);
                        }
                        break;
                    case "zaloz":
                        {
                            TakeActionOnItem(player, command, player.Inventory,
                                  "Co chcesz założyć?", "Nie posiadasz takiego przedmiotu.", player.Wear);
                        }
                        break;
                    case "zdejmij":
                        {
                            TakeActionOnItem(player, command, player.WornItems,
                                     "Co chcesz zdjąć?", "Nie masz na sobie czegoś takiego.", player.TakeOff);
                        }
                        break;
                    case "zjedz":
                        {
                            TakeActionOnItem(player, command, player.Inventory,
                                "Co chcesz zjeść?", "Nie posiadasz takiego przedmiotu", eat: player.Eat);
                        }
                        break;
                    case "wypij":
                        {
                            TakeActionOnItem(player, command, player.Inventory,
                                "Co chcesz wypić?", "Nie posiadasz takiego przedmiotu", drink: player.Drink);
                        }
                        break;
                    case "ekwipunek":
                        {
                            player.ShowItems();
                        }
                        break;
                    case "dane":
                        {
                            PlayerData(player);
                        }
                        break;
                    case "czytaj":
                        {
                            TakeActionOnItem(player, command, player.Inventory, "Co chcesz przeczytać?",
                                         "Nie posiadasz takiego przedmiotu.", read: player.Read);
                        }
                        break;
                    case "obejrzyj":
                        {
                            Watch(player, command);
                        }
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("\nNieprawidłowa komenda\n");
                        break;
                }


                foreach (var c in world.NPCs)
                {
                    if (c is IMove)
                    {
                        MoveCharacter(c);
                    }

                    if (c is IAttack)
                    {
                        LetCharacterAttack(c, world);
                    }
                }

                foreach (var trash in Battle.Garbage)
                {
                    trash.presentLocation.CharactersInLocation.Remove(trash);
                    world.NPCs.Remove(trash);
                }

                Battle.Garbage.Clear();
            }
        }
    }
}
