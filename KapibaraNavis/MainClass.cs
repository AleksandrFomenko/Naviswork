using System;
using System.IO;
using System.Reflection;
using Autodesk.Navisworks.Api.Plugins;
using System.Drawing;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Application = Autodesk.Navisworks.Api.Application;


namespace KapibaraNavis
{ 
    // Plugin constructor - custom tab attributes
    [Plugin("KapibaraNavis", "Kapibara", DisplayName = "KapibaraNavis")]
    // xaml file - layout of custom ribbon (panel & buttons)
    [RibbonLayout("KapibaraNavis.xaml")]
    // ribbon tab ID from xaml file
    [RibbonTab("KapibaraNavis")]
    // ribbon button ID & icon files
    [Command("ModelChecker", Icon = "1_16.png", LargeIcon = "1_32.png")]
    [Command("ID_Button_1", DisplayName = "Объем", Icon = "1_16.png", LargeIcon = "1_32.png", ToolTip = "Посчитать объем", CanToggle = true, Shortcut = "Shift+X")]
    [Command("ID_Button_2", DisplayName = "Площадь", Icon = "1_16.png", LargeIcon = "1_16.png", ToolTip = "Посчитать площадь", CanToggle = true, Shortcut = "Shift+A")]

    public class MainClass : CommandHandlerPlugin
    {
        public override int ExecuteCommand(string name, params string[] parameters)
        {
            switch (name)
            {
                case "ModelChecker":
                    ExecuteModelChecker();
                    break;

                case "ID_Button_1":
                    ExecuteCalculateVolume();
                    break;

                case "ID_Button_2":
                    ExecuteCalculateArea();
                    break;

                default:
                    
                    break;
            }
            return 0;
        }
        
        private void ExecuteModelChecker()
        {
            try
            {
                var pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var assemblyPath = Path.Combine(pluginDirectory, "Checker.dll");
                var checkerAssembly = Assembly.LoadFrom(assemblyPath);

                var checkerAppType = checkerAssembly.GetType("Checker.App.CheckerApplication");
                var goAppMethod = checkerAppType.GetMethod("goApp", BindingFlags.Public | BindingFlags.Static);
                goAppMethod.Invoke(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске ModelChecker: {ex.Message}");
            }
        }
        
        private void ExecuteCalculateVolume()
        {
            try
            {
                double totalVolume = 0.0;
                ModelItemCollection selectedItems = new ModelItemCollection();

                foreach (ModelItem item in Application.ActiveDocument.CurrentSelection.SelectedItems)
                {
                    DataProperty volumeProperty = item.PropertyCategories.FindPropertyByDisplayName("Объект", "Объем");
                    if (volumeProperty != null)
                    {
                        totalVolume += volumeProperty.Value.ToDoubleVolume() * 0.3048 * 0.3048 * 0.3048; // Перевод в кубические метры
                        selectedItems.Add(item);
                    }
                }

                if (selectedItems.Count > 0)
                {
                    Selection selection = new Selection(selectedItems);
                    Application.ActiveDocument.CurrentSelection.CopyFrom(selection);
                    ShowMyDialog("Объем выборки", totalVolume.ToString("F2"), "Объем = \n", "м³");
                }
                else
                {
                    MessageBox.Show("Нет выбранных элементов с данными по объему.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при расчете объема: {ex.Message}");
            }
        }
        
        private void ExecuteCalculateArea()
        {
            try
            {
                double totalArea = 0.0;
                ModelItemCollection selectedItems = new ModelItemCollection();

                foreach (ModelItem item in Application.ActiveDocument.CurrentSelection.SelectedItems)
                {
                    DataProperty areaProperty = item.PropertyCategories.FindPropertyByDisplayName("Объект", "Площадь");
                    if (areaProperty != null)
                    {
                        totalArea += areaProperty.Value.ToDoubleArea() * 0.3048 * 0.3048; // Перевод в квадратные метры
                        selectedItems.Add(item);
                    }
                }

                if (selectedItems.Count > 0)
                {
                    Selection selection = new Selection(selectedItems);
                    Application.ActiveDocument.CurrentSelection.CopyFrom(selection);
                    ShowMyDialog("Площадь выборки", totalArea.ToString("F2"), "Площадь = \n", "м²");
                }
                else
                {
                    MessageBox.Show("Нет выбранных элементов с данными по площади.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при расчете площади: {ex.Message}");
            }
        }
        
        private void ShowMyDialog(string title, string value, string labelText, string unit)
        {
            using (Form form = new Form())
            {
                form.Text = title;
                form.Size = new Size(400, 150);
                form.StartPosition = FormStartPosition.CenterScreen;

                Label label = new Label()
                {
                    Text = labelText,
                    Location = new Point(50, 30),
                    AutoSize = true
                };

                TextBox textBox = new TextBox()
                {
                    Text = value,
                    Location = new Point(140, 25),
                    Size = new Size(120, 30),
                    ReadOnly = true
                };

                Label unitLabel = new Label()
                {
                    Text = unit,
                    Location = new Point(270, 30),
                    AutoSize = true
                };

                Button okButton = new Button()
                {
                    Text = "OK",
                    Location = new Point(150, 70),
                    DialogResult = DialogResult.OK
                };

                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(unitLabel);
                form.Controls.Add(okButton);
                form.AcceptButton = okButton;

                form.ShowDialog();
            }
        }
    }
}
