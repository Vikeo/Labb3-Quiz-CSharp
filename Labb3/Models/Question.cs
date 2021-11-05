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
        public string Statement { get; set; }
        public string[] Options { get; set; }
        public bool Asked { get; set; }
        public string ImagePath { get; set; }
        public Theme Theme { get; set; }

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
            Statement = statement;
            Options = options;
            Theme = theme;
            _correctAnswer = correctAnswer;
            Asked = asked;
            ImagePath = imagePath;
        }

        public Question()
        {

        }
    }
}
