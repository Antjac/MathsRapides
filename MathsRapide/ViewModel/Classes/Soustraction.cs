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
        public override string OperateurPrononciation => "Moins";
        public override bool IsValid(int res)
        {
            return (ValA - ValB == res);
        }

        public override Choices GetChoices()
        {
            Choices numbers = new Choices();
            for (int i = Math.Max(ValA - ValB - 3, 0); i <= (ValA - ValB + 3); i++)
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
