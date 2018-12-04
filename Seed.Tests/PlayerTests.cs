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
        [Category("Player.Constructor")]
        public void ShouldSetNameAsBezimiennyIfUserDontTypeAnyChars()
        {
            var location = new Location();
            var name = "";
            var player = new Player(name, presentLocation: location);

            player.Name.Should().Be("Bezimienny");
            player.HP.Should().Be(50);
            player.MaxHP.Should().Be(50);
            player.Energy.Should().Be(25);
            player.MaxEnergy.Should().Be(25);
            player.MaxEnergy.Should().Be(player.Energy);
            player.presentLocation.Should().Be(location);
        }
        
        [Test]
        [Category("Player.Properties")]
        public void ShouldHaveNotHPLessThanZero()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);

            player.HP = -5;

            player.HP.Should().Be(0);
        }

        [Test]
        [Category("Player.Move")]
        public void ShouldHaveLessEnergyAfterMove()
        {
            var location = new Location();
            //var location2 = new Location(parentDirection: Direction.Down, parentLocation: location);
            var player = new Player(presentLocation: location);

            player.Move(Direction.Up);

            player.Energy.Should().BeLessThan(player.MaxEnergy);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(1)]
        [TestCase(0)]
        [Category("Player.Move")]
        public void ShouldNotHaveNegativeEnergyAfterMove(int energy)
        {
            var location = new Location();
            var location2 = new Location(parentDirection: Direction.Down, parentLocation: location);
            var player = new Player(presentLocation: location, energy: energy);

            player.Move(Direction.Up);

            player.Energy.Should().BeGreaterOrEqualTo(0);
        }
        
        [TestCase(Direction.North, Direction.South)]
        [TestCase(Direction.South, Direction.North)]
        [TestCase(Direction.East, Direction.West)]
        [TestCase(Direction.West, Direction.East)]
        [TestCase(Direction.Up, Direction.Down)]
        [TestCase(Direction.Down, Direction.Up)]
        [Category("Player.Move")]
        public void ShouldNotMoveIfEnergyIsZero(Direction parent, Direction walk)
        {
            var location1 = new Location();
            var location2 = new Location(parentLocation: location1, parentDirection: parent);
            var player = new Player(presentLocation: location1, energy: 0);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Move(walk);

            player.presentLocation.Should().Be(location1);
            swr.ToString().Should().Be("Nie pochodzisz sobie. Musisz odpocząć\r\n");
        }

        [TestCase(Direction.North, Direction.South)]
        [TestCase(Direction.South, Direction.North)]
        [TestCase(Direction.West, Direction.East)]
        [TestCase(Direction.East, Direction.West)]
        [TestCase(Direction.Up, Direction.Down)]
        [TestCase(Direction.Down, Direction.Up)]
        [Category("Player.Move")]
        public void ShouldMoveToAnotherLocation(Direction parent, Direction move)
        {
            var location1 = new Location();
            var location2 = new Location(parentLocation: location1, parentDirection: parent);
            var player = new Player(presentLocation: location1);

            player.Move(move);

            player.presentLocation.Should().Be(location2);
        }

        [TestCase(DoorState.Hidden, Direction.North, Direction.South)]
        [TestCase(DoorState.Closed, Direction.North, Direction.South)]
        [TestCase(DoorState.Open, Direction.North, Direction.South)]
        [TestCase(DoorState.Hidden, Direction.South, Direction.North)]
        [TestCase(DoorState.Closed, Direction.South, Direction.North)]
        [TestCase(DoorState.Open, Direction.South, Direction.North)]
        [TestCase(DoorState.Hidden, Direction.East, Direction.West)]
        [TestCase(DoorState.Closed, Direction.East, Direction.West)]
        [TestCase(DoorState.Open, Direction.East, Direction.West)]
        [TestCase(DoorState.Hidden, Direction.West, Direction.East)]
        [TestCase(DoorState.Closed, Direction.West, Direction.East)]
        [TestCase(DoorState.Open, Direction.West, Direction.East)]
        [TestCase(DoorState.Hidden, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Closed, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Open, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Hidden, Direction.Down, Direction.Up)]
        [TestCase(DoorState.Closed, Direction.Down, Direction.Up)]
        [TestCase(DoorState.Open, Direction.Down, Direction.Up)]
        [Category("Player.Move")]
        public void ShouldNotMoveIfDoorIsClosed(DoorState fromHere,
            Direction parentDirection, Direction moveDirection)
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: parentDirection, parentLocation: location1,
                fromParentDoorState: DoorState.Closed, fromHereDoorState: fromHere);
            var player = new Player(presentLocation: location1);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Move(moveDirection);

            player.presentLocation.Should().Be(location1);
            swr.ToString().Should().Be("Nie możesz tam iść\r\n");
        }

        [TestCase(DoorState.Hidden, Direction.North, Direction.South)]
        [TestCase(DoorState.Closed, Direction.North, Direction.South)]
        [TestCase(DoorState.Open, Direction.North, Direction.South)]
        [TestCase(DoorState.Hidden, Direction.South, Direction.North)]
        [TestCase(DoorState.Closed, Direction.South, Direction.North)]
        [TestCase(DoorState.Open, Direction.South, Direction.North)]
        [TestCase(DoorState.Hidden, Direction.East, Direction.West)]
        [TestCase(DoorState.Closed, Direction.East, Direction.West)]
        [TestCase(DoorState.Open, Direction.East, Direction.West)]
        [TestCase(DoorState.Hidden, Direction.West, Direction.East)]
        [TestCase(DoorState.Closed, Direction.West, Direction.East)]
        [TestCase(DoorState.Open, Direction.West, Direction.East)]
        [TestCase(DoorState.Hidden, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Closed, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Open, Direction.Up, Direction.Down)]
        [TestCase(DoorState.Hidden, Direction.Down, Direction.Up)]
        [TestCase(DoorState.Closed, Direction.Down, Direction.Up)]
        [TestCase(DoorState.Open, Direction.Down, Direction.Up)]
        [Category("Player.Move")]
        public void ShouldMoveWhenDoorIsHidden(DoorState fromHere, Direction parentDirection, 
            Direction moveDirection)
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: parentDirection, parentLocation: location1,
                fromParentDoorState: DoorState.Hidden, fromHereDoorState: fromHere);
            var player = new Player(presentLocation: location1);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Move(moveDirection);

            player.presentLocation.Should().Be(location2);
        }

        [Test]
        [Category("Player.PickUp")]
        public void ShouldTakeItemFromLocationStack()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item(location: location);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.PickUp(item);

            player.Inventory.Should().Contain(item);
            swr.ToString().Should().Be($"Podnosisz {item.Name}.\r\n");
        }

        [Test]
        [Category("Player.PickUp")]
        public void ShouldTakeItemFromLocationStackAndItShouldNoLongerBeThere()
        {
            var location = new Location();
            var item = new Item(location: location);
            var player = new Player(presentLocation: location);

            player.PickUp(item);

            location.Stack.Should().NotContain(item);
        }

        [Test]
        [Category("Player.PutAway")]
        public void ShouldPutAwayItemFromInventory()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item(location: location);
            player.PickUp(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.PutAway(item);

            player.Inventory.Should().NotContain(item);
            swr.ToString().Should().Be($"Wyrzucasz {item.Name}.\r\n");
        }

        [Test]
        [Category("Player.PutAway")]
        public void ShouldPutAwayItemAndPutItOnLocationStack()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Item(location: location);
            player.PickUp(item);

            player.PutAway(item);

            location.Stack.Should().Contain(item);
        }

        [Test]
        [Category("Player.Wear")]
        public void ShouldWearItemFromInventory()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Weapon(location: location);
            player.PickUp(item);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Wear(item);

            player.WornItems.Should().Contain(item);
            swr.ToString().Should().Be($"Zakładasz na siebie {item.Name}.\r\n");
        }
        [Test]
        [Category("Player.Wear")]
        public void ShouldNotHaveAnItemInInventoryAfterWear()
        {
            var location = new Location();
            var player = new Player(presentLocation:location);
            var item = new Weapon(location: location);
            player.PickUp(item);

            player.Wear(item);

            player.Inventory.Should().NotContain(item);
        }

        [Test]
        [Category("Player.TakeOff")]
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

            player.Inventory.Should().Contain(item);
            swr.ToString().Should().Be($"Zdejmujesz z siebie {item.Name}.\r\n");
        }

        [Test]
        [Category("Player.TakeOff")]
        public void ShouldNotHaveAnItemInWornItemsAfterTakeOff()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Weapon(location: location);
            player.PickUp(item);
            player.Wear(item);

            player.TakeOff(item);

            player.WornItems.Should().NotContain(item);
        }

        [Test]
        [Category("Player.Wear")]
        public void ShouldIncreasePlayerDamageWhenWearWeapon()
        {
            var location = new Location();
            var weapon = new Weapon(location: location, damage: 1);
            var player = new Player(strength: 3, presentLocation: location);
            player.PickUp(weapon);

            player.Wear(weapon);

            player.Damage.Should().Be(10);
        }

        [Test]
        [Category("Player.TakeOff")]
        public void ShouldDecreasePlayerDamageWhenTakeOffWeapon()
        {
            var location = new Location();
            var weapon = new Weapon(location: location, damage: 1);
            var player = new Player(strength: 3, presentLocation: location);
            player.PickUp(weapon);
            player.Wear(weapon);

            player.TakeOff(weapon);

            player.Damage.Should().Be(9);
        }

        [Test]
        [Category("Player.Wear")]
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

            player.Armor.Should().Be(8);
        }

        [Test]
        [Category("Player.TakeOff")]
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

            player.Armor.Should().Be(6);
        }

        [Test]
        [Category("Player.Rest")]
        public void ShouldHaveEnergyOnMaxEnergyLevelAfterRest()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var player = new Player(presentLocation: location1, energy: 30);
            player.Move(Direction.South);
            player.Move(Direction.North);
            player.Move(Direction.South);
            player.Move(Direction.North);

            player.Rest();

            player.Energy.Should().Be(player.MaxEnergy);
        }

        [Test]
        [Category("Player.Rest")]
        public void ShouldWriteCommentInsteadOfRestingIfEnergyIsMaxed()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var player = new Player(presentLocation: location1, energy: 30);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Rest();

            swr.ToString().Should().Be("\nJesteś wypoczęty\r\n");
        }

        [Test]
        [Category("Player.Properties")]
        public void ShouldHaveBurdenEqualsToZeroIfNaked()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);

            player.Inventory.Clear();
            player.WornItems.Clear();

            player.Burden.Should().Be(0);
        }


        [Test]
        [Category("Player.PickUp")]
        public void ShouldAddWeightOfTakenItemToBurden()
        {
            var location = new Location();
            var item = new Item(weight: 1, location: location);
            var player = new Player(strength: 5, presentLocation: location);
            player.Inventory.Clear();
            player.WornItems.Clear();

            player.PickUp(location.Stack[0]);

            player.Burden.Should().Be(1);
        }

        [Test]
        [Category("Player.PickUp")]
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

            player.Inventory.Should().NotContain(item1);
            swr.ToString().Should().Be("To jest za ciężkie.\r\n");
        }

        [TestCase((uint)0, (uint)2)]
        [TestCase((uint)0, (uint)0)]
        [TestCase((uint)2, (uint)0)]
        [TestCase((uint)5, (uint)4)]
        [Category("Player.PutAway")]
        public void ShouldReduceBurdenWhenPutAwayItem(uint weight1, uint weight2)
        {
            var location = new Location();
            var player = new Player(strength: 3, presentLocation: location);
            var item1 = new Item(weight: weight1, location: location);
            var item2 = new Item(weight: weight2, location: location);
            player.PickUp(item1);
            player.PickUp(item2);

            player.PutAway(item2);

            player.Burden.Should().Be(weight1);
        }

        [Test]
        [Category("Player.Wear")]
        public void ShouldHaveExtendedCarryingCapacityAfterWearABag()
        {
            var location = new Location();
            var player = new Player(strength: 3, presentLocation: location);
            var bag = new Bag(capacity: 3, location: location);
            var sumOfCapacity = player.CarryingCapacity + bag.CarryingCapacity;
            player.PickUp(bag);

            player.Wear(bag);

            player.CarryingCapacity.Should().Be(sumOfCapacity);
        }

        [Test]
        [Category("Player.TakeOff")]
        public void ShouldReduceCarryingCapacityAfterTakeOffBag()
        {
            var location = new Location();
            var player = new Player(strength: 3, presentLocation: location);
            var bag = new Bag(capacity: 3, location: location);
            player.PickUp(bag);
            player.Wear(bag);
            var capacity = player.CarryingCapacity;

            player.TakeOff(bag);

            player.CarryingCapacity.Should().BeLessThan(capacity);
        }

        [Test]
        [Category("Player.TakeOff")]
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

            player.Inventory.Should().BeEquivalentTo(expectedItems);
        }

        [TestCase(ArmorType.Boots, "buty")]
        [TestCase(ArmorType.Gloves, "rękawice")]
        [TestCase(ArmorType.Helmet, "hełm")]
        [TestCase(ArmorType.Jacket, "zbroję")]
        [TestCase(ArmorType.Shield, "tarczę")]
        [TestCase(ArmorType.Trousers, "spodnie")]
        [Category("Player.Wear")]
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

            player.WornItems.Should().Contain(item1);
            player.WornItems.Should().NotContain(item2);
            player.Inventory.Should().Contain(item2);
            player.Inventory.Should().NotContain(item1);
            swr.ToString().Should().Be($"Już masz na sobie {wornThing}.\r\n");
        }

        [Test]
        [Category("Player.Wear")]
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

            player.Inventory.Should().Contain(club);
            player.WornItems.Should().NotContain(club);
            player.Inventory.Should().NotContain(gun);
            player.WornItems.Should().Contain(gun);
            swr.ToString().Should().Be("Już trzymasz broń.\r\n");
        }

        [Test]
        [Category("Player.Wear")]
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

            player.Inventory.Should().Contain(bag2);
            player.WornItems.Should().NotContain(bag2);
            player.Inventory.Should().NotContain(bag1);
            player.WornItems.Should().Contain(bag1);
            swr.ToString().Should().Be("Już masz na sobie torbę.\r\n");
        }

        [Test]
        [Category("Player.Wear")]
        public void ShouldNotBeAbleToWearAnythingExceptWeaponOrArmorOrBag()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var book = new Book(location: location);
            var food = new Food(location: location);
            var drink = new Drink(location: location);
            var expectedItemsList = new List<Item>();
            expectedItemsList.Add(book);
            expectedItemsList.Add(food);
            expectedItemsList.Add(drink);
            player.PickUp(book);
            player.PickUp(food);
            player.PickUp(drink);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Wear(book);
            player.Wear(food);
            player.Wear(drink);

            player.Inventory.Should().BeEquivalentTo(expectedItemsList);
            player.WornItems.Should().BeEmpty();
            swr.ToString().Should().Be("Nie możesz tego na siebie założyć.\r\n" +
            "Nie możesz tego na siebie założyć.\r\nNie możesz tego na siebie założyć.\r\n");
        }

        [Test]
        [Category("Player.Wear")]
        public void ShouldReduceBurdenWhenWearItem()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var item = new Weapon(weight: 3, location: location);
            player.PickUp(item);
            var burden = player.Burden;

            player.Wear(item);

            player.Burden.Should().BeLessThan(burden);
        }

        [Test]
        [Category("Player.TakeOff")]
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

            player.Burden.Should().BeGreaterThan(burden2);
            player.Burden.Should().Be(burden);
        }

        [Test]
        [Category("Player.TakeOff")]
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

            player.Inventory.Should().Contain(item);
            player.Inventory.Should().NotContain(plate);
            player.WornItems.Should().BeEmpty();
            swr.ToString().Should().Be($"To jest zbyt ciężkie! Upuszczasz na ziemię {plate.Name}.\r\n");
        }

        [Test]
        [Category("Player.Rest")]
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
        [Category("Player.Read")]
        public void ShouldBeAbleToReadBooksFromInventory()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            var book = new Book(text: "Nie zapomnij zapisać karteczki", location: location);
            player.PickUp(book);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Read((Book)player.Inventory[0]);

            swr.ToString().Should().Be("\r\n\"Nie zapomnij zapisać karteczki\"\r\n\r\n");
        }

        [Test]
        [Category("Player.Lookout")]
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

            swr.ToString().Should().Be("\nRozglądasz się i widzisz:\r\n" +
                "Na północy\r\nWiking jest niedaleko.\r\n" +
                "Na południu\r\nEgipcjanin jest blisko.\r\nPigmej jest niedaleko.\r\nBur jest daleko." +
                "\r\nAustralijczyk jest daleko.\r\n" +
                "Na wschodzie\r\nAzjata jest blisko.\r\n" +
                "Na zachodzie\r\nAmericano jest daleko.\r\n");
            zyzyx.presentLocation.Should().Be(prison);
        }

        [Test]
        [Category("Player.Lookout")]
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

            swr.ToString().Should().Be("\nRozglądasz się i widzisz:\r\n" +
                "Na północy\r\nWiking jest niedaleko.\r\n" +
                "Na południu\r\nEgipcjanin jest blisko.\r\n" +
                "Na wschodzie\r\nAzjata jest blisko.\r\n" +
                "Na zachodzie\r\n");
            zyzyx.presentLocation.Should().Be(prison);
        }

        [Test]
        [Category("Player.Watch")]
        public void ShouldDisplayOverviewOfLocation()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Watch(location);

            swr.ToString().Should().Be("\r\n" + location.Overview + "\r\n\r\n");
        }

        [Test]
        [Category("Player.Watch")]
        public void ShouldDisplayOverviewOfCharacter()
        {
            var location = new Location();
            var human = new Human(presentLocation: location);
            var player = new Player(presentLocation: location);
            var healthDescriptions = new List<string>()
            {
                "w pełni sił.",
                "lekko ranny.",
                "ciężko ranny.",
                "umierający."
            };
            var compareStrengthDescriptions = new List<string>()
            {
                "Nie masz szans w walce.",
                "Jesteś słabszy.",
                "Szanse w walce są wyrównane.",
                "Chyba masz jakieś szanse.",
                "Jesteś silniejszy.",
                "Wygrasz jednym strzałem."
            };
            StringWriter swr = new StringWriter();
            Console.SetOut(swr);

            player.Watch(human);

            swr.ToString().Should().Contain(human.Overview).And.Contain(human.Name).And.ContainAny(healthDescriptions)
                .And.ContainAny(compareStrengthDescriptions);
        }

        [Test]
        [Category("Player.Eat")]
        public void ShouldRegenerateHPAWhenEat()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            player.HP -= 10;
            var snack = new Food(restoredHP: 3);
            int expectedHP = player.HP + 3;

            player.Eat(snack);

            player.HP.Should().Be(expectedHP);
        }

        [Test]
        [Category("Player.Eat")]
        public void ShouldNotRegenerateHPAboveMaxHP()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            player.HP -= 4;
            var snack = new Food(restoredHP: 10);

            player.Eat(snack);

            player.HP.Should().Be(player.MaxHP);
        }

        [Test]
        [Category("Player.Drink")]
        public void ShouldRegenerateEnergyWhenDrink()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            player.Energy -= 10;
            var water = new Drink(restoredEnergy: 3);
            int expectedEnergy = player.Energy + 3;

            player.Drink(water);

            player.Energy.Should().Be(expectedEnergy);
        }

        [Test]
        [Category("Player.Drink")]
        public void ShouldNotRegenerateEnergyAboveMaxEnergy()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            player.HP -= 5;
            var water = new Drink(restoredEnergy: 50);

            player.Drink(water);

            player.Energy.Should().Be(player.MaxEnergy);
        }
    }
}
