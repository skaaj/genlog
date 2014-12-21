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
using System.Threading;

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour MemoryTestView.xaml
    /// </summary>
    public partial class MemoryTestView : UserControl
    {

        private MemoryTestActivity _parent;
        private int NImage;

        public MemoryTestView(MemoryTestActivity parent)
        {
            
            InitializeComponent();
            _parent = parent;
            NumberOfImage();
        }


        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            try {_parent.tempsMemorisation = int.Parse(champs_temps.Text);}
            catch { _parent.tempsMemorisation = 0; }
            try { _parent.difficulte = int.Parse(champs_nombre.Text); }
            catch { _parent.difficulte = 0; }
            var bc = new BrushConverter();
            
            if (_parent.difficulte < 11 && _parent.difficulte > 0 && _parent.tempsMemorisation > 0 && _parent.tempsMemorisation < 61)
            {
                if (_parent.difficulte > NImage)
                {
                    Passez_au_test.Content = "Le test ne contient pas assez d'images ("+NImage+"). Revalidez une fois changés";
                    Passez_au_test.Background = (Brush)bc.ConvertFrom("#FFFF9090"); ;
                }

                else{
                champs_temps.Text = String.Empty;
                champs_nombre.Text = String.Empty;
                _parent.Show("challenge");
                }
            }

            else
            {
                champs_temps.Text = String.Empty;
                champs_nombre.Text = String.Empty;

                
                Passez_au_test.Content = "Paramètres non valides. Revalidez une fois changés";
                Passez_au_test.Background = (Brush)bc.ConvertFrom("#FFFF9090"); ;
            }
            
        }

        public void NumberOfImage()
        {
            DirectoryInfo directory = new DirectoryInfo("../../Images");
            string filePath;
            string extension;

            foreach (FileInfo fichier in directory.GetFiles())
            {
                filePath = System.IO.Path.GetFullPath("../../Images/" + fichier.Name);
                extension = System.IO.Path.GetExtension(filePath);

                if (extension == ".png" || extension == ".btm")
                {
                    this.NImage = this.NImage + 1;
                }
            }
        }

    }
}
