using System;
using System.IO;
using System.Reflection;
using Autodesk.Navisworks.Api.Plugins;


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

    public class MainClass : CommandHandlerPlugin
    {
        public override int ExecuteCommand(string name, params string[] parameters)
        {
            switch (name) 
            {
                case "ModelChecker":
                    string pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string assemblyPath = Path.Combine(pluginDirectory, "Checker.dll");
                    Assembly checkerAssembly = Assembly.LoadFrom(assemblyPath);
                    
                    Type checkerAppType = checkerAssembly.GetType("Checker.App.CheckerApplication");
                    MethodInfo goAppMethod = checkerAppType.GetMethod("goApp", BindingFlags.Public | BindingFlags.Static);
                    goAppMethod.Invoke(null, null);

                    break;

            }
            return 0; 
        }
    }
}
