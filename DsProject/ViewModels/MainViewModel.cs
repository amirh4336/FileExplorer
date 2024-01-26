using FileExplorer.Explorer;
using FileExplorer.Files;
using NamespaceHere;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileExplorer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
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
        }

        #region Navigation

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

        #endregion

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
    }
}
