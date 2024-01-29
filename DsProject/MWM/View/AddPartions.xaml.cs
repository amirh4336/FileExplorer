using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Shapes;

namespace DsProject.MWM.View
{
    /// <summary>
    /// Interaction logic for AddPartions.xaml
    /// </summary>
    public partial class AddPartions : Window
    {

        public bool Success { get; set; }
        public string InputName { get; set; }
        public string InputSize { get; set; }

        long SpaceLeft { get; set; }

        public AddPartions(Window parentWindow, long blankSize)
        {
            Owner = parentWindow;
            SpaceLeft = blankSize;
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Success = true;
            InputName = txtNameInput.Text;
            InputSize = txtSizeInput.Text;

            Close();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void txtNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            inputValidate();
        }

        private void txtSizeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            inputValidate();
        }


        void inputValidate()
        {
            if (!string.IsNullOrEmpty(txtSizeInput.Text) && !string.IsNullOrEmpty(txtNameInput.Text) && Int32.TryParse(txtSizeInput.Text, out int n))
            {
                long sizePart = long.Parse(txtSizeInput.Text) * 1024;
                if (SpaceLeft >= sizePart)
                {
                    btnOk.IsEnabled = true;
                }
                else
                {
                    btnOk.IsEnabled = false;
                }
            }
            else
            {
                btnOk.IsEnabled = false;
            }
        }
    }
}
