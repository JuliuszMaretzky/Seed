using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Seed.Characters;
using Seed.Locations;

namespace Seed.Tests
{
    [TestFixture]
    public class WorldTests
    {
        [TestCase("Sypialnia")]
        [TestCase("Twój pokój")]
        [TestCase("Klatka schodowa")]
        [Category("World.Constructor")]
        public void PlayersApartmentShouldBeBuiltCorrectly(string target)
        {
            var home = new World();
            var player = new Player(presentLocation: home.Locations[0]);

            switch (target)
            {
                case "Sypialnia":
                    {
                        player.Move(Direction.North);
                        player.Move(Direction.West);
                        player.Move(Direction.South);
                        player.Move(Direction.South);

                        player.presentLocation.Name.Should().Be("Sypialnia");
                    }
                    break;
                case "Twój pokój":
                    {
                        player.Move(Direction.North);
                        player.Move(Direction.East);
                        player.Move(Direction.South);
                        player.Move(Direction.South);
                        player.Move(Direction.West);

                        player.presentLocation.Name.Should().Be("Twój pokój");
                    }
                    break;
                case "Klatka schodowa":
                    {
                        player.Move(Direction.North);
                        player.Move(Direction.East);
                        player.Move(Direction.South);
                        player.Move(Direction.South);
                        player.Move(Direction.South);

                        player.presentLocation.Name.Should().Be("Przedsionek");
                    }
                    break;
            }
        }
    }
}
