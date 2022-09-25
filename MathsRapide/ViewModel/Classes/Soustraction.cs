using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace MathsRapide.ViewModel.Classes
{
    internal class Soustraction : Operation
    {
        public Soustraction(int valA, int valB)
        {
            ValA = valA;
            ValB = valB;
        }

        public override string Operateur => "-";

        public override bool CheckValid(int res)
        {
            return (ValA - ValB == res);
        }

        public override Choices GetChoices()
        {
            Choices numbers = new Choices();
            for (int i = 1; i <= 9; i++)
            {
                numbers.Add(i.ToString());
            }

            return numbers;
        }

        public override int GetResult()
        {
            return (ValA - ValB);
        }
    }
}
