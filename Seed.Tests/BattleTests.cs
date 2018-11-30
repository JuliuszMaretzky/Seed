using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Seed.Characters;
using Seed.Locations;
using Seed.Scenarios;

namespace Seed.Tests
{
    [TestFixture]
    public class BattleTests
    {
        [TestCase((uint)3, (uint)2, (uint)1, (uint)7, (uint)8, (uint)2)]
        public void ShouldSetDamageOnCertainValues(uint playerStrength, uint playerArmor, uint foeStrength,
            uint foeArmor, uint expectedPlayerDamage, uint expectedFoeDamage)
        {
            var location = new Location();
            var player = new Player(strength: playerStrength, armor: playerArmor, presentLocation: location);
            var human = new Human(strength: foeStrength, armor: foeArmor, presentLocation: location);
            

            var computedDamage=Battle.ComputeDamage(player, human);
            uint playerDamage=computedDamage.Item1, foeDamage=computedDamage.Item2;

            Assert.That(playerDamage, Is.EqualTo(expectedPlayerDamage));
            Assert.That(foeDamage, Is.EqualTo(expectedFoeDamage));
        }
    }
}
