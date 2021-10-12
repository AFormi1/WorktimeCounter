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
        public double TgtBreakMinutes { get; set; } = 0; //in minutes
        public double TgtBreakSeconds { get; set; } = 10; //in Seconds
        public double ProgressValue { get; set; } = 0;
        public bool Reset { get; set; } = false;
        private bool mFinished = false;

        private string mClockValue;
        public string ClockValue { get => mClockValue; set { mClockValue = value; OnPropertyChanged(); } }

        // private double mImageRotationLeft;
        public double ImageRotationLeft { get; set; }
        public double ImageRotationRight { get; set; }
        public double ImageScale { get; set; } = 1;
        private double mScaleHelper = 0.05f;


        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }



        /// <summary>
        /// Runs the Cycle Code until the specified limit and resets the start value when finished
        /// </summary>
        /// <param name="cycleTime">time between two frames in ms</param>
        /// <returns></returns>
        public async Task RunClockAsync()
        {
            DateTime ActTime = DateTime.Now;
            DateTime StartTime = DateTime.Now.AddMinutes(-5);
            DateTime ExpectedEndTime = DateTime.Now.AddHours(TgtHours).AddMinutes(TgtBreakMinutes).AddSeconds(TgtBreakSeconds);

            TimeSpan maxSeconds = ExpectedEndTime - StartTime;
            TimeSpan remainingTime;


            StartTimeString = ActTime.ToShortTimeString();
            EndTimeString = ExpectedEndTime.ToShortTimeString();

            double remainingSecondsPerc;

            while (true)
            {
                while (ActTime <= ExpectedEndTime)
                {

                    if (Reset)
                    {
                        mFinished = false;
                        StartTime = DateTime.Now;
                        ExpectedEndTime = DateTime.Now.AddHours(TgtHours).AddMinutes(TgtBreakMinutes).AddSeconds(TgtBreakSeconds);
                        EndTimeString = ExpectedEndTime.ToShortTimeString();
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

                mFinished = true;

                if (Reset)
                {
                    mFinished = false;
      
                    StartTime = DateTime.Parse(StartTimeString);
                    ExpectedEndTime = DateTime.Now.AddHours(TgtHours).AddMinutes(TgtBreakMinutes).AddSeconds(TgtBreakSeconds);
                    EndTimeString = ExpectedEndTime.ToShortTimeString();
                    maxSeconds = ExpectedEndTime - StartTime;
                    Reset = false;
                    ImageScale = 1;
                }

                await Task.Delay(10);
            }
        }


        public async Task RotateImagesAsync()
        {
            while (true)
            {

                if (!mFinished)
                {
                    ImageRotationLeft += 5;
                    ImageRotationRight -= 5;
                }

                if (mFinished)
                {
                    ImageRotationLeft = 0;
                    ImageRotationRight = 0;

                    ImageScale += mScaleHelper;

                    if (ImageScale >= 1.2)
                    {
                        mScaleHelper = -0.05;
                    }
                    if (ImageScale <= 0.8)
                    {
                        mScaleHelper = 0.05;
                    }
                }

                await Task.Delay(20);
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
