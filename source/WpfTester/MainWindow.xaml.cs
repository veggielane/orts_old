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
using Orts.Core;
using Orts.Core.Timing;
using Orts.Core.GameObjects;
using Orts.Core.Primitives;
using System.Diagnostics;
using System.ComponentModel;
using Ninject;
using Ninject.Modules;
using Orts.Core.Messages;
using TestGame;
using Orts.Core.Players;
using System.Concurrency;
using System.Threading.Tasks;

namespace WpfTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameEngine Engine { get; set; }
        public Dictionary<IGameObject, TestTankView> Views { get; set; }
        public PlayerController Player { get; set; }

        public IObservable<Point> LeftSingleClick { get; set; }
        public IObservable<Point> RightSingleClick { get; set; }
        public IObservable<Point> LeftDoubleClick { get; set; }
        public IObservable<Key> KeyPress { get; set; }

        public IObservable<Rect> LeftDrag { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            SetupInputObs();

            var kernel = new StandardKernel(new TestGameModule());

            kernel.Bind<GameObjectFactory>().To<TestGameObjectFactory>().InSingletonScope();
            kernel.Bind<Dictionary<IGameObject, TestTankView>>().ToConstant(new Dictionary<IGameObject, TestTankView>());
            kernel.Bind<Canvas>().ToConstant(this.Panel);
            kernel.Bind<PlayerController>().ToSelf().InSingletonScope();

            Player = kernel.Get<PlayerController>();

            Views = kernel.Get<Dictionary<IGameObject, TestTankView>>();
            Engine = kernel.Get<GameEngine>();

            Setup(Engine);

            var viewFactory = kernel.Get<TestGameViewFactory>();

            Engine.Start();
        }

        private void SetupInputObs()
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

            LeftDoubleClick = Observable.FromEvent((EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                ev => this.MouseDoubleClick += ev,
                ev => this.MouseDoubleClick -= ev).Where(e => e.EventArgs.ChangedButton == MouseButton.Left).Select(e => e.EventArgs.GetPosition(this));

            LeftSingleClick = MouseLeftButtonDownObs.Zip(MouseLeftButtonUpObs, (md, mu) => Tuple.Create(md,mu))
                .Where(t => (t.Item2 - t.Item1).Length <= 5).Select(t => t.Item1);

            RightSingleClick = MouseRightButtonDownObs.Zip(MouseRightButtonUpObs, (md, mu) => Tuple.Create(md, mu))
                .Where(t => (t.Item2 - t.Item1).Length <= 5).Select(t => t.Item1);

            LeftDrag = MouseLeftButtonDownObs.Zip(MouseLeftButtonUpObs, (md, mu) => Tuple.Create(md, mu))
                .Where(t => (t.Item2 - t.Item1).Length > 5).Select(t => new Rect(t.Item1, t.Item2));

            KeyPress = Observable.FromEvent((EventHandler<KeyEventArgs> ev) => new KeyEventHandler(ev),
                ev => this.KeyDown += ev,
                ev => this.KeyDown -= ev).Select(e => e.EventArgs.Key);
        }

        private void Setup(GameEngine engine)
        {

            engine.Players.Add(Player);

            //LeftDoubleClick.Subscribe(e => 
            //        {
            //            if (engine.IsRunning)
            //                engine.Stop();
            //            else
            //                engine.Start();
            //        });

            LeftDrag.Subscribe(r =>
                {
                    foreach (var tank in Player.SelectedObjects.Objects.OfType<TestTank>())
                    {
                        var view = Views[tank];
                        view.Model.Color = Colors.Blue;
                    }

                    var tanks = from t in Views
                                where t.Value.Model.Position.IsInside(r)
                                select t.Value.Model.Tank;

                    Player.SelectedObjects = new GOGroup(tanks.Cast<IGameObject>().ToList());

                    foreach (var tank in Player.SelectedObjects.Objects.OfType<TestTank>())
                    {
                        var view = Views[tank];
                        view.Model.Color = Colors.Red;
                    }
                });

            //TODO: movement;
            RightSingleClick.Subscribe(p =>
            {
                foreach (var tank in Player.SelectedObjects.Objects.OfType<TestTank>())
                {
                    var pos = p;

                    var moveCommand = new UnitMoveRequest(tank, p.ToVector2());
                    engine.Bus.Add(moveCommand);
                }

            });

            KeyPress.Where(k => k == Key.Escape).Subscribe(k =>
            {
                foreach (var tank in Player.SelectedObjects.Objects.OfType<TestTank>())
                {
                    var view = Views[tank];
                    view.Model.Color = Colors.Blue;
                }

                Player.SelectedObjects = GOGroup.Empty;
            });

            KeyPress.Where(k => k == Key.C).Subscribe(k =>
                {
                    engine.Bus.Add(new ObjectCreationRequest(typeof(TestTank)));
                });


            KeyPress.Where(k => k == Key.D).Subscribe(k =>
            {

                foreach (var tank in Player.SelectedObjects.Objects.OfType<TestTank>())
                {
                    var view = Views[tank];
                    view.Model.Destroy();
                    view.Model.Color = Colors.Blue;
                }

                Player.SelectedObjects = GOGroup.Empty;
            });

            engine.Timer.Subscribe(t =>
            {
                foreach (var view in Views.Values)
                {
                    view.Model.Update();
                }
            });

            engine.Bus.OfType<SystemMessage>().Subscribe(m => SysMessage.Dispatcher.Invoke(new Action(() => SysMessage.Content += m.Message + "\n")));
            engine.Bus.Filters.ObjectLifeTimeNotifications.Subscribe(m => SysMessage.Dispatcher.Invoke(new Action(() => SysMessage.Content += m.ToString() + "\n")));





            engine.Bus.Add(new ObjectCreationRequest(typeof(TestTank)));
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Engine.Stop();
            base.OnClosing(e);
        }
    }


}
