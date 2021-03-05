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

namespace GTA_V_Phoenix_Handling_Editor
{
    /// <summary>
    /// Interaction logic for ProgressWaiter.xaml
    /// </summary>
    public partial class ProgressWaiter : Window
    {
        public ProgressWaiter()
        {
            InitializeComponent();
        }
        public Task Update(float precentage)
        {
            ProgressBar.Value = precentage;
            return Task.CompletedTask;
        }
    }
}
