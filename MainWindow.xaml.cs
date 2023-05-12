using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool timerRunning = false;
        private bool workMode = true;
        private int workTime = 25 * 60; // 25 minutes of work
        private int breakTime = 5 * 60; // 5 minutes of break
        private int timeRemaining;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timeRemaining = workTime;
            UpdateTimerDisplay();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timeRemaining--;
            if (timeRemaining <= 0)
            {
                timer.Stop();
                timeRemaining = workMode ? breakTime : workTime;
                workMode = !workMode;
                modeDisplay.Text = workMode ? "Work" : "Break";
                SoundPlayer player = new SoundPlayer(Properties.Resources.bell);
                player.Play();
            }
            UpdateTimerDisplay();
        }

        private void startStopButton_Click (object sender, RoutedEventArgs e)
        {
            if (timerRunning)
            {
                timerRunning = false;
                timer.Stop();
                startStopButton.Content = "Start";
            }
            else
            {
                timerRunning = true;
                timer.Start();
                startStopButton.Content = "Stop";
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            timerRunning = false;
            timer.Stop();
            workMode = true;
            timeRemaining = workTime;
            UpdateTimerDisplay();
            modeDisplay.Text = "Work";
            startStopButton.Content = "Start";
        }

        private void UpdateTimerDisplay()
        {
            int minutes = timeRemaining / 60;
            int seconds = timeRemaining % 60;
            timerDisplay.Text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void MenuBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
