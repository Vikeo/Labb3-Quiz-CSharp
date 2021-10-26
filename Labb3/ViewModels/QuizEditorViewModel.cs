using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Labb3.Managers;
using Labb3.Models;
using Labb3.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb3.ViewModels
{
    public class QuizEditorViewModel : ObservableObject
    {
        //TODO Vill att _questions ska vara = Quizzes[n].Questions / det som man är Selected på Quizzes comboboxen.
        private readonly NavigationStore _navigationStore;
        private readonly QuizManager _quizManager;

        #region Properties

        private string _newStatement;
        public string NewStatement
        {
            get { return _newStatement; }
            set
            {
                SetProperty(ref _newStatement, value);
                _newStatement = value;
            }
        }

        //Kanske gör ett nytt objekt, "QuizOptions", mata in allt som behövs visas i den.
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
                //Den första if-satsen gör så att jag kan skippa att ha med en extra TextBox. Kan skapa ny med ComboBox med denna.
                //if (Quizzes.Any(q => q.Title == SelectedQuiz.Title))
                //{
                    SetProperty(ref _selectedQuiz, value);
                    _selectedQuiz = value;

                    if (_questions != null && _selectedQuiz != null)
                    {
                        _questions.Clear();

                        foreach (var question in _selectedQuiz.Questions)
                        {
                            _questions.Add(question);
                        }
                    }
                //}
            }
        }

        private ObservableCollection<Question> _questions = new();
        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set
            {
                SetProperty(ref _questions, value);
                
            }
        }

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                SetProperty(ref _selectedQuestion, value);
                
                //TODO value blir null av någon anledning, kanske borde fixa det men det funkar "som vanligt" med if-satsen.
                if (value != null)
                {
                    _selectedQuestion = value;

                    NewStatement = SelectedQuestion.Statement;
                    Option1 = SelectedQuestion.Option1;
                    Option2 = SelectedQuestion.Option2;
                    Option3 = SelectedQuestion.Option3;
                    Theme = SelectedQuestion.Theme;
                    CorrectAnswer = SelectedQuestion.CorrectAnswer;
                }
            }
        }

        private string _newQuizTitle;
        public string NewQuizTitle
        {
            get { return _newQuizTitle; }
            set
            {
                SetProperty(ref _newQuizTitle, value);
                
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
                
            }
        }

        private ObservableCollection<string> _options = new ObservableCollection<string>() { "", "", "", "" };
        public ObservableCollection<string> Options
        {
            get { return _options; }
            set
            {
                SetProperty(ref _options, value);
                
                
            }
        }

        private string _option1;
        public string Option1
        {
            get { return _option1; }
            set
            {
                SetProperty(ref _option1, value);
                
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
              
                _options[3] = _option3;
                
            }
        }

        private string _theme;
        private NavigationStore navigationStore;

        public string Theme
        {
            get { return _theme; }
            set
            {
                SetProperty(ref _theme, value);
                _theme = value;
                
            }
        }
        #endregion

        //TODO Lägg till ett Command för att ändra namnet på Quiz. Ha det på samma Button som CreateQuizCommand om det går?
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand RemoveQuizCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand CreateQuizCommand { get; }

        //Konstruktor
        public QuizEditorViewModel(NavigationStore navigationStore, QuizManager quizManager)
        {
            _navigationStore = navigationStore;
            _quizManager = quizManager;
            //CreateQuizCommand = new CreateQuizCommand(this, quizzes, quiz);

            AddCommand = new RelayCommand(AddQuestionToQuiz, CanAddQuestionToQuiz);
            CreateQuizCommand = new RelayCommand(AddQuiz, CanAddQuiz);
            EditCommand = new RelayCommand(EditQuestion, CanEditQuestion);
            RemoveCommand = new RelayCommand(RemoveQuestion, CanRemoveQuestion);
            AddImageCommand = new RelayCommand(AddImageToQuestion, CanAddImage);
            RemoveQuizCommand = new RelayCommand(RemoveQuiz, CanRemoveQuiz);

            PropertyChanged += OnViewModelPropertyChanged;
        }

        //Det som sker när man trycker på AddCommand
        public void AddQuestionToQuiz()
        {
            Question question = new Question(NewStatement,
                Option1,
                Option2,
                Option3,
                Theme,
                CorrectAnswer);

            SelectedQuiz.Questions.Add(question);

            //Gör så att Propertyn uppdateras
            var tempQuiz = SelectedQuiz;
            SelectedQuiz = tempQuiz;

            QuizManager.SaveQuizzes(QuizManager._allQuizzes);
        }

        //Kollar om man kan lägga till frågor till quizen.
        public bool CanAddQuestionToQuiz()
        {
            //Om man lägger till ny fråga
            if (!string.IsNullOrEmpty(Option1) &&
                !string.IsNullOrEmpty(Option2) &&
                !string.IsNullOrEmpty(Option3) &&
                !string.IsNullOrEmpty(CorrectAnswer) &&
                !string.IsNullOrEmpty(NewStatement) &&
                !string.IsNullOrEmpty(Theme) &&
                SelectedQuiz.Title != "TEMP" &&
                Option1 != Option2 &&
                Option1 != Option3 &&
                Option2 != Option1 &&
                Option2 != Option3 &&
                Option3 != Option1 &&
                Option3 != Option2)
            {

                //Kolla om det finns ett en fråga med samma NewStatement som det man har skrivit in. LINQ
                if (SelectedQuiz.Questions.All(q => q.Statement != NewStatement))
                {
                    return true;
                }

                return false;

            }

            else
            {
                return false;
            }
        }

        public void AddQuiz()
        {
            //Gör så att Propertyn uppdateras
            Quiz tempNewQuiz = new Quiz(NewQuizTitle, new List<Question>());

            QuizManager._allQuizzes.Add(tempNewQuiz);
            SelectedQuiz = QuizManager._allQuizzes[^1];


            Option1 = "";
            Option2 = "";
            Option3 = "";
            CorrectAnswer = "";
            NewStatement = "";
        }
        public bool CanAddQuiz()
        {
            //Om man vill lägga till en ny quiz.
            //Kollar först det finns en quiz med samma Titel.
            if (QuizManager._allQuizzes.All(q => q.Title != NewQuizTitle))
            {
                return !string.IsNullOrEmpty(NewQuizTitle);
            }
            return false;
        }

        public void EditQuestion()
        {
            //TODO Kan inte ändra Statement till något annat. Det som står i NewStatement måste just nu vara == SelectedQuiz.Questions.Statement,
            //så det råkar alltid bli samma
            SelectedQuestion.Statement = NewStatement;
            SelectedQuestion.Option1 = Option1;
            SelectedQuestion.Option2 = Option2;
            SelectedQuestion.Option3 = Option3;
            Question.ChangeCorrectAnswer(SelectedQuestion, CorrectAnswer);
            SelectedQuestion._theme = Theme;

            //TODO Gör så att listan uppdateras i vyn.........

            //EditCommand.NotifyCanExecuteChanged();

            if (Questions != null)
            {
                Questions.Clear();

                foreach (var question in SelectedQuiz.Questions)
                {
                    Questions.Add(question);
                }
            }
        }
        public bool CanEditQuestion()
        {
            //Om man redigerar en fråga.
            //Kollar om man har en SelectedQuiz, SelectedQuestion och att alla fält är ifyllda

            //Kollar om fälten är ifyllda
            if (!string.IsNullOrEmpty(NewStatement) &&
                !string.IsNullOrEmpty(Option1) &&
                !string.IsNullOrEmpty(Option2) &&
                !string.IsNullOrEmpty(Option3) &&
                !string.IsNullOrEmpty(CorrectAnswer) &&
                !string.IsNullOrEmpty(Theme) &&
                SelectedQuiz.Title != "TEMP" &&
                SelectedQuestion != null &&
                SelectedQuestion.Statement != NewStatement)
            {
                //TODO Lägga till en if-sats som kolla SelectedQuestion.Statement så att det är samma som man editerar.
                return true;
            }

            return false;

        }

        public void RemoveQuestion()
        {
            SelectedQuiz.Questions.Remove(SelectedQuestion);
            ClearTextboxes();

            EditCommand.NotifyCanExecuteChanged();

            //Gör så att listan uppdateras i vyn.........
            //if (Questions != null)
            //{
            //    Questions.Clear();

            //    foreach (var question in SelectedQuiz.Questions)
            //    {
            //        Questions.Add(question);
            //    }
            //}
        }
        public bool CanRemoveQuestion()
        {
            if (SelectedQuestion != null)
            {
                return true;
            }

            return false;
        }

        public void RemoveQuiz()
        {
            Quizzes.Remove(SelectedQuiz);

            SelectedQuiz = DefaultSelectedQuiz();
            ClearTextboxes();
        }
        public bool CanRemoveQuiz()
        {
            if (SelectedQuiz != null)
            {
                return true;
            }

            return false;
        }

        public void AddImageToQuestion()
        {
            //TODO Not implemented
        }
        public bool CanAddImage()
        {
            //TODO Not implemented
            return true;
        }

        private void ClearTextboxes()
        {
            Option1 = "";
            Option2 = "";
            Option3 = "";
            CorrectAnswer = "";
            NewStatement = "";
            Theme = "";
        }
        private Quiz DefaultSelectedQuiz()
        {
            if (Quizzes.Count == 0)
            {
                return SelectedQuiz = new Quiz("", new List<Question>());
            }
            else
            {
                return SelectedQuiz = Quizzes.First();
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedQuiz) ||
                e.PropertyName == nameof(SelectedQuestion) ||
                e.PropertyName == nameof(Option1) ||
                e.PropertyName == nameof(Option2) ||
                e.PropertyName == nameof(Option3) ||
                e.PropertyName == nameof(CorrectAnswer) ||
                e.PropertyName == nameof(NewQuizTitle) ||
                e.PropertyName == nameof(NewStatement) ||
                e.PropertyName == nameof(Theme))
            {
                CreateQuizCommand.NotifyCanExecuteChanged();
                AddCommand.NotifyCanExecuteChanged();
                EditCommand.NotifyCanExecuteChanged();
                RemoveCommand.NotifyCanExecuteChanged();
            }

            QuizManager.SaveQuizzes(QuizManager._allQuizzes);
        }

    }
}
