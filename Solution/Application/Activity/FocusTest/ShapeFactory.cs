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
    class ShapeFactory
    {
        private static double equilateralFactor = Math.Sqrt(3.0) / 2.0;

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

            double height = equilateralFactor * s;
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

        public static Canvas AddDots(Shape s, int n, int diameter, int margin)
        {
            Canvas c = new Canvas();

            s.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            // On ajoute la forme au fond
            c.Children.Add(s);

            // On ajoute les points par dessus
            c.Width = s.DesiredSize.Width;
            c.Height = s.DesiredSize.Height;

            int boxWidth = (n * diameter) + (margin * (n - 1)); // largeur de la boite englobant les points
            double boxOrigin = (c.Width / 2) - (boxWidth / 2);

            Ellipse[] dots = new Ellipse[n];
            for (int i = 0; i < n; i++)
            {
                dots[i] = Dot(diameter, Brushes.Black, 2, Brushes.White);

                c.Children.Add(dots[i]);
                Canvas.SetLeft(dots[i], boxOrigin + i * (diameter + margin));
                Canvas.SetTop(dots[i], (c.Height / 2) - (diameter / 2));
            }

            return c;
        }
    }
}
