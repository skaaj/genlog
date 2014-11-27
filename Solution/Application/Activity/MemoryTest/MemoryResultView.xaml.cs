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

                Image image = new Image();
                image.Height = 20;
                image.Width = 50;

                Label lbl = new Label();
                lbl.Width = 40;
                lbl.Content = imgnb._nombre;

                image.Source = new BitmapImage(new Uri(imgnb._image));

                Affichage_resultat.Children.Add(image);


                BrushConverter bc = new BrushConverter();       // Changement de couleur selon la véracité de la réponse
                if (imgnb._result != true)
                {
                    lbl.Background = (Brush)bc.ConvertFrom("#FFFF9090");
                }
                else 
                {
                    nbreponsejuste = nbreponsejuste + 1;
                    lbl.Background = (Brush)bc.ConvertFrom("#8ad628");
                }


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
