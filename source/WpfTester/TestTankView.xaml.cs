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

namespace WpfTester
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TestTankView : UserControl
    {
        public TestTankViewModel Model { get; set; }

        public TestTankView()
        {
            InitializeComponent();
        }

        public TestTankView(TestTankViewModel model)
            :this()
        {
            this.Model = model;
            this.DataContext = model;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Model.Destroy();
        }
    }
}
