using System;
using System.Linq;
using Seed.Locations;
using Seed.Scenarios;

namespace Seed.Characters
{
    public class WildAnimal:Character, IFollow
    {
        public bool IsFollowing { get; private set; }
        public uint StepsRemaining { get; set; }
        public Character FollowedCharacter { get; private set; }

        public WildAnimal(string name="Szczur", string description="drapie się po zaropiałych ranach.", 
            string overview="Pewnie przenosi więcej chorób niż ty masz włosów na głowie.", 
            int hp=3, uint strength=1, uint armor=5, Location presentLocation=null) : 
            base(name, description, overview, hp, strength, armor, presentLocation)
        {
            IsFollowing = false;
            StepsRemaining = 0;
            FollowedCharacter = null;
        }
        
        public void ThinkAboutFollowing()
        {
            var characterToFollow=
                from characters in presentLocation.CharactersInLocation
                where characters.HP>0 && characters!=this
                orderby characters.HP
                select characters;

            if (characterToFollow.Any())
            {
                FollowedCharacter = characterToFollow.First();
                StepsRemaining=(uint)new Random().Next(1,5);
                IsFollowing = true;
            }
        }

        public void Follow()
        {
            if (FollowedCharacter.HP == 0)
            {
                IsFollowing = false;
                StepsRemaining = 0;
                FollowedCharacter = null;
                return;
            }
            if (CanFollowFollowedCharacter(presentLocation.North, FollowedCharacter.presentLocation) ||
                CanFollowFollowedCharacter(presentLocation.South, FollowedCharacter.presentLocation) ||
                CanFollowFollowedCharacter(presentLocation.East, FollowedCharacter.presentLocation) ||
                CanFollowFollowedCharacter(presentLocation.West, FollowedCharacter.presentLocation) ||
                CanFollowFollowedCharacter(presentLocation.Up, FollowedCharacter.presentLocation) ||
                CanFollowFollowedCharacter(presentLocation.Down, FollowedCharacter.presentLocation))
            {
                presentLocation.CharactersInLocation.Remove(this);
                presentLocation = FollowedCharacter.presentLocation;
                FollowedCharacter.presentLocation.CharactersInLocation.Add(this);
                StepsRemaining--;
                if (StepsRemaining == 0)
                {
                    var defender = FollowedCharacter;
                    IsFollowing = false;
                    FollowedCharacter = null;
                    Battle.Fight(this, defender);
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
