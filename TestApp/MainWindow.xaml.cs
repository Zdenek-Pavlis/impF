using System;
using System.Collections.Generic;
using System.ComponentModel;
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



namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Example1Click(object sender, RoutedEventArgs e)
        {
            new E1_MutualUpdate().Show();
        }

        private void Example2Click(object sender, RoutedEventArgs e)
        {
            new E2_SingleVm().Show();
        }

        private void Example3Click(object sender, RoutedEventArgs e)
        {
            new E3_VmComposition().Show();
        }

        private void Example4Click(object sender, RoutedEventArgs e)
        {
            new E4_Contract().Show();
        }

        
    }
}
