using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using Seed.Characters;
using Seed.Items;
using Seed.Locations;
using FluentAssertions;

namespace Seed.Tests
{
    [TestFixture]
    public class PlayerTests
    {
        [Test]
        public void ShouldSetNameAsBezimiennyIfUserDontTypeAnyChars()
        {
            var location = new Location();
            var name = "";
            var player = new Player(name, presentLocation: location);

            Assert.That(player.Name, Is.EqualTo("Bezimienny"));
        }

        [Test]
        public void ShouldHave50HPOnStart()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);

            Assert.That(player.HP, Is.EqualTo(50));
        }

        [Test]
        public void ShouldHave50maxHPOnStart()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);

            Assert.That(player.MaxHP, Is.EqualTo(50));
        }

        [Test]
        public void ShouldHaveNotHPLessThanZero()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);

            player.HP = -5;

            Assert.That(player.HP, Is.EqualTo(0));
        }


        [Test]
        public void ShouldHave25Energy()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);

            Assert.That(player.Energy, Is.EqualTo(25));
        }

        [Test]
        public void ShouldHave25MaxEnergy()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);

            Assert.That(player.MaxEnergy, Is.EqualTo(25));
        }

        [Test]
        public void ShouldHaveEnergyEqualsToMaxEnergy()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);

            Assert.That(player.MaxEnergy, Is.EqualTo(player.Energy));
        }

        [Test]
        public void ShouldHaveLessEnergyAfterMove()
        {
            var location = new Location();
            var location2 = new Location(parentDirection: Direction.Down, parentLocation: location);
            var player = new Player(presentLocation: location);

            player.Move(Direction.Up);

            Assert.That(player.Energy, Is.LessThan(player.MaxEnergy));
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(1)]
        [TestCase(0)]
        public void ShouldNotHaveNegativeEnergyAfterMove(int energy)
        {
            var location = new Location();
            var location2 = new Location(parentDirection: Direction.Down, parentLocation: location);
            var player = new Player(presentLocation: location, energy: energy);

            player.Move(Direction.Up);

            Assert.That(player.Energy, Is.Not.LessThan(0));
        }


        [Test]
        public void ShouldBeInSpecificLocationAfterInitialization()
        {
            var BornPlace = new Location();
            var player = new Player("", presentLocation: BornPlace);

            Assert.That(player.presentLocation, Is.EqualTo(BornPlace));
        }

        [TestCase(Direction.North, Direction.South)]
        [TestCase(Direction.South, Direction.North)]
        [TestCase(Direction.East, Direction.West)]
        [TestCase(Direction.West, Direction.East)]
        [TestCase(Direction.Up, Direction.Down)]
        [TestCase(Direction.Down, Direction.Up)]
        public void ShouldNotMoveIfEnergyIsZero(Direction parent, Direction walk)
        {
            var location1 = new Location();
            var location2 = new Location(parentLocation: location1, parentDirection: parent);
            var player = new Player(presentLocation: location1, energy: 0);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Move(walk);

            Assert.That(player.presentLocation, Is.EqualTo(location1));
            Assert.That(swr.ToString(), Is.EqualTo("Nie pochodzisz sobie. Musisz odpocząć\r\n"));

        }

        [TestCase(Direction.North, Direction.South)]
        [TestCase(Direction.South, Direction.North)]
        [TestCase(Direction.West, Direction.East)]
        [TestCase(Direction.East, Direction.West)]
        [TestCase(Direction.Up, Direction.Down)]
        [TestCase(Direction.Down, Direction.Up)]
        public void ShouldMoveToAnotherLocation(Direction parent, Direction move)
        {
            var location1 = new Location();
            var location2 = new Location(parentLocation: location1, parentDirection: parent);
            var player = new Player(presentLocation: location1);

            player.Move(move);

            Assert.That(player.presentLocation, Is.EqualTo(location2));
        }

        [TestCase(DoorState.Closed, DoorState.Hidden, Direction.North, Direction.South)]
        [TestCase(DoorState.Closed, DoorState.Closed, Direction.North, Direction.South)]
        [TestCase(DoorState.Closed, DoorState.Open, Direction.North, Direction.South)]
        [TestCase(DoorState.Closed, DoorState.Hidden, Direction.South, Direction.North)]
        [TestCase(DoorState.Closed, DoorState.Closed, Direction.South, Direction.North)]
        [TestCase(DoorState.Closed, DoorState.Open, Direction.South, Direction.North)]
        [TestCase(DoorState.Closed, DoorState.Hidden, Direction.East, Direction.West)]
        [TestCase(DoorState.Closed, DoorState.Closed, Direction.East, Direction.West)]
        [TestCase(DoorState.Closed, DoorState.Open, Direction.East, Direction.West)]
        [TestCase(DoorState.Closed, DoorState.Hidden, Direction.West, Direction.East)]
        [TestCase(DoorState.Closed, DoorState.Closed, Direction.West, Direction.East)]
        [TestCase(DoorState.Closed, DoorState.Open, Direction.West, Direction.East)]
        [TestCase(DoorState.Closed, DoorState.Hidden, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Closed, DoorState.Closed, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Closed, DoorState.Open, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Closed, DoorState.Hidden, Direction.Down, Direction.Up)]
        [TestCase(DoorState.Closed, DoorState.Closed, Direction.Down, Direction.Up)]
        [TestCase(DoorState.Closed, DoorState.Open, Direction.Down, Direction.Up)]
        public void ShouldNotMoveIfDoorIsClosed(DoorState fromLocation1, DoorState fromLocation2,
            Direction parentDirection, Direction moveDirection)
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: parentDirection, parentLocation: location1,
                fromParentDoorState: fromLocation1, fromHereDoorState: fromLocation2);
            var player = new Player(presentLocation: location1);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Move(moveDirection);

            Assert.That(player.presentLocation, Is.EqualTo(location1));
            Assert.That(swr.ToString(), Is.EqualTo("Nie możesz tam iść\r\n"));
        }

        [TestCase(DoorState.Hidden, DoorState.Hidden, Direction.North, Direction.South)]
        [TestCase(DoorState.Hidden, DoorState.Closed, Direction.North, Direction.South)]
        [TestCase(DoorState.Hidden, DoorState.Open, Direction.North, Direction.South)]
        [TestCase(DoorState.Hidden, DoorState.Hidden, Direction.South, Direction.North)]
        [TestCase(DoorState.Hidden, DoorState.Closed, Direction.South, Direction.North)]
        [TestCase(DoorState.Hidden, DoorState.Open, Direction.South, Direction.North)]
        [TestCase(DoorState.Hidden, DoorState.Hidden, Direction.East, Direction.West)]
        [TestCase(DoorState.Hidden, DoorState.Closed, Direction.East, Direction.West)]
        [TestCase(DoorState.Hidden, DoorState.Open, Direction.East, Direction.West)]
        [TestCase(DoorState.Hidden, DoorState.Hidden, Direction.West, Direction.East)]
        [TestCase(DoorState.Hidden, DoorState.Closed, Direction.West, Direction.East)]
        [TestCase(DoorState.Hidden, DoorState.Open, Direction.West, Direction.East)]
        [TestCase(DoorState.Hidden, DoorState.Hidden, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Hidden, DoorState.Closed, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Hidden, DoorState.Open, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Hidden, DoorState.Hidden, Direction.Down, Direction.Up)]
        [TestCase(DoorState.Hidden, DoorState.Closed, Direction.Down, Direction.Up)]
        [TestCase(DoorState.Hidden, DoorState.Open, Direction.Down, Direction.Up)]
        public void ShouldMoveWhenDoorIsHidden(DoorState fromLocation1, DoorState fromLocation2,
            Direction parentDirection, Direction moveDirection)
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: parentDirection, parentLocation: location1,
                fromParentDoorState: fromLocation1, fromHereDoorState: fromLocation2);
            var player = new Player(presentLocation: location1);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Move(moveDirection);

            Assert.That(player.presentLocation, Is.EqualTo(location2));
        }

        [Test]
        public void ShouldTakeItemFromLocationStack()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item(location: location);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.PickUp(item);

            Assert.That(player.Inventory, Contains.Item(item));
            Assert.That(swr.ToString(), Is.EqualTo($"Podnosisz {item.Name}.\r\n"));
        }

        [Test]
        public void ShouldTakeItemFromLocationStackAndItShouldNoLongerBeThere()
        {
            var location = new Location();
            var item = new Item(location: location);
            var player = new Player(presentLocation: location);

            player.PickUp(item);

            Assert.That(location.Stack, Has.None.EqualTo(item));
        }

        [Test]
        public void ShouldPutAwayItemFromInventory()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item(location: location);
            player.PickUp(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.PutAway(item);

            Assert.That(player.Inventory, Has.None.EqualTo(item));
            Assert.That(swr.ToString(), Is.EqualTo($"Wyrzucasz {item.Name}.\r\n"));
        }

        [Test]
        public void ShouldPutAwayItemAndPutItOnLocationStack()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item(location: location);
            player.PickUp(item);

            player.PutAway(item);

            Assert.That(location.Stack, Contains.Item(item));
        }

        [Test]
        public void ShouldWearItemFromInventory()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Weapon(location: location);
            player.PickUp(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Wear(item);

            Assert.That(player.WornItems, Contains.Item(item));
            Assert.That(swr.ToString(), Is.EqualTo($"Zakładasz na siebie {item.Name}.\r\n"));
        }

        public void ShouldNotHaveAnItemInInventoryAfterWear()
        {
            var player = new Player();
            var item = new Weapon();
            player.Inventory.Add(item);

            player.Wear(item);

            Assert.That(player.Inventory, Has.None.EqualTo(item));
        }

        [Test]
        public void ShouldTakeOffItemAndPutItIntoInventory()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item(location: location);
            player.PickUp(item);
            player.Wear(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.TakeOff(item);

            Assert.That(player.Inventory, Contains.Item(item));
            Assert.That(swr.ToString(), Is.EqualTo($"Zdejmujesz z siebie {item.Name}.\r\n"));
        }

        [Test]
        public void ShouldNotHaveAnItemInWornItemsAfterTakeOff()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Weapon(location: location);
            player.PickUp(item);
            player.Wear(item);

            player.TakeOff(item);

            Assert.That(player.WornItems, Has.None.EqualTo(item));
        }

        [Test]
        public void ShouldIncreasePlayerDamageWhenWearWeapon()
        {
            var location = new Location();
            var weapon = new Weapon(location: location, damage: 1);
            var player = new Player(strength: 3, presentLocation: location);
            player.PickUp(weapon);

            player.Wear(weapon);

            Assert.That(player.Damage, Is.EqualTo(10));
        }

        [Test]
        public void ShouldDecreasePlayerDamageWhenTakeOffWeapon()
        {
            var location = new Location();
            var weapon = new Weapon(location: location, damage: 1);
            var player = new Player(strength: 3, presentLocation: location);
            player.PickUp(weapon);
            player.Wear(weapon);

            player.TakeOff(weapon);

            Assert.That(player.Damage, Is.EqualTo(9));
        }

        [Test]
        public void ShouldIncreasePlayerArmorWhenWearArmor()
        {
            var location = new Location();
            var armor = new Armor(location: location, toughness: 2, type: ArmorType.Jacket);
            var helmet = new Armor(location: location, toughness: 4, type: ArmorType.Helmet);
            var player = new Player(presentLocation: location, armor: 2);
            player.PickUp(helmet);
            player.PickUp(armor);

            player.Wear(helmet);
            player.Wear(armor);

            Assert.That(player.Armor, Is.EqualTo(8));
        }

        [Test]
        public void ShouldDecreasePlayerArmorWhenTakeOffArmor()
        {
            var location = new Location();
            var armor = new Armor(location: location, toughness: 2, type: ArmorType.Jacket);
            var helmet = new Armor(location: location, toughness: 4, type: ArmorType.Helmet);
            var player = new Player(presentLocation: location, armor: 2);
            player.PickUp(helmet);
            player.PickUp(armor);
            player.Wear(helmet);
            player.Wear(armor);

            player.TakeOff(armor);

            Assert.That(player.Armor, Is.EqualTo(6));
        }

        [Test]
        public void ShouldHaveEnergyOnMaxEnergyLevelAfterRest()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var player = new Player(presentLocation: location1, energy: 30);
            player.Move(Direction.South);
            player.Move(Direction.North);
            player.Move(Direction.South);
            player.Move(Direction.North);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Rest();

            Assert.That(player.Energy, Is.EqualTo(player.MaxEnergy));
            Assert.That(swr.ToString(), Contains.Substring("Odpoczywasz sobie"));
        }

        [Test]
        public void ShouldWriteCommentInsteadOfRestingIfEnergyIsMaxed()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var player = new Player(presentLocation: location1, energy: 30);


            StringWriter swr = new StringWriter();
            Console.SetOut(swr);
            player.Rest();

            Assert.That(swr.ToString(), Is.EqualTo("\nJesteś wypoczęty\r\n"));
        }

        [Test]
        public void ShouldHaveBurdenEqualsToZeroIfNaked()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);

            player.Inventory.Clear();
            player.WornItems.Clear();

            Assert.That(player.Burden, Is.EqualTo(0));
        }


        [Test]
        public void ShouldAddWeightOfTakenItemToBurden()
        {
            var location = new Location();
            var item = new Item(weight: 1, location: location);
            var player = new Player(strength: 5, presentLocation: location);
            player.Inventory.Clear();
            player.WornItems.Clear();

            player.PickUp(location.Stack[0]);

            Assert.That(player.Burden, Is.EqualTo(1));
        }

        [Test]
        public void ShouldNotTakeItemAndDisplayCommentIfBurdenEqualsToCarryingCapacity()
        {
            var location = new Location();
            var item1 = new Item(weight: 1, location: location);
            var item3 = new Item(weight: 3, location: location);
            var player = new Player(strength: 1, presentLocation: location);
            player.PickUp(item3);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.PickUp(item1);

            Assert.That(player.Inventory, Has.None.EqualTo(item1));
            Assert.That(swr.ToString(), Is.EqualTo("To jest za ciężkie.\r\n"));
        }

        [TestCase((uint)0, (uint)2)]
        [TestCase((uint)0, (uint)0)]
        [TestCase((uint)2, (uint)0)]
        [TestCase((uint)5, (uint)4)]
        public void ShouldReduceBurdenWhenPutAwayItem(uint weight1, uint weight2)
        {
            var location = new Location();
            var player = new Player(strength: 3, presentLocation: location);
            var item1 = new Item(weight: weight1, location: location);
            var item2 = new Item(weight: weight2, location: location);
            player.PickUp(item1);
            player.PickUp(item2);

            player.PutAway(item2);

            Assert.That(player.Burden, Is.EqualTo(weight1));
        }

        [Test]
        public void ShouldHaveExtendedCarryingCapacityAfterWearABag()
        {
            var location = new Location();
            var player = new Player(strength: 3, presentLocation: location);
            var bag = new Bag(capacity: 3, location: location);
            var sumOfCapacity = player.CarryingCapacity + bag.CarryingCapacity;
            player.PickUp(bag);

            player.Wear(bag);

            Assert.That(player.CarryingCapacity, Is.EqualTo(sumOfCapacity));
        }

        [Test]
        public void ShouldReduceCarryingCapacityAfterTakeOffBag()
        {
            var location = new Location();
            var player = new Player(strength: 3, presentLocation: location);
            var bag = new Bag(capacity: 3, location: location);
            player.PickUp(bag);
            player.Wear(bag);
            var capacity = player.CarryingCapacity;

            player.TakeOff(bag);

            Assert.That(player.CarryingCapacity, Is.LessThan(capacity));
        }

        [Test]
        public void ShouldThrowAwayLightestItemsWhenCarryingCapacityIsInsufficientAfterTakeOffBag()
        {
            var location = new Location();
            var player = new Player(strength: 3, presentLocation: location);
            var item1 = new Item(weight: 8, location: location);
            var bag = new Bag(weight: 1, capacity: 4, location: location);
            var item2 = new Item(name: "2", weight: 2, location: location);
            var item3 = new Item(name: "3", weight: 1, location: location);
            var expectedItems = new List<Item>();
            expectedItems.Add(item1);
            expectedItems.Add(bag);
            player.PickUp(item1);
            player.PickUp(bag);
            player.Wear(bag);
            player.PickUp(item2);
            player.PickUp(item3);

            player.TakeOff(bag);

            Assert.That(player.Inventory, Is.EqualTo(expectedItems));
        }

        [TestCase(ArmorType.Boots, "buty")]
        [TestCase(ArmorType.Gloves, "rękawice")]
        [TestCase(ArmorType.Helmet, "hełm")]
        [TestCase(ArmorType.Jacket, "zbroję")]
        [TestCase(ArmorType.Shield, "tarczę")]
        [TestCase(ArmorType.Trousers, "spodnie")]
        public void ShouldNotBeAbleToWearSecondArmorOfTheSameType(ArmorType type, string wornThing)
        {
            var location = new Location();
            var item1 = new Armor("", "", 0, 0, type, location);
            var item2 = new Armor("", "", 0, 0, type, location);
            var player = new Player(presentLocation: location);
            player.PickUp(item1);
            player.PickUp(item2);
            player.Wear(item1);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Wear(item2);

            Assert.That(player.WornItems, Contains.Item(item1));
            Assert.That(player.WornItems, Does.Not.Contain(item2));
            Assert.That(player.Inventory, Contains.Item(item2));
            Assert.That(player.Inventory, Does.Not.Contain(item1));
            Assert.That(swr.ToString(), Is.EqualTo($"Już masz na sobie {wornThing}.\r\n"));
        }

        [Test]
        public void ShouldHaveOnlyOneWeaponInHand()
        {
            var location = new Location();
            var club = new Weapon("Kij bejzbolowy", "", 1, 3, WeaponType.Melee, location);
            var gun = new Weapon("Rewolwer", "", 1, 4, WeaponType.Ranged, location);
            var player = new Player(presentLocation: location);
            player.PickUp(gun);
            player.PickUp(club);
            player.Wear(gun);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Wear(club);

            Assert.That(player.Inventory, Contains.Item(club));
            Assert.That(player.WornItems, Does.Not.Contain(club));
            Assert.That(player.Inventory, Does.Not.Contain(gun));
            Assert.That(player.WornItems, Contains.Item(gun));
            Assert.That(swr.ToString(), Is.EqualTo("Już trzymasz broń.\r\n"));
        }

        [Test]
        public void ShouldHaveOnlyOneBagWorn()
        {
            var location = new Location();
            var bag1 = new Bag(location: location);
            var bag2 = new Bag(location: location);
            var player = new Player(presentLocation: location);
            player.PickUp(bag1);
            player.PickUp(bag2);
            player.Wear(bag1);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Wear(bag2);

            Assert.That(player.Inventory, Contains.Item(bag2));
            Assert.That(player.WornItems, Does.Not.Contain(bag2));
            Assert.That(player.Inventory, Does.Not.Contain(bag1));
            Assert.That(player.WornItems, Contains.Item(bag1));
            Assert.That(swr.ToString(), Is.EqualTo("Już masz na sobie torbę.\r\n"));
        }

        [Test]
        public void ShouldNotBeAbleToWearAnythingExceptWeaponOrArmorOrBag()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var book = new Book(location: location);
            var food = new Food(location: location);
            var drink = new Drink(location: location);
            var ExpectedItemsList = new List<Item>();
            ExpectedItemsList.Add(book);
            ExpectedItemsList.Add(food);
            ExpectedItemsList.Add(drink);
            player.PickUp(book);
            player.PickUp(food);
            player.PickUp(drink);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Wear(book);
            player.Wear(food);
            player.Wear(drink);

            Assert.That(player.Inventory, Is.EqualTo(ExpectedItemsList));
            Assert.That(player.WornItems, Is.Empty);
            Assert.That(swr.ToString(), Is.EqualTo("Nie możesz tego na siebie założyć.\r\n" +
                "Nie możesz tego na siebie założyć.\r\nNie możesz tego na siebie założyć.\r\n"));
        }

        [Test]
        public void ShouldReduceBurdenWhenWearItem()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Weapon(weight: 3, location: location);
            player.PickUp(item);
            var burden = player.Burden;

            player.Wear(item);

            Assert.That(player.Burden, Is.LessThan(burden));
        }

        [Test]
        public void ShouldIncreaseBurdenWhenTakeOffItem()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Weapon(weight: 3, location: location);
            player.PickUp(item);
            var burden = player.Burden;
            player.Wear(item);
            var burden2 = player.Burden;

            player.TakeOff(item);

            Assert.That(player.Burden, Is.GreaterThan(burden2));
            Assert.That(player.Burden, Is.EqualTo(burden));
        }

        [Test]
        public void ShouldLetFallItemAfterTakeOffIfBurdenIsTooBig()
        {
            var location = new Location();
            var player = new Player(presentLocation: location, strength: 1);
            var plate = new Armor(name: "Talerz", weight: 3, location: location);
            var item = new Item(weight: 3, location: location);
            player.PickUp(plate);
            player.Wear(plate);
            player.PickUp(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.TakeOff(plate);

            Assert.That(player.Inventory, Contains.Item(item));
            Assert.That(player.Inventory, Does.Not.Contain(plate));
            Assert.That(player.WornItems, Is.Empty);
            Assert.That(swr.ToString(), Is.EqualTo($"To jest zbyt ciężkie! Upuszczasz na ziemię " +
                $"{plate.Name}.\r\n"));
        }

        [Test]
        public void ShouldDisplayOneOfCommentsFromListWhenRest()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var player = new Player(presentLocation: location1);
            do
            {
                player.Move(Direction.South);
                player.Move(Direction.North);
            } while (player.Energy > 0);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);
            var RestSamples = new List<string>()
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

            player.Rest();

            swr.ToString().Should().ContainAny(RestSamples);
        }

        [Test]
        public void ShouldBeAbleToReadBooksFromInventory()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var book = new Book(text: "Nie zapomnij zapisać karteczki", location: location);
            player.PickUp(book);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Read((Book)player.Inventory[0]);

            Assert.That(swr.ToString(), Is.EqualTo("\r\n\"Nie zapomnij zapisać karteczki\"\r\n\r\n"));
        }

        [Test]
        public void ShouldBeAbleToSeeCharactersInASmallDistance()
        {
            var prison = new Location();
            var zyzyx = new Player(presentLocation:prison);
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.South, parentLocation: location1);
            var location3 = new Location(parentDirection: Direction.South, parentLocation: location2);
            var location4 = new Location(parentDirection: Direction.South, parentLocation: location3);
            var location5 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var location6 = new Location(parentDirection: Direction.North, parentLocation: location5);
            var location7 = new Location(parentDirection: Direction.North, parentLocation: location6);
            var location8 = new Location(parentDirection: Direction.West, parentLocation: location1);
            var location9 = new Location(parentDirection: Direction.West, parentLocation: location8);
            var location10 = new Location(parentDirection: Direction.West, parentLocation: location9);
            var location11 = new Location(parentDirection: Direction.East, parentLocation: location1);
            var location12 = new Location(parentDirection: Direction.East, parentLocation: location11);
            var location13 = new Location(parentDirection: Direction.East, parentLocation: location12);
            var player = new Player(presentLocation: location1, familiarSpirit:zyzyx);
            var human1 = new Human("Azjata", presentLocation: location8);
            var human2 = new Human("Wiking", presentLocation: location3);
            var human3 = new Human("Americano", presentLocation: location13);
            var human4 = new Human("Egipcjanin", presentLocation: location5);
            var human5 = new Human("Pigmej", presentLocation: location6);
            var human6 = new Human("Bur", presentLocation: location7);
            var human7 = new Human("Australijczyk", presentLocation: location7);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Lookout();

            Assert.That(swr.ToString(), Is.EqualTo("\nRozglądasz się i widzisz:\r\n" +
                "Na północy\r\nWiking jest niedaleko.\r\n" +
                "Na południu\r\nEgipcjanin jest blisko.\r\nPigmej jest niedaleko.\r\nBur jest daleko." +
                "\r\nAustralijczyk jest daleko.\r\n" +
                "Na wschodzie\r\nAzjata jest blisko.\r\n" +
                "Na zachodzie\r\nAmericano jest daleko.\r\n"));
            Assert.That(zyzyx.presentLocation, Is.EqualTo(prison));

        }

        [Test]
        public void ShouldBeAbleToSeeCharactersInASmallDistanceAndSomeDoorsAreNotOpen()
        {
            var prison = new Location();
            var zyzyx = new Player(presentLocation: prison);
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.South, parentLocation: location1);
            var location3 = new Location(parentDirection: Direction.South, parentLocation: location2);
            var location4 = new Location(parentDirection: Direction.South, parentLocation: location3);
            var location5 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var location6 = new Location(parentDirection: Direction.North, parentLocation: location5,
                fromParentDoorState: DoorState.Closed);
            var location7 = new Location(parentDirection: Direction.North, parentLocation: location6);
            var location8 = new Location(parentDirection: Direction.West, parentLocation: location1);
            var location9 = new Location(parentDirection: Direction.West, parentLocation: location8);
            var location10 = new Location(parentDirection: Direction.West, parentLocation: location9,
                fromParentDoorState: DoorState.Hidden);
            var location11 = new Location(parentDirection: Direction.East, parentLocation: location1,
                fromParentDoorState: DoorState.Hidden);
            var location12 = new Location(parentDirection: Direction.East, parentLocation: location11);
            var location13 = new Location(parentDirection: Direction.East, parentLocation: location12);
            var player = new Player(presentLocation: location1, familiarSpirit: zyzyx);
            var human1 = new Human("Azjata", presentLocation: location8);
            var human2 = new Human("Wiking", presentLocation: location3);
            var human3 = new Human("Americano", presentLocation: location13);
            var human4 = new Human("Egipcjanin", presentLocation: location5);
            var human5 = new Human("Pigmej", presentLocation: location6);
            var human6 = new Human("Bur", presentLocation: location7);
            var human7 = new Human("Australijczyk", presentLocation: location7);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Lookout();

            Assert.That(swr.ToString(), Is.EqualTo("\nRozglądasz się i widzisz:\r\n" +
                "Na północy\r\nWiking jest niedaleko.\r\n" +
                "Na południu\r\nEgipcjanin jest blisko.\r\n" +
                "Na wschodzie\r\nAzjata jest blisko.\r\n" +
                "Na zachodzie\r\n"));
            Assert.That(zyzyx.presentLocation, Is.EqualTo(prison));
        }

        [Test]
        public void ShouldRegenerateHPAWhenEat()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            player.HP -= 10;
            var snack = new Food(restoredHP: 3);
            int expectedHP = player.HP + 3;

            player.Eat(snack);

            Assert.That(player.HP, Is.EqualTo(expectedHP));
        }

        [Test]
        public void ShouldNotRegenerateHPAboveMaxHP()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            player.HP -= 4;
            var snack = new Food(restoredHP: 10);

            player.Eat(snack);

            Assert.That(player.HP, Is.EqualTo(player.MaxHP));
        }

        [Test]
        public void ShouldRegenerateEnergyWhenDrink()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            player.Energy -= 10;
            var water = new Drink(restoredEnergy: 3);
            int expectedEnergy = player.Energy + 3;

            player.Drink(water);

            Assert.That(player.Energy, Is.EqualTo(expectedEnergy));
        }

        [Test]
        public void ShouldNotRegenerateEnergyAboveMaxEnergy()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            player.HP -= 5;
            var water = new Drink(restoredEnergy: 50);

            player.Drink(water);

            Assert.That(player.Energy, Is.EqualTo(player.MaxEnergy));
        }
    }
}
