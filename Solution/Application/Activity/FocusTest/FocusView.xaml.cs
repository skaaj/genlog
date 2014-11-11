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
    /// Logique d'interaction pour FocusTestView.xaml
    /// </summary>
    public partial class FocusView : UserControl
    {
        private Activity _parent;
        
        public FocusView(Activity parent)
        {
            InitializeComponent();

            _parent = parent;
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            _parent.Show("stream"); // fixme : string = danger
        }
    }
}
