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
using System.Windows.Threading;

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour MemorizationView.xaml
    /// </summary>
    public partial class MemorizationView : UserControl
    {
        private Activity _parent;
        private DispatcherTimer timermemory;

        public MemorizationView(Activity parent)
        {
            InitializeComponent();

            _parent = parent;

            // Création du chrono
            int timeleft = 15;

            timermemory = new DispatcherTimer();
            timermemory.Tick += new EventHandler(timermemory_Tick);
            timermemory.Interval = new TimeSpan(0,0,1);
            timermemory.Start();

            TBCountDown.Text = timeleft.ToString();
        }

            // A chaque tick, rafraichir l'affichage
            private void timermemory_Tick(object sender, EventArgs e)
            {
               
                    TBCountDown.Text = ((int.Parse(TBCountDown.Text) - 1)).ToString();

                    if (int.Parse(TBCountDown.Text) <= 0)
                    {
                        _parent.SetView("resultatmemory");
                    }
                
                    
            }
        
    }
}
