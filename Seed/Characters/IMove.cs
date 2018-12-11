using Seed.Locations;

namespace Seed.Characters
{
    public interface IMove
    {
        bool IsLazy { get; }
        void Move(Direction direction);
    }
}
