namespace Seed.Characters
{
    public interface IFollow
    {
        bool IsFollowing { get; }
        uint StepsRemaining { get; }
        Character FollowedCharacter { get; }

        void ThinkAboutFollowing();
        void Follow();
    }
}
