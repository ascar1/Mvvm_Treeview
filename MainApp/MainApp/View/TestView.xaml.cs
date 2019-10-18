using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainApp.View
{
    /// <summary>
    /// Логика взаимодействия для TestView.xaml
    /// </summary>
    public partial class TestView : UserControl
    {
        public TestView()
        {
         /*   Grid grid = new Grid();
            grid.ShowGridLines = true;
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());


            Button btn = new Button();
            btn.Content = "test";
            Grid.SetRow(btn, 2);
            Grid.SetColumn(btn, 0);
            Grid.SetColumnSpan(btn, 2);
            grid.Children.Add(btn);
            */
            
            InitializeComponent();
           // this.Content = grid;
        }
    }
}
