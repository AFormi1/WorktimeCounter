using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;


namespace WorkingtimeCounter
{
    public class LogicClass : FrameworkElement, INotifyPropertyChanged
    {


        #region Auto Clock

        public double TgtHours { get; set; } = 0; //in hours
        public double TgtBreak { get; set; } = 1; //in minutes
        public double TgtBreakSec { get; set; } = 0; //in Seconds
        public double ProgressValue { get; set; } = 0;
        public bool Reset { get; set; } = false;
        private string mClockValue;
        public string ClockValue { get => mClockValue; set { mClockValue = value; OnPropertyChanged(); } }

       // private double mImageRotationLeft;
        public double ImageRotationLeft { get; set; }// => mImageRotationLeft; set { mImageRotationLeft = value; OnPropertyChanged(); } }
        public double ImageRotationRight { get; set; }// => mImageRotationLeft; set { mImageRotationLeft = value; OnPropertyChanged(); } }



        /// <summary>
        /// Runs the Cycle Code until the specified limit and resets the start value when finished
        /// </summary>
        /// <param name="cycleTime">time between two frames in ms</param>
        /// <returns></returns>
        public async Task RunClockAsync()
        {
            DateTime ActTime = DateTime.Now;
            DateTime StartTime = DateTime.Now;
            DateTime ExpectedEndTime = DateTime.Now.AddHours(TgtHours).AddMinutes(TgtBreak).AddSeconds(TgtBreakSec);

            TimeSpan maxSeconds = ExpectedEndTime - StartTime;
            TimeSpan remainingTime;

            double remainingSecondsPerc;

            while (true)
            {
                while (ActTime <= ExpectedEndTime)
                {
                    if (Reset)
                    {
                        StartTime = DateTime.Now;
                        ExpectedEndTime = DateTime.Now.AddHours(TgtHours).AddMinutes(TgtBreak).AddSeconds(TgtBreakSec);
                        maxSeconds = ExpectedEndTime - StartTime;
                        Reset = false;
                    }


                    ActTime = DateTime.Now;

                    remainingTime = ExpectedEndTime - ActTime;


                    remainingSecondsPerc = (remainingTime.Hours * 60 * 60 + remainingTime.Minutes * 60 + remainingTime.Seconds) * 100 /
                                     (maxSeconds.Hours * 60 * 60 + maxSeconds.Minutes * 60 + maxSeconds.Seconds);

                    ClockValue = $"{remainingTime.Hours}:{remainingTime.Minutes}:{remainingTime.Seconds}";

                    ProgressValue = (100 - remainingSecondsPerc);

                    await Task.Delay(10);

                }

                await Task.Delay(10);
            }
        }


        public async Task RotateImagesAsync()
        {
            while (true)
            {
                ImageRotationLeft += 1;
                ImageRotationRight -= 1;
                await Task.Delay(10);
            }
        }



            /// <summary>
            /// Runs the Code and Updates the UI according the specified cycle time/>
            /// </summary>
            public void WPF_UpdateTasks()
        {
            Task.Factory.StartNew(() => RunClockAsync());
            Task.Factory.StartNew(() => RotateImagesAsync());
        }
        #endregion

        #region OropertyChanged      
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}
