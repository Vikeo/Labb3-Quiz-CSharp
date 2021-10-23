using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3.Models
{
    public class Question
    {
        //Fullprops
        private string _statement;

        public string Statement
        {
            get { return _statement; }
            set { _statement = value; }
        }

        private string _option1;
        public string Option1
        {
            get { return _option1; }
            set { _option1 = value; }
        }

        private string _option2;
        public string Option2
        {
            get { return _option2; }
            set { _option2 = value; }
        }

        private string _option3;
        public string Option3
        {
            get { return _option3; }
            set { _option3 = value; }
        }

        public string _theme;
        public string Theme
        {
            get { return _theme;  }
            set { _theme = value; }
        }

        //Readonly variabler kan man tilldela värde till i en konstruktor.
        //TODO Fråga niklas om det måste vara INT
        private readonly string _correctAnswer;

        public string CorrectAnswer
        {
            get { return _correctAnswer; }
        }

        //Konstruktor. [JsonConstructor] visar Deserializern vilken konstruktor den ska använda.
        [JsonConstructor]
        public Question(string statement, string option1, string option2, string option3, string theme, string correctAnswer)
        {
            _statement = statement;
            _option1 = option1;
            _option2 = option2;
            _option3 = option3;
            _theme = theme;
            _correctAnswer = correctAnswer;
        }

        public static void ChangeCorrectAnswer(Question question, string newCorrectAnswer)
        {
            //Detta gör det möjligt att ändra värdet på ett readonly-fält.
            typeof(Question).GetField("_correctAnswer", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(question, newCorrectAnswer);
        }

        
        public Question(string newCorrectAnswer)
        {
            _correctAnswer = newCorrectAnswer;
        }
        public Question()
        {
            
        }

      




    }
}
