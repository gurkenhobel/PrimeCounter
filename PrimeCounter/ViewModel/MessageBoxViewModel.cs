using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCounter.ViewModel
{
    public class MessageBoxViewModel: BaseViewModel
    {
        private string _message;
        private string _title;


        public MessageBoxViewModel()
        {
            
        }

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }
}
