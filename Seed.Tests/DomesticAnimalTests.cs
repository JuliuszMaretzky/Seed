using NUnit.Framework;
using FluentAssertions;
using Seed.Characters;
using Seed.Locations;

namespace Seed.Tests
{
    [TestFixture]
    public class DomesticAnimalTests
    {
        [Test]
        [Category("DomesticAnimal.ThinkAboutFollowing")]
        public void ShouldStartFollowSomeoneInLocation()
        {
            var location = new Location();
            var someGuy = new Human(presentLocation:location);
            var anotherGuy=new Human(presentLocation:location);
            var poorKitten = new DomesticAnimal(presentLocation:location);

            poorKitten.ThinkAboutFollowing();

            poorKitten.IsFollowing.Should().Be(true);
            poorKitten.FollowedCharacter.Should().NotBeNull();
            poorKitten.StepsRemaining.Should().BeGreaterThan(0);
        }

        [Test]
        [Category("DomesticAnimal.Follow")]
        public void ShouldNotChangeFollowedCharacter()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var location3=new Location(parentDirection:Direction.Down, parentLocation:location2);
            var someGuy=new Human(presentLocation:location1);
            var anotherGuy=new Human(presentLocation:location2);
            var goodBoi=new DomesticAnimal(presentLocation:location1);
            goodBoi.ThinkAboutFollowing();
            goodBoi.StepsRemaining = 4;
            someGuy.Move(Direction.South);
            goodBoi.Follow();
            goodBoi.ThinkAboutFollowing();
            someGuy.Move(Direction.Up);
            anotherGuy.Move(Direction.North);
            goodBoi.Follow();

            someGuy.presentLocation.Should().Be(location3);
            goodBoi.presentLocation.Should().BeSameAs(someGuy.presentLocation);
            someGuy.presentLocation.CharactersInLocation.Should().Contain(goodBoi);
            location1.CharactersInLocation.Should().NotContain(goodBoi);
            location2.CharactersInLocation.Should().NotContain(goodBoi);
            goodBoi.FollowedCharacter.Should().NotBe(anotherGuy);
        }

        [Test]
        [Category("DomesticAnimal.Follow")]
        public void ShouldStopFollowIfFollowedCharacterWentTooFar()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var location3 = new Location(parentDirection: Direction.Down, parentLocation: location2);
            var location4=new Location(parentDirection:Direction.East, parentLocation:location3);
            var someGuy = new Human(presentLocation: location1);
            var goodBoi = new DomesticAnimal(presentLocation: location1);
            goodBoi.ThinkAboutFollowing();
            goodBoi.StepsRemaining = 10;
            someGuy.Move(Direction.South);
            goodBoi.Follow();
            someGuy.Move(Direction.Up);
            someGuy.Move(Direction.West);
            goodBoi.Follow();

            someGuy.presentLocation.Should().Be(location4);
            goodBoi.presentLocation.Should().Be(location2);
            location2.CharactersInLocation.Should().Contain(goodBoi);
            goodBoi.IsFollowing.Should().Be(false);
            goodBoi.FollowedCharacter.Should().Be(null);
            goodBoi.StepsRemaining.Should().Be(0);
        }

        [Test]
        [Category("DomesticAnimal.Follow")]
        public void ShouldStopFollowIfFollowedCharacterIsBehindClosedDoor()
        {
            var location1 = new Location();
            var location2 = new Location(parentDirection: Direction.North, parentLocation: location1);
            var player = new Player(presentLocation: location1);
            var cockatoo = new DomesticAnimal(presentLocation: location1);
            cockatoo.ThinkAboutFollowing();
            cockatoo.StepsRemaining = 2;
            player.Move(Direction.South);
            location1.ChangeDoorState(Direction.South,DoorState.Closed);
            cockatoo.Follow();

            cockatoo.IsFollowing.Should().Be(false);
            cockatoo.FollowedCharacter.Should().BeNull();
            cockatoo.StepsRemaining.Should().Be(0);
        }

        [Test]
        [Category("DomesticAnimal.Follow")]
        public void ShouldStopFollowingIfStepsRemainingFellToZero()
        {
            var location1 = new Location();
            var location2=new Location(parentDirection:Direction.North, parentLocation:location1);
            var location3=new Location(parentDirection:Direction.Up, parentLocation:location2);
            var location4=new Location(parentDirection:Direction.West, parentLocation:location3);
            var player = new Player(presentLocation:location1);
            var cockatoo=new DomesticAnimal(presentLocation:location1);
            cockatoo.ThinkAboutFollowing();
            cockatoo.StepsRemaining = 2;
            player.Move(Direction.South);
            cockatoo.Follow();
            player.Move(Direction.Down);
            cockatoo.Follow();
            player.Move(Direction.East);

            if(cockatoo.StepsRemaining>0)
                cockatoo.Follow();

            player.presentLocation.Should().Be(location4);
            cockatoo.StepsRemaining.Should().Be(0);
            cockatoo.IsFollowing.Should().Be(false);
            cockatoo.presentLocation.Should().Be(location3);
            location3.CharactersInLocation.Should().Contain(cockatoo);
        }

        [Test]
        [Category("DomesticAnimal.ThinkAboutFollowing")]
        public void ShouldNotFollowIfThereIsNoCharactersInLocation()
        {
            var location = new Location();
            var animal=new DomesticAnimal(presentLocation:location);

            animal.ThinkAboutFollowing();

            animal.IsFollowing.Should().Be(false);
        }

        [Test]
        [Category("DomesticAnimal.ThinkAboutFollowing")]
        public void ShouldNotFollowDeadCharacter()
        {
            var location = new Location();
            var slug=new DomesticAnimal(presentLocation:location);
            var corpse=new Human(hp:0, presentLocation:location);

            slug.ThinkAboutFollowing();

            slug.IsFollowing.Should().Be(false);
            slug.FollowedCharacter.Should().BeNull();
            slug.StepsRemaining.Should().Be(0);
        }

        [Test]
        [Category("DomesticAnimal.Follow")]
        public void ShouldStopFollowIfFollowedCharacterDied()
        {
            var location=new Location();
            var hamster=new DomesticAnimal(presentLocation:location);
            var almostDeadMan = new Human(presentLocation:location);
            hamster.ThinkAboutFollowing();
            almostDeadMan.HP = 0;

            hamster.Follow();

            hamster.IsFollowing.Should().Be(false);
            hamster.FollowedCharacter.Should().BeNull();
            hamster.StepsRemaining.Should().Be(0);
        }
    }
}
