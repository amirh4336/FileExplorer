using DsProject.Files;
using DsProject.TreeStructure;
using FileExplorer.Explorer;
using FileExplorer.Files;
using NamespaceHere;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace FileExplorer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        // file system
        public ObservableCollection<FilesControlSystem> FileItemsSystem { get; set; }

        public IPosition<ElementItem> ParentNode;

        public IPosition<ElementItem> SelectedPosition;

        public IPosition<ElementItem> CopyPosition;

        public IPosition<ElementItem> CutPosition;

        public ElementItem File;

        public GeneralTree<ElementItem> PCtree;

        private string pathWaySys = "";


        public string PathSys
        {
            get { return pathWaySys; }
            set
            {
                pathWaySys = value;

                OnPropertyChanged();
            }
        }


        // file Explorer
        public ObservableCollection<FilesControl> FileItems { get; set; }



        Stack<string> stackLastPrev = new Stack<string>();

        Stack<string> stackLastNext = new Stack<string>();

        private string pathWay = "";


        public string Path
        {
            get { return pathWay; }
            set
            {
                pathWay = value;
                stackLastPrev.Push(pathWay);
                OnPropertyChanged();
            }
        }



        public MainViewModel()
        {
            FileItems = new ObservableCollection<FilesControl>();
            FileItemsSystem = new ObservableCollection<FilesControlSystem>();
        }

        #region Navigation

        // file Explorer

        public void TryNavigateToPath(string path)
        {
            Path = path;
            // is a drive
            if (path == string.Empty)
            {
                ClearFiles();

                foreach (FileModel drive in Fetcher.GetDrives())
                {
                    FilesControl fc = CreateFileControl(drive);
                    AddFile(fc);
                }
            }

            else if (path.IsFile())
            {
                // Open the file
                MessageBox.Show($"Opening @{path}");
                //Process.Start(@path);
                //Process.Start(@"D:\Screenshot 2024-01-10 125229.png");
                try
                {
                }
                catch (Exception e)
                {
                    throw new Exception("af");
                    // Errorhandling
                }
            }

            else if (path.IsDirectory())
            {
                ClearFiles();

                foreach (FileModel dir in Fetcher.GetDirectories(path))
                {
                    FilesControl fc = CreateFileControl(dir);
                    AddFile(fc);
                }

                foreach (FileModel file in Fetcher.GetFiles(path))
                {
                    FilesControl fc = CreateFileControl(file);
                    AddFile(fc);
                }
            }

            else
            {
                // something bad has happened...
            }
        }

        public void NavigateFromModel(FileModel file)
        {
            TryNavigateToPath(file.Path);
        }

        // file System

        public void TryNavigateWithTree(GeneralTree<ElementItem> tempTree, IPosition<ElementItem> p)
        {

            PCtree = tempTree;

            if (p.Element.Path != null)
            {
                MessageBox.Show($"Opening @{p.Element.Path}");
            }
            else
            {

                ParentNode = p;
                PathSys = ShowingPath();

                IEnumerable<IPosition<ElementItem>> Children = PCtree.Children(p);

                ClearFilesSystem();

                foreach (IPosition<ElementItem> child in Children)
                {

                    FilesControlSystem fc = CreateFileControl(child);
                    AddFile(fc);
                }

            }
        }
        public void NavigateFromModel(IPosition<ElementItem> p)
        {


            if (p.Element.Path != null)
            {
                MessageBox.Show($"Opening @{p.Element.Path}");

            }
            else
            {
                ParentNode = p;

                if (ParentNode == CutPosition)
                {
                    CutPosition = null;
                }
                PathSys = ShowingPath();

                IEnumerable<IPosition<ElementItem>> Children = PCtree.Children(p);

                ClearFilesSystem();

                foreach (IPosition<ElementItem> child in Children)
                {

                    FilesControlSystem fc = CreateFileControl(child);
                    AddFile(fc);
                }

            }

        }


        #endregion

        // file System

        public void AddFile(FilesControlSystem file)
        {
            FileItemsSystem.Add(file);
        }

        public FilesControlSystem CreateFileControl(IPosition<ElementItem> fModel)
        {
            FilesControlSystem fc = new FilesControlSystem(fModel);
            SetupFileControlCallbacks(fc);
            fc.SelectedItemCallback = SelectItem;
            return fc;
        }

        public void SetupFileControlCallbacks(FilesControlSystem fc)
        {
            fc.NavigateToPathCallback = NavigateFromModel;
        }
        public void ClearFilesSystem()
        {
            FileItemsSystem.Clear();
        }

        // file Explorer

        public void AddFile(FilesControl file)
        {
            FileItems.Add(file);
        }

        public void RemoveFile(FilesControl file)
        {
            FileItems.Remove(file);
        }

        public void ClearFiles()
        {
            FileItems.Clear();
        }

        public FilesControl CreateFileControl(FileModel fModel)
        {
            FilesControl fc = new FilesControl(fModel);
            SetupFileControlCallbacks(fc);
            fc.SelectedItemCallback = SelectItem;
            return fc;
        }

        public void SetupFileControlCallbacks(FilesControl fc)
        {
            fc.NavigateToPathCallback = NavigateFromModel;
        }


        public void BtnBack_Click()
        {
            if (stackLastPrev.Count > 1)
            {
                stackLastNext.Push(stackLastPrev.Pop());
                stackLastNext.Push(stackLastPrev.Peek());
                TryNavigateToPath(stackLastPrev.Pop());
            }
            else if (stackLastPrev.Count == 1)
            {
                stackLastNext.Push(stackLastPrev.Pop());
                TryNavigateToPath("");
            }
        }

        public void BtnNext_Click()
        {
            if (stackLastNext.Count > 1)
            {
                stackLastPrev.Push(stackLastNext.Pop());
                stackLastPrev.Push(stackLastNext.Peek());
                TryNavigateToPath(stackLastNext.Pop());
            }
            else if (stackLastNext.Count == 1)
            {
                stackLastPrev.Push(stackLastNext.Pop());
                TryNavigateToPath("");
            }
        }

        public void SelectItem(FileModel select)
        {
            ElementItem target = new ElementItem(select.Name, select.Path);

            File = target;
        }

        // copy past cut add file System

        public void AddPartion(string namePartion, long size)
        {
            PCtree.AddChild(PCtree.Root, new ElementItem(namePartion, size));
            Refresh();
        }


        public void AddFolder(string nameFolder)
        {
            IPosition<ElementItem> partion = ParentNode;

            while (partion.Element.Size == null)
            {
                partion = PCtree.Parent(partion);
            }

            int partionjson = System.Text.Encoding.UTF8.GetByteCount(ConvertTreeToJson(partion)) + System.Text.Encoding.UTF8.GetByteCount(nameFolder) + 64;

            if (partionjson <= partion.Element.Size)
            {
                PCtree.AddChild(ParentNode, new ElementItem(nameFolder));
            }
            else
            {
                Window errorWindow = new Window
                {
                    Title = "Error",
                    Content = "Partion is full",
                    SizeToContent = SizeToContent.WidthAndHeight,
                };

                // Show the window modally
                Nullable<bool> dialogResult = errorWindow.ShowDialog();
            }



            Refresh();
        }


        public void AddFile(ElementItem el)
        {

            IPosition<ElementItem> partion = ParentNode;

            while (partion.Element.Size == null)
            {
                partion = PCtree.Parent(partion);
            }

            int partionjson = System.Text.Encoding.UTF8.GetByteCount(ConvertTreeToJson(partion)) + 164;

            if (partionjson <= partion.Element.Size)
            {
                if (ParentNode != PCtree.Root)
                    PCtree.AddChild(ParentNode, el);
            }
            else
            {
                Window errorWindow = new Window
                {
                    Title = "Error",
                    Content = "Partion is full",
                    SizeToContent = SizeToContent.WidthAndHeight,
                };

                // Show the window modally
                Nullable<bool> dialogResult = errorWindow.ShowDialog();
            }


            Refresh();
        }

        public string ShowingPath()
        {
            List<string> pathNodes = new List<string>();

            IPosition<ElementItem> position = ParentNode;

            while (position != null)
            {
                pathNodes.Add(position.Element.Name);
                position = PCtree.Parent(position);
            }

            pathNodes.Reverse();


            return string.Join("/", pathNodes);
            ;
        }


        public void BackDirctory()
        {
            IPosition<ElementItem> position = PCtree.Parent(ParentNode);

            if (position != null)
            {
                NavigateFromModel(position);
            }
        }

        public void SelectItem(IPosition<ElementItem> select)
        {
            SelectedPosition = select;
        }

        public void DeleteItem()
        {
            if (SelectedPosition != null)
            {
                PCtree.Delete(SelectedPosition);

            }
            Refresh();
        }

        public void CopyItem()
        {
            if (SelectedPosition != null)
            {
                CopyPosition = SelectedPosition;
                SelectedPosition = null;
                CutPosition = null;
            }
        }


        public void CutItem()
        {
            if (SelectedPosition != null)
            {
                CutPosition = SelectedPosition;
                SelectedPosition = null;
                CopyPosition = null;
            }
        }

        public void PasteItem()
        {

            if (CopyPosition != null)
            {
                if (HasEnoughSpace(ParentNode, CopyPosition))
                {
                    PCtree.Copy(CopyPosition, ParentNode);
                }
                else
                {
                    Window errorWindow = new Window
                    {
                        Title = "Error",
                        Content = "Partion is full",
                        SizeToContent = SizeToContent.WidthAndHeight,
                    };

                    // Show the window modally
                    Nullable<bool> dialogResult = errorWindow.ShowDialog();
                }


            }
            if (CutPosition != null)
            {
                if (HasEnoughSpace(ParentNode, CutPosition))
                {
                    PCtree.Cut(CutPosition, ParentNode);
                }
                else
                {
                    Window errorWindow = new Window
                    {
                        Title = "Error",
                        Content = "Partion is full",
                        SizeToContent = SizeToContent.WidthAndHeight,
                    };

                    // Show the window modally
                    Nullable<bool> dialogResult = errorWindow.ShowDialog();
                }
            }



            CopyPosition = null;
            CutPosition = null;
            Refresh();
        }

        private bool HasEnoughSpace(IPosition<ElementItem> p, IPosition<ElementItem> targetPostion)
        {
            IPosition<ElementItem> partion = p;

            while (partion.Element.Size == null)
            {
                partion = PCtree.Parent(partion);
            }

            string partionSize = ConvertTreeToJson(partion);

            string targetSize = ConvertTreeToJson(targetPostion);

            int fullSize = System.Text.Encoding.UTF8.GetByteCount(targetSize) + System.Text.Encoding.UTF8.GetByteCount(partionSize);

            return fullSize <= partion.Element.Size;
        }

        private string ConvertTreeToJson(IPosition<ElementItem> node)
        {
            JsonTreeNode jsonTree = ConvertNodeToJson(node);


            return System.Text.Json.JsonSerializer.Serialize(jsonTree);
        }

        private JsonTreeNode ConvertNodeToJson(IPosition<ElementItem> node)
        {
            IEnumerable<JsonTreeNode> arr = PCtree.Children(node).Select(child => ConvertNodeToJson(child));

            return new JsonTreeNode { Element = node.Element, Children = arr };
        }

        public void ImportFile()
        {
            IPosition<ElementItem> partion = ParentNode;

            while (partion.Element.Size == null)
            {
                partion = PCtree.Parent(partion);
            }

            int partionjson = System.Text.Encoding.UTF8.GetByteCount(ConvertTreeToJson(partion)) + 164;

            if (partionjson <= partion.Element.Size)
            {
                if (File != null && ParentNode != PCtree.Root)
                {
                    PCtree.AddChild(ParentNode, File);
                    File = null;
                }
            }
            else
            {
                Window errorWindow = new Window
                {
                    Title = "Error",
                    Content = "Partion is full",
                    SizeToContent = SizeToContent.WidthAndHeight,
                };

                // Show the window modally
                Nullable<bool> dialogResult = errorWindow.ShowDialog();
            }


            Refresh();
        }

        public void Refresh()
        {
            if (ParentNode != null)
            {

                NavigateFromModel(ParentNode);
            }
        }


    }
}
