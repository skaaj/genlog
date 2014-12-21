using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl, IStartable ,IStoppable///Permet de gérer la View Home
    {
        public HomeView()
        {
            InitializeComponent();

            Console.WriteLine("HomeView::Create called");
        }

        public void Stop()///Stop de la view
        {
            Console.WriteLine("I'm a view and because my activity stops, I stop.");
        }

        public void Start()///Start de la view
        {
            Console.WriteLine("I'm a view and because my activity starts, I start.");
        }
    }
}
