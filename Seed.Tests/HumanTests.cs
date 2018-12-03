using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Seed.Characters;
using Seed.Items;
using Seed.Locations;

namespace Seed.Tests
{
    [TestFixture]
    public class HumanTests
    {
        [TestCase(Direction.North)]
        [TestCase(Direction.South)]
        [TestCase(Direction.East)]
        [TestCase(Direction.West)]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Down)]
        public void ShouldMoveToNeighbouringLocationIfDoorIsOpen(Direction parentDirection)
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: parentDirection, parentLocation: location1,
                fromHereDoorState: DoorState.Open);
            var human = new Human("Zenon", hp: 5, strength: 3, presentLocation: location2);

            human.Move(parentDirection);

            Assert.That(human.presentLocation, Is.EqualTo(location1));
        }

        [TestCase(Direction.North, DoorState.Closed)]
        [TestCase(Direction.South, DoorState.Closed)]
        [TestCase(Direction.East, DoorState.Closed)]
        [TestCase(Direction.West, DoorState.Closed)]
        [TestCase(Direction.Up, DoorState.Closed)]
        [TestCase(Direction.Down, DoorState.Closed)]
        [TestCase(Direction.North, DoorState.Hidden)]
        [TestCase(Direction.South, DoorState.Hidden)]
        [TestCase(Direction.East, DoorState.Hidden)]
        [TestCase(Direction.West, DoorState.Hidden)]
        [TestCase(Direction.Up, DoorState.Hidden)]
        [TestCase(Direction.Down, DoorState.Hidden)]
        public void ShouldNotMoveToNeighbouringLocationIfDoorIsClosedOrHidden(
            Direction parentDirection, DoorState doorState)
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: parentDirection, parentLocation: location1,
                fromHereDoorState: doorState);
            var human = new Human("Zenon", hp: 5, strength: 3, presentLocation: location2);

            human.Move(parentDirection);

            Assert.That(human.presentLocation, Is.EqualTo(location2));
        }

        [Test]
        public void ShouldHaveDamageAndArmorEqualToProperValues()
        {
            var location = new Location();
            var human = new Human(presentLocation: location, strength: 2, armor: 1);

            human.ReceiveItems(new List<Item>()
            {
                new Weapon(damage: 1),
                new Weapon(damage: 2),
                new Armor(toughness: 1),
                new Armor(toughness: 5)
            });

            Assert.That(human.Damage, Is.EqualTo(8));
            Assert.That(human.Armor, Is.EqualTo(6));
        }



    }
}
