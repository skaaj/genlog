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
        private MemoryTestActivity _parent;

        public MemorizationView(MemoryTestActivity parent)
        {
            InitializeComponent();

            _parent = parent;

            TBCountDown.Text = (15).ToString();
        }

        // A chaque tick, rafraichir l'affichage
        public void TimerTick(object sender, EventArgs e)
        {
            int timeLeft = int.Parse(TBCountDown.Text);
            TBCountDown.Text = (timeLeft - 1).ToString();

            if (timeLeft <= 0)
            {
                _parent.timer.Stop();
                _parent.Show("result");
            }
        }
    }
}
