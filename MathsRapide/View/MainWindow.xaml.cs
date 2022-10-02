using MathsRapide.ViewModel;
using System.Windows;

namespace MathsRapide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainWindowViewModel();
        }
    }
}
