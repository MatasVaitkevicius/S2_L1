using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S2_L1
{
    // Moksleivų konteineris
    class PupilsContainer
    {
        public Pupil[] Pupils; // Moksleivių masyvas

        private const int MaxPupils = 100; // Konteinerio dydis

        public int Count { get; private set; } // Skaitliukas

        // Konteinerio konstruktorius
        public PupilsContainer()
        {
            Pupils = new Pupil[MaxPupils];
            Count = 0;
        }

        // Pridėti moksleivį
        public void AddPupil(Pupil pupil)
        {
            Pupils[Count++] = pupil;
        }

        // Pridėti moksleivį pagal indeksą
        public void AddPupil(Pupil pupil, int index)
        {
            Pupils[index] = pupil;
        }

        // Pasiimti moksleivį pagal indeksą
        public Pupil GetPupil(int index)
        {
            return Pupils[index];
        }

        // Pašalinti moksleivį
        public Pupil[] RemovePupil(int indexToRemove)
        {
            Pupils = Pupils.Where(p => p != Pupils[indexToRemove]).ToArray();
            Count--;
            return Pupils;
        }

        // Konteinerio kopija
        public PupilsContainer ShallowCopy()
        {
            return (PupilsContainer)MemberwiseClone();
        }
    }
}
