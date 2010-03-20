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
    public partial class TempItemView : UserControl
    {
        public TempItem Item { get; set; }
        public Canvas ParentCanvas { get; set; }


        public TempItemView()
        {
            InitializeComponent();
        }

        public TempItemView(TempItem item, Canvas canvas)
            :this()
        {
            this.Item = item;
            this.DataContext = item;
            this.ParentCanvas = canvas;

            this.ParentCanvas.Children.Add(this);
        }
    }
}
