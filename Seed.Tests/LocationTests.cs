using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Seed.Characters;
using Seed.Locations;

namespace Seed.Tests
{
    [TestFixture]
    class LocationTests
    {
        [Test]
        public void ShouldHaveAllDirectionsClosedAndNullAfterNoParametersConstructor()
        {
            Location location = new Location();

            var direction = new Door(DoorState.Closed, null);

            Assert.That(location.North, Is.EqualTo(direction));
            Assert.That(location.South, Is.EqualTo(direction));
            Assert.That(location.East, Is.EqualTo(direction));
            Assert.That(location.West, Is.EqualTo(direction));
            Assert.That(location.Up, Is.EqualTo(direction));
            Assert.That(location.Down, Is.EqualTo(direction));
        }

        [Test]
        public void ShouldHaveConnectionWithNeighbourLocation()
        {
            var location1 = new Location();
            var location2 = new Location(parentLocation: location1, parentDirection: Direction.North);
            var player = new Player(presentLocation: location1);

            player.Move(Direction.South);

            Assert.That(player.presentLocation, Is.EqualTo(location2));
        }

        [TestCase(DoorState.Open, DoorState.Closed)]
        [TestCase(DoorState.Closed, DoorState.Open)]
        [TestCase(DoorState.Open, DoorState.Hidden)]
        [TestCase(DoorState.Hidden, DoorState.Open)]
        [TestCase(DoorState.Hidden, DoorState.Closed)]
        [TestCase(DoorState.Closed, DoorState.Hidden)]
        [TestCase(DoorState.Open, DoorState.Open)]
        [TestCase(DoorState.Closed, DoorState.Closed)]
        [TestCase(DoorState.Hidden, DoorState.Hidden)]
        public void ShouldChangeStateOfDoor(DoorState Before, DoorState After)
        {
            Location location1 = new Location();
            Location location2 = new Location(parentDirection: Direction.North, parentLocation: location1,
                fromParentDoorState: Before);

            location1.ChancheDoorState(Direction.South, After);

            Assert.That(location1.South.isOpen, Is.EqualTo(After));
        }

        [TestCase(Direction.North, DoorState.Open, DoorState.Open)]
        [TestCase(Direction.North, DoorState.Open, DoorState.Closed)]
        [TestCase(Direction.North, DoorState.Open, DoorState.Hidden)]
        [TestCase(Direction.North, DoorState.Closed, DoorState.Open)]
        [TestCase(Direction.North, DoorState.Closed, DoorState.Closed)]
        [TestCase(Direction.North, DoorState.Closed, DoorState.Hidden)]
        [TestCase(Direction.North, DoorState.Hidden, DoorState.Open)]
        [TestCase(Direction.North, DoorState.Hidden, DoorState.Closed)]
        [TestCase(Direction.North, DoorState.Hidden, DoorState.Hidden)]
        [TestCase(Direction.South, DoorState.Open, DoorState.Open)]
        [TestCase(Direction.South, DoorState.Open, DoorState.Closed)]
        [TestCase(Direction.South, DoorState.Open, DoorState.Hidden)]
        [TestCase(Direction.South, DoorState.Closed, DoorState.Open)]
        [TestCase(Direction.South, DoorState.Closed, DoorState.Closed)]
        [TestCase(Direction.South, DoorState.Closed, DoorState.Hidden)]
        [TestCase(Direction.South, DoorState.Hidden, DoorState.Open)]
        [TestCase(Direction.South, DoorState.Hidden, DoorState.Closed)]
        [TestCase(Direction.South, DoorState.Hidden, DoorState.Hidden)]
        [TestCase(Direction.East, DoorState.Open, DoorState.Open)]
        [TestCase(Direction.East, DoorState.Open, DoorState.Closed)]
        [TestCase(Direction.East, DoorState.Open, DoorState.Hidden)]
        [TestCase(Direction.East, DoorState.Closed, DoorState.Open)]
        [TestCase(Direction.East, DoorState.Closed, DoorState.Closed)]
        [TestCase(Direction.East, DoorState.Closed, DoorState.Hidden)]
        [TestCase(Direction.East, DoorState.Hidden, DoorState.Open)]
        [TestCase(Direction.East, DoorState.Hidden, DoorState.Closed)]
        [TestCase(Direction.East, DoorState.Hidden, DoorState.Hidden)]
        [TestCase(Direction.West, DoorState.Open, DoorState.Open)]
        [TestCase(Direction.West, DoorState.Open, DoorState.Closed)]
        [TestCase(Direction.West, DoorState.Open, DoorState.Hidden)]
        [TestCase(Direction.West, DoorState.Closed, DoorState.Open)]
        [TestCase(Direction.West, DoorState.Closed, DoorState.Closed)]
        [TestCase(Direction.West, DoorState.Closed, DoorState.Hidden)]
        [TestCase(Direction.West, DoorState.Hidden, DoorState.Open)]
        [TestCase(Direction.West, DoorState.Hidden, DoorState.Closed)]
        [TestCase(Direction.West, DoorState.Hidden, DoorState.Hidden)]
        [TestCase(Direction.Up, DoorState.Open, DoorState.Open)]
        [TestCase(Direction.Up, DoorState.Open, DoorState.Closed)]
        [TestCase(Direction.Up, DoorState.Open, DoorState.Hidden)]
        [TestCase(Direction.Up, DoorState.Closed, DoorState.Open)]
        [TestCase(Direction.Up, DoorState.Closed, DoorState.Closed)]
        [TestCase(Direction.Up, DoorState.Closed, DoorState.Hidden)]
        [TestCase(Direction.Up, DoorState.Hidden, DoorState.Open)]
        [TestCase(Direction.Up, DoorState.Hidden, DoorState.Closed)]
        [TestCase(Direction.Up, DoorState.Hidden, DoorState.Hidden)]
        [TestCase(Direction.Down, DoorState.Open, DoorState.Open)]
        [TestCase(Direction.Down, DoorState.Open, DoorState.Closed)]
        [TestCase(Direction.Down, DoorState.Open, DoorState.Hidden)]
        [TestCase(Direction.Down, DoorState.Closed, DoorState.Open)]
        [TestCase(Direction.Down, DoorState.Closed, DoorState.Closed)]
        [TestCase(Direction.Down, DoorState.Closed, DoorState.Hidden)]
        [TestCase(Direction.Down, DoorState.Hidden, DoorState.Open)]
        [TestCase(Direction.Down, DoorState.Hidden, DoorState.Closed)]
        [TestCase(Direction.Down, DoorState.Hidden, DoorState.Hidden)]
        public void ShouldCreatePassageBetweenLocations(Direction targetLocationDirection,
            DoorState fromHereDoorState, DoorState fromThereDoorState)
        {
            var location1 = new Location();
            var location2 = new Location();

            location1.CreateDoor(location2, targetLocationDirection, fromHereDoorState, fromThereDoorState);

            switch (targetLocationDirection)
            {
                case Direction.North:
                    {
                        Assert.That(location1.North.isOpen, Is.EqualTo(fromHereDoorState));
                        Assert.That(location2.South.isOpen, Is.EqualTo(fromThereDoorState));
                        break;
                    }
                case Direction.South:
                    {
                        Assert.That(location1.South.isOpen, Is.EqualTo(fromHereDoorState));
                        Assert.That(location2.North.isOpen, Is.EqualTo(fromThereDoorState));
                        break;
                    }
                case Direction.East:
                    {
                        Assert.That(location1.East.isOpen, Is.EqualTo(fromHereDoorState));
                        Assert.That(location2.West.isOpen, Is.EqualTo(fromThereDoorState));
                        break;
                    }
                case Direction.West:
                    {
                        Assert.That(location1.West.isOpen, Is.EqualTo(fromHereDoorState));
                        Assert.That(location2.East.isOpen, Is.EqualTo(fromThereDoorState));
                        break;
                    }
                case Direction.Up:
                    {
                        Assert.That(location1.Up.isOpen, Is.EqualTo(fromHereDoorState));
                        Assert.That(location2.Down.isOpen, Is.EqualTo(fromThereDoorState));
                        break;
                    }
                case Direction.Down:
                    {
                        Assert.That(location1.Down.isOpen, Is.EqualTo(fromHereDoorState));
                        Assert.That(location2.Up.isOpen, Is.EqualTo(fromThereDoorState));
                        break;
                    }
            }

        }



    }
}
