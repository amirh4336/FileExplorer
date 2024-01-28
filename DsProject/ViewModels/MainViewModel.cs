using DsProject.Files;
using DsProject.TreeStructure;
using FileExplorer.Explorer;
using FileExplorer.Files;
using NamespaceHere;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace FileExplorer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        // file system
        public ObservableCollection<FilesControlSystem> FileItemsSystem { get; set; }

        public IPosition<string> ParentNode;

        public IPosition<string> SelectedPosition;

        public IPosition<string> CopyPosition;

        public IPosition<string> CutPosition;

        public GeneralTree<string> PCtree;

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
                Process.Start(@path);
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

        public void TryNavigateWithTree(GeneralTree<string> tempTree, IPosition<string> p)
        {

            PCtree = tempTree;
            ParentNode = p;

            PathSys = ShowingPath();

            IEnumerable<IPosition<string>> Children = PCtree.Children(p);

            ClearFilesSystem();

            foreach (IPosition<string> child in Children)
            {

                FilesControlSystem fc = CreateFileControl(child);
                AddFile(fc);
            }
        }
        public void NavigateFromModel(IPosition<string> p)
        {

            ParentNode = p;
            PathSys = ShowingPath();

            if (ParentNode == CutPosition)
            {
                CutPosition = null;
            }

            ClearFilesSystem();
            IEnumerable<IPosition<string>> Children = PCtree.Children(p);
            foreach (IPosition<string> child in Children)
            {

                FilesControlSystem fc = CreateFileControl(child);
                AddFile(fc);
            }
        }


        #endregion

        // file System

        public void AddFile(FilesControlSystem file)
        {
            FileItemsSystem.Add(file);
        }

        public FilesControlSystem CreateFileControl(IPosition<string> fModel)
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

        // copy past cut add file System

        public void AddPartion(string namePartion)
        {
            PCtree.AddChild(ParentNode, namePartion);
            Refresh();
        }


        public void AddFolder(string nameFolder)
        {
            if (ParentNode != PCtree.Root)
                PCtree.AddChild(ParentNode, nameFolder);
            Refresh();
        }

        public string ShowingPath()
        {
            List<string> pathNodes = new List<string>();

            IPosition<string> position = ParentNode;

            while (position != null)
            {
                pathNodes.Add(position.Element);
                position = PCtree.Parent(position);
            }

            pathNodes.Reverse();


            return string.Join("/", pathNodes);
            ;
        }


        public void BackDirctory()
        {
            IPosition<string> position = PCtree.Parent(ParentNode);

            if (position != null)
            {
                NavigateFromModel(position);
            }
        }

        public void SelectItem(IPosition<string> select)
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
                PCtree.Copy(CopyPosition, ParentNode);
            }
            if (CutPosition != null)
            {
                PCtree.Cut(CutPosition, ParentNode);
            }


            CopyPosition = null;
            CutPosition = null;
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
