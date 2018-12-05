using System;
using System.Collections.Generic;
using System.Text;
using Seed.Characters;
using Seed.Items;

namespace Seed.Locations
{
    public class World
    {
        public List<Location> Locations;
        public List<Character> NPCs;
        public List<Item> Items;
        public World()
        {
            Locations = new List<Location>();
            NPCs = new List<Character>();
            Items = new List<Item>();
            //Here map begins
            //0
            Locations.Add(new Location("Toaleta", "Znajdujesz się w świątyni dumania. " +
                "Zza drzwi znajdujących się na północnej ścianie dobiegają dziwne odgłosy. W muszli bulgocze " +
                "woda. Zdaje się, że można tam zanurkować."));
            //1
            Locations.Add(new Location("Korytarz", "Jesteś w korytarzu. Na zachodzie znajduje się " +
                "kuchnia, z której wydobywają się smakowite zapachy walczące dzielnie z potwornym smrodem " +
                "zalatującym z południa.", Direction.South, Locations[0], DoorState.Open, DoorState.Open));
            //2
            Locations.Add(new Location("Kuchnia", "Typowa kuchnia.Coś smakowicie bulgocze w garach na gazie.",
                Direction.East, Locations[1], DoorState.Open, DoorState.Open));
            //3
            Locations.Add(new Location("Salon", "Znajdujesz się w głównym pokoju tego mieszkania. " +
                "Być może ktoś tu kiedyś zaprosi jakichś gości.", Direction.North, Locations[2],
                DoorState.Open, DoorState.Open));
            //4
            Locations.Add(new Location("Sypialnia", "Sypialnia przypomina barłóg, w którym dosłownie " +
                "przed chwilą kotłowały się jakieś rosochate bydlęta. Masz niejasne podejrzenia co do roli, " +
                "którą odegrał w tym przedstawieniu twój sąsiad...", Direction.North, Locations[3],
                DoorState.Open, DoorState.Open));
            //5
            Locations.Add(new Location("Korytarz", "Jesteś w narożniku mieszkania. Dalej na zachód " +
                "widzisz (i czujesz) kuchnię, na południe idzie się do wyjścia.", Direction.West,
                Locations[1], DoorState.Open, DoorState.Open));
            //6
            Locations.Add(new Location("Korytarz", "Długi ten korytarz...", Direction.North,
                Locations[5], DoorState.Open, DoorState.Open));
            //7
            Locations.Add(new Location("Przedsionek", "Jesteś na zakręcie. Północny korytarz doprowadzi " +
                "cię do kuchni, zachodni do twojej sypialni. Na południu znajdują się drzwi na klatkę schodową.",
                Direction.North, Locations[6], DoorState.Open, DoorState.Open));
            //8
            Locations.Add(new Location("Twój pokój", "Jesteś w swoim pokoju. Ciężki zapach starych skarpet " +
                "unosi się w powietrzu, ale czujesz się tu... tak, jak u siebie.", Direction.East,
                Locations[7], DoorState.Open, DoorState.Open));
            //9
            Locations.Add(new Location("Klatka schodowa", "Jesteś na klatce schodowej. Możesz stąd wejść " +
                "do mieszkania na północy, albo też uciec schodami w dół.", Direction.North, Locations[7],
                DoorState.Closed, DoorState.Closed));
            //10
            Locations.Add(new Location("Więzienie dla chowańców"));

            //Here horde of NPCs begins
            //0
            NPCs.Add(new Human("Twoja stara", "gotuje coś na kuchence.", "Twoja rodzicielka jest jak koń - " +
                "jaka jest, każdy widzi.", 5, 1, 1, Locations[2]));
            //1
            NPCs.Add(new Human("Somsiad", "patrzy na ciebie spode łba.", "Potężny, zwalisty, obrzydliwy " +
                "i śmierdzący typ. Do tego zawsze ślinił się do twojej matki. Chcesz mu przyłożyć, ale bez" +
                "jakiejkolwiek broni do niego nie podchodź. Zmiażdży cię.", 10, 5, 3, Locations[2], false));
            NPCs[1].ReceiveItems(new List<Item>() {
                new Food("Zgniły batonik", "lekko zajeżdża pleśnią", 0, 1),
                new Item("Kamyk zielony", 1, "błyszczy niezdrowym światłem")
                });
            //2
            NPCs.Add(new Human("Starszy brat", "chce kogoś zlać.", "Głupi, brzydki i śmierdzący. Prawdopodobnie" +
                "bękart sąsiada", 5, 50, 1, Locations[3], true, true));
            //3
            NPCs.Add(new Player("Chowaniec", presentLocation: Locations[10]));

            //Here stack of Items begins
            //0
            Items.Add(new Weapon("Kij bejzbolowy", "dawno nikomu nie wlał", 1, 5, WeaponType.Melee, Locations[8]));
            //1
            Items.Add(new Food("Jabłko", "błyszczy soczystą czerwienią.", 0, 3, Locations[2]));
            //2
            Items.Add(new Drink("Butelka wody", "stoi na stole.", 0, 3, Locations[2]));
            //3
            Items.Add(new Armor("Plastikowa zbroja", "czeka na bal przebierańców", 1, 3, ArmorType.Jacket,
                Locations[8]));
            //4


        }
    }

}
