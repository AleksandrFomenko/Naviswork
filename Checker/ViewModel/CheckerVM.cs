using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Navisworks.Api;
using Checker.model;
using CommunityToolkit.Mvvm.ComponentModel;


namespace Checker.ViewModel
{
    public partial class CheckerVM: ObservableObject
    {
        [ObservableProperty]
        private string _result;
        
        private readonly CheckerModel _model;

        public CheckerVM(CheckerModel checkerModel)
        {
            _result = GetAllModelItems().Count().ToString();
            _model = checkerModel;
        }

        private static IEnumerable<ModelItem> GetAllModelItems()
        {
            var doc = Application.ActiveDocument;
            var allItems = new List<ModelItem>();

            foreach (var model in doc.Models)
            {
                var rootItem = model.RootItem;
                if (rootItem == null) continue;
                var descendants = rootItem.DescendantsAndSelf.Where(i => i.IsInsert).ToList();
                allItems.AddRange(descendants);
            }
            return allItems;
        }

    }
}