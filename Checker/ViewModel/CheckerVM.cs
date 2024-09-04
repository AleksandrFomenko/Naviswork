using Checker.model;
using CommunityToolkit.Mvvm.ComponentModel;


namespace Checker.ViewModel
{
    public class CheckerVM: ObservableObject
    {
        private CheckerModel _model;
        public CheckerVM(CheckerModel checkerModel)
        {
            _model = checkerModel;
        }
    }
}