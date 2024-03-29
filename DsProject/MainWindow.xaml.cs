﻿using FileExplorer.MWM.View;
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
using DsProject.MWM.View;
using static DsProject.TreeStructure.GeneralTree<DsProject.TreeStructure.ElementItem>;
using System.Linq;

namespace FileExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 






    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public GeneralTree<ElementItem> PCTree = new GeneralTree<ElementItem>(new ElementItem("THIS PC"));


        public MainViewModel Model
        {
            get => this.DataContext as MainViewModel;
            set => this.DataContext = value;
        }

        public MainWindow()
        {



            IPosition<ElementItem> treeRoot = PCTree.Root;
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
            Model.TryNavigateToPath("");
            Model.TryNavigateWithTree(PCTree, treeRoot);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            string filePath = Environment.CurrentDirectory;
            string filePathWithFileName = System.IO.Path.Combine(filePath, "TreeJson.json");

            if (File.Exists(filePathWithFileName))
            {
                LoadTreeFromJsonFile(filePathWithFileName);
                Model.TryNavigateWithTree(PCTree, PCTree.Root);
            }
            else
            {
                // for showing Modal
                OpenModal();

            }

            IPosition<ElementItem> treeRoot = PCTree.Root;
            // Specify the JSON file path


            txtDir.Text = treeRoot.Element.Name;

            SideBarBtn();




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
                    Model.TryNavigateToPath($@"{s}");
                };
                dynamicVolumes.Children.Add(rb);
            }
        }

        // side Bar button 
        private void SideBarBtn()
        {
            IPosition<ElementItem> treeRoot = PCTree.Root;
            dynamicFileSystem.Children.Clear();


            RadioButton rbFile = new RadioButton();
            rbFile.Content = treeRoot.Element.Name;
            rbFile.Height = 50;
            rbFile.Foreground = new SolidColorBrush(Colors.White);
            rbFile.FontSize = 14;
            rbFile.Style = (Style)FindResource("MenuButtomTheme");
            rbFile.Click += (sender, e) =>
            {
                Model.TryNavigateWithTree(PCTree, treeRoot);
            };
            dynamicFileSystem.Children.Add(rbFile);

            foreach (IPosition<ElementItem> child in PCTree.Children(treeRoot))
            {

                RadioButton rb = new RadioButton();
                rb.Content = child.Element.Name;
                rb.Height = 50;
                rb.Foreground = new SolidColorBrush(Colors.White);
                rb.FontSize = 14;
                rb.Style = (Style)FindResource("MenuButtomTheme");
                rb.Click += (sender, e) =>
                {
                    Model.NavigateFromModel(child);
                };
                dynamicFileSystem.Children.Add(rb);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        // serilize and deserilize 
        private void LoadTreeFromJsonFile(string filePath)
        {
            var json = File.ReadAllText(filePath);

            JsonTreeNode objectTree = System.Text.Json.JsonSerializer.Deserialize<JsonTreeNode>(json);

            if (objectTree.Children != null)
            {

                GeneralTree<ElementItem> tempTree = new GeneralTree<ElementItem>(objectTree.Element);
                foreach (JsonTreeNode item in objectTree.Children)
                    ConvertJsonTreeToTree(tempTree, tempTree.Root, item.Element, item.Children);
                PCTree = tempTree;
            }
            else
            {
                Window errorWindow = new Window
                {
                    Title = "Error",
                    Content = "the json file has not contain tree structur",
                    SizeToContent = SizeToContent.WidthAndHeight,
                };

                // Show the window modally
                Nullable<bool> dialogResult = errorWindow.ShowDialog();
            }

        }

        private void ConvertJsonTreeToTree(GeneralTree<ElementItem> tree, IPosition<ElementItem> parentNode, ElementItem element, IEnumerable<JsonTreeNode> children)
        {
            IPosition<ElementItem> node = tree.AddChild(parentNode, element);
            foreach (JsonTreeNode item in children)
            {
                ConvertJsonTreeToTree(tree, node, item.Element, item.Children);
            }
        }


        private string ConvertTreeToJson(GeneralTree<ElementItem> tree)
        {
            JsonTreeNode jsonTree = ConvertNodeToJson(tree.Root);


            return System.Text.Json.JsonSerializer.Serialize(jsonTree);
        }

        private JsonTreeNode ConvertNodeToJson(IPosition<ElementItem> node)
        {
            IEnumerable<JsonTreeNode> arr = PCTree.Children(node).Select(child => ConvertNodeToJson(child));

            return new JsonTreeNode { Element = node.Element, Children = arr };
        }

        private void SaveTreeToJsonFile()
        {
            // Convert the general tree to a JSON representation
            string json = ConvertTreeToJson(PCTree);

            // Specify the JSON file path
            string filePath = Environment.CurrentDirectory;
            string filePathWithFileName = System.IO.Path.Combine(filePath, "TreeJson.json");
            File.WriteAllText(filePathWithFileName, json);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SaveTreeToJsonFile();
            Close();
            //Application.Current.Shutdown();
        }
        // showing Modals Func 
        private void OpenModal()
        {

            CreateDataBase createDataBase = new CreateDataBase(this);
            createDataBase.ShowDialog();
            if (createDataBase.Success)
            {
                long targetSize = int.Parse(createDataBase.Input) * 1024; // 1KB
                GeneralTree<ElementItem> tenpTree = new GeneralTree<ElementItem>(new ElementItem("THIS PC", targetSize));
                PCTree = tenpTree;
                Model.TryNavigateWithTree(PCTree, PCTree.Root);
            }
        }


        private void OpenPartionnModal()
        {
            IPosition<ElementItem> root = PCTree.Root;

            long rootSize = root.Element.Size ?? 0;
            long partionsSize = 0;
            foreach (IPosition<ElementItem> item in PCTree.Children(root))
            {
                partionsSize += item.Element.Size ?? 0;
            }

            long blankSpace = rootSize - partionsSize;


            AddPartions addPartions = new AddPartions(this, blankSpace);
            addPartions.ShowDialog();
            if (addPartions.Success)
            {
                long sizePart = long.Parse(addPartions.InputSize) * 1024;
                string namePart = addPartions.InputName;

                Model.AddPartion(namePart, sizePart);
            }
        }

        private void OpenFolderModal()
        {
            AddFolder addFolder = new AddFolder(this);
            addFolder.ShowDialog();
            if (addFolder.Success)
            {
                string nameFolder = addFolder.Input;

                Model.AddFolder(nameFolder);
            }
        }

        private void OpenTextFile()
        {

            string currentDir = Environment.CurrentDirectory;

            CreateTextFile createTextFile = new CreateTextFile(this);
            createTextFile.ShowDialog();
            if (createTextFile.Success)
            {
                string nameText = $"{createTextFile.Input}.txt";

                string PathText = $"{currentDir}/{nameText}";

                File.WriteAllText(PathText, PathText);

                ElementItem element = new ElementItem(nameText, PathText);

                Model.AddFile(element);
            }
        }

        // btns


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

        private void AddPartion_Click(object sender, RoutedEventArgs e)
        {

            OpenPartionnModal();
            SideBarBtn();
        }

        private void addFolder_Click(object sender, RoutedEventArgs e)
        {
            if (Model.ParentNode != PCTree.Root && Model.ParentNode != null)
            {
                OpenFolderModal();
            }
        }

        private void btnBackFileSystem_Click(object sender, RoutedEventArgs e)
        {
            Model.BackDirctory();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Model.CopyItem();
        }

        private void cut_Click(object sender, RoutedEventArgs e)
        {
            Model.CutItem();
        }

        private void paste_Click(object sender, RoutedEventArgs e)
        {
            Model.PasteItem();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Model.DeleteItem();
            SideBarBtn();
        }

        private void AddFileTxt_Click(object sender, RoutedEventArgs e)
        {
            if (Model.ParentNode != PCTree.Root && Model.ParentNode != null)
                OpenTextFile();
        }

        private void ImportFile_Click(object sender, RoutedEventArgs e)
        {
            Model.ImportFile();
        }

        private void ImportTree_Click(object sender, RoutedEventArgs e)
        {
            ElementItem file = Model.File;
            if (file != null && File.Exists(file.Path) && file.Path.Contains(".json"))
            {
                LoadTreeFromJsonFile(file.Path);
                Model.TryNavigateWithTree(PCTree, PCTree.Root);
                SideBarBtn();
            }
        }
    }
}
