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
    /// Logique d'interaction pour StreamChallengeView.xaml
    /// </summary>
    public partial class StreamChallengeView : UserControl
    {
        private Activity _parent;
        private DispatcherTimer timer;

        public StreamChallengeView(Activity parent)
        {
            InitializeComponent();

            _parent = parent;
        }
    }
}
