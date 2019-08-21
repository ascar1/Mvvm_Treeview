using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MvvmLight1.Behavior
{
    class TreeViewBehavior : Behavior<TreeView>
    {
   /*     private static Dictionary<TreeViewBehavior, DependencyObject> behaivors = new Dictionary<TreeViewBehavior, DependencyObject>();


        protected override void OnAttached()
        {
            base.OnAttached();
            if (!behaivors.ContainsKey(this))
            {
                behaivors.Add(this, AssociatedObject);
            }
            this.AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            throw new NotImplementedException();
        }*/
    }
}
