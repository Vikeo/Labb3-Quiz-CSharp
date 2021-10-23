using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Labb3.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3.ViewModels
{
    class PlayQuizViewModel : ObservableObject
    {
        private readonly ObservableCollection<QuestionViewModel> _questions;

        //Är IEnumerable för att då inte klassen utanför denna klass lägga till/ta bort eller ändra saker i listan.
        //Om man inte vill kapsla in de så kan den vara ObservableCollection istället.

        public ObservableCollection<QuestionViewModel> Questions => _questions;

        public ICommand StartGameCommand
        {
            get;
        }

        public PlayQuizViewModel()
        {


            
        }
    }
}
