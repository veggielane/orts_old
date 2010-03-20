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
using Orts.Core.MessageTypes;

namespace WpfTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameEngine Engine { get; set; }
        public List<TempItemView> ItemViews { get; set; }

        public MainWindow()
        {
            InitializeComponent();


            var kernal = new StandardKernel();

            kernal.Bind<GameEngine>().ToSelf();
            kernal.Bind<ObservableTimer>().To<AsyncObservableTimer>();
            kernal.Bind<MessageBus>().ToSelf().InSingletonScope();
            kernal.Bind<GameObjectFactory>().To<TempGameObjectFactory>();
            kernal.Bind<List<TempItemView>>().ToConstant(new List<TempItemView>());
            kernal.Bind<Canvas>().ToConstant(this.Panel);

            ItemViews = kernal.Get<List<TempItemView>>();
            Engine = kernal.Get<GameEngine>();

            Setup(Engine);

            Engine.Start();
        }


        private void Setup(GameEngine engine)
        {

            Observable.FromEvent((EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                ev => this.MouseDoubleClick += ev,
                ev => this.MouseDoubleClick -= ev).Subscribe(e => 
                    {
                        if (engine.IsRunning)
                            engine.Stop();
                        else
                            engine.Start();
                    });

            Observable.FromEvent((EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                ev => this.MouseRightButtonUp += ev,
                ev => this.MouseRightButtonUp -= ev).Subscribe(e =>
                {
                    engine.Bus.Add(new ObjectCreationRequest(engine.Timer.LastTickTime, typeof(TempItem)));
                });

            engine.Timer.Subscribe(t =>
            {
                foreach (var item in engine.MapItems().OfType<TempItemViewModel>())
                {
                    item.NotifyPropertyChanged();
                }
            });

            engine.Timer.Subscribe(t =>
            {
                foreach (var item in engine.MapItems().OfType<TempItem>())
                {
                    Debug.WriteLine(item.ToString());
                }
            });

            engine.Bus.OfType<SystemMessage>().Subscribe(m => SysMessage.Dispatcher.Invoke(new Action(() => SysMessage.Content += m.Message + "\n")));

            var item1 = new TempItemViewModel(engine.Bus) { Velocity = new Vector2(30, 30) };
            var item1View = new TempItemView(item1, this.Panel);


            var item2 = new TempItemViewModel(engine.Bus) { Position = new Vector2(100, 100), Velocity = new Vector2(-20, -20) };
            var item2View = new TempItemView(item2, this.Panel);
            item2.Color = Colors.Red;

            ItemViews.Add(item1View);
            ItemViews.Add(item2View);

            Engine.ObjectFactory.GameObjects.Add(item1);
            Engine.ObjectFactory.GameObjects.Add(item2);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Engine.Stop();
            base.OnClosing(e);
        }
    }

    public class TempItem : IMapGO
    {
        public MessageBus Bus { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public TempItem(MessageBus bus)
        {
            Bus = bus;
            Position = new Vector2();
            Velocity = new Vector2();
        }


        public void Update(TickTime tickTime)
        {
            Position = Position.Add(Velocity.Multiply(tickTime.GameTimeDelta.TotalSeconds));
        }



        public override string ToString()
        {
            return "TempItem - {{Pos:{0}}}".fmt(Position);
        }
    }

    public class TempItemViewModel : TempItem, INotifyPropertyChanged
    {
        public Color Color { get; set; }

        public TempItemViewModel(MessageBus bus)
            :base(bus)
        {
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



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class TempGameObjectFactory : GameObjectFactory
    {
        public Canvas Panel { get; private set; }
        public List<TempItemView> ItemViews { get; private set; }

        public TempGameObjectFactory(MessageBus bus, Canvas panel, List<TempItemView> itemViews)
            :base(bus)
        {
            this.Panel = panel;
            ItemViews = itemViews;
        }

        public override void CreateGameObject(ObjectCreationRequest request)
        {

            if (request.ObjectType == typeof(TempItem))
            {
                var item = new TempItemViewModel(this.Bus) { Velocity = new Vector2(30, 30) };
                TempItemView itemView = null;
                Panel.Dispatcher.Invoke(new Action(() => itemView = new TempItemView(item, this.Panel)));

                this.GameObjects.Add(item);
                Panel.Dispatcher.Invoke(new Action(() => ItemViews.Add(itemView)));
            }
            base.CreateGameObject(request);
        }

    }
}
