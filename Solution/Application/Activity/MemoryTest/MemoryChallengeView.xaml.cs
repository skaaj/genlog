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

            parent.listeMemorisation = new List<ImageNombre>();

            //CreateList(parent);



            TBCountDown.Text = (parent.tempsMemorisation).ToString();
        }

        // A chaque tick, rafraichir le compteur
        public void TimerTick(object sender, EventArgs e)
        {
            int timeLeft = int.Parse(TBCountDown.Text);
            TBCountDown.Text = (timeLeft - 1).ToString();

            if (timeLeft <= 0)
            {
                _parent.timer.Stop();
                _parent.Show("answer");
            }
        }

        public void CreateList(MemoryTestActivity P)
        {
           /* //Ajout des images du dossier dans une liste
            DirectoryInfo path = new DirectoryInfo("Images Mémoire/");

            foreach (FileInfo fichier in path.GetFiles())
            {
                Random rnd = new Random();
                int nombre = rnd.Next(1, 999);

                P.listeMemorisation.Add(new ImageNombre(fichier.Name, (nombre).ToString()));
            }

            foreach (ImageNombre imgnb in _parent.listeMemorisation)
            {
                Image image = new Image();

                ImageSource imageS = new BitmapImage(new Uri("Images Mémoire/" + imgnb));

                image.Source = imageS;

                Affichage_image.Children.Add(image);

            }*/

        }
    }
}