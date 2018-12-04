using Seed.Locations;

namespace Seed.Characters
{
    public interface IAttack
    {
        bool IsAggressive { get; }
        void Attack(Character character, World world);
    }
}
