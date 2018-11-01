using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace SocketServer
{
    class UpdateText:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
