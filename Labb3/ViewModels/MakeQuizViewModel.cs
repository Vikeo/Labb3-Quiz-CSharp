using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Labb3.Commands;
using Labb3.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb3.ViewModels
{
    public class MakeQuizViewModel : ObservableObject
    {

        private string _statement;
        public string Statement
        {
            get { return _statement; }
            set
            {
                _statement = value;
                OnPropertyChanged(nameof(Statement));
            }
        }

        private string _quiz;
        public string Quiz
        {
            get { return _quiz; }
            set
            {
                _quiz = value;
                OnPropertyChanged(nameof(Quiz));
            }
        }

        //TODO göra så att CorrectAnswer comboboxen listar de tre alternativ som man har skrivit in.
        private string _correctAnswer;
        public string CorrectAnswer
        {
            get { return _correctAnswer; }
            set
            {
                _correctAnswer = value;
                OnPropertyChanged(nameof(CorrectAnswer));
            }
        }

        private string _option1;
        public string Option1
        {
            get { return _option1; }
            set
            {
                _option1 = value;
                OnPropertyChanged(nameof(Option1));
            }
        }

        private string _option2;
        public string Option2
        {
            get { return _option2; }
            set
            {
                _option2 = value;
                OnPropertyChanged(nameof(Option2));
            }
        }

        private string _option3;
        public string Option3
        {
            get { return _option3; }
            set
            {
                _option3 = value;
                OnPropertyChanged(nameof(Option3));
            }
        }

        private string _theme;
        public string Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        public MakeQuizViewModel(Quiz quiz)
        {
            AddCommand = new AddQuestionToQuizCommand(this, quiz);
        }
    }
}
