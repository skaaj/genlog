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
using MahApps.Metro.Controls;

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private Activity _currentActivity;
        private Dictionary<Activities, Activity> _activities;
        public enum Activities { Home, Help, Stats, Memory, Focus };

        #region Window

        public MainWindow()
        {
            InitializeComponent();

            _activities = new Dictionary<Activities, Activity>();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            AddActivity(Activities.Home, new HomeActivity(this));
            AddActivity(Activities.Help, new HelpActivity(this));
            AddActivity(Activities.Focus, new FocusActivity(this));
            AddActivity(Activities.Stats, new StatisticsActivity(this));
            AddActivity(Activities.Memory, new MemoryTestActivity(this));

            Run(Activities.Home);
        }

        private void WindowUpdateRequested(Activity sender, ViewChangedArgs e)
        {
            // seul l'activité courante peut changer la vue
            if (sender == _currentActivity)
                Area = e.RequestedView;
        }

        public UserControl Area
        {
            set
            {
                // testme
                value.Width = contentArea.Width;
                value.Height = contentArea.Height;

                contentArea.Content = value;
            }
        }

        #endregion

        #region Activity

        private void AddActivity(Activities activityID, Activity activityRef)
        {
            // on ajoute la nouvelle activité
            _activities.Add(activityID, activityRef);
            
            // on abonne la fenêtre à l'activité
            _activities[activityID].OnViewChanged += WindowUpdateRequested;
        }

        public void Run(Activities activityID)
        {
            // Si l'activitée demandée existe
            if (_activities.ContainsKey(activityID))
            {
                Activity tmp = _activities[activityID];

                // on ne relance pas l'activité courante
                if (tmp != _currentActivity)
                {
                    // on arrête l'activité courante
                    if(_currentActivity != null)
                        _currentActivity.Stop();

                    // on met à jour l'activité et sa vue actuelle
                    _currentActivity = tmp;

                    // on démarre l'activité
                    _currentActivity.Start();

                    Area = _currentActivity.CurrentView;
                }
            }
        }

        #endregion

        #region Button callbacks

        private void OnClickHome(object sender, RoutedEventArgs e)
        {
            Run(Activities.Home);
        }

        private void OnClickMemoryTest(object sender, RoutedEventArgs e)
        {
            Run(Activities.Memory);
        }

        private void OnClickFocusTest(object sender, RoutedEventArgs e)
        {
            Run(Activities.Focus);
        }

        private void OnClickStats(object sender, RoutedEventArgs e)
        {
            Run(Activities.Stats);
        }

        private void OnClickHelp(object sender, RoutedEventArgs e)
        {
            Run(Activities.Help);
        }

        #endregion

    }
}
