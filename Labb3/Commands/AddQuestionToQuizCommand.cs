using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.Managers;
using Labb3.Models;
using Labb3.ViewModels;

namespace Labb3.Commands
{
    
    class AddQuestionToQuizCommand : CommandBase
    {
        private readonly MakeQuizViewModel _makeQuizViewModel;
        private readonly Quiz _quiz;


        public AddQuestionToQuizCommand(MakeQuizViewModel makeQuizViewModel, QuizManager quizzes, Quiz quiz)
        {
            _makeQuizViewModel = makeQuizViewModel;
            _quiz = quiz;

            _makeQuizViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        //Kan ändra den här och lägga in t.ex., om man inte har matat in något.
        public override bool CanExecute(object parameter)
        {
            //Om man lägger till ny fråga
            if (!string.IsNullOrEmpty(_makeQuizViewModel.Option1) &&
                !string.IsNullOrEmpty(_makeQuizViewModel.Option2) &&
                !string.IsNullOrEmpty(_makeQuizViewModel.Option3) &&
                !string.IsNullOrEmpty(_makeQuizViewModel.CorrectAnswer) &&
                !string.IsNullOrEmpty(_makeQuizViewModel.Statement) &&
                !string.IsNullOrEmpty(_makeQuizViewModel.Theme) &&
                base.CanExecute(parameter))
            {
                return true;
            }

            //Om man redigerar en fråga
            if (!string.IsNullOrEmpty(_makeQuizViewModel.Option1) &&
                   !string.IsNullOrEmpty(_makeQuizViewModel.Option2) &&
                   !string.IsNullOrEmpty(_makeQuizViewModel.Option3) &&
                   !string.IsNullOrEmpty(_makeQuizViewModel.CorrectAnswer) &&
                   !string.IsNullOrEmpty(_makeQuizViewModel.Theme) &&
                   base.CanExecute(parameter))
            {
                
            }
            return !string.IsNullOrEmpty(_makeQuizViewModel.Option1) &&
                   !string.IsNullOrEmpty(_makeQuizViewModel.Option2) &&
                   !string.IsNullOrEmpty(_makeQuizViewModel.Option3) &&
                   !string.IsNullOrEmpty(_makeQuizViewModel.CorrectAnswer) &&
                   !string.IsNullOrEmpty(_makeQuizViewModel.QuizTitle) &&
                   !string.IsNullOrEmpty(_makeQuizViewModel.Statement) &&
                   !string.IsNullOrEmpty(_makeQuizViewModel.Theme) &&
                   base.CanExecute(parameter);
            //return true;
            
        }

        //Det som händer när man trycker på knappen
        public override void Execute(object? parameter)
        {
            Question question = new Question(_makeQuizViewModel.Statement, 
                _makeQuizViewModel.Option1, 
                _makeQuizViewModel.Option2, 
                _makeQuizViewModel.Option3, 
                _makeQuizViewModel.Theme, 
                _makeQuizViewModel.CorrectAnswer);


            //Lägg till något till quiz-objektet
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MakeQuizViewModel.Option1) || 
                e.PropertyName == nameof(MakeQuizViewModel.Option2) || 
                e.PropertyName == nameof(MakeQuizViewModel.Option3) || 
                e.PropertyName == nameof(MakeQuizViewModel.CorrectAnswer) || 
                e.PropertyName == nameof(MakeQuizViewModel.QuizTitle) || 
                e.PropertyName == nameof(MakeQuizViewModel.Statement) || 
                e.PropertyName == nameof(MakeQuizViewModel.Theme))
            {
                NotifyCanExecuteChanged();
            }
        }
    }
}