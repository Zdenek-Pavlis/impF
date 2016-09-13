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

    public partial class E2_SingleVm : Window
    {
        public E2_SingleVm()
        {
            InitializeComponent();
            DataContext = Example2_SingleVm.VmFactory.newVm();
        }
    }
}
