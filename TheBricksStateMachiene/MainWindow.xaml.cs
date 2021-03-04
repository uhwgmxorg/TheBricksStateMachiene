using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Media;
using System.Windows;

namespace TheBricksStateMachiene
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        #region INotifyPropertyChanged Properties
        private string currentState;
        public string CurrentState
        {
            get { return currentState; }
            set { SetField(ref currentState, value, nameof(CurrentState)); }
        }
        private int brickNo;
        public int BrickNo
        {
            get { return brickNo; }
            set { SetField(ref brickNo, value, nameof(BrickNo)); }
        }

        private string yPos1;
        public string YPos1
        {
            get { return yPos1; }
            set { SetField(ref yPos1, value, nameof(YPos1)); }
        }
        private string yPos2;
        public string YPos2
        {
            get { return yPos2; }
            set { SetField(ref yPos2, value, nameof(YPos2)); }
        }
        private string yPos3;
        public string YPos3
        {
            get { return yPos3; }
            set { SetField(ref yPos3, value, nameof(YPos3)); }
        }

        private ObservableCollection<String> transition;
        public ObservableCollection<String> Transition
        {
            get { return transition; }
            set { SetField(ref transition, value, nameof(Transition)); }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { SetField(ref message, value, nameof(Message)); }
        }
        #endregion

        System.Windows.Threading.DispatcherTimer dispatcherRunningTimer = null;

        private static System.Random _random = new System.Random();

        BrickStateMachine brickStateMachine;
        BrickStateMachine.ProcessState processState;
        BrickStateMachine.Command randomCommand;

        LogWindow logWindow;

        SoundPlayer SplayerLoop { get; set; }
        SoundPlayer SplayerGameOver { get; set; }
        SoundPlayer SplayerJump { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            BrickNo = 0;
            YPos1 = "0";
            YPos2 = "0";
            YPos3 = "0";
            Message = "";

            brickStateMachine = new BrickStateMachine(0, BrickStateMachine.ProcessState.StateOne06);
            logWindow = new LogWindow();
            Transition = logWindow.Transition;

            //  Load the music and sounds
            SplayerLoop = new System.Media.SoundPlayer(Properties.Resources.BACKGROUND_LOOP);
            SplayerGameOver = new System.Media.SoundPlayer(Properties.Resources.GAME_OVER);
            SplayerJump = new System.Media.SoundPlayer(Properties.Resources.JUMP);

            // Init dispatcher
            dispatcherRunningTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherRunningTimer.Tick += new EventHandler(DispatcherTimer);
            dispatcherRunningTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Button_Click_LogWin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_LogWin(object sender, RoutedEventArgs e)
        {
            Message = "LogWin...";

            logWindow.Left = Left + Width;
            logWindow.Top = Top;
            if(logWindow.IsVisible)
                logWindow.Hide();
            else
                logWindow.Show();
        }

        /// <summary>
        /// Button_Click_Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            Message = "Start...";

            button_Next_Brick.IsEnabled = false;
            SplayerLoop.PlayLooping();
            dispatcherRunningTimer.Start();
        }

        /// <summary>
        /// Button_Click_Stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Stop(object sender, RoutedEventArgs e)
        {
            Message = "Stop";

            button_Next_Brick.IsEnabled = true;
            SplayerLoop.Stop();
            dispatcherRunningTimer.Stop();
        }

        /// <summary>
        /// Button_Click_NextBrick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_NextBrick(object sender, RoutedEventArgs e)
        {
            Message = "Next Brick";

            SplayerJump.Play();
            NextBrick();
        }
        private void NextBrick()
        {
            BrickNo++;

            /***************************************************************/
            float yPos1, yPos2, yPos3;
            CallBrickState(out yPos1, out yPos2, out yPos3);
            /***************************************************************/

            YPos1 = yPos1 == 8 ? "N/A" : yPos1.ToString();
            YPos2 = yPos2 == 8 ? "N/A" : yPos2.ToString();
            YPos3 = yPos3 == 8 ? "N/A" : yPos3.ToString();

            CurrentState = processState.ToString();
            Message = String.Format("processState = {0}  |  command = {1}", processState, randomCommand);
            Transition.Insert(0, String.Format("{0} {1} {2} {3} {4}", BrickNo, Message, yPos1 == 8 ? "N/A" : yPos1.ToString(), yPos2 == 8 ? "N/A" : yPos2.ToString(), yPos3 == 8 ? "N/A" : yPos3.ToString()));
        }
        private void CallBrickState(out float yPos1, out float yPos2, out float yPos3)
        {
            float[] yPos = new float[3];
            processState = BrickStateMachine.ProcessState.StateOne06;

            bool isInvalidState;
            do
            {
                isInvalidState = false;
                randomCommand = (BrickStateMachine.Command)Random.Range(0, 43);

                try
                {
                    processState = brickStateMachine.MoveNext(randomCommand);
                }
                catch (System.Exception ex)
                {
                    string exeptionMessage = ex.Message;
                    isInvalidState = true;
                }
            }
            while (isInvalidState);

            Debug.Log(processState);

            yPos = brickStateMachine.GetYPosFromState(brickStateMachine.CurrentState);

            yPos1 = yPos[0];
            yPos2 = yPos[1];
            yPos3 = yPos[2];
            Debug.Log(processState);
        }

        /// <summary>
        /// Button_Close_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// Lable_Message_MouseDown
        /// Clear Message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lable_Message_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Message = "";
        }

        /// <summary>
        /// Window_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            logWindow.Close();
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer(object sender, EventArgs e)
        {
            NextBrick();
            System.Diagnostics.Debug.WriteLine("Calling DispatcherTimer");
        }

        /// <summary>
        /// RandomDouble
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="deci"></param>
        /// <returns></returns>
        private static double RandomDouble(double min, double max, int deci)
        {
            double d;
            d = _random.NextDouble() * (max - min) + min;
            return Math.Round(d, deci);
        }

        /// <summary>
        /// SetField
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void OnPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }
}
