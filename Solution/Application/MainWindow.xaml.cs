using Genlog.View;
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
        private ViewManager _viewManager;

        public MainWindow()
        {
            InitializeComponent();

            _viewManager = ViewManager.Instance;
        }

        private bool Show(string page)
        {
            UserControl newView = _viewManager[page];

            if (newView == null) return false;

            contentArea.Content = newView;
            return true;
        }

        /*
         * EVENTS
         */

        private void OnClickHome(object sender, RoutedEventArgs e)
        {
            Stopwatch s = Stopwatch.StartNew();
            Show("home");

            s.Stop();
            Console.WriteLine(s.Elapsed);
        }

        private void OnClickMemTest(object sender, RoutedEventArgs e)
        {
            Show("memtest");
        }

        private void OnClickMenu(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = (MenuItem)sender;
           
            switch (clickedItem.Header.ToString())
            {
                case("_Accueil"):
                    Show("home");
                    break;
                case ("Test _1"):
                    Show("memtest");
                    break;
                case ("Test _2"):
                    Show("foctest");
                    break;
                case ("_Statistiques"):
                    Show("stats");
                    break;
                case ("A_ide"):
                    Show("help");
                    break;
                default:break;
            }
        }
    }
}
