using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Labb3.Models
{
    public class Quiz
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private ICollection<Question> _questions = new List<Question>();
        public ICollection<Question> Questions
        {
            get { return _questions; }
            set { _questions = value; }
        }

        public Quiz(string title, ICollection<Question> questions)
        {
            _title = title;
            _questions = questions;
        }

        public Quiz()
        {
            //JsonDeserialize använder denna
        }

        public Question GetRandomQuestion()
        {
            //Den quizen som utför GetRandomQuestion kommer åt sina egna properties här. Bara att skriva Questions så blir det rätt.
            if (Questions.Count > 0)
            {
                Random r = new Random();

                //Random foreach. Ordnar listan
                foreach (var question in Questions.Where(q => q.Asked == false && q.Theme.Selected).OrderBy(q => r.Next()))
                {
                    //TODO bara 'else' borde räcka här, men vill vara säker på at Theme.Selected = true ATM.
                    if (question.Theme.Selected)
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

        internal void ResetThemeSelected()
        {
            foreach (var question in Questions)
            {
                question.Theme.Selected = false;
            }
        }

        public void AddQuestion(string statement, Theme theme, int correctAnswer, bool asked, string image, params string[] answers)
        {
            Question newQuestion = new Question(statement, theme, correctAnswer, asked, image,  answers);
            Questions.Add(newQuestion);
        }
        public void RemoveQuestion(int index)
        {
            List<Question> tempQuestionsList = Questions.ToList();

            Question tempQuestion = tempQuestionsList[index];

            Questions.Remove(tempQuestion);
        }
    }
}
