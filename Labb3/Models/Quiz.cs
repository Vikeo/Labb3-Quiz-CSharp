using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        //Copy Constructur
        public Quiz(Quiz otherQuiz)
        {
            _questions = otherQuiz.Questions;
            _title = otherQuiz.Title;
        }

        public Question GetRandomQuestion()
        {
            //Den quizen som utför GetRandomQuestion kommer åt sina egna properties här. Bara att skriva Questions så blir det rätt.
            if (Questions.Count > 0)
            {
                Random r = new Random();

                //Random foreach. Ordnar listan
                foreach (var question in Questions.Where(q => q.Asked == false).OrderBy(q => r.Next()))
                {
                    if (question.Asked)
                    {
                        GetRandomQuestion();
                    }
                    else
                    {
                        question.Asked = true;
                        return question;
                    }
                }
            }
            return null;
        }

        internal void ResetQuestionsAsked()
        {
            foreach (var question in Questions)
            {
                question.Asked = false;
            }
        }

        public void AddQuestion(string statement, Theme theme, int correctAnswer, bool asked, string image, params string[] answers)
        {
            Question newQuestion = new Question(statement, theme, correctAnswer, asked, image,  answers);
            Questions.Add(newQuestion);
        }

        public void RemoveQuestion(Question question)
        {
            Questions.Remove(question);
        }

        public void RemoveQuestion(int index)
        {
        }
    }
}
