using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Messages;
using TestGame;
using Orts.Core;
using System.Windows.Controls;
using Orts.Core.GameObjects;
using Orts.Core.Players;
using System.Windows.Media;

namespace WpfTester
{
    public class TestGameViewFactory : IHasMessageBus
    {
        public GameEngine Engine { get; private set; }
        public MessageBus Bus { get; private set; }
        public Canvas Panel { get; private set; }
        public Dictionary<IGameObject, TestTankView> Views { get; private set; }
        public PlayerController Player { get; set; }

        public TestGameViewFactory(GameEngine engine, MessageBus bus, Canvas panel, Dictionary<IGameObject, TestTankView> views, PlayerController player)
        {
            Engine = engine;
            Bus = bus;
            this.Panel = panel;
            Views = views;
            Player = player;

            Bus.Filters.ObjectLifeTimeNotifications.OfType<ObjectCreated>().Subscribe(m => CreateView(m));
            Bus.Filters.ObjectLifeTimeNotifications.OfType<ObjectDestroyed>().Subscribe(m => DestroyView(m));
        }

        public void CreateView(ObjectCreated notification)
        {
            Panel.Dispatcher.Invoke(new Action(() =>
            {
                if (notification.GameObject is TestTank)
                {
                    var model = new TestTankViewModel(Bus, (TestTank)notification.GameObject);

                    TestTankView view = new TestTankView(model);

                    view.LeftSingleClick.Subscribe(p =>
                        {
                            foreach (var tank in Player.SelectedObjects.Objects.OfType<TestTank>())
                            {
                                var view2 = Views[tank];
                                view2.Model.Color = Colors.Blue;
                            }

                            Player.SelectedObjects = new GOGroup(notification.GameObject);

                            foreach (var tank in Player.SelectedObjects.Objects.OfType<TestTank>())
                            {
                                var view2 = Views[tank];
                                view2.Model.Color = Colors.Red;
                            }
                        });

                    Panel.Children.Add(view);

                    Views.Add(notification.GameObject, view);
                }
            }));
        }

        public void DestroyView(ObjectDestroyed notification)
        {
            Panel.Dispatcher.Invoke(new Action(() =>
            {
                if (Views.ContainsKey(notification.GameObject))
                {
                    var view = Views[notification.GameObject];
                    Panel.Children.Remove(view);
                    Views.Remove(notification.GameObject);
                }
            }));
        }

    }
}
