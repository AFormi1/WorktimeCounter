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

            //Set DataContext for entire Stackpanel (or whatever needed)
            MyData.DataContext = LogicClassInstance;

            //Start Clock Automatically
            LogicClassInstance.WPF_UpdateTasks();


        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            //Start Clock Automatically
            LogicClassInstance.Reset = true;
        }
    }
}
