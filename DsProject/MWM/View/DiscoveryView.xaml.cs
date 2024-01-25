using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace FileExplorer.MWM.View
{
    /// <summary>
    /// Interaction logic for DiscoveryView.xaml
    /// </summary>
    public partial class DiscoveryView : UserControl , INotifyPropertyChanged
    {

        private object dummyNode = null;
        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; OnPropertyChanged();
                ListEntries.Items.Clear();
                LabelHandler();
            }
        }

        public DiscoveryView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (path == null)
            {
                foreach (string s in Directory.GetDirectories("C://"))
                {
                    Label item = new Label();
                    item.Content = s;
                    item.Tag = s;
                    item.FontWeight = FontWeights.Normal;
                    item.Foreground = Brushes.White;
                    item.MouseDoubleClick += Label_MouseDoubleClick;
                    ListEntries.Items.Add(item);
                }
            }
            else
            {
                LabelHandler();
            }
        }
        private void foldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Your code here. For example:
            TreeViewItem selectedItem = (TreeViewItem)e.NewValue;
            // Do something with selectedItem
        }

        private void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception) { }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;


        private void LabelHandler ()
        {
            foreach (var s in Directory.GetDirectories(path))
            {
                Label item = new Label();
                item.Content = s;
                item.Tag = s;
                item.FontWeight = FontWeights.Normal;
                item.Foreground = Brushes.White;
                item.MouseDoubleClick += Label_MouseDoubleClick;
                ListEntries.Items.Add(item);
            }
        }


        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
            {
                Path = lbl.Content.ToString();

            }
        }
    }
}
