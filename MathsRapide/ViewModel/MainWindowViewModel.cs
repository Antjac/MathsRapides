using MathsRapide.ViewModel.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace MathsRapide.ViewModel
{
    internal class MainWindowViewModel :  INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Operation Operation;

   
        public bool IsAdditionAllowed { get; set; }

        public bool IsMultiplicationAllowed { get; set; }

        public bool IsSoustractionAllowed  { get; set; }


        private string _colorRes;
        public string ColorRes
        {
            get => _colorRes;
            set
            {
                if (value == _colorRes) return;
                _colorRes = value;
                OnPropertyChanged();
            }
        }

        private string _averageTimeDisplayed;
        public string AverageTimeDisplayed
        {
            get => _averageTimeDisplayed;
            set
            {
                if (value == _averageTimeDisplayed) return;
                _averageTimeDisplayed = value;
                OnPropertyChanged();
            }
        }

        private string _goodRes;
        public string GoodRes
        {
            get => _goodRes;
            set
            {
                if (value == _goodRes) return;
                _goodRes = value;
                OnPropertyChanged();
            }
        }

        private string _userRes;
        public string UserRes
        {
            get => _userRes;
            set
            {
                if (value == _userRes) return;
                _userRes = value;
                OnPropertyChanged();
            }
        }

        private List<Double> timeElapsed = new List<Double>();

        private Random rand;
        private Stopwatch timer = new Stopwatch();
        private SpeechRecognitionEngine speechRecognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("fr-FR"));

        public int ValA { get { return Operation.ValA; } }
        public int ValB { get { return Operation.ValB; } }

        public string Operateur { get { return Operation.Operateur; } }
        public double AverageTime
        {
            get { return timeElapsed.Average(); }
        }

        public MainWindowViewModel()
        {
            rand = new Random();
            IsAdditionAllowed = true;
            GenerateNew();
            RecognizeVoice();
            speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void GenerateNew()
        {
            var choiceMade = false;
            while (choiceMade == false)
            {
                var op = rand.Next(1, 4);
                switch (op)
                {
                    case 1:
                        if (IsAdditionAllowed)
                        {
                            Operation = new Addition(rand.Next(1, 10), rand.Next(1, 10));
                            choiceMade = true;
                        }
                        break;
                    case 2:
                        if (IsSoustractionAllowed)
                        {
                            var a = rand.Next(1, 10);
                            var b = rand.Next(1, 10);
                            Operation = new Soustraction(Math.Max(a,b), Math.Min(a,b));
                            choiceMade = true;
                        }
                        break;
                    case 3:
                        if (IsMultiplicationAllowed)
                        {
                            Operation = new Multiplication(rand.Next(1, 10), rand.Next(1, 10));
                            choiceMade = true;
                        }
                        break;
                }
            }
            UserRes = String.Empty;
            GoodRes = String.Empty;
            ColorRes = "White";
            this.OnPropertyChanged(nameof(ValA));
            this.OnPropertyChanged(nameof(ValB));
            this.OnPropertyChanged(nameof(Operateur));
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(Operation.GetChoices());
            speechRecognizer.UnloadAllGrammars();
            speechRecognizer.LoadGrammar(new Grammar(grammarBuilder));

            timer.Restart();
            timer.Start();
        }


        private void RecognizeVoice()
        {
            speechRecognizer.SpeechRecognized += speechRecognizer_SpeechRecognized;
            speechRecognizer.SetInputToDefaultAudioDevice();
        }


        private async void speechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            
            UserRes = e.Result.Text;

            if (Operation.CheckValid(int.Parse(UserRes)))
            {
                ColorRes = "Green";
                timer.Stop();
                timeElapsed.Add(timer.Elapsed.TotalMilliseconds / 1000);
                AverageTimeDisplayed = "Temps moyen : " + AverageTime.ToString("F") + " s";
            }
            else
            {
                ColorRes = "Red";
                GoodRes = Operation.GetResult().ToString();
            }

            await Task.Delay(2000);
            GenerateNew();
        }

    }
}
