using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Seed.Characters;
using Seed.Items;
using Seed.Locations;
using FluentAssertions;
using Seed.Scenarios;

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
        [Category("Human.Move")]
        public void ShouldMoveToNeighbouringLocationIfDoorIsOpen(Direction parentDirection)
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: parentDirection, parentLocation: location1,
                fromHereDoorState: DoorState.Open);
            var human = new Human("Zenon", hp: 5, strength: 3, presentLocation: location2);

            human.Move(parentDirection);

            human.presentLocation.Should().Be(location1);
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
        [Category("Human.Move")]
        public void ShouldNotMoveToNeighbouringLocationIfDoorIsClosedOrHidden(
            Direction parentDirection, DoorState doorState)
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: parentDirection, parentLocation: location1,
                fromHereDoorState: doorState);
            var human = new Human("Zenon", hp: 5, strength: 3, presentLocation: location2);

            human.Move(parentDirection);

            human.presentLocation.Should().Be(location2);
        }

        [TestCase((uint)10)]
        [TestCase((uint)5)]
        [Category("Human.Attack")]
        public void DefenderShouldBeDeadIfHumanAttacksHumanAndHasAtLeast3TimesMoreStrengthThanDefendersArmor(
            uint defenderArmor)
        {
            var location = new Location();
            var attacker = new Human(strength: 30, presentLocation: location);
            var defender = new Human(armor: defenderArmor, presentLocation: location);

            attacker.Attack(defender);

            Battle.Garbage.Should().Contain(defender);
        }
    }
}
