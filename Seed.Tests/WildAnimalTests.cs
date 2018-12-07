using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using Seed.Locations;
using Seed.Characters;
using Seed.Scenarios;

namespace Seed.Tests
{
    [TestFixture]
    public class WildAnimalTests
    {
        [Test]
        [Category("WildAnimal.ThinkAboutFollowing")]
        public void ShouldFollowNearestDeadCharacterExceptThatWhichIsActuallyDead()
        {
            var location = new Location();
            var human1=new Human(presentLocation:location, hp:1);
            var human2=new Human(presentLocation:location, hp:10);
            var human3 = new Human(presentLocation: location, hp: 5);
            var unlucker=new Human(presentLocation:location, hp:0);
            var predator=new WildAnimal(presentLocation:location);
            
            predator.ThinkAboutFollowing();

            predator.IsFollowing.Should().Be(true);
            predator.FollowedCharacter.Should().Be(human1);
            predator.StepsRemaining.Should().BeGreaterThan(0);
        }

        [Test]
        [Category("WildAnimal.Follow")]
        public void ShouldFollowFollowedCharacter()
        {
            var location1=new Location();
            var location2=new Location(parentDirection:Direction.North, parentLocation:location1);
            var human=new Human(presentLocation:location1);
            var hunter=new WildAnimal(presentLocation:location1);
            hunter.ThinkAboutFollowing();
            hunter.StepsRemaining++;
            human.Move(Direction.South);

            hunter.Follow();

            hunter.presentLocation.Should().BeSameAs(human.presentLocation);
        }

        [Test]
        [Category("WildAnimal.Follow")]
        public void ShouldStopFollowIfFollowedCharacterWentTooFar()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var location3 = new Location(parentDirection: Direction.Down, parentLocation: location2);
            var location4 = new Location(parentDirection: Direction.East, parentLocation: location3);
            var someGuy = new Human(presentLocation: location1);
            var badBoi = new DomesticAnimal(presentLocation: location1);
            badBoi.ThinkAboutFollowing();
            badBoi.StepsRemaining = 10;
            someGuy.Move(Direction.South);
            badBoi.Follow();
            someGuy.Move(Direction.Up);
            someGuy.Move(Direction.West);
            badBoi.Follow();

            someGuy.presentLocation.Should().Be(location4);
            badBoi.presentLocation.Should().Be(location2);
            badBoi.IsFollowing.Should().Be(false);
            badBoi.FollowedCharacter.Should().Be(null);
            badBoi.StepsRemaining.Should().Be(0);
        }

        [Test]
        [Category("WildAnimal.Follow")]
        public void ShouldStopFollowIfFollowedCharacterIsBehindClosedDoor()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var player = new Player(presentLocation: location1);
            var bloodyParrot = new DomesticAnimal(presentLocation: location1);
            bloodyParrot.ThinkAboutFollowing();
            bloodyParrot.StepsRemaining = 2;
            player.Move(Direction.South);
            location1.ChangeDoorState(Direction.South, DoorState.Closed);
            bloodyParrot.Follow();

            bloodyParrot.IsFollowing.Should().Be(false);
            bloodyParrot.FollowedCharacter.Should().BeNull();
            bloodyParrot.StepsRemaining.Should().Be(0);
        }

        [Test]
        [Category("WildAnimal.ThinkAboutFollowing")]
        public void ShouldChangeFollowedCharacterToWeaker()
        {
            var location1 = new Location();
            var location2=new Location(parentDirection:Direction.North, parentLocation:location1);
            var poorHuman = new Human(presentLocation: location1, hp:5);
            var morePoorHuman = new Human(presentLocation: location2, hp: 3);
            var mosquito=new WildAnimal(presentLocation:location1);
            mosquito.ThinkAboutFollowing();
            poorHuman.Move(Direction.South);
            mosquito.Follow();

            mosquito.ThinkAboutFollowing();

            mosquito.FollowedCharacter.Should().Be(morePoorHuman);
        }

        [Test]
        [Category("WildAnimal.Follow")]
        public void ShouldAttackFollowedCharacterWhenStepsRemainingEqualsToZero()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection:Direction.Up, parentLocation:location1);
            var human = new Human(presentLocation: location1);
            var boar=new WildAnimal(presentLocation:location1);
            boar.ThinkAboutFollowing();
            boar.StepsRemaining = 1;
            human.Move(Direction.Down);
            Battle.Garbage.Clear();

            boar.Follow();

            Battle.Garbage.Should().NotBeEmpty();
        }
    }
}
