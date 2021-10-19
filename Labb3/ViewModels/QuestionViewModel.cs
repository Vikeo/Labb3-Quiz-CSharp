using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3.ViewModels
{
    class QuestionViewModel : ObservableObject
    {
        private readonly Question _question;
        public string Statement => _question.Statement;
        public string Option1 => _question.Option1;
        public string Option2 => _question.Option2;
        public string Option3 => _question.Option3;
        public string Theme => _question.Theme;
        public string CorrectAnswer => _question.CorrectAnswer;

        public QuestionViewModel(Question question)
        {
            _question = question;
        }

    }
}
