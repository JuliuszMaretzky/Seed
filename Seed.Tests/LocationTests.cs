﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Seed.Characters;
using Seed.Locations;
using FluentAssertions;

namespace Seed.Tests
{
    [TestFixture]
    class LocationTests
    {
        [Test]
        [Category("Location.Constructor")]
        public void ShouldHaveAllDirectionsClosedAndNullAfterNoParametersConstructor()
        {
            Location location = new Location();

            var direction = new Door(DoorState.Closed, null);

            location.North.Should().Be(direction);
            location.South.Should().Be(direction);
            location.East.Should().Be(direction);
            location.West.Should().Be(direction);
            location.Up.Should().Be(direction);
            location.Down.Should().Be(direction);
        }

        [Test]
        public void ShouldHaveConnectionWithNeighbourLocation()
        {
            var location1 = new Location();
            var location2 = new Location(parentLocation: location1, parentDirection: Direction.North);
            var player = new Player(presentLocation: location1);

            player.Move(Direction.South);

            player.presentLocation.Should().Be(location2);
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
        [Category("Location.ChangeDoorState")]
        public void ShouldChangeStateOfDoor(DoorState Before, DoorState After)
        {
            Location location1 = new Location();
            Location location2 = new Location(parentDirection: Direction.North, parentLocation: location1,
                fromParentDoorState: Before);

            location1.ChangeDoorState(Direction.South, After);

            location1.South.DoorState.Should().Be(After);
        }

        [Test]
        [Category("Location.CreateDoor")]
        public void ShouldCreatePassageBetweenLocations(
            [Values]Direction targetLocationDirection,
            [Values]DoorState fromHereDoorState,
            [Values]DoorState fromThereDoorState)
        {
            var location1 = new Location();
            var location2 = new Location();

            location1.CreateDoor(location2, targetLocationDirection, fromHereDoorState, fromThereDoorState);

            switch (targetLocationDirection)
            {
                case Direction.North:
                    {
                        location1.North.DoorState.Should().Be(fromHereDoorState);
                        location2.South.DoorState.Should().Be(fromThereDoorState);
                    }
                    break;
                case Direction.South:
                    {
                        location1.South.DoorState.Should().Be(fromHereDoorState);
                        location2.North.DoorState.Should().Be(fromThereDoorState);
                    }
                    break;
                case Direction.East:
                    {
                        location1.East.DoorState.Should().Be(fromHereDoorState);
                        location2.West.DoorState.Should().Be(fromThereDoorState);
                    }
                    break;
                case Direction.West:
                    {
                        location1.West.DoorState.Should().Be(fromHereDoorState);
                        location2.East.DoorState.Should().Be(fromThereDoorState);
                    }
                    break;
                case Direction.Up:
                    {
                        location1.Up.DoorState.Should().Be(fromHereDoorState);
                        location2.Down.DoorState.Should().Be(fromThereDoorState);
                    }
                    break;
                case Direction.Down:
                    {
                        location1.Down.DoorState.Should().Be(fromHereDoorState);
                        location2.Up.DoorState.Should().Be(fromThereDoorState);
                    }
                    break;
            }

        }

        [Test]
        [Category("Location.Constructor")]
        public void ShouldCreateThreeWayPassage()
        {
            var location1 = new Location();
            var location2=new Location(parentDirection:Direction.West, parentLocation:location1);
            var location3 = new Location(parentDirection: Direction.West, parentLocation: location1);

            location1.East.Location.Should().Be(location3);
            location1.East.DoorState.Should().Be(DoorState.Open);
            location2.West.Location.Should().Be(location1);
            location2.West.DoorState.Should().Be(DoorState.Open);
            location3.West.Location.Should().Be(location1);
            location3.West.DoorState.Should().Be(DoorState.Open);
        }
    }
}
