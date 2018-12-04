using Seed.Locations;

namespace Seed.Items
{
    public class Weapon : Item
    {
        private uint ammo;
        public uint Damage { get; set; }
        public WeaponType Type { get; private set; }
        public uint Ammo
        {
            get { return ammo; }
            set
            {
                if (this.Type == WeaponType.Melee)
                    this.ammo = 1;
                else
                    this.ammo = value;
            }
        }

        public Weapon(string name = "Coś, co może służyć za broń", string description = "aż się pali, " +
            "by komuś przywalić.", uint weight = 1, uint damage = 1, WeaponType type = WeaponType.Melee,
            Location location = null) : base(name, weight, description, location)
        {
            this.Damage = damage;
            this.Type = type;
        }

    }
}
