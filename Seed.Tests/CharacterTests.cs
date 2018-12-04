using System;
using System.Collections.Generic;
using System.Text;
using Seed.Characters;
using NUnit.Framework;
using FluentAssertions;
using Seed.Items;
using Seed.Locations;

namespace Seed.Tests
{
    [TestFixture]
    public class CharacterTests
    {
        [Test]
        [Category("Character.ReceiveItems")]
        public void ShouldHaveDamageAndArmorEqualToProperValues()
        {
            var location = new Location();
            var human = new Human(presentLocation: location, strength: 2, armor: 1);

            human.ReceiveItems(new List<Item>()
            {
                new Weapon(damage: 1),
                new Weapon(damage: 2),
                new Armor(toughness: 1),
                new Armor(toughness: 5)
            });

            human.Damage.Should().Be(8);
            human.Armor.Should().Be(6);
        }

        [Test]
        [Category("Character.ReceiveItems")]
        public void ShouldThrowException()
        {
            var location = new Location();
            var player = new Player(presentLocation: location);
            
            Action act = () => player.ReceiveItems(new List<Item>());

            act.Should().Throw<Exception>().WithMessage("To nie jest metoda dla gracza!");
        }



    }
}
