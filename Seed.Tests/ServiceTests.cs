using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Seed.Characters;
using Seed.Items;
using Seed.Locations;

namespace Seed.Tests
{
    [TestFixture]
    public class ServiceTests
    {
        [TestCase("1234567890123456789012345678901234567890123456789012345678901234567890" +
                "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890123456789012345678901234567890" +
                "12345678901234567890",
            "12345678901234567890123456789012345678901234567890" +
                "123456789012345678901234567890\r\n12345678901234567890123456789012345678901234567890" +
                "123456789012345678901234567890\r\n12345678901234567890123456789012345678901234567890" +
                "123456789012345678901234567890\r\n12345678901234567890\r\n")]
        [TestCase("1234567890123456 8901234567890123456789012 4567890123456789012345678901234567 90" +
            "123456789012345678901234567 9012345678901234567890123456789012 45678901234567890" +
            "123456789012345678901234567890123 5678901234567890123456789012345 78901234567890" +
            "12345678901234567890", "1234567890123456 8901234567890123456789012 456789012345678901234567" +
            "8901234567\r\n90123456789012345678901234567 9012345678901234567890123456789012\r\n45678901234567890" +
            "123456789012345678901234567890123\r\n5678901234567890123456789012345 78901234567890" +
            "12345678901234567890\r\n")]
        public void ShouldDisplayLongStringInAProperWay(string attribute, string returned)
        {
            string longstring = attribute;
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.DisplayLongString(longstring);

            Assert.That(swr.ToString, Is.EqualTo(returned));
        }

        [TestCase(DoorState.Open, DoorState.Open, DoorState.Open, DoorState.Open, DoorState.Open, DoorState.Open,
            "North South East West Up Down \r\n")]
        [TestCase(DoorState.Open, DoorState.Hidden, DoorState.Open, DoorState.Closed, DoorState.Open, DoorState.Open,
            "North East Up Down \r\n")]
        [TestCase(DoorState.Closed, DoorState.Closed, DoorState.Closed, DoorState.Closed, DoorState.Closed,
            DoorState.Closed, "\r\n")]
        [TestCase(DoorState.Hidden, DoorState.Hidden, DoorState.Hidden, DoorState.Hidden, DoorState.Hidden,
            DoorState.Hidden, "\r\n")]
        public void ShouldDisplayAllAndOnlyOpenDoorsInLocation(DoorState northState, DoorState southState,
            DoorState eastState, DoorState westState, DoorState upState, DoorState downState, string expected)
        {
            var location = new Location();
            location.ChancheDoorState(Direction.North, northState);
            location.ChancheDoorState(Direction.South, southState);
            location.ChancheDoorState(Direction.East, eastState);
            location.ChancheDoorState(Direction.West, westState);
            location.ChancheDoorState(Direction.Up, upState);
            location.ChancheDoorState(Direction.Down, downState);
            var player = new Player(presentLocation: location);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.DisplayDoors(location);

            Assert.That(swr.ToString(), Is.EqualTo(expected));
        }

        [Test]
        public void ShouldDisplayAllAndOnlyOpenDoorsInLocationAfterAddAnotherLocations()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.South, parentLocation: location1,
                fromHereDoorState: DoorState.Open, fromParentDoorState: DoorState.Open);
            var player = new Player(presentLocation: location1);
            player.Move(Direction.North);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.DisplayDoors(location2);

            Assert.That(swr.ToString(), Is.EqualTo("South \r\n"));
        }

        [Test]
        public void ShouldDisplayAllNPCsInLocation()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var listOfNPCs = new List<Character>()
            {
                new Human(presentLocation:location),
                new Human("Zonk", description: "rusza wąsikami.", presentLocation: location),
                new Human("Twoja stara", description: "ma rogala.", presentLocation:location)
            };
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.DisplayNPCsInLocation(location.CharactersInLocation);

            Assert.That(swr.ToString(), Is.EqualTo("Jakiś człowiek tu jest.\r\nZonk rusza wąsikami.\r\n" +
                "Twoja stara ma rogala.\r\n"));
        }

        [Test]
        public void ShouldDisplayAllItemsInLocation()
        {
            var location = new Location();
            var item = new Item(location: location);
            var weapon = new Weapon(location: location);
            var armor = new Armor(location: location);
            var food = new Food(location: location);
            var drink = new Drink(location: location);
            var book = new Book(location: location);
            var player = new Player(presentLocation: location);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.DisplayItemsInLocation(location.Stack);

            Assert.That(swr.ToString, Is.EqualTo($"{item.Name} {item.Description}\r\n" +
                $"{weapon.Name} {weapon.Description}\r\n{armor.Name} {armor.Description}\r\n" +
                $"{food.Name} {food.Description}\r\n{drink.Name} {drink.Description}\r\n" +
                $"{book.Name} {book.Description}\r\n"));
        }

        [Test]
        public void PlayerShouldPickUpItem()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item("Banan", location: location);

            Service.TakeActionOnItem(player, "podnies banan".Split(' '), player.presentLocation.Stack,
                "Co chcesz podnieść?", "Nie ma tu takiego przedmiotu.", player.PickUp);

            Assert.That(player.Inventory, Does.Contain(item));
        }

        [Test]
        public void PlayerShouldPickUpProperItem()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item1 = new Item("banan", location: location);
            var item2 = new Item("banan z czekoladą", location: location);
            var item3 = new Item("bananowe kakao", location: location);

            Service.TakeActionOnItem(player, "podnies banan".Split(' '), player.presentLocation.Stack,
                "Co chcesz podnieść?", "Nie ma tu takiego przedmiotu.", player.PickUp);

            Assert.That(player.Inventory, Does.Contain(item1));
            Assert.That(player.Inventory, Does.Not.Contain(item2));
            Assert.That(player.Inventory, Does.Not.Contain(item3));
        }

        [TestCase("podnies japko", "\nNie ma tu takiego przedmiotu.\n\r\n")]
        [TestCase("podnies", "\nCo chcesz podnieść?\n\r\n")]
        public void PlayerShouldNotPickUpItem(string command, string expectedConsoleOutput)
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item("Banan", location: location);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.TakeActionOnItem(player, command.Split(' '), player.presentLocation.Stack,
                "Co chcesz podnieść?", "Nie ma tu takiego przedmiotu.", player.PickUp);

            Assert.That(player.Inventory, Does.Not.Contain(item));
            Assert.That(swr.ToString(), Is.EqualTo(expectedConsoleOutput));
        }

        [Test]
        public void PlayerShouldPutAwayItem()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item("Banan", location: location);
            player.PickUp(item);

            Service.TakeActionOnItem(player, "wyrzuc banan".Split(' '), player.Inventory,
                "Co chcesz wyrzucić?", "Nie posiadasz takiego przedmiotu.", player.PutAway);

            Assert.That(player.Inventory, Does.Not.Contain(item));
        }

        [TestCase("wyrzuc japko", "\nNie posiadasz takiego przedmiotu.\n\r\n")]
        [TestCase("wyrzuc", "\nCo chcesz wyrzucić?\n\r\n")]
        public void PlayerShouldNotPutAwayItem(string command, string expectedConsoleOutput)
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item("Banan", location: location);
            player.PickUp(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.TakeActionOnItem(player, command.Split(' '), player.Inventory,
                "Co chcesz wyrzucić?", "Nie posiadasz takiego przedmiotu.", player.PutAway);

            Assert.That(player.Inventory, Does.Contain(item));
            Assert.That(swr.ToString(), Is.EqualTo(expectedConsoleOutput));
        }

        [Test]
        public void PlayerShouldWearArmor()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Armor("Kapota", location: location);
            player.PickUp(item);

            Service.TakeActionOnItem(player, "Podnies Kapota".Split(' '), player.Inventory,
                "Co chcesz podnieść?", "Nie ma tu takiego przedmiotu.", player.Wear);

            Assert.That(player.WornItems, Does.Contain(item));
        }

        [TestCase("zaloz japko", "\nNie posiadasz takiego przedmiotu.\n\r\n")]
        [TestCase("zaloz", "\nCo chcesz założyć?\n\r\n")]
        public void PlayerShouldNotWearArmor(string command, string expectedConsoleOutput)
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Armor("Arbuz", location: location);
            player.PickUp(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.TakeActionOnItem(player, command.Split(' '), player.Inventory,
                "Co chcesz założyć?", "Nie posiadasz takiego przedmiotu.", player.Wear);

            Assert.That(player.WornItems, Does.Not.Contain(item));
            Assert.That(swr.ToString(), Is.EqualTo(expectedConsoleOutput));
        }

        [Test]
        public void PlayerShouldTakeOffArmor()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Armor("Kapota", location: location);
            player.PickUp(item);
            player.Wear(item);

            Service.TakeActionOnItem(player, "Zdejmij Kapota".Split(' '), player.WornItems,
                "Co chcesz zdjąć?", "Nie masz na sobie czegoś takiego.", player.TakeOff);

            Assert.That(player.WornItems, Does.Not.Contain(item));
        }

        [TestCase("zdejmij klapki", "\nNie masz na sobie czegoś takiego.\n\r\n")]
        [TestCase("zdejmij", "\nCo chcesz zdjąć?\n\r\n")]
        public void PlayerShouldNotTakeOffArmor(string command, string expectedConsoleOutput)
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Armor("Arbuz", location: location);
            player.PickUp(item);
            player.Wear(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.TakeActionOnItem(player, command.Split(' '), player.WornItems,
                "Co chcesz zdjąć?", "Nie masz na sobie czegoś takiego.", player.TakeOff);

            Assert.That(player.WornItems, Does.Contain(item));
            Assert.That(swr.ToString(), Is.EqualTo(expectedConsoleOutput));
        }

        [Test]
        public void PlayerShouldReadBook()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Book("Pamiętniczek", text: "Drogi pamiętniczku...", location: location);
            player.PickUp(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.TakeActionOnItem(player, "czytaj pamiętniczek".Split(' '), player.Inventory,
                "Co chcesz przeczytać?", "Nie posiadasz takiego przedmiotu.", read: player.Read);

            Assert.That(swr.ToString(), Is.EqualTo("\r\n\"Drogi pamiętniczku...\"\r\n\r\n"));
        }

        [TestCase("czytaj ten kod", "\nNie posiadasz takiego przedmiotu.\n\r\n")]
        [TestCase("czytaj", "\nCo chcesz przeczytać?\n\r\n")]
        public void PlayerShouldNotReadBook(string command, string expectedConsoleOutput)
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Book("Lista zakupów", location: location);
            player.PickUp(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.TakeActionOnItem(player, command.Split(' '), player.Inventory,
                "Co chcesz przeczytać?", "Nie posiadasz takiego przedmiotu.", read: player.Read);

            Assert.That(player.Inventory, Does.Contain(item));
            Assert.That(swr.ToString(), Is.EqualTo(expectedConsoleOutput));
        }
        
        [Test]
        public void ShouldDisplayProperCommentIfThereIsNoCharacterInLocationWhoseOverviewPlayerWantsToSee()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.Watch(player, "obejrzyj twoja stara".Split(' '));

            Assert.That(swr.ToString(), Is.EqualTo("\nNie ma tu nikogo takiego.\n\r\n"));
        }

        [Test]
        public void PlayerShouldEatFood()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Food("Jabłko", restoredHP: 4, location: location);
            player.PickUp(item);
            player.HP -= 10;
            var expectedHP = player.HP + item.RestoredHP;

            Service.TakeActionOnItem(player, "zjedz jabłko".Split(' '), player.Inventory,
                "Co chcesz zjeść?", "Nie posiadasz takiego przedmiotu.", eat: player.Eat);

            Assert.That(player.Inventory, Does.Not.Contain(item));
            Assert.That(player.HP, Is.EqualTo(expectedHP));
        }

        [TestCase("zjedz czapka", "\nNie posiadasz takiego przedmiotu.\n\r\n")]
        [TestCase("zjedz", "\nCo chcesz zjeść?\n\r\n")]
        public void PlayerShouldNotEatFood(string command, string expectedConsoleOutput)
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Food("Stołowa noga", location: location);
            player.PickUp(item);
            player.HP -= 10;
            var expectedHP = player.HP;

            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.TakeActionOnItem(player, command.Split(' '), player.Inventory,
                "Co chcesz zjeść?", "Nie posiadasz takiego przedmiotu.", eat: player.Eat);

            Assert.That(player.HP, Is.EqualTo(expectedHP));
            Assert.That(player.Inventory, Does.Contain(item));
            Assert.That(swr.ToString(), Is.EqualTo(expectedConsoleOutput));
        }

        [Test]
        public void PlayerShouldDrinkDrink()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Drink("Jabol", restoredEnergy: 4, location: location);
            player.PickUp(item);
            player.Energy -= 10;
            var expectedEnergy = player.Energy + item.RestoredEnergy;

            Service.TakeActionOnItem(player, "wypij jabol".Split(' '), player.Inventory,
                "Co chcesz wypić?", "Nie posiadasz takiego przedmiotu.", drink: player.Drink);

            Assert.That(player.Energy, Is.EqualTo(expectedEnergy));
            Assert.That(player.Inventory, Does.Not.Contain(item));
        }

        [TestCase("wypij smarki", "\nNie posiadasz takiego przedmiotu.\n\r\n")]
        [TestCase("wypij", "\nCo chcesz wypić?\n\r\n")]
        public void PlayerShouldNotDrinkDrink(string command, string expectedConsoleOutput)
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Drink("Płynny smalec", restoredEnergy: 3, location: location);
            player.PickUp(item);
            player.Energy -= 10;
            var expectedEnergy = player.Energy;

            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.TakeActionOnItem(player, command.Split(' '), player.Inventory,
                "Co chcesz wypić?", "Nie posiadasz takiego przedmiotu.", drink: player.Drink);

            Assert.That(player.Energy, Is.EqualTo(expectedEnergy));
            Assert.That(player.Inventory, Does.Contain(item));
            Assert.That(swr.ToString(), Is.EqualTo(expectedConsoleOutput));
        }
    }
}
