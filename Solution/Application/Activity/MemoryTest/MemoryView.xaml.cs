using System;
using System.IO;
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
    /// Logique d'interaction pour MemoryTestView.xaml
    /// </summary>
    public partial class MemoryTestView : UserControl
    {

        private MemoryTestActivity _parent;

        public MemoryTestView(MemoryTestActivity parent)
        {
            InitializeComponent();

            _parent = parent;


        }



        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            if (_parent.difficulte < 11 && _parent.difficulte > 0 && _parent.tempsMemorisation > 0 && _parent.tempsMemorisation < 61)
            {
                _parent.Show("challenge");
                _parent.timer.Start();

            }

            else
            {
                var bc = new BrushConverter();
                Passez_au_test.Content = "Paramètres non valide. Revalidez une fois changés";
                Passez_au_test.Background = (Brush)bc.ConvertFrom("#FFFF9090"); ;
            }

        }

        private void champs_temps_TextChanged(object sender, TextChangedEventArgs e)
        {
            _parent.tempsMemorisation = int.Parse(champs_temps.Text);

        }

        private void champs_nombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            _parent.difficulte = int.Parse(champs_nombre.Text);
        }
    }
}
