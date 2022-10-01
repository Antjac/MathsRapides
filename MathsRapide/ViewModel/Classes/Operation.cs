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
    public abstract class Operation
    {
        private int _valA;
        private int _valB;

        public int ValA
        {
            get => _valA;
            set
            {
                if (value == _valA) return;
                _valA = value;
            }
        }

        public int ValB
        {
            get => _valB;
            set
            {
                if (value == _valB) return;
                _valB = value;
            }
        }

        public abstract string Operateur { get; }

        public abstract string OperateurPrononciation { get; }

        public abstract bool IsValid(int res);

        public abstract int GetResult();

        public abstract  Choices GetChoices();
    }
}
