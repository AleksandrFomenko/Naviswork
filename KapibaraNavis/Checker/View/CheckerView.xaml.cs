using System.Windows;
using KapibaraNavis.Checker.ViewModel;

namespace KapibaraNavis.Checker.View
{
    public partial class CheckerView : Window
    {
        public CheckerView(CheckerVM cv)
        {
            DataContext = cv;
            InitializeComponent();
        }
    }
}