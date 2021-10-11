using System;
using System.Windows;

namespace WorkingtimeCounter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LogicClass LogicClassInstance = new LogicClass();

        public MainWindow()
        {
            InitializeComponent();

            //Define Window Location
            Rect desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Bottom - Height;
            taskBarItem.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;



            //Set DataContext for entire Stackpanel (or whatever needed)
            MyData.DataContext = LogicClassInstance;

            //Define StartUpValues
            LogicClassInstance.TgtHours = 10;
            LogicClassInstance.TgtBreakMinutes = 30;
            LogicClassInstance.TgtBreakSeconds = 0;

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
