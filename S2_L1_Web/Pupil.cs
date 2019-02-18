using System.Collections.Generic;

namespace S2_L1_Web
{
    //Moksleivio klasė
    class Pupil
    {
        public string Name { get; set; } // Vardas
        public int FriendsCount { get; set; } // Draugų skaičius
        public List<string> Friends { get; set; } // Moksleivio draugai

        // Konstruktorius su parametrais
        public Pupil(string name, int friendsCount, List<string> friends)
        {

            Name = name;
            FriendsCount = friendsCount;
            Friends = friends;
        }

        // Konstruktorius be parametrų
        public Pupil()
        {

        }

        // Spausdinti duomenis į konsolę
        public string PrintToConsolePupil()
        {
            return $"Moksleivio vardas: {Name, 10}| Moksleivio draugu kiekis: {FriendsCount, 5}| Moksleivio draugai: {string.Join(", ", Friends)}";
        }

        // Spausdinti duomenis i lentelę
        public string PrintPupilToReportTable()
        {
            return $"| {Name, -25} | {FriendsCount, 25} | {string.Join(", ", Friends), -25} |";
        }
    }
}
