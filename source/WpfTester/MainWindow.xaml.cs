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

namespace WpfTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameEngine Engine { get; set; }
        public Dictionary<IGameObject, TestTankView> Views { get; set; }

        public MainWindow()
        {
            InitializeComponent();


            var kernel = new StandardKernel();

            kernel.Bind<GameEngine>().ToSelf();
            kernel.Bind<ObservableTimer>().To<AsyncObservableTimer>();
            kernel.Bind<MessageBus>().ToSelf().InSingletonScope();
            kernel.Bind<GameObjectFactory>().To<TestGameObjectFactory>().InSingletonScope();
            kernel.Bind<BusFilters>().ToSelf();
            kernel.Bind<Dictionary<IGameObject, TestTankView>>().ToConstant(new Dictionary<IGameObject, TestTankView>());
            kernel.Bind<Canvas>().ToConstant(this.Panel);

            Views = kernel.Get<Dictionary<IGameObject, TestTankView>>();
            Engine = kernel.Get<GameEngine>();

            Setup(Engine);

            var viewFactory = kernel.Get<TestGameViewFactory>();

            Engine.Start();
        }


        private void Setup(GameEngine engine)
        {

            Observable.FromEvent((EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                ev => this.MouseDoubleClick += ev,
                ev => this.MouseDoubleClick -= ev).Where(e => e.EventArgs.ChangedButton == MouseButton.Left).Subscribe(e => 
                    {
                        if (engine.IsRunning)
                            engine.Stop();
                        else
                            engine.Start();
                    });

            Observable.FromEvent((EventHandler<KeyEventArgs> ev) => new KeyEventHandler(ev),
                ev => this.KeyDown += ev,
                ev => this.KeyDown -= ev).Where(e => e.EventArgs.Key == Key.C).Subscribe(e =>
                {
                    Debug.WriteLine("C pressed.");
                    engine.Bus.Add(new ObjectCreationRequest(engine.Timer.LastTickTime, typeof(TestTank)));
                });

            engine.Timer.Subscribe(t =>
            {
                foreach (var view in Views.Values)
                {
                    view.Model.NotifyPropertyChanged();
                }
            });

            engine.Bus.OfType<SystemMessage>().Subscribe(m => SysMessage.Dispatcher.Invoke(new Action(() => SysMessage.Content += m.Message + "\n")));
            engine.Bus.Filters.ObjectLifeTimeNotifications.Subscribe(m => SysMessage.Dispatcher.Invoke(new Action(() => SysMessage.Content += m.ToString() + "\n")));


            engine.Bus.Add(new ObjectCreationRequest(engine.Timer.LastTickTime,typeof(TestTank)));
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Engine.Stop();
            base.OnClosing(e);
        }
    }

    public class TestTankViewModel : INotifyPropertyChanged
    {
        public Color Color { get; set; }
        public TestTank Tank { get; private set; }

        public TestTankViewModel(TestTank tank)
        {
            Tank = tank;
            Color = Colors.Blue;
        }

        public void NotifyPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Position"));
                PropertyChanged(this, new PropertyChangedEventArgs("Velocity"));
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
            Tank.Bus.Add(new ObjectDestructionRequest(null, Tank));
        }
    }

    public class TestGameViewFactory:IHasMessageBus
    {
        public GameEngine Engine { get; private set; }
        public MessageBus  Bus { get; private set; }
        public Canvas Panel { get; private set; }
        public Dictionary<IGameObject,TestTankView> Views { get; private set; }

        public TestGameViewFactory(GameEngine engine, MessageBus bus, Canvas panel, Dictionary<IGameObject, TestTankView> views)
        {
            Engine = engine;
            Bus = bus;
            this.Panel = panel;
            Views = views;

            Bus.Filters.ObjectLifeTimeNotifications.OfType<ObjectCreated>().Subscribe(m => CreateView(m));
            Bus.Filters.ObjectLifeTimeNotifications.OfType<ObjectDestroyed>().Subscribe(m => DestroyView(m));
        }
        
        public void CreateView(ObjectCreated notification)
        {
            Panel.Dispatcher.Invoke(new Action(() => 
                {
                    if (notification.GameObject is TestTank)
                    {
                        var model = new TestTankViewModel((TestTank)notification.GameObject);

                        TestTankView view = new TestTankView(model);

                        Panel.Children.Add(view);

                        Views.Add(notification.GameObject, view);
                    }
                }));
        }

        public void DestroyView(ObjectDestroyed notification)
        {
            Panel.Dispatcher.Invoke(new Action(() =>
                {
                    if(Views.ContainsKey(notification.GameObject))
                    {
                        var view = Views[notification.GameObject];
                        Panel.Children.Remove(view);
                        Views.Remove(notification.GameObject);
                    }
                }));
        }

    }
}
