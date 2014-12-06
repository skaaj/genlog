using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour StreamChallengeView.xaml
    /// </summary>
    public partial class StreamChallengeView : UserControl, IStartable, IStoppable
    {
        private FocusActivity _parent;

        private DispatcherTimer _timer;
        private DoubleAnimation _animation;

        private Random _randomizer;

        private int _interval;

        private int _shapeSize;
        private double _yCenter;
        private double _xCenter;

        private double equilateralFactor = 2.0 / Math.Sqrt(3.0);

        private int _good;
        private int _bad;

        private Instruction instruction;

        public StreamChallengeView(FocusActivity parent)
        {
            InitializeComponent();

            _parent = parent;

            _randomizer = new Random();
        }

        public void Start()
        {
            _interval = _parent.Speed * 1000;

            instruction = new Instruction(_parent.Level);
            ruleLabel.Content = instruction;

            InitializeAnimation();

            _timer.Start();
            Console.WriteLine("Timer started :)");
        }

        public void Stop()
        {
            _timer.Stop();
            Console.WriteLine("Timer stopped sir !");
        }

        #region shapes

        private void InitializeAnimation()
        {
            _animation = new DoubleAnimation();
            _animation.From = -200.0;
            _animation.Duration = new Duration(TimeSpan.FromMilliseconds(_interval));
            EasingFunctionBase ease = new CircleEase();
            ease.EasingMode = EasingMode.EaseIn;
            _animation.EasingFunction = ease;


            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = TimeSpan.FromMilliseconds(_interval / 2);
        }

        private void SpawnShape()
        {
            // FORME
            // Si on veut une forme valide
            //      On donne une forme qui va bien
            // Sinon
            //      On donne une forme aléatoire

            // COULEUR
            //      Pareil

            // POINTS
            // Si on veut une forme valide
            //      On donne les points ou pas selon la consigne
            // Sinon 
            //      On laisse l'aléatoire

            // -> Ici on est sûr que si on voulait un forme valide on l'a
            //      Du coups, on met la callback
            // -> Si on voulait une forme incorrect
            // On vérifie que la consigne est pas respectée
            // Oui ? on met la callback error | Non ? on change un critére au aléatoire

            Shape shape;
            bool valid = _randomizer.NextDouble() < 0.5 ? true : false;

            shape = ShapeFactory.BuildRandomShape(_shapeSize);
            shape.Fill = ShapeFactory.GetRandomColor();

            if (valid)
            {
                if(!instruction.Negations[0]) // Pas de négation
                    shape = ShapeFactory.BuildShape(instruction.Shape, _shapeSize);
                else
                    shape = ShapeFactory.BuildOtherShape(instruction.Shape, _shapeSize);

                if (!instruction.Negations[1])
                    shape.Fill = instruction.Color;
                else
                    shape.Fill = ShapeFactory.GetOtherColor(instruction.Color);

                shape.MouseDown += OnCorrectAnswer;
                shape.Unloaded += (s, e) =>
                {
                    _bad++; // la forme est supprimée sans avoir été cliquée, on ajoute une erreur
                };
            }
            else
            {
                shape.MouseDown += OnWrongAnswer;
            }

            canvas.Children.Add(shape);
            Canvas.SetTop(shape, _yCenter - (_shapeSize / 2));
            shape.BeginAnimation(Canvas.LeftProperty, _animation);

            if (canvas.Children.Count > 4)
                canvas.Children.RemoveRange(2, 1);
            
            /*
            Shape shape;
            bool validity = _randomizer.NextDouble() < 0.5 ? true : false;


            int chosenColor = _randomizer.Next(0, _colors.Count);
            ShapeEnum chosenShape = (ShapeEnum)_randomizer.Next(0, 3);
            bool hasDots = _randomizer.NextDouble() < 0.5 ? true : false;

            if (validity)
            {
                hasDots = _hasDots;
                chosenColor = _correctColor;
                chosenShape = _correctShape;
            }
            else
            {
                while (chosenShape == _correctShape && chosenColor == _correctColor && _hasDots == hasDots)
                {
                    chosenShape = (ShapeEnum)_randomizer.Next(0, 3);
                    chosenColor = _randomizer.Next(0, _colors.Count);
                    hasDots     = _randomizer.NextDouble() < 0.5 ? true : false;
                }
            }

            // On créer la forme
            switch (chosenShape)
            {
                case ShapeEnum.Square:
                    shape = ShapeFactory.Rectangle(_shapeSize, _shapeSize);
                    break;
                case ShapeEnum.Triangle:
                    shape = ShapeFactory.EquilateralTriangle((int)(_shapeSize * equilateralFactor));
                    break;
                case ShapeEnum.Circle:
                    shape = ShapeFactory.Ellipse(_shapeSize, _shapeSize);
                    break;
                default:
                    shape = ShapeFactory.Rectangle(_shapeSize, _shapeSize);
                    break;
            }

            // On lui donne sa couleur
            shape.Fill = _colors[chosenColor];

            if (validity)
            {
                shape.MouseDown += OnCorrectAnswer;
                shape.Unloaded += (s, e) =>
                {
                    _bad++; // la forme est supprimée sans avoir été cliquée, on ajoute une erreur
                };
            }
            else
            {
                shape.MouseDown += OnWrongAnswer;
            }

            if (hasDots)
            {
                Canvas complexShape = ShapeFactory.AddDots(shape, 2, 20, 10);
                canvas.Children.Add(complexShape);
                Canvas.SetTop(complexShape, shapeY);
                complexShape.BeginAnimation(Canvas.LeftProperty, _animation);
            }
            else
            {
                canvas.Children.Add(shape);
                Canvas.SetTop(shape, shapeY);
                shape.BeginAnimation(Canvas.LeftProperty, _animation);
            }

            if (canvas.Children.Count > 4)
                canvas.Children.RemoveRange(2, 1);
            Console.WriteLine(canvas.Children.Count);
            */
        }

        void OnCorrectAnswer(object sender, MouseButtonEventArgs e)
        {
            Shape s = sender as Shape;

            s.Stroke = Brushes.Green;
            s.StrokeThickness = 10;
            
            s.MouseDown -= OnCorrectAnswer;

            _good++;

            Console.WriteLine("Correct answer");
        }

        void OnWrongAnswer(object sender, MouseButtonEventArgs e)
        {
            Shape s = sender as Shape;

            s.Stroke = Brushes.Red;
            s.StrokeThickness = 10;
            
            s.MouseDown -= OnWrongAnswer;

            _bad++;

            Console.WriteLine("Bad answer");
        }

        private void TimerTick(object sender, EventArgs e)
        {
            SpawnShape();
        }

        #endregion

        private void CanvasLoaded(object sender, RoutedEventArgs e)
        {
            _animation.To = canvas.ActualWidth;

            _shapeSize = (int)(canvas.ActualHeight / 3);
            _yCenter = canvas.ActualHeight / 2;
            _xCenter = canvas.ActualWidth / 2;

            Line middle = new Line();
            middle.Stroke = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
            middle.X1 = canvas.ActualWidth / 2;
            middle.X2 = middle.X1;
            middle.Y1 = 0;
            middle.Y2 = canvas.ActualHeight;
            middle.StrokeThickness = 2;

            canvas.Children.Add(middle);

            SpawnShape();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // refactoring inc
        }
    }
}
