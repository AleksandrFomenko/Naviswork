using System.Windows;
using Checker.ViewModel;

namespace Checker.View
{
    public partial class CheckerView : Window
    {
        public CheckerView(CheckerVM checkerVm)
        {
            DataContext = checkerVm;
            InitializeComponent();
        }
    }
}