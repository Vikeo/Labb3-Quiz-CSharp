using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
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

        private string[] _options;
        public string[] Options
        {
            get { return _options; }
            set { _options = value; }
        }

        private bool _asked;
        public bool Asked
        {
            get { return _asked; }
            set { _asked = value; }
        }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

        private Theme _theme;
        public Theme Theme
        {
            get { return _theme;  }
            set { _theme = value; }
        }

        //Readonly variabler kan man tilldela värde till i en konstruktor.
        private readonly int _correctAnswer;
        public int CorrectAnswer
        {
            get { return _correctAnswer; }
        }

        //Konstruktor. [JsonConstructor] visar Deserializern vilken konstruktor den ska använda.
        [JsonConstructor]
        public Question(string statement, Theme theme, int correctAnswer, bool asked, string imagePath, params string[] options)
        {
            _statement = statement;
            _options = options;
            _theme = theme;
            _correctAnswer = correctAnswer;
            _asked = asked;
            _imagePath = imagePath;
        }

        public static void ChangeCorrectAnswer(Question question, int newCorrectAnswer)
        {
            //TODO Mutera inte den förra frågan, använd data från den och skapa en ny.
            //Detta gör det möjligt att ändra värdet på ett readonly-fält.
            typeof(Question).GetField("_correctAnswer", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(question, newCorrectAnswer);
        }

        public Question(int newCorrectAnswer)
        {
            _correctAnswer = newCorrectAnswer;
        }
        public Question()
        {

        }






    }
}
