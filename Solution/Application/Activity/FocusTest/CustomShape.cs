using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Genlog
{
    public class CustomShape : Canvas
    {
        private static double eqFactor = Math.Sqrt(3.0) / 2.0;
        private static double invEqFactor = 2.0 / Math.Sqrt(3.0);

        private int _dotCount;
        private int _dotMargin;
        private int _dotDiameter;

        public ShapeProperty Property { get; set; }

        public Shape InnerShape
        {
            get
            {
                Shape s = this.Children[0] as Shape;

                if (s != null)
                    return s;
                else
                    throw new Exception("Impossible de récupérer la forme liée au canvas.");
            }
        }

        public double RuntimeWidth { 
            get 
            {
                Shape s = InnerShape;
                s.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                // On ajoute les points par dessus
                return s.DesiredSize.Width;
            }
        }

        public CustomShape(ShapeProperty property, int size)
        {
            Property = property;

            _dotCount = 3;
            _dotMargin = 10;
            _dotDiameter = 20;

            // Construit la bonne forme
            Shape s;
            switch (Property.Shape)
            {
                case FocusModel.Shapes.Square:
                    s = Rectangle(size, size);
                    break;
                case FocusModel.Shapes.Triangle:
                    s = EquilateralTriangle((int)(size * invEqFactor));
                    break;
                case FocusModel.Shapes.Circle:
                    s = Ellipse(size, size);
                    break;
                default:
                    throw new Exception("La forme désirée n'existe pas.");
            }

            // Applique la couleur associée
            s.Fill = Property.Color;

            // On ajoute la forme au fond
            this.Children.Add(s);

            // Ajoute (ou non) des points
            if (Property.HasDots)
            {
                s.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                // On ajoute les points par dessus
                this.Width  = s.DesiredSize.Width;
                this.Height = s.DesiredSize.Height;

                int boxWidth = (_dotCount * _dotDiameter) + (_dotMargin * (_dotCount - 1)); // largeur de la boite englobant les points
                double boxOrigin = (this.Width / 2) - (boxWidth / 2);

                Ellipse[] dots = new Ellipse[_dotCount];
                for (int i = 0; i < _dotCount; i++)
                {
                    dots[i] = Dot(_dotDiameter, Brushes.Black, 2, Brushes.White);

                    this.Children.Add(dots[i]);
                    Canvas.SetLeft(dots[i], boxOrigin + i * (_dotDiameter + _dotMargin));
                    Canvas.SetTop(dots[i], (this.Height / 2) - (_dotDiameter / 2));
                }
            }
        }

        public static Rectangle Rectangle(int w, int h)
        {
            Rectangle r = new Rectangle();
            r.Width = w;
            r.Height = h;

            return r;
        }

        public static Ellipse Ellipse(int w, int h)
        {
            Ellipse e = new Ellipse();
            e.Width = w;
            e.Height = h;

            return e;
        }

        public static Polygon EquilateralTriangle(int s)
        {
            Polygon p = new Polygon();

            double height = eqFactor * s;
            p.Points = new PointCollection(){
                new Point(0, height),
                new Point(s / 2, 0),
                new Point(s, height),
            };

            return p;
        }

        public static Line Line(int x1, int y1, int x2, int y2, int thickness)
        {
            Line l = new Line();
            l.X1 = x1;
            l.X2 = x2;
            l.Y1 = y1;
            l.Y2 = y2;
            l.StrokeThickness = thickness;

            return l;
        }

        private static Ellipse Dot(int diameter, Brush backgroundColor, int borderSize, Brush borderColor)
        {
            Ellipse dot = new Ellipse();
            dot.Width = diameter;
            dot.Height = diameter;
            dot.Fill = backgroundColor;
            dot.Stroke = borderColor;
            dot.StrokeThickness = borderSize;

            return dot;
        }
    }
}
