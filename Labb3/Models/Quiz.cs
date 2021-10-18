using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3.Models
{
    class Quiz
    {
        private ICollection<Question> _question;
        public ICollection<Question> Question
        {
            get { return _question; }
            set { _question = value; }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
    }
}
