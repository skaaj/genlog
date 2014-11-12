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
    public partial class MemorizationView : UserControl, IStartable
    {
        private MemoryTestActivity _parent;

        public MemorizationView(MemoryTestActivity parent)
        {
            InitializeComponent();

            _parent = parent;

            parent.listeMemorisation = new List<ImageNombre>();

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
            //Ajout des images du dossier dans une liste
            DirectoryInfo path = new DirectoryInfo("Images Mémoire/");
            string chemin;
            Random rnd = new Random();
            int nombre;


            foreach (FileInfo fichier in path.GetFiles())
            {
                
                nombre = rnd.Next(1, 999);

                chemin = System.IO.Path.GetFullPath("Images Mémoire/"+fichier.Name);

                P.listeMemorisation.Add(new ImageNombre(chemin, (nombre).ToString()));
            }

            foreach (ImageNombre imgnb in _parent.listeMemorisation)
            {
                Image image = new Image();
                image.Height = 20 ;
                image.Width = 50;

                Label lbl = new Label();
                lbl.Width = 40;
                lbl.Content = imgnb._nombre;



                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                //"C:/Users/Gwendal/Documents/GitHub/Projet génie Logiciel/genlog/Solution/Application/bin/Debug/Images Mémoire/"
                myBitmapImage.UriSource = new Uri(imgnb._image);
                myBitmapImage.DecodePixelWidth = 50;
                myBitmapImage.EndInit();
                image.Source = myBitmapImage;

                Affichage_image.Children.Add(image);
                Affichage_image.Children.Add(lbl);

            }

        }

        public void Start()
        {

            TBCountDown.Text = (_parent.tempsMemorisation).ToString();
            _parent.timer.Start();
            CreateList(_parent);
        }
    }
}