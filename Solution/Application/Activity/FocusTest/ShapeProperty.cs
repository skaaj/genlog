using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Genlog
{
    public class ShapeProperty
    {
        public Brush Color { get; set; }
        public bool HasDots { get; set; }
        public FocusModel.Shapes Shape { get; set; }

        public bool Respect(Instruction instruction)
        {
            Brush instColor = instruction.Property.Color;
            bool instHasDots = instruction.Property.HasDots;
            FocusModel.Shapes instShape = instruction.Property.Shape;

            bool[] negations = instruction.Negations;

            if (negations[0] && (Shape == instShape))
                return false;
            if (negations[1] && (Color == instColor))
                return false;
            if (!negations[0] && (Shape != instShape))
                return false;
            if (!negations[1] && (Color != instColor))
                return false;
            if (instHasDots != HasDots)
                return false;

            return true;
        }

        public static bool operator ==(ShapeProperty lhs, ShapeProperty rhs)
        {
            return lhs.Color == rhs.Color &&
                   lhs.Shape == rhs.Shape &&
                   lhs.HasDots == rhs.HasDots;
        }

        public static bool operator !=(ShapeProperty lhs, ShapeProperty rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            ShapeProperty sp = obj as ShapeProperty;
            return sp == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
