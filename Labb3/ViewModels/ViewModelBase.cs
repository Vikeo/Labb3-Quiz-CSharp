using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Labb3.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //Kallar på metoder för att meddela programmet
        //att en property har förändrats, så att Viewsen
        //kan hämta det uppdaterade property-värder och
        //uppdateran UIn 
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
