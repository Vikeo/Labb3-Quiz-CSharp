using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Labb3.Models
{
    public class Quiz: ICloneable
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

        //Copy Constructur
        public Quiz(Quiz otherQuiz)
        {
            _questions = otherQuiz.Questions;
            _title = otherQuiz.Title;
        }

        public Question GetRandomQuestion(Quiz quiz)
        {
            List<Question> questionsList = quiz.Questions.ToList();
            if (quiz.Questions.Count > 0)
            {
                Random r = new Random();
                
                var tempRandom = r.Next(questionsList.Count);

                var tempQuestion = questionsList[tempRandom];

                return tempQuestion;
            }
            else
            {
                return null;
            }
        }

        public void AddQuestion(string statement, Theme theme, int correctAnswer, params string[] answers)
        {
            Question newQuestion = new Question(statement, theme, correctAnswer, answers);
            Questions.Add(newQuestion);
        }

        public void RemoveQuestion(Question question)
        {
            Questions.Remove(question);
        }

        public void RemoveQuestion(int index)
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
