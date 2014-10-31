using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, Activity> _activities;
        private Activity _currentActivity;

        public MainWindow()
        {
            InitializeComponent();

            _activities = new Dictionary<string, Activity>();

            _activities.Add("home", new HomeActivity(this));
            _activities.Add("help", new HelpActivity(this));
            _activities.Add("stats", new StatisticsActivity(this));
            _activities.Add("memtest", new MemoryTestActivity(this));
            _activities.Add("focustest", new FocusTestActivity(this));

            _activities["focustest"].OnViewChanged += MainWindow_OnViewChanged;

            Launch("home");
        }

        void MainWindow_OnViewChanged(UserControl sender, EventArgs e)
        {
            Area = sender;
        }

        public UserControl Area
        {
            set
            {
                contentArea.Content = value;
            }
        }

        // TODO : vérifier si l'activité n'est pas déjà celle courante
        public void Launch(string activity)
        {
            if (_activities.ContainsKey(activity))
            {
                _currentActivity = _activities[activity];

                Area = _currentActivity.View;
            }
        }

        /*
         * Events
         */

        private void OnClickHome(object sender, RoutedEventArgs e)
        {
            Launch("home");
        }

        private void OnClickMemoryTest(object sender, RoutedEventArgs e)
        {
            Launch("memtest");
        }

        private void OnClickFocusTest(object sender, RoutedEventArgs e)
        {
            Launch("focustest");
        }

        private void OnClickStats(object sender, RoutedEventArgs e)
        {
            Launch("stats");
        }

        private void OnClickHelp(object sender, RoutedEventArgs e)
        {
            Launch("help");
        }
    }
}
