using Checker.model;
using Checker.View;
using Checker.ViewModel;

namespace Checker.App
{
    public abstract class CheckerApplication
    {
        public static void goApp()
        { 
            var cm = new CheckerModel();
            var cvm = new CheckerVM(cm);
            var cv = new CheckerView(cvm);
            cv.ShowDialog();
        }
    }
}