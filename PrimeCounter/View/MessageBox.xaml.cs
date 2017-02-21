using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaktionslogik für MessageBox.xaml
    /// </summary>
    public partial class MessageBox : Window
    {
        
        public MessageBox()
        {
            Init("Message", "", null);
        }

        public MessageBox(string message)
        {
            Init("Message", message, null);
        }

        public MessageBox(string message, string title)
        {
            Init(title, message, null);
        }

        public MessageBox(string message, Window owner)
        {
            Init("Message", message, owner);
        }

        public MessageBox(string message, string title, Window owner)
        {
            Init(title, message, owner);
        }

        private void Init(string title, string message, Window owner)
        {
            InitializeComponent();
            var dc = (MessageBoxViewModel) DataContext;

            this.Owner = owner;
            dc.Message = message;
            dc.Title = title;
        }

        private void OKButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
