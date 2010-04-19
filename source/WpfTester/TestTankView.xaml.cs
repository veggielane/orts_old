using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using TestGame;
using Orts.Core.Primitives;
using Orts.Core.Messages;

namespace WpfTester
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TestTankView : UserControl
    {
        public TestTankViewModel Model { get; set; }
        public IObservable<Point> LeftSingleClick { get; set; }
        public IObservable<Point> RightSingleClick { get; set; }

        public TestTankView()
        {
            InitializeComponent();
        }

        public TestTankView(TestTankViewModel model)
            :this()
        {
            this.Model = model;
            this.DataContext = model;

            SetupMouseObs();  
        }


        private void SetupMouseObs()
        {

            var MouseLeftButtonDownObs = Observable.FromEvent((EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                ev => this.MouseLeftButtonDown += ev,
                ev => this.MouseLeftButtonDown -= ev).Select(e => e.EventArgs.GetPosition(this)); ;

            var MouseLeftButtonUpObs = Observable.FromEvent((EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                ev => this.MouseLeftButtonUp += ev,
                ev => this.MouseLeftButtonUp -= ev).Select(e => e.EventArgs.GetPosition(this)); ;

            var MouseRightButtonDownObs = Observable.FromEvent((EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                ev => this.MouseRightButtonDown += ev,
                ev => this.MouseRightButtonDown -= ev).Select(e => e.EventArgs.GetPosition(this)); ;

            var MouseRightButtonUpObs = Observable.FromEvent((EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                ev => this.MouseRightButtonUp += ev,
                ev => this.MouseRightButtonUp -= ev).Select(e => e.EventArgs.GetPosition(this)); ;

            LeftSingleClick = MouseLeftButtonDownObs.Zip(MouseLeftButtonUpObs, (md, mu) => Tuple.Create(md, mu))
                .Where(t => (t.Item2 - t.Item1).Length <= 5).Select(t => t.Item1);

            RightSingleClick = MouseRightButtonDownObs.Zip(MouseRightButtonUpObs, (md, mu) => Tuple.Create(md, mu))
                .Where(t => (t.Item2 - t.Item1).Length <= 5).Select(t => t.Item1);
        }
    }


    public class TestTankViewModel : INotifyPropertyChanged
    {
        public MessageBus Bus { get; private set; }
        public Color Color { get; set; }
        public TestTank Tank { get; private set; }
        public Visibility Visible { get; set; }

        public TestTankViewModel(MessageBus bus, TestTank tank)
        {
            Bus = bus;
            Tank = tank;
            Color = Colors.Blue;
            Visible = Visibility.Visible;
        }

        public void Update()
        {
            if (Tank.Visible)
                Visible = Visibility.Visible;
            else
                Visible = Visibility.Hidden;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Position"));
                PropertyChanged(this, new PropertyChangedEventArgs("Velocity"));
                PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
                PropertyChanged(this, new PropertyChangedEventArgs("Color"));
            }
        }

        public Vector2 Position
        {
            get { return Tank.Position; }
        }

        public Vector2 Velocity
        {
            get { return Tank.Velocity; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        internal void Destroy()
        {
            Bus.Add(new ObjectDestructionRequest(Tank));
        }
    }
}
