using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Genlog
{
    public class Instruction
    {
        public bool[] Negations { get; set; }
        public ShapeProperty Property { get; set; }

        public Instruction(int level, FocusModel model)
        {
            Negations = new bool[2];

            int randomIndex;
            int nbPlaced = 0;

            Random randomizer = new Random();

            Property = new ShapeProperty();
            Property.Color = model.Colors[randomizer.Next(model.Colors.Count)];

            if (level == 4)
                Property.HasDots = false;
            else
                Property.HasDots = randomizer.NextDouble() < 0.5 ? true : false;
            
            Property.Shape = (FocusModel.Shapes) randomizer.Next(FocusModel.ShapesCount);

            while (nbPlaced < (level - 2)) // Tant qu'on a pas placé toutes les négations
            {
                randomIndex = randomizer.Next(0, Negations.Length);

                if (!Negations[randomIndex])
                {
                    Negations[randomIndex] = true;
                    nbPlaced++;
                }
            }
        }

        private string ShapeEnumToString(FocusModel.Shapes se)
        {
            if (se == FocusModel.Shapes.Square)
                return " carrés, ";
            if (se == FocusModel.Shapes.Triangle)
                return " triangles, ";
            if (se == FocusModel.Shapes.Circle)
                return " cercles, ";
            else
            {
                se = FocusModel.Shapes.Square;
                return " carrés, ";
            }
        }

        private string ColorToString(Brush b)
        {
            if (b == Brushes.DarkRed)
                return " rouges et ";
            if (b == Brushes.DarkGreen)
                return " vertes et ";
            if (b == Brushes.DarkBlue)
                return " bleues et ";
            else
            {
                b = Brushes.Black;
                return " noir et ";
            }
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            if (Negations[0])
                strBuilder.Append("Cliquez sur les formes qui ne sont pas des");
            else
                strBuilder.Append("Cliquez sur les formes");

            strBuilder.Append(ShapeEnumToString(Property.Shape));

            if (Negations[1])
                strBuilder.Append("qui ne sont pas");
            else
                strBuilder.Append("de couleur");

            strBuilder.Append(ColorToString(Property.Color));

            if (!Property.HasDots)
            {
                strBuilder.Append("qui n'ont pas de points noirs.");
            }
            else
            {
                strBuilder.Append("qui ont des points noirs.");
            }

            return strBuilder.ToString();
        }
    }
}
