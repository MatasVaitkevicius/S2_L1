using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S2_L1
{
    // Ieškomi moksleivių poros
    class PupilsNetwork
    {
        public string FirstFriend { get; set; } // Pirmasis moksleivis
        public string SecondFriend { get; set; } // Antrasis moksleivis

        // Konstruktorius su parametrais
        public PupilsNetwork(string firstFriend, string secondFriend)
        {
            FirstFriend = firstFriend;
            SecondFriend = secondFriend;
        }

        // Konstruktroius be parametrų
        public PupilsNetwork()
        {

        }

        public string PrintToConsoleNetwork()
        {
            return $"{FirstFriend} {SecondFriend,5}";
        }

        public string PrintNetworkToReportTable()
        {
            return $"| {FirstFriend, -30}| {SecondFriend, -40} |";
        }
    }
}
