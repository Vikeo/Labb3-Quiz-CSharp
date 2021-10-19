using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.Commands;

namespace Labb3.Models
{
    public class Quiz
    {
        private ICollection<Question> _questions;
        public ICollection<Question> Questions
        {
            get { return _questions; }
            set { _questions = value; }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public Quiz(string title, ICollection<Question> questions)
        {
            _title = title;
            _questions = questions;

        }

        //TODO Helt fel metod, använder inte det som man matar in
        public void AddQuestionToQuiz(Quiz quiz, Question question)
        {
            //Lägg till en Questions till Questions-listan
            _questions.Add(question);
        }
    }
}
