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
        private FocusActivity _parent;
        
        public FocusView(FocusActivity parent)
        {
            InitializeComponent();

            _parent = parent;
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            int speed;
            int.TryParse(speedTextBox.Text, out speed);
            int level;
            int.TryParse(levelTextBox.Text, out level);

            if (speed > 1 && speed < 9 && level > 0 && level < 5)
            {
                _parent.Speed = speed;
                _parent.Level = level;
                _parent.Show("example");
            }
        }
    }
}
