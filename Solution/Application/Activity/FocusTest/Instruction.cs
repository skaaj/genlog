using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Genlog
{
    public class Instruction
    {
        public Brush Color { get; set; }
        public bool[] Negations { get; set; }
        public ShapeFactory.ShapeEnum Shape { get; set; }

        public Instruction(int level)
        {
            Negations = new bool[3];

            int randomIndex;
            int nbPlaced = 0;

            Random randomizer = new Random();

            Color = ShapeFactory.GetRandomColor();
            Shape = (ShapeFactory.ShapeEnum)randomizer.Next(0, ShapeFactory.ShapeEnumCount);

            while (nbPlaced < (level - 1)) // Tant qu'on a pas placé toutes les négations
            {
                randomIndex = randomizer.Next(0, 3);

                if (!Negations[randomIndex])
                {
                    Negations[randomIndex] = true;
                    nbPlaced++;
                }
            }
        }

        private string ShapeEnumToString(ShapeFactory.ShapeEnum se)
        {
            if (se == ShapeFactory.ShapeEnum.Square)
                return " carrés, ";
            if (se == ShapeFactory.ShapeEnum.Triangle)
                return " triangles, ";
            if (se == ShapeFactory.ShapeEnum.Circle)
                return " cercles, ";
            else
            {
                se = ShapeFactory.ShapeEnum.Square;
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
                strBuilder.Append("Cliquez sur les formes qui ne sont pas des ");
            else
                strBuilder.Append("Cliquez sur les formes");

            strBuilder.Append(ShapeEnumToString(Shape));

            if (Negations[1])
                strBuilder.Append("qui ne sont pas");
            else
                strBuilder.Append("de couleur");

            strBuilder.Append(ColorToString(Color));

            if (Negations[2])
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
