using DsProject.TreeStructure;
using FileExplorer.Files;
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

namespace DsProject.Files
{
    /// <summary>
    /// Interaction logic for FilesControlSystem.xaml
    /// </summary>
    public partial class FilesControlSystem : UserControl
    {
        public FilesControlSystem()
        {
            InitializeComponent();
        }

        public IPosition<string> File
        {
            get => this.DataContext as IPosition<string>;
            set => this.DataContext = value;
        }

        /// <summary>
        /// A callback used for telling 'something' to navigate to the path
        /// </summary>
        public Action<IPosition<string>> NavigateToPathCallback { get; set; }

        public Action<IPosition<string>> SelectedItemCallback { get; set; }

        //public FilesControl()
        //{
        //    InitializeComponent();
        //    File = new FileModel();
        //}

        public FilesControlSystem(IPosition<string> fModel)
        {
            InitializeComponent();
            TxtNameEl.Text = fModel.Element;
            File = fModel;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left &&
                e.LeftButton == MouseButtonState.Pressed &&
                e.ClickCount == 2)
            {
                NavigateToPathCallback?.Invoke(File);
            }

            if (e.ChangedButton == MouseButton.Right &&
                e.RightButton == MouseButtonState.Pressed &&
                e.ClickCount == 1)
            {
                SelectedItemCallback?.Invoke(File);
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NavigateToPathCallback?.Invoke(File);
            }
        }
    }
}
