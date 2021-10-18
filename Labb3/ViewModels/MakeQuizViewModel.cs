using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Labb3.ViewModels
{
    public class MakeQuizViewModel : ViewModelBase
    {
        private string _question;
        public string Question
        {
            get { return _question; }
            set
            {
                _question = value;
                OnPropertyChanged(nameof(Question));
            }
        }

        private string _correctAnswer;

        public string CorrectAnswer
        {
            get { return _correctAnswer; }
            set
            {
                _correctAnswer = value;
                OnPropertyChanged(nameof(CorrectAnswer));
            }
        }

        private string _otherAnswer;

        public string OtherAnswer
        {
            get { return _otherAnswer; }
            set
            {
                _otherAnswer = value;
                OnPropertyChanged(nameof(OtherAnswer));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }
    }
}
