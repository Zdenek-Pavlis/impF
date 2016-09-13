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
using System.Windows.Shapes;

namespace TestApp
{

    public partial class E1_MutualUpdate : Window
    {
        public E1_MutualUpdate()
        {
            InitializeComponent();
            DataContext = Example1_MutualUpdate.VmFactory.newVm();
        }
    }
}
