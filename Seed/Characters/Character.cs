using System;
using System.Collections.Generic;
using Seed.Items;
using Seed.Locations;

namespace Seed.Characters
{
    public abstract class Character
    {
        private int _HP;
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Overview { get; protected set; }
        public int HP
        {
            get
            {
                return _HP;
            }
            set
            {
                _HP = value;
                if (_HP < 0)
                {
                    _HP = 0;
                }
                else if (_HP > MaxHP)
                {
                    _HP = MaxHP;
                }
            }
        }
        public int MaxHP { get; protected set; }
        public uint Strength { get; protected set; }
        public uint Damage { get; protected set; }
        public uint Armor { get; protected set; }
        public Location presentLocation { get; protected set; }
        public List<Item> Inventory { get; protected set; }

        public Character(string name, string description, string overview, int hp, uint strength, uint armor,
            Location presentLocation)
        {
            Name = name;
            HP = MaxHP = hp;
            Strength = strength;
            Damage = 3 * strength;
            Armor = armor;
            this.presentLocation = presentLocation;
            presentLocation.CharactersInLocation.Add(this);
            Description = description;
            Overview = overview;
            Inventory = new List<Item>();
        }

        public void ReceiveItems(List<Item> items)
        {
            if (this is Player)
            {
                throw new Exception("To nie jest metoda dla gracza!");
            }

            uint addDamage = 0, addArmor = 0;
            Inventory.AddRange(items);

            foreach (var item in items)
            {
                if (item is Weapon)
                {
                    addDamage = GreaterDamage(addDamage, (Weapon)item);
                }
                else if (item is Armor)
                {
                    addArmor = GreaterArmor(addArmor, (Armor)item);
                }
            }

            Armor += addArmor;
            Damage += addDamage;
        }

        private uint GreaterDamage(uint damage, Weapon item)
        {
            return item.Damage > damage ? item.Damage : damage;
        }

        private uint GreaterArmor(uint armor, Armor item)
        {
            return item.Toughness > armor ? item.Toughness : armor;
        }
    }
}
