using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Seed.Characters;
using Seed.Items;
using Seed.Locations;
using Seed.Scenarios;
using FluentAssertions;
using Seed.Items;

namespace Seed.Tests
{
    [TestFixture]
    public class BattleTests
    {
        //[TestCase((uint), (uint), (uint), (uint), (uint), (uint), (uint), (uint), (uint), (uint))]
        [TestCase((uint)3, (uint)2, (uint)1, (uint)7, (uint)0, (uint)0, (uint)0, (uint)0, (uint)8, (uint)2)]
        [TestCase((uint)1, (uint)1, (uint)1, (uint)1, (uint)0, (uint)0, (uint)0, (uint)0, (uint)3, (uint)3)]
        [TestCase((uint)4, (uint)2, (uint)5, (uint)2, (uint)1, (uint)1, (uint)1, (uint)1, (uint)13, (uint)17)]
        [TestCase((uint)50, (uint)20, (uint)10, (uint)40, (uint)25, (uint)20, (uint)50, (uint)40, (uint)43, (uint)20)]
        [TestCase((uint)10, (uint)30, (uint)10, (uint)300, (uint)5, (uint)20, (uint)30, (uint)400, (uint)1, (uint)1)]
        public void ShouldSetDamageOnCertainValues(uint playerStrength, uint playerArmor, uint foeStrength,
            uint foeArmor, uint playerWeaponDamage, uint playerJacketToughness, uint foeWeaponDamage,
            uint foeJacketToughness, uint expectedPlayerDamage, uint expectedFoeDamage)
        {
            var location = new Location();
            var playerWeapon=new Weapon(damage:playerWeaponDamage);
            var playerJacket = new Armor(toughness: playerJacketToughness);
            var foeItemList = new List<Item>()
            {
                new Weapon(damage:foeWeaponDamage),
                new Armor(toughness:foeJacketToughness)
            };
            var player = new Player(strength: playerStrength, armor: playerArmor, presentLocation: location);
            player.PickUp(playerWeapon);
            player.PickUp(playerJacket);
            player.Wear(playerWeapon);
            player.Wear(playerJacket);
            var human = new Human(strength: foeStrength, armor: foeArmor, presentLocation: location);
            human.ReceiveItems(foeItemList);
            
            var computedDamage=Battle.ComputeDamage(player, human);
            uint playerDamage=computedDamage.Item1, foeDamage=computedDamage.Item2;

            playerDamage.Should().Be(expectedPlayerDamage);
            foeDamage.Should().Be(expectedFoeDamage);
        }
    }
}
