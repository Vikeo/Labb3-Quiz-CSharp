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

        //TODO Fixa bild.
        private string _image;
        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }

        private Theme _theme;
        public Theme Theme
        {
            get { return _theme;  }
            set { _theme = value; }
        }

        private int _correctAnswer;
        public int CorrectAnswer
        {
            get { return _correctAnswer; }
            set { _correctAnswer = value; }
        }

        //Konstruktor. [JsonConstructor] visar Deserializern vilken konstruktor den ska använda.
        [JsonConstructor]
        public Question(string statement, Theme theme, int correctAnswer, bool asked, string image, params string[] options)
        {
            _statement = statement;
            _options = options;
            _theme = theme;
            _correctAnswer = correctAnswer;
            _asked = asked;
            _image = image;
        }

        public static void ChangeCorrectAnswer(Question question, int newCorrectAnswer)
        {
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
