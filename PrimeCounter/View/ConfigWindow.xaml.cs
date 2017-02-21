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
using PrimeCounter.ViewModel;

namespace PrimeCounter.View
{
    /// <summary>
    /// Interaktionslogik für ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow(ConfigViewmodel config)
        {
            InitializeComponent();
            DataContext = config;
        }

        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
