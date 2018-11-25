using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

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
            uint playerDamage, foeDamage;

            Battle.ComputeDamage(player, human, out playerDamage, out foeDamage);

            Assert.That(playerDamage, Is.EqualTo(expectedPlayerDamage));
            Assert.That(foeDamage, Is.EqualTo(expectedFoeDamage));
        }
    }
}
