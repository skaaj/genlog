using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Genlog
{
    public class FocusModel
    {
        public enum Shapes { Square, Triangle, Circle };
        public List<Brush> Colors { get; set; }

        public FocusModel()
        {
            Colors = new List<Brush>()
            {
	            Brushes.DarkRed,
	            Brushes.DarkGreen,
	            Brushes.DarkBlue
	        };
        }

        public static int ShapesCount
        {
            get
            {
                return Enum.GetNames(typeof(Shapes)).Length;
            }
        }

    }
}
