using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _timerRunning = false;
        private bool _workMode = true;
        private int _workTime = 25 * 60; // 25 minutes of work
        private int _breakTime = 5 * 60; // 5 minutes of break
        private int _timeRemaining;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _timeRemaining = _workTime;
            UpdateTimerDisplay();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeRemaining--;
            if (_timeRemaining <= 0)
            {
                _timer.Stop();
                _timeRemaining = _workMode ? _breakTime : _workTime;
                _workMode = !_workMode;
                modeDisplay.Text = _workMode ? "WORK" : "BREAK";
                SoundPlayer player = new SoundPlayer(Properties.Resources.bell);
                player.Play();
            }
            UpdateTimerDisplay();
        }

        private void StartStopButton_Click (object sender, RoutedEventArgs e)
        {
            if (_timerRunning)
            {
                _timerRunning = false;
                _timer.Stop();
                StartStopButton.Content = "START";
            }
            else
            {
                _timerRunning = true;
                _timer.Start();
                StartStopButton.Content = "STOP";
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _timerRunning = false;
            _timer.Stop();
            _workMode = true;
            _timeRemaining = _workTime;
            UpdateTimerDisplay();
            modeDisplay.Text = "WORK";
            StartStopButton.Content = "START";
        }

        private void UpdateTimerDisplay()
        {
            int minutes = _timeRemaining / 60;
            int seconds = _timeRemaining % 60;
            TimerDisplay.Text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void MenuBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
