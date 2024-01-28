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
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Windows.Documents;
using System.Text;
using DsProject.TreeStructure;

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

        //public static int CalculateJsonSize(object obj)
        //{
        //    var options = new JsonSerializerOptions
        //    {
        //        WriteIndented = true
        //    };

        //    using var stream = new MemoryStream();
        //    using var writer = new Utf8JsonWriter(stream);
        //    JsonSerializer.Serialize(writer, obj, options);
        //    writer.Flush();

        //    return (int)stream.Length;
        //}







        public event PropertyChangedEventHandler? PropertyChanged;


        public MainViewModel Model
        {
            get => this.DataContext as MainViewModel;
            set => this.DataContext = value;
        }

        public GeneralTree<string> PCTree = new GeneralTree<string>("This PC");

        public MainWindow()
        {
        IPosition<string> treeRoot = PCTree.Root;
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
            Model.TryNavigateToPath("");
            Model.TryNavigateWithTree(PCTree, treeRoot);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Specify the JSON file path
            IPosition<string> treeRoot = PCTree.Root;
            // for showing Modal
            OpenModal();


            PCTree.AddChild(treeRoot , "vulme 1");
            PCTree.AddChild(treeRoot, "vulme 2");
            PCTree.AddChild(treeRoot, "vulme 3");


            txtDir.Text = treeRoot.Element;




            RadioButton rbFile = new RadioButton();
            rbFile.Content = treeRoot.Element;
            rbFile.Height = 50;
            rbFile.Foreground = new SolidColorBrush(Colors.White);
            rbFile.FontSize = 14;
            rbFile.Style = (Style)FindResource("MenuButtomTheme");
            rbFile.Click += (sender, e) =>
            {
                //stackLastPrev.Push(path);
                Model.TryNavigateWithTree(PCTree, treeRoot);
                //Path = s;
            };
            dynamicFileSystem.Children.Add(rbFile);

            foreach (IPosition<string> child in PCTree.Children(treeRoot))
            {

                RadioButton rb = new RadioButton();
                rb.Content = child.Element;
                rb.Height = 50;
                rb.Foreground = new SolidColorBrush(Colors.White);
                rb.FontSize = 14;
                rb.Style = (Style)FindResource("MenuButtomTheme");
                dynamicVolumes.Children.Add(rb);
            }

            //dynamicFileSystem.Children.Add();

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
                //txtSearch.Text = createDataBase.Input;
                //string filePath = "data.json";

                //// Specify the desired file size in bytes

                //// Generate dummy data to fill the file
                //string dummyData = GenerateDummyData(fileSizeInBytes);

                //// Write the dummy data to the file
                //File.WriteAllText(filePath, dummyData);

                string path = "YourFilePath.json";
                long targetSize = int.Parse(createDataBase.Input) * 1024; // 1KB
                //int targetSize = 1024; // Target size in bytes

                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    while (fs.Length < targetSize)
                    {
                        var data = new { Key = "Value" }; // Your data here
                        string json = JsonConvert.SerializeObject(data);
                        byte[] bytes = Encoding.UTF8.GetBytes(json);
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }


                txtSearch.Text = $"{targetSize} bytes";
            }
        }

        private void txtDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
                Model.BtnBack_Click();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Model.BtnNext_Click();
        }
    }
}
