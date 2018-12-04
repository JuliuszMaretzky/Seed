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
        [Category("Service.DisplayLongString")]
        public void ShouldDisplayLongStringInAProperWay(string attribute, string returned)
        {
            string longstring = attribute;
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.DisplayLongString(longstring);

            swr.ToString().Should().Be(returned);
        }

        [TestCase(DoorState.Open, DoorState.Open, DoorState.Open, DoorState.Open, DoorState.Open, DoorState.Open,
            "North South East West Up Down \r\n")]
        [TestCase(DoorState.Open, DoorState.Hidden, DoorState.Open, DoorState.Closed, DoorState.Open, DoorState.Open,
            "North East Up Down \r\n")]
        [TestCase(DoorState.Closed, DoorState.Closed, DoorState.Closed, DoorState.Closed, DoorState.Closed,
            DoorState.Closed, "\r\n")]
        [TestCase(DoorState.Hidden, DoorState.Hidden, DoorState.Hidden, DoorState.Hidden, DoorState.Hidden,
            DoorState.Hidden, "\r\n")]
        [Category("Service.DisplayDoors")]
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

            swr.ToString().Should().Be(expected);
        }

        [Test]
        [Category("Service.DisplayDoors")]
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

            swr.ToString().Should().Be("South \r\n");
        }

        [Test]
        [Category("Service.DisplayNPCsInLocation")]
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

            swr.ToString().Should().Be("Jakiś człowiek tu jest.\r\nZonk rusza wąsikami.\r\n" +
                                       "Twoja stara ma rogala.\r\n");
        }

        [Test]
        [Category("Service.DisplayItemsInLocation")]
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

            swr.ToString().Should().Be($"{item.Name} {item.Description}\r\n" +
                $"{weapon.Name} {weapon.Description}\r\n{armor.Name} {armor.Description}\r\n" +
                $"{food.Name} {food.Description}\r\n{drink.Name} {drink.Description}\r\n" +
                $"{book.Name} {book.Description}\r\n");
        }

        [Test]
        [Category("Service.TakeActionOnItem")]
        public void PlayerShouldPickUpItem()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item("Banan", location: location);

            Service.TakeActionOnItem(player, "podnies banan".Split(' '), player.presentLocation.Stack,
                "Co chcesz podnieść?", "Nie ma tu takiego przedmiotu.", player.PickUp);

            player.Inventory.Should().Contain(item);
        }

        [Test]
        [Category("Service.TakeActionOnItem")]
        public void PlayerShouldPickUpProperItem()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item1 = new Item("banan", location: location);
            var item2 = new Item("banan z czekoladą", location: location);
            var item3 = new Item("bananowe kakao", location: location);

            Service.TakeActionOnItem(player, "podnies banan".Split(' '), player.presentLocation.Stack,
                "Co chcesz podnieść?", "Nie ma tu takiego przedmiotu.", player.PickUp);

            player.Inventory.Should().Contain(item1).And.NotContain(item2).And.NotContain(item3);
        }

        [TestCase("podnies japko", "\nNie ma tu takiego przedmiotu.\n\r\n")]
        [TestCase("podnies", "\nCo chcesz podnieść?\n\r\n")]
        [Category("Service.TakeActionOnItem")]
        public void PlayerShouldNotPickUpItem(string command, string expectedConsoleOutput)
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item("Banan", location: location);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.TakeActionOnItem(player, command.Split(' '), player.presentLocation.Stack,
                "Co chcesz podnieść?", "Nie ma tu takiego przedmiotu.", player.PickUp);

            player.Inventory.Should().NotContain(item);
            swr.ToString().Should().Be(expectedConsoleOutput);
        }

        [Test]
        [Category("Service.TakeActionOnItem")]
        public void PlayerShouldPutAwayItem()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item("Banan", location: location);
            player.PickUp(item);

            Service.TakeActionOnItem(player, "wyrzuc banan".Split(' '), player.Inventory,
                "Co chcesz wyrzucić?", "Nie posiadasz takiego przedmiotu.", player.PutAway);

            player.Inventory.Should().NotContain(item);
        }

        [TestCase("wyrzuc japko", "\nNie posiadasz takiego przedmiotu.\n\r\n")]
        [TestCase("wyrzuc", "\nCo chcesz wyrzucić?\n\r\n")]
        [Category("Service.TakeActionOnItem")]
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

            player.Inventory.Should().Contain(item);
            swr.ToString().Should().Be(expectedConsoleOutput);
        }

        [Test]
        [Category("Service.TakeActionOnItem")]
        public void PlayerShouldWearArmor()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Armor("Kapota", location: location);
            player.PickUp(item);

            Service.TakeActionOnItem(player, "Podnies Kapota".Split(' '), player.Inventory,
                "Co chcesz podnieść?", "Nie ma tu takiego przedmiotu.", player.Wear);

            player.WornItems.Should().Contain(item);
        }

        [TestCase("zaloz japko", "\nNie posiadasz takiego przedmiotu.\n\r\n")]
        [TestCase("zaloz", "\nCo chcesz założyć?\n\r\n")]
        [Category("Service.TakeActionOnItem")]
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

            player.WornItems.Should().NotContain(item);
            swr.ToString().Should().Be(expectedConsoleOutput);
        }

        [Test]
        [Category("Service.TakeActionOnItem")]
        public void PlayerShouldTakeOffArmor()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Armor("Kapota", location: location);
            player.PickUp(item);
            player.Wear(item);

            Service.TakeActionOnItem(player, "Zdejmij Kapota".Split(' '), player.WornItems,
                "Co chcesz zdjąć?", "Nie masz na sobie czegoś takiego.", player.TakeOff);

            player.WornItems.Should().NotContain(item);
        }

        [TestCase("zdejmij klapki", "\nNie masz na sobie czegoś takiego.\n\r\n")]
        [TestCase("zdejmij", "\nCo chcesz zdjąć?\n\r\n")]
        [Category("Service.TakeActionOnItem")]
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

            player.WornItems.Should().Contain(item);
            swr.ToString().Should().Be(expectedConsoleOutput);
        }

        [Test]
        [Category("Service.TakeActionOnItem")]
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

            swr.ToString().Should().Be("\r\n\"Drogi pamiętniczku...\"\r\n\r\n");
        }

        [TestCase("czytaj ten kod", "\nNie posiadasz takiego przedmiotu.\n\r\n")]
        [TestCase("czytaj", "\nCo chcesz przeczytać?\n\r\n")]
        [Category("Service.TakeActionOnItem")]
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

            player.Inventory.Should().Contain(item);
            swr.ToString().Should().Be(expectedConsoleOutput);
        }
        
        [Test]
        [Category("Service.Watch")]
        public void ShouldDisplayProperCommentIfThereIsNoCharacterInLocationWhoseOverviewPlayerWantsToSee()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            Service.Watch(player, "obejrzyj twoja stara".Split(' '));

            swr.ToString().Should().Be("\nNie ma tu nikogo takiego.\n\r\n");
        }

        [Test]
        [Category("Service.TakeActionOnItem")]
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

            player.Inventory.Should().NotContain(item);
            player.HP.Should().Be((int)expectedHP);
        }

        [TestCase("zjedz czapka", "\nNie posiadasz takiego przedmiotu.\n\r\n")]
        [TestCase("zjedz", "\nCo chcesz zjeść?\n\r\n")]
        [Category("Service.TakeActionOnItem")]
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

            player.HP.Should().Be(expectedHP);
            player.Inventory.Should().Contain(item);
            swr.ToString().Should().Be(expectedConsoleOutput);
        }

        [Test]
        [Category("Service.TakeActionOnItem")]
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

            player.Energy.Should().Be((int)expectedEnergy);
            player.Inventory.Should().NotContain(item);
        }

        [TestCase("wypij smarki", "\nNie posiadasz takiego przedmiotu.\n\r\n")]
        [TestCase("wypij", "\nCo chcesz wypić?\n\r\n")]
        [Category("Service.TakeActionOnItem")]
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

            player.Energy.Should().Be(expectedEnergy);
            player.Inventory.Should().Contain(item);
            swr.ToString().Should().Be(expectedConsoleOutput);
        }
    }
}
