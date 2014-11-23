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
    /// Logique d'interaction pour ResulatatMemorizationView.xaml
    /// </summary>
    public partial class ResultatMemorizationView : UserControl, IStartable
    {
        private MemoryTestActivity _parent;

        public ResultatMemorizationView(MemoryTestActivity parent)
        {
            InitializeComponent();

            _parent = parent;


        }
        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            _parent.Show("home");
        }



        public void AffichageResultat()
        {
            Affichage_resultat.Children.Clear();
            int nbreponsejuste = 0;

            // Affichage des images
            foreach (ImageNombre imgnb in _parent.ListeReponse)
            {
                if( imgnb._result == true)
                {
                    nbreponsejuste = nbreponsejuste+1;
                }

                Image image = new Image();
                image.Height = 20;
                image.Width = 50;

                Label lbl = new Label();
                lbl.Width = 40;
                lbl.Content = imgnb._nombre;

                /*
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(imgnb._image);
                myBitmapImage.DecodePixelWidth = 50;
                myBitmapImage.EndInit();
                image.Source = myBitmapImage;
                */


                image.Source = new BitmapImage(new Uri(imgnb._image));

                Affichage_resultat.Children.Add(image);
                Affichage_resultat.Children.Add(lbl);
            }


            Nombre_reponses_justes.Content = (nbreponsejuste).ToString() + " / " + (_parent.ListeReponse.Count).ToString();

        }

        public void Start()
        {
            AffichageResultat();
        }

    }
}
