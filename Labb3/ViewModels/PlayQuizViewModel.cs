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
            //Kan sätta denna som "ItemSource" till grejer
            _questions = new ObservableCollection<QuestionViewModel>();

            _questions.Add(new QuestionViewModel(new Question("lol1?", "pog", "log", "dog", "Dogs", "dog")));
            _questions.Add(new QuestionViewModel(new Question("lol2?", "pog", "log", "dog", "Twitch", "pog")));
            _questions.Add(new QuestionViewModel(new Question("lol3?", "pog", "log", "dog", "Nature", "log")));
        }
    }
}
