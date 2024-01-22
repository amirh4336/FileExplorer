using DsProject.Core;
using DsProject.MWM.View;
using DsProject.MWM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DsProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public class Purchase
    {
        public string? ProductName { get; set; }
        public DateTime DateTime { get; set; }

        public Decimal ProductPrice { get; set; }
    }

    public partial class MainWindow : Window
    {


        bool running = false;

        Purchase purchase = new Purchase
        {
            ProductName = "amir Hossien",
            DateTime = DateTime.UtcNow,
            ProductPrice = 2.49m
        };

        private object dummyNode = null;


        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // for showing Modal
            OpenModal();
            MainViewModel mainViewModel = new MainViewModel();
            Dictionary<string, RelayCommand> driveCommands = mainViewModel.DriveCommands;

            foreach (string s in Directory.GetLogicalDrives())
            {

                RadioButton rb = new RadioButton();
                rb.Content = s;
                rb.Height = 50;
                rb.Foreground = new SolidColorBrush(Colors.White);
                rb.FontSize = 14;
                rb.Style = (Style)FindResource("MenuButtomTheme");
                if (driveCommands.ContainsKey(s))
                {
                    rb.Command = driveCommands[s];
                }
                dynamicVolumes.Children.Add(rb);
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

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            //Application.Current.Shutdown();
        }
        // showing Modal Func 
        private void OpenModal()
        {
            CreateDataBase createDataBase = new CreateDataBase(this);
            createDataBase.ShowDialog();
            if (createDataBase.Success)
            {
                txtSearch.Text = createDataBase.Input;
            }
        }
    }
}
