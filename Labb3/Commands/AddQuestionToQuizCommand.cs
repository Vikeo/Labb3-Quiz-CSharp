using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.Models;
using Labb3.ViewModels;

namespace Labb3.Commands
{
    
    class AddQuestionToQuizCommand : CommandBase
    {
        private readonly MakeQuizViewModel _makeQuizViewModel;
        private readonly Quiz _quiz;

        public AddQuestionToQuizCommand(MakeQuizViewModel makeQuizViewModel, Quiz quiz)
        {
            _makeQuizViewModel = makeQuizViewModel;
            _quiz = quiz;

            //_makeQuizViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        

        //Kan ändra den här och lägga in t.ex., om man inte har matat in något.
        public override bool CanExecute(object parameter)
        {
            //return !string.IsNullOrEmpty(_question.option1) && base.CanExecute(parameter);
            return true;
        }

        public override void Execute(object? parameter)
        {
            Question question = new Question(_makeQuizViewModel.Statement, _makeQuizViewModel.Option1, 
                _makeQuizViewModel.Option2, _makeQuizViewModel.Option3, _makeQuizViewModel.Theme, _makeQuizViewModel.CorrectAnswer);


            _quiz.AddQuestionToQuiz(_quiz, question);

            //Lägg till något till quiz-objektet

        }

        //private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == nameof(MakeQuizViewModel.Option1))
        //    {   
        //        OnCanExecutedChanged();
        //    }
        //}

    }
}