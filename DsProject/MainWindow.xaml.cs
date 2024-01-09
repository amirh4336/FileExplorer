using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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


        public MainWindow()
        {
            InitializeComponent();


        }

    }
}
