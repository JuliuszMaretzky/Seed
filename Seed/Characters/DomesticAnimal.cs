using System;
using Seed.Locations;

namespace Seed.Characters
{
    public class DomesticAnimal : Character, IFollow
    {
        public bool IsFollowing { get; private set; }
        public uint StepsRemaining { get; set; }
        public Character FollowedCharacter { get; private set; }

        public DomesticAnimal(string name = "Sierściuch", string description = "szuka jakichś śmieci do zjedzenia.",
            string overview = "Zwykły wypłosz, jakich pełno po miastach i wsiach.", int hp = 2, uint strength = 1,
            uint armor = 0, Location presentLocation = null) :
            base(name, description, overview, hp, strength, armor, presentLocation)
        {
            IsFollowing = false;
            StepsRemaining = 0;
            FollowedCharacter = null;
        }

        public void ThinkAboutFollowing()
        {
            presentLocation.CharactersInLocation.Remove(this);

            if (presentLocation.CharactersInLocation.Count > 0)
            {
                var chooseFollowedCharacter = new Random().Next(presentLocation.CharactersInLocation.Count);
                FollowedCharacter = presentLocation.CharactersInLocation[chooseFollowedCharacter];
                IsFollowing = true;
                StepsRemaining = (uint)(new Random().Next(10) + 1);
            }

            presentLocation.CharactersInLocation.Add(this);
        }

        public void Follow()
        {
            if (CanFollowFollowedCharacter(presentLocation.North, FollowedCharacter.presentLocation) ||
                CanFollowFollowedCharacter(presentLocation.South, FollowedCharacter.presentLocation) ||
                CanFollowFollowedCharacter(presentLocation.East, FollowedCharacter.presentLocation) ||
                CanFollowFollowedCharacter(presentLocation.West, FollowedCharacter.presentLocation) ||
                CanFollowFollowedCharacter(presentLocation.Up, FollowedCharacter.presentLocation) ||
                CanFollowFollowedCharacter(presentLocation.Down, FollowedCharacter.presentLocation))
            {
                presentLocation = FollowedCharacter.presentLocation;
                StepsRemaining--;
                if (StepsRemaining == 0)
                {
                    IsFollowing = false;
                    FollowedCharacter = null;
                }
            }
            else
            {
                IsFollowing = false;
                StepsRemaining = 0;
                FollowedCharacter = null;
            }
        }

        private static bool CanFollowFollowedCharacter(Door gate, Location followedCharacterPresentLocation)
        {
            return gate.Location == followedCharacterPresentLocation && gate.DoorState != DoorState.Closed;
        }
    }
}
