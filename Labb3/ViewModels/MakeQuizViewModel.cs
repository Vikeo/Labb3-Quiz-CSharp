using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Labb3.Commands;
using Labb3.Managers;
using Labb3.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb3.ViewModels
{
    public class MakeQuizViewModel : ObservableObject
    {
        //TODO Vill att _statements ska vara = Quizzes[n].Questions / det som man är Selected på Quizzes comboboxen.
        

        private string _statement;
        public string Statement
        {
            get { return _statement; }
            set
            {
                SetProperty(ref _statement, value);
                _statement = value;
                
            }
        }

        private ObservableCollection<Quiz> _quizzes = QuizManager._allQuizzes;
        public ObservableCollection<Quiz> Quizzes
        {
            get { return _quizzes; }
            set
            {
                SetProperty(ref _quizzes, value);
                _quizzes = value;
 
            }
        }

        private Quiz _selectedQuiz = new Quiz("TEMP", new List<Question>());

        public Quiz SelectedQuiz
        {
            get { return _selectedQuiz; }
            set
            {
                SetProperty(ref _selectedQuiz, value);
                _selectedQuiz = value;
                _statements.Clear();
                foreach (var question in _selectedQuiz.Questions)
                {
                    _statements.Add(question);
                }
                
            }
        }

        private ObservableCollection<Question> _statements = new ObservableCollection<Question>();
        public ObservableCollection<Question> Statements
        {
            get { return _statements; }
            set
            {
                SetProperty(ref _statements, value);
                _statements = value;

            }
        }

        private Question _selectedQuestion;

        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                SetProperty(ref _selectedQuestion, value);
                _selectedQuestion = value;

            }
        }

        private string _quizTitle;
        public string QuizTitle
        {
            get { return _quizTitle; }
            set
            {
                SetProperty(ref _quizTitle, value);
                _quizTitle = value;

            }
        }

        //CorrectAnswer comboboxen listar de tre alternativ som man har skrivit in.
        private string _correctAnswer;
        public string CorrectAnswer
        {
            get { return _correctAnswer; }
            set
            {
                SetProperty(ref _correctAnswer, value);
                _correctAnswer = value;
            }
        }

        private ObservableCollection<string> _options = new ObservableCollection<string>() { "", "", "", "" };
        public ObservableCollection<string> Options
        {
            get { return _options; }
            set
            {
                SetProperty(ref _options, value);
                _options = value;
                
            }
        }

        private string _option1;
        public string Option1
        {
            get { return _option1; }
            set
            {
                SetProperty(ref _option1, value);
                _option1 = value;
                _options[1] = _option1;
                
            }
        }

        private string _option2;
        public string Option2
        {
            get { return _option2; }
            set
            {
                SetProperty(ref _option2, value);
                _option2 = value;
                _options[2] = _option2;

            }
        }

        private string _option3;
        public string Option3
        {
            get { return _option3; }
            set
            {
                SetProperty(ref _option3, value);
                _option3 = value;
                _options[3] = _option3;
                
            }
        }

        private string _theme;
        public string Theme
        {
            get { return _theme; }
            set
            {
                SetProperty(ref _theme, value);
                _theme = value;
                
            }
        }

        private ObservableCollection<Quiz> _listOfQuizzes;
        public ObservableCollection<Quiz> ListOfQuizzes
        {
            get { return _listOfQuizzes; }
            set
            {
                SetProperty(ref _listOfQuizzes, value);
                //_listOfQuizzes = QuizManager._allQuizzes;

            }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand CreateQuizCommand { get; }

        public MakeQuizViewModel(QuizManager quizzes, Quiz quiz)
        {
            //CreateQuizCommand = new CreateQuizCommand(this, quizzes, quiz);
            AddCommand = new RelayCommand(AddQuestionToQuiz, CanAddQuestionToQuiz);
            CreateQuizCommand = new RelayCommand(AddQuiz, CanAddQuiz);
            EditCommand = new RelayCommand(EditQuestion, CanEditQuestion);
            
            PropertyChanged += OnViewModelPropertyChanged;  
        }

        //Det som sker när man trycker på AddCommand TODO Gör så att det lägger till frågan i SelectedQuiz
        public void AddQuestionToQuiz()
        {
            Question question = new Question(Statement,
                Option1,
                Option2,
                Option3,
                Theme,
                CorrectAnswer);

            SelectedQuiz.Questions.Add(question);
        }

        //Kollar om man kan lägga till frågor till quizen.
        public bool CanAddQuestionToQuiz()
        {
            //Om man lägger till ny fråga
            if (!string.IsNullOrEmpty(Option1)&&
                !string.IsNullOrEmpty(Option2) &&
                !string.IsNullOrEmpty(Option3) &&
                !string.IsNullOrEmpty(CorrectAnswer) &&
                !string.IsNullOrEmpty(Statement) &&
                !string.IsNullOrEmpty(Theme))
            {
                return true;
            }

            //Om man redigerar en fråga
            if (//!string.IsNullOrEmpty(SelectedQuiz) &&
                !string.IsNullOrEmpty(Statement) &&
                !string.IsNullOrEmpty(Option1) &&
                !string.IsNullOrEmpty(Option2) &&
                !string.IsNullOrEmpty(Option3) &&
                !string.IsNullOrEmpty(CorrectAnswer) &&
                !string.IsNullOrEmpty(Theme))

            {
                return true;

            }
            else
            {
                return false;
            }

        }

        public void AddQuiz()
        {
            QuizManager._allQuizzes.Add(new Quiz(QuizTitle, new List<Question>()));
        }

        public bool CanAddQuiz()
        {
            //Om man vill lägga till en ny quiz
            return !string.IsNullOrEmpty(QuizTitle);
        }

        public void EditQuestion()
        {
            SelectedQuestion.Statement = Statement;
            SelectedQuestion.Option1 = Option1;
            SelectedQuestion.Option2 = Option2;
            SelectedQuestion.Option3 = Option3;
            //SelectedQuestion.CorrectAnswer = CorrectAnswer;
            SelectedQuestion._theme = Theme;
        }

        //Kollar om man kan lägga till frågor till quizen.
        public bool CanEditQuestion()
        {
            //Om man redigerar en fråga
            //if (SelectedQuestion.Option1 != "" || SelectedQuestion.Option1 != null)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(MakeQuizViewModel.Option1) ||
                e.PropertyName == nameof(MakeQuizViewModel.Option2) ||
                e.PropertyName == nameof(MakeQuizViewModel.Option3) ||
                e.PropertyName == nameof(MakeQuizViewModel.CorrectAnswer) ||
                e.PropertyName == nameof(MakeQuizViewModel.QuizTitle) ||
                e.PropertyName == nameof(MakeQuizViewModel.Statement) ||
                e.PropertyName == nameof(MakeQuizViewModel.Theme))
            {
                CreateQuizCommand.NotifyCanExecuteChanged();
                AddCommand.NotifyCanExecuteChanged();
            }
        }
    }
}
