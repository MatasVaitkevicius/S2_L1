using System;
using System.Collections.Generic;
using System.IO;

namespace S2_L1_Web
{
    public partial class Forma1 : System.Web.UI.Page
    {
        const string friendsNetworkData = "U31DUOM.txt"; //Duomenų failas, kuriame yra moksleivių ryšiai
        const string networkData = "U32DUOM.txt"; //Duomenų failas, kuriame yra ieškomi moksleivių ryšiai
        const string resultsFile = "U3REZ.TXT"; // Pradiniai duomenys ir rezultatai išspausdinti lentele failas

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var pupilsContainer = ReadPupilsData(friendsNetworkData); // Mokinių konteineris
            var network = ReadNetworkData(networkData); //Kuriuos rysius surasti
            var pupilContainerClone = pupilsContainer.ShallowCopy(); //Mokinių konteinerio kopija
            var alreadyKnowTest = true; // Ar jau pažįstami testas
            var networkList = new List<string>(); // Ryšių sąrašas
            var removedPupil = new Pupil(); // Išimtas moksleivis
            var mutualFriends = new List<string>(); // Bendri draugai
            var alreadyBeenPupils = new List<string>(); // Jau buvę draugai

            PrintToReportTable(pupilsContainer, network, alreadyKnowTest, networkList, removedPupil, mutualFriends, alreadyBeenPupils, friendsNetworkData, networkData, resultsFile);
            var printToWeb = File.ReadAllText(Server.MapPath(resultsFile));
            TextBox1.Text = printToWeb;
        }

        /// <summary>
        /// Nuskaito moksleivių duomenys
        /// </summary>
        /// <param name="file"> Duomenų failas </param>
        /// <returns> Moksleivių konteinrį </returns>
        PupilsContainer ReadPupilsData(string file)
        {
            var pupils = new PupilsContainer();

            using (StreamReader reader = new StreamReader(Server.MapPath(file)))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    var values = line.Split(' ');
                    var name = values[0];
                    var friendsCount = int.Parse(values[1]);
                    var friends = new List<string>();
                    for (int i = 2; i < values.Length; i++)
                    {
                        friends.Add(values[i]);
                    }

                    pupils.AddPupil(new Pupil(name, friendsCount, friends));
                    line = reader.ReadLine();
                }
            }
            return pupils;
        }

        /// <summary>
        /// Nuskaito ieškomų moksleivių ryšius
        /// </summary>
        /// <param name="file"></param>
        /// <returns> Sąrašą ieškomų moksleivių ryšių </returns>
        List<PupilsNetwork> ReadNetworkData(string file)
        {
            var network = new List<PupilsNetwork>();

            using (StreamReader reader = new StreamReader(Server.MapPath(file)))
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    var values = line.Split(' ');
                    var firstFriend = values[0];
                    var secondFriend = values[1];

                    network.Add(new PupilsNetwork(firstFriend, secondFriend));
                    line = reader.ReadLine();
                }
            }
            return network;
        }

        /// <summary>
        /// Rekursijos metodas
        /// </summary>
        /// <param name="pupilsContainer"> Moksleiviu konteineris </param>
        /// <param name="firstPupil"> Pirmojo ieskomo moksleivio vardas </param>
        /// <param name="secondPupil"> Antrojo ieskomo moksleivio vardas </param>
        /// <param name="alreadyKnowTest"> Testas ar jau pažįstamas</param>
        /// <param name="networkList"> Sąrašas, kuriame saugomi tolimesni ryšiai </param>
        /// <param name="removedPupil"> Išimtas moksleivis </param>
        /// <param name="mutualFriends"> Bendri draugai </param>
        /// <returns> Rysio apibudinima </returns>
        static string NetworkResult(PupilsContainer pupilsContainer, string firstPupil, string secondPupil, bool alreadyKnowTest, List<string> networkList, Pupil removedPupil, List<string> mutualFriends,
            List<string> alreadyBeenPupils)
        {
            if (networkList.Count != 0) // Tikrina ar ryšių sąrašas tuščias, jei ne tuščias išima moksleivį, kurį pridėjome praeitą kartą kviesdami metodą
            {
                networkList.RemoveAt(0);
            }
            for (int i = 0; i < pupilsContainer.Count; i++)
            {
                if (pupilsContainer.GetPupil(i).Name == firstPupil)
                {
                    if (alreadyKnowTest) // Tikrina ar jau pažįstami
                    {
                        if (FriendsCheck(pupilsContainer.GetPupil(i).Friends, secondPupil))
                        {
                            return "Jau pazistami";
                        }
                        else
                        {
                            alreadyKnowTest = false;
                        }
                    }
                    for (int j = 0; j < pupilsContainer.GetPupil(i).Friends.Count; j++)
                    {
                        if (FirstNetwork(pupilsContainer, pupilsContainer.GetPupil(i).Friends[j], secondPupil))
                        {
                            mutualFriends.Add(pupilsContainer.GetPupil(i).Friends[j]);
                            return $"Bendri pazistami: {string.Join(", ", mutualFriends)}";
                        }

                        if (NetworkList(alreadyBeenPupils, pupilsContainer.GetPupil(i).Friends[j])) // Tikrina ar ryšių sąraše nėra to pačio mokinio
                        {
                            alreadyBeenPupils.Add(pupilsContainer.GetPupil(i).Name);
                            networkList.Add(pupilsContainer.GetPupil(i).Friends[j]);
                        }

                        if (j + 1 == pupilsContainer.GetPupil(i).Friends.Count) // Tikrina ar baigėsi moksleivio draugai
                        {
                            if (networkList.Count != 0) // Jeigu ryšių sąraše kažkas yra eina į vidu ir kviečia save
                            {
                                alreadyBeenPupils.Add(pupilsContainer.GetPupil(i).Name);
                                pupilsContainer.RemovePupil(i);
                                return NetworkResult(pupilsContainer, networkList[0], secondPupil, alreadyKnowTest, networkList, removedPupil, mutualFriends, alreadyBeenPupils);
                            }
                        }
                    }
                }
            }
            return "Negali susipazinti";
        }

        /// <summary>
        /// Tikrina moksleivio draugus
        /// </summary>
        /// <param name="friendNames"> Moksleivio draugų vardai </param>
        /// <param name="resultPupil"> Ieškomo moksleivio vardas </param>
        /// <returns> Taip arba ne </returns>
        static bool FriendsCheck(List<string> friendNames, string resultPupil)
        {
            for (int i = 0; i < friendNames.Count; i++)
            {
                if (friendNames[i] == resultPupil)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tikrina moksleivio draugo draugus
        /// </summary>
        /// <param name="pupilsContainer"> Moksleivių konteineris </param>
        /// <param name="friendOfFriend"> Moksleivio draugas </param>
        /// <param name="resultPupil"> Ieškomo moksleivio vardas </param>
        /// <returns>Taip arba ne </returns>
        static bool FirstNetwork(PupilsContainer pupilsContainer, string friendOfFriend, string resultPupil)
        {
            for (int i = 0; i < pupilsContainer.Count; i++)
            {
                if (pupilsContainer.GetPupil(i).Name == friendOfFriend)
                {
                    if (FriendsCheck(pupilsContainer.GetPupil(i).Friends, resultPupil))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Ryšių sąraše tikrina ar yra išimtas moksleivis
        /// </summary>
        /// <param name="networkList"> Ryšių sąrašas </param>
        /// <param name="removedPupil"> Išimtas moksleivis</param>
        /// <returns> Taip arba ne </returns>
        static bool NetworkList(List<string> alreadyBeenPupils, string givenPupil)
        {
            for (int i = 0; i < alreadyBeenPupils.Count; i++)
            {
                if (alreadyBeenPupils[i] == givenPupil)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Spausdina duomenys ir rezultatus į tekstinį failą
        /// </summary>
        /// <param name="pupilsContainer"> Moksleivių konteineris </param>
        /// <param name="network"> Ieškomų moksleivių ryšys </param>
        /// <param name="alreadyKnowTest"> Ar jau pažįstami </param>
        /// <param name="networkList"> Ryšių sąrašas </param>
        /// <param name="removedPupil"> Pašalintas moksleivis </param>
        /// <param name="mutualFriends"> Bendri draugai </param>
        /// <param name="friendsNetworkData"> Moksleivių ryšių duomenų failas </param>
        /// <param name="networkData"> Ieškomų moksleivių ryšių duomenų failas </param>
        /// <param name="resultsFile"> Rezultatų failas </param>
        void PrintToReportTable(PupilsContainer pupilsContainer, List<PupilsNetwork> network, bool alreadyKnowTest, List<string> networkList, Pupil removedPupil, List<string> mutualFriends, List<string> alreadyBeenPupils,
        string friendsNetworkData, string networkData, string resultsFile)
        {
            using (StreamWriter writer = new StreamWriter(Server.MapPath(resultsFile))) 
            {
                writer.WriteLine("Pradiniai Duomenys");
                writer.WriteLine();
                writer.WriteLine(friendsNetworkData);
                writer.WriteLine("Moksleiviai");
                writer.WriteLine(new string('-', 85));
                for (int i = 0; i < pupilsContainer.Count; i++)
                {
                    writer.WriteLine("| {0, -25} | {1, 25} | {2, -25} |", "Moksleivio vardas", "Kiek draugų turi", "Moksleivio draugai");
                    writer.WriteLine(pupilsContainer.GetPupil(i).PrintPupilToReportTable());
                    writer.WriteLine(new string('-', 85));
                }
                writer.WriteLine();
                writer.WriteLine("Ieškomų moksleivių ryšiai");
                writer.WriteLine(networkData);
                writer.WriteLine(new string('-', 76));
                writer.WriteLine("| {0, -29} | {1, -40} |", "Pirmojo moksleivio vardas", "Antrojo moksleivio vardas");
                writer.WriteLine(new string('-', 76));
                foreach (var friend in network)
                {
                    writer.WriteLine(friend.PrintNetworkToReportTable());
                }
                writer.WriteLine(new string('-', 76));

                writer.WriteLine();
                writer.WriteLine("Rezultatai");
                writer.WriteLine(resultsFile);
                writer.WriteLine(new string('-', 119));
                writer.WriteLine("| {0, -29} | {1, -40} | {2, -40} |", "Pirmojo moksleivio vardas", "Antrojo moksleivio vardas", "Ryšys");
                writer.WriteLine(new string('-', 119));
                foreach (var friend in network)
                {
                    var pupilsContainerCopy = pupilsContainer.ShallowCopy();
                    var result = NetworkResult(pupilsContainerCopy, friend.FirstFriend, friend.SecondFriend, alreadyKnowTest, networkList, removedPupil, mutualFriends, alreadyBeenPupils);
                    writer.WriteLine($"{friend.PrintNetworkToReportTable()} {result,-40} |");
                }
                writer.WriteLine(new string('-', 119));
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}