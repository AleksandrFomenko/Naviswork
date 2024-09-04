using Checker.model;
using Checker.View;
using Checker.ViewModel;

namespace Checker.App
{
    public abstract class CheckerApplication
    {
        public static void goApp()
        { 
            CheckerModel cm = new CheckerModel();
            CheckerVM cvm = new CheckerVM(cm);
            CheckerView cv = new CheckerView(cvm);
            cv.ShowDialog();
        }
    }
}