using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.Managers;
using Labb3.Models;
using Labb3.ViewModels;

namespace Labb3.Commands
{
    class CreateQuizCommand : CommandBase
    {
        private readonly MakeQuizViewModel _makeQuizViewModel;
        private readonly Quiz _quiz;
        private readonly string _quizTitle;


        public CreateQuizCommand(MakeQuizViewModel makeQuizViewModel, QuizManager quizzes, Quiz quiz)
        {
            _makeQuizViewModel = makeQuizViewModel;
            _quiz = quiz;

            _makeQuizViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        //Det som händer när man trycker på knappen
        public override void Execute(object? parameter)
        {
            //Skapa en ny Quiz och lägg till den i QuizManager-listan
            QuizManager._allQuizzes.Add(new Quiz(_makeQuizViewModel.QuizTitle, new List<Question>()));
        }

        //Kan ändra den här och lägga in t.ex., om man inte har matat in något.
        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_makeQuizViewModel.QuizTitle) && 
                   base.CanExecute(parameter);
            //return true;

        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MakeQuizViewModel.QuizTitle))
            {
                NotifyCanExecuteChanged();
            }
        }
    }
}
