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
        public string Title { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public Quiz(string title, ICollection<Question> questions)
        {
            Title = title;
            Questions = questions;
        }

        [JsonConstructor]
        public Quiz()
        {
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

        internal void ChangeCorrectAnswer(int index, int newCorrectAnswer)
        {
            //Detta gör det möjligt att ändra värdet på ett readonly-fält.

            List<Question> tempQuestionsList = Questions.ToList();
            Question tempNewQuestion = new Question(tempQuestionsList[index].Statement, tempQuestionsList[index].Theme, newCorrectAnswer, tempQuestionsList[index].Asked, tempQuestionsList[index].ImagePath, tempQuestionsList[index].Options);

            tempQuestionsList[index] = tempNewQuestion;
            Questions = tempQuestionsList;
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
    }
}
