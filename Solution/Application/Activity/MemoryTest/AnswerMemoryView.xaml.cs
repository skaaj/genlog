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
    /// Logique d'interaction pour AnswerMemoryView.xaml
    /// </summary>
    public partial class AnswerMemoryView : UserControl, IStartable
    {
        private MemoryTestActivity _parent;

        public AnswerMemoryView(MemoryTestActivity parent)
        {
            InitializeComponent();

            _parent = parent;

            
        }

        // Onsubmit
        private void OnSubmit(object sender, RoutedEventArgs e)
        {
                _parent.Show("result");
        }

        public void AffichageRandom()
        {
            // Affichage des images
            foreach (ImageNombre imgnb in _parent.listeMemorisation)
            {
                Image image = new Image();
                image.Height = 20;
                image.Width = 50;

                Label lbl = new Label();
                lbl.Width = 40;
                lbl.Content = imgnb._nombre;



                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(imgnb._image);
                myBitmapImage.DecodePixelWidth = 50;
                myBitmapImage.EndInit();
                image.Source = myBitmapImage;

                Affichage_image_answer.Children.Add(image);

            }
        }

        public void Start()
        {
            AffichageRandom();
        }
    }
}
