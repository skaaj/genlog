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
        public bool _result { get; set; }

        public ImageNombre(string img, string nb)
        {
            _image = img;
            _nombre = nb;
            _result = true;

        }

        public ImageNombre(string img, string nb, bool rep)
        {
            _image = img;
            _nombre = nb;
            _result = rep;
        }

    }
}
