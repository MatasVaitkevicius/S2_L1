namespace S2_L1_Web
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

        // Spausdina duomenis į konsolę
        public string PrintToConsoleNetwork()
        {
            return $"{FirstFriend} {SecondFriend,5}";
        }

        // Spausdina duomenis į lentelę
        public string PrintNetworkToReportTable()
        {
            return $"| {FirstFriend, -30}| {SecondFriend, -40} |";
        }
    }
}
