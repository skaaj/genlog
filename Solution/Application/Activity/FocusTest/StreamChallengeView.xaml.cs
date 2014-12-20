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
        private int _gone;

        private FocusModel _model;
        private Instruction instruction;

        private bool _example;

        public StreamChallengeView(FocusActivity parent, bool example)
        {
            InitializeComponent();

            _parent = parent;

            _example = example;

            _randomizer = new Random();
            _model = new FocusModel();
        }

        public void Start()
        {
            _interval = _parent.Speed * 1000;

            instruction = new Instruction(_parent.Level, _model);
            ruleLabel.Content = instruction;

            InitializeAnimation();

            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        #region shapes

        private void InitializeAnimation()
        {
            _animation = new DoubleAnimation();
            _animation.Duration = new Duration(TimeSpan.FromMilliseconds(_interval));
            EasingFunctionBase ease = new CircleEase();
            ease.EasingMode = EasingMode.EaseIn;
            _animation.EasingFunction = ease;


            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(TimerTick);
            _timer.Interval = TimeSpan.FromMilliseconds(_interval / 2);
        }

        private FocusModel.Shapes PickOtherShape(FocusModel.Shapes s)
        {
            FocusModel.Shapes[] availableShapes = new FocusModel.Shapes[FocusModel.ShapesCount - 1];

            int i = 0;
            Type enumType = typeof(FocusModel.Shapes);
            
            foreach (FocusModel.Shapes value in Enum.GetValues(enumType))
                if (value != s)
                    availableShapes[i++] = value;

            i = _randomizer.Next(availableShapes.Length);

            return availableShapes[i];
        }
        
        private Brush PickOtherColor(Brush b)
        {
            return _model.Colors.Where(brush => (brush != b)).ToArray()[_randomizer.Next(_model.Colors.Count - 1)];
        }

        private void SpawnShape()
        {
            CustomShape shape;
            ShapeProperty property = new ShapeProperty();

            bool isValid = _randomizer.NextDouble() < 0.5 ? true : false;

            if (isValid)
            {
                if (!instruction.Negations[0])
                    property.Shape = instruction.Property.Shape;
                else
                    property.Shape = PickOtherShape(instruction.Property.Shape);

                if (!instruction.Negations[1])
                    property.Color = instruction.Property.Color;
                else
                    property.Color = PickOtherColor(instruction.Property.Color);

                property.HasDots = instruction.Property.HasDots;

                if (property.Respect(instruction))
                {
                    ;
                }

                shape = new CustomShape(property, _shapeSize);
                shape.MouseDown += OnCorrectAnswer;
                shape.Unloaded += OnShapeGoneUnclicked;
                shape.Unloaded += OnShapeUnloaded;
            }
            else
            {
                property.Shape = (FocusModel.Shapes) _randomizer.Next(FocusModel.ShapesCount);
                property.Color = _model.Colors[_randomizer.Next(_model.Colors.Count)];
                property.HasDots = _randomizer.NextDouble() < 0.5 ? true : false;


                if (property.Respect(instruction))
                {
                    property.HasDots = !property.HasDots;
                }

                shape = new CustomShape(property, _shapeSize);
                shape.MouseDown += OnWrongAnswer;
            }

            _animation.From = -shape.RuntimeWidth;
            canvas.Children.Add(shape);
            Canvas.SetTop(shape, _yCenter - (_shapeSize / 2));
            shape.BeginAnimation(Canvas.LeftProperty, _animation);

            if (canvas.Children.Count > 6)
                canvas.Children.RemoveRange(4, 1);
        }

        void OnShapeUnloaded(object sender, RoutedEventArgs e)
        {
            _gone++;

            if (_gone == 3)
            {
                ruleLabel.Content += " (Fin détectée)";

                if (!_example)
                    _parent.GoToRegister(_good);
                else
                    _parent.Show("stream");
            }
        }

        void OnShapeGoneUnclicked(object sender, RoutedEventArgs e)
        {
            _bad++; // la forme est supprimée sans avoir été cliquée, on ajoute une erreur
            labelBad.Content = _bad.ToString();
        }

        void OnCorrectAnswer(object sender, MouseButtonEventArgs e)
        {
            CustomShape canvas = sender as CustomShape;

            if (_example)
            {
                canvas.InnerShape.Stroke = Brushes.Green;
                canvas.InnerShape.StrokeThickness = 10;
            }

            canvas.MouseDown -= OnCorrectAnswer;
            canvas.Unloaded -= OnShapeGoneUnclicked;

            _good++;

            labelGood.Content = _good.ToString();

            Console.WriteLine("Correct answer");
        }

        void OnWrongAnswer(object sender, MouseButtonEventArgs e)
        {
            CustomShape canvas = sender as CustomShape;

            if (_example)
            {
                canvas.InnerShape.Stroke = Brushes.Red;
                canvas.InnerShape.StrokeThickness = 10;
            }
            canvas.MouseDown -= OnWrongAnswer;

            _bad++;
            labelBad.Content = _bad.ToString();

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
