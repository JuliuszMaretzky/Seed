using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Seed.Tests
{
    [TestFixture]
    public class WorldTests
    {
        [TestCase("Sypialnia")]
        [TestCase("Twój pokój")]
        [TestCase("Klatka schodowa")]
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

                        Assert.That(player.presentLocation.Name, Is.EqualTo("Sypialnia"));
                    }
                    break;
                case "Twój pokój":
                    {
                        player.Move(Direction.North);
                        player.Move(Direction.East);
                        player.Move(Direction.South);
                        player.Move(Direction.South);
                        player.Move(Direction.West);

                        Assert.That(player.presentLocation.Name, Is.EqualTo("Twój pokój"));
                    }
                    break;
                case "Klatka schodowa":
                    {
                        player.Move(Direction.North);
                        player.Move(Direction.East);
                        player.Move(Direction.South);
                        player.Move(Direction.South);
                        player.Move(Direction.South);

                        Assert.That(player.presentLocation.Name, Is.EqualTo("Przedsionek"));
                    }
                    break;
            }
        }
    }
}
