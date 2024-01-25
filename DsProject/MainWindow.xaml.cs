using FileExplorer.Core;
using FileExplorer.MWM.View;
using FileExplorer.MWM.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using FileExplorer.ViewModels;

namespace FileExplorer
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

    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        //Image img = new Image();
        //img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Logo.png"));

//        myImageSource.UriSource = new Uri("Images/MyImage.png", UriKind.Relative);
//        myImageSource.EndInit();
//myImage.Source = myImageSource;


        Stack<string> stackLastPrev = new Stack<string>();

        Purchase purchase = new Purchase
        {
            ProductName = "amir Hossien",
            DateTime = DateTime.UtcNow,
            ProductPrice = 2.49m
        };

        public class MyDataItem
        {
            public string ImagePath { get; set; }
            public string Text { get; set; }
            public string Tag { get; set; }
        }


        

        

        public event PropertyChangedEventHandler? PropertyChanged;


        public MainViewModel Model
        {
            get => this.DataContext as MainViewModel;
            set => this.DataContext = value;
        }



        public MainWindow()
        {
            InitializeComponent();
            Model.TryNavigateToPath("");
            this.Loaded += new RoutedEventHandler(Window_Loaded);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // for showing Modal
            OpenModal();

            //ListEntries.Items.Clear();
            //if (path == null)
            //{
            //    foreach (string s in Directory.GetDirectories("C://"))
            //    {
            //        Label item = new Label();
            //        item.Content = s;
            //        item.Tag = s;
            //        item.FontWeight = FontWeights.Normal;
            //        item.Foreground = Brushes.White;
            //        item.MouseDoubleClick += Label_MouseDoubleClick;
            //        ListEntries.Items.Add(item);

            //    }
            //}

            foreach (string s in Directory.GetLogicalDrives())
            {

                RadioButton rb = new RadioButton();
                rb.Content = s;
                rb.Height = 50;
                rb.Foreground = new SolidColorBrush(Colors.White);
                rb.FontSize = 14;
                rb.Style = (Style)FindResource("MenuButtomTheme");
                rb.Click += (sender, e) =>
                {
                    //stackLastPrev.Push(path);
                    Model.TryNavigateToPath($@"{s}");
                    //Path = s;
                };
                dynamicVolumes.Children.Add(rb);
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



        //private void LabelHandler()
        //{

            
        //    //ListEntries.ItemsSource = items;


        //    //ListEntries.ItemsSource.Clear();
        //    if (!Directory.Exists(path))
        //    {
        //        foreach (var s in Directory.GetLogicalDrives())
        //        {
        //            Label item = new Label();
        //            string result = s.Replace(path, "");
        //            item.Content = result;
        //            item.Tag = s;
        //            item.FontWeight = FontWeights.Normal;
        //            item.Foreground = Brushes.White;
        //            item.MouseDoubleClick += Label_MouseDoubleClick;
        //            items.Add(new MyDataItem() { ImagePath = "Images/folderImg.jpg", Text = result, Tag =s });
        //            //ListEntries.Items.Add(item);
        //        }

        //    }
        //    else
        //    {
        //        foreach (var s in Directory.GetDirectories(path))
        //        {
        //            Label item = new Label();
        //            string result = s.Replace(path, "");
        //            item.Content = result;
        //            item.Tag = s;
        //            item.FontWeight = FontWeights.Normal;
        //            item.Foreground = Brushes.White;
        //            item.MouseDoubleClick += Label_MouseDoubleClick;
        //            //ListEntries.Items.Add(item);
        //            items.Add(new MyDataItem() { ImagePath = "Images/folderImg.jpg", Text = result, Tag = s });
        //        }
        //        foreach (var s in Directory.GetFiles(path))
        //        {
        //            Label item = new Label();
        //            string result = s.Replace(path, "");
        //            item.Content = result;
        //            item.Tag = s;
        //            item.FontWeight = FontWeights.Normal;
        //            item.Foreground = Brushes.White;
        //            item.MouseDoubleClick += Label_MouseDoubleClick;
        //            //ListEntries.Items.Add(item);
        //            items.Add(new MyDataItem() { ImagePath = "Images/folderImg.jpg", Text = result, Tag = s });
        //        }
        //        ListEntries.ItemsSource = items;
        //    }
        //}

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
            {
                //stackLastPrev.Push(path);
                //Path = lbl.Tag.ToString();

            }
        }

        private void txtDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Path = txtDir.Text;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (stackLastPrev.Count != 0)
            {
                //Path = stackLastPrev.Pop();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
