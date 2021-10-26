using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3.Models
{
    public class Quiz
    {
        //TODO Behöver inte tilldela ett värde?
        private ICollection<Question> _questions = new List<Question>();
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

        public Quiz()
        {
        }

        public void AddQuestionToQuiz(Quiz quiz, Question question)
        {
            //Lägg till en Questions till Questions-listan
        }

        public Question GetRandomQuestion(Quiz quiz)
        {
            return quiz.Questions.First();
        }


        //TODO Kolla med Niklas om jag måset använda den här metoden:
        public void AddQuestion(string statement, string theme, int correctAnswer, params string[] answers)
        {
            
        }
        public void RemoveQuestion(int index)
        {
        }
    }
}
