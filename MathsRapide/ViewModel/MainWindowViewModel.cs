using MathsRapide.ViewModel.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Speech.Synthesis;

namespace MathsRapide.ViewModel
{
    internal class MainWindowViewModel :  INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private SpeechSynthesizer speaker = new SpeechSynthesizer();

        public Operation Operation;

        public bool IsSpeaking { get; set; }

        public int Sensibility { get; set; }

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
        private SpeechRecognitionEngine speechRecognizerLocal = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("fr-FR"));

        public int ValA { get { return Operation.ValA; } }
        public int ValB { get { return Operation.ValB; } }

        public string Operateur { get { return Operation.Operateur; } }

        public string OperateurPrononciation { get { return Operation.OperateurPrononciation; } }
        public double AverageTime
        {
            get { return timeElapsed.Average(); }
        }

        public MainWindowViewModel()
        {
            rand = new Random();
            Sensibility = 70;
            IsAdditionAllowed = true;
            RecognizeVoiceLocal();
            GenerateNew();
        }

        private void GenerateNew()
        {
            var choiceMade = false;
            while (choiceMade == false)
            {
                var op = rand.Next(1, 4);
                var a = rand.Next(1, 10);
                var b = rand.Next(1, 10);
                switch (op)
                {
                    case 1:
                        if (IsAdditionAllowed)
                        {
                            Operation = new Addition(a,b);
                            choiceMade = true;
                        }
                        break;
                    case 2:
                        if (IsSoustractionAllowed)
                        {

                            Operation = new Soustraction(Math.Max(a,b), Math.Min(a,b));
                            choiceMade = true;
                        }
                        break;
                    case 3:
                        if (IsMultiplicationAllowed)
                        {
                            Operation = new Multiplication(a, b);
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

            if (IsSpeaking)
            {
                Speak();
            }
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(Operation.GetChoices());
            speechRecognizerLocal.UnloadAllGrammars();
            speechRecognizerLocal.LoadGrammar(new Grammar(grammarBuilder));
            speechRecognizerLocal.RecognizeAsync(RecognizeMode.Multiple);
            timer.Restart();
            timer.Start();

        }

        private void Speak()
        {
            speaker.Rate = 1;
            speaker.Volume = 100;
            speaker.Speak(ValA.ToString() + OperateurPrononciation + ValB.ToString());
        }

        private void RecognizeVoiceLocal()
        {
            speechRecognizerLocal.SpeechRecognized += speechRecognizer_SpeechRecognizedLocal;
            speechRecognizerLocal.SetInputToDefaultAudioDevice();
        }

        

        private async void speechRecognizer_SpeechRecognizedLocal(object sender, SpeechRecognizedEventArgs e)
        {

            if (UserRes != string.Empty)
                return;

            if (e.Result.Confidence < Sensibility / 100)                                                                                                                                                                                                                                   if (e.Result.Confidence < Sensibility / 100)
                return;

            UserRes = e.Result.Text;
            CheckAndShowNext();
        }

        private async void CheckAndShowNext()
        {
            speechRecognizerLocal.RecognizeAsyncStop();
            if (Operation.IsValid(int.Parse(UserRes)))
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
