using System;
using System.Collections.Generic;
using System.Web.UI.MobileControls;
using System.Windows;

namespace WorkingtimeCounter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LogicClass LogicClassInstance = new LogicClass();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow(StartupEventArgs startparameters)
        {



            InitializeComponent();

            //Define Window Location
            Rect desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Bottom - Height;
            taskBarItem.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;



            //Set DataContext for entire Stackpanel (or whatever needed)
            MyData.DataContext = LogicClassInstance;

            //Standardwerte
            LogicClassInstance.TgtHours = 10;
            LogicClassInstance.TgtBreakMinutes = 30;
            LogicClassInstance.TgtBreakSeconds = 0;

            //Define StartUpValues from Command Line
            if (startparameters.Args.Length > 0)
            {

                //Stelle 2-3 = Pausenzeit
                //Stelle 0-1 = Arbeitszeit

                for (int i = 0; i < startparameters.Args.Length; i++)
                {
                    if (startparameters.Args[i] == "-Worktime" && startparameters.Args[i+1] != null)
                    {
                        LogicClassInstance.TgtHours = Convert.ToDouble(startparameters.Args[i+1]);
                    }
                    if (startparameters.Args[i] == "-Break" && startparameters.Args[i + 1] != null)
                    {
                        LogicClassInstance.TgtBreakMinutes = Convert.ToDouble(startparameters.Args[i+1]);
                    }
                } 


   
            }



            LogicClassInstance.WPF_UpdateTasks();


        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            //Start Clock Automatically
            LogicClassInstance.Reset = true;
        }

        private void KeepOnTop_Click(object sender, RoutedEventArgs e)
        {
            Topmost ^= true;
        }
    }
}
