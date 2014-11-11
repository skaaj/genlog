using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlog
{
    public class ImageNombre
    {
        public string _image { get; set; }
        public string _nombre { get; set; }

        public ImageNombre(string img, string nb)
        {
            _image = img;
            _nombre = nb;

        }

    }
}
