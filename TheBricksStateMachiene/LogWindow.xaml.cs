using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace TheBricksStateMachiene
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region INotify Changed Properties  
        private ObservableCollection<String> transition;
        public ObservableCollection<String> Transition
        {
            get { return transition; }
            set { SetField(ref transition, value, nameof(Transition)); }
        }


        // Template for a new INotify Changed Property
        // for using CTRL-R-R
        private bool xxx;
        public bool Xxx
        {
            get { return xxx; }
            set { SetField(ref xxx, value, nameof(Xxx)); }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public LogWindow()
        {
            InitializeComponent();
            DataContext = this;

            Transition = new ObservableCollection<string>();
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

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

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// SetField
        /// for INotify Changed Properties
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
