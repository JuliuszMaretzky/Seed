using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Seed.Characters;
using Seed.Items;
using Seed.Locations;
using Seed.Scenarios;

namespace Seed.Tests
{
    [TestFixture]
    public class BattleTests
    {
        //[TestCase((uint), (uint), (uint), (uint), (uint), (uint), (uint), (uint), (uint), (uint))]
        [TestCase((uint)3, (uint)2, (uint)1, (uint)7, (uint)0, (uint)0, (uint)0, (uint)0, (uint)8, (uint)2)]
        [TestCase((uint)5, (uint)2, (uint)7, (uint)3, (uint)1, (uint)1, (uint)1, (uint)1, (uint)16, (uint)26)]
        [TestCase((uint)2, (uint)6, (uint)3, (uint)10, (uint)1, (uint)2, (uint)2, (uint)1, (uint)5, (uint)9)]
        [TestCase((uint)5, (uint)20, (uint)5, (uint)100, (uint)2, (uint)25, (uint)5, (uint)300, (uint)1, (uint)1)]
        public void ShouldSetDamageOnCertainValues(uint playerStrength, uint playerArmor, uint foeStrength,
            uint foeArmor, uint playerWeaponDamage, uint playerJacketToughness, uint humanWeaponDamage, 
            uint humanJacketToughness, uint expectedPlayerDamage, uint expectedFoeDamage)
        {
            var location = new Location();
            var playerWeapon = new Weapon(location: location, damage: playerWeaponDamage);
            var playerJacket=new Armor(location:location, toughness:playerJacketToughness);
            var humanItemList = new List<Item>()
            {
                new Weapon(damage: humanWeaponDamage),
                new Armor(toughness: humanJacketToughness)
            };
            var player = new Player(strength: playerStrength, armor: playerArmor, presentLocation: location);
            player.PickUp(playerJacket);
            player.PickUp(playerWeapon);
            player.Wear(playerJacket);
            player.Wear(playerWeapon);
            var human = new Human(strength: foeStrength, armor: foeArmor, presentLocation: location);
            human.ReceiveItems(humanItemList);
            
            var fightersDamage = Battle.ComputeDamage(player, human);
            uint playerDamage=fightersDamage.Item1, foeDamage=fightersDamage.Item2;

            Assert.That(playerDamage, Is.EqualTo(expectedPlayerDamage));
            Assert.That(foeDamage, Is.EqualTo(expectedFoeDamage));
        }
    }
}
