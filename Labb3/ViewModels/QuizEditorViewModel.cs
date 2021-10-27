using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
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
        private ObservableCollection<Theme> _themes;

        #region Properties

        private string _newStatement;
        public string NewStatement
        {
            get { return _newStatement; }
            set
            {
                SetProperty(ref _newStatement, value);
            }
        }

        //Kanske gör ett nytt objekt, "QuizOptions", mata in allt som behövs visas i den.
        private ObservableCollection<Quiz> _quizzes;
        public ObservableCollection<Quiz> Quizzes
        {
            get { return _quizzes = _quizManager._allQuizzes; }
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
                OnPropertyChanged(nameof(Questions));
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

                    NewStatement  = SelectedQuestion.Statement;
                    Options[1]    = SelectedQuestion.Options[1];
                    Options[2]    = SelectedQuestion.Options[2];
                    Options[3]    = SelectedQuestion.Options[3];
                    Option1       = Options[1];
                    Option2       = Options[2];
                    Option3       = Options[3];
                    Theme         = SelectedQuestion.Theme;
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
        private int _correctAnswer;
        public int CorrectAnswer
        {
            get { return _correctAnswer; }
            set
            {
                SetProperty(ref _correctAnswer, value);
                EditCommand.NotifyCanExecuteChanged();
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
                EditCommand.NotifyCanExecuteChanged();
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
                EditCommand.NotifyCanExecuteChanged();
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
                EditCommand.NotifyCanExecuteChanged();
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
        #endregion

        #region RelayCommand
        //TODO Lägg till ett Command för att ändra namnet på Quiz. Ha det på samma Button som CreateQuizCommand om det går?
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand RemoveQuizCommand { get; }
        public RelayCommand ReturnCommand { get; }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand CreateQuizCommand { get; }

        public void AddQuiz()
        {
            _quizManager.CreateNewQuiz(NewQuizTitle, new List<Question>());

            //OnPropertyChanged(nameof(SelectedQuiz));

            Option1 = "";
            Option2 = "";
            Option3 = "";
            CorrectAnswer = 0;
            NewStatement = "";

            NewQuizTitle = "";

            //Gör så att Propertyn uppdateras
            SelectedQuiz = _quizManager._allQuizzes[^1];

        }
        public bool CanAddQuiz()
        {
            //Om man vill lägga till en ny quiz.
            //Kollar först det finns en quiz med samma Titel.
            if (_quizManager._allQuizzes.All(q => q.Title != NewQuizTitle))
            {
                return !string.IsNullOrEmpty(NewQuizTitle);
            }
            return false;
        }

        //Det som sker när man trycker på AddCommand
        public void AddQuestionToQuiz()
        {

            SelectedQuiz.AddQuestion(NewStatement, Theme, CorrectAnswer, Options.ToArray());

            //Gör så att Propertyn uppdateras

            //OnPropertyChanged(nameof(SelectedQuiz.Questions)); ????

            var tempQuiz = SelectedQuiz;
            SelectedQuiz = tempQuiz;

            _quizManager.SaveQuizzes(_quizManager._allQuizzes);
        }

        //Kollar om man kan lägga till frågor till quizen.
        public bool CanAddQuestionToQuiz()
        {
            if (SelectedQuiz == null)
            {
                return false;
            }
            else if (!string.IsNullOrEmpty(Option1)      &&
                     !string.IsNullOrEmpty(Option2)      &&
                     !string.IsNullOrEmpty(Option3)      &&
                     CorrectAnswer != 0                  &&
                     !string.IsNullOrEmpty(NewStatement) &&
                     !string.IsNullOrEmpty(Theme)        &&
                     SelectedQuiz.Title != "TEMP"        &&
                     Option1 != Option2                  &&
                     Option1 != Option3                  &&
                     Option2 != Option1                  &&
                     Option2 != Option3                  &&
                     Option3 != Option1                  &&
                     Option3 != Option2)
            {
                //TODO Finns kanske ett bättre LINQ-uttryck för detta.
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

        public void EditQuestion()
        {
            SelectedQuestion.Statement = NewStatement;
            SelectedQuestion.Options[1]   = Options[1];
            SelectedQuestion.Options[2]   = Options[2];
            SelectedQuestion.Options[3]   = Options[3];
            Question.ChangeCorrectAnswer(SelectedQuestion, CorrectAnswer);
            SelectedQuestion.Theme     = Theme;


            //TODO Gör så att listan av frågor uppdateras i vyn.........

            //TODO Implementera denna lite här och där. Man kan göra 
            //OnPropertyChanged(nameof(Questions));

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
            //TODO Bättre sätt att göra detta på??????
            if (SelectedQuestion != null)
            {
                if (SelectedQuestion.Statement != NewStatement ||
                    SelectedQuestion.Theme != Theme            ||
                    SelectedQuestion.Options[1] != Options[1]  ||
                    SelectedQuestion.Options[2] != Options[2]  ||
                    SelectedQuestion.Options[3] != Options[3]  ||
                    SelectedQuestion.CorrectAnswer != CorrectAnswer) 
                {
                    if (CorrectAnswer > 0                   &&
                        !string.IsNullOrEmpty(Option1)      &&
                        !string.IsNullOrEmpty(Option2)      &&
                        !string.IsNullOrEmpty(Option3)      &&
                        !string.IsNullOrEmpty(NewStatement) &&
                        !string.IsNullOrEmpty(Theme)        &&
                        Option1 != Option2                  &&
                        Option1 != Option3                  &&
                        Option2 != Option1                  &&
                        Option2 != Option3                  &&
                        Option3 != Option1                  &&
                        Option3 != Option2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void RemoveQuiz()
        {
            _quizManager.RemoveQuiz(SelectedQuiz);

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

        public void RemoveQuestion()
        {
            SelectedQuiz.RemoveQuestion(SelectedQuestion);
            ClearTextboxes();

            //TODO Gör så att listan av frågor uppdateras i vyn.........
            if (Questions != null)
            {
                Questions.Clear();
                foreach (var question in SelectedQuiz.Questions)
                {
                    Questions.Add(question);
                }
            }
        }
        public bool CanRemoveQuestion()
        {
            if (SelectedQuestion != null)
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
        public void ReturnToStartMenu()
        {
            _navigationStore.CurrentViewModel = new StartMenuViewModel(_navigationStore, _quizManager, _themes);
        }
        private void ClearTextboxes()
        {
            Option1 = "";
            Option2 = "";
            Option3 = "";
            CorrectAnswer = 0;
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
        #endregion
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedQuiz)     ||
                e.PropertyName == nameof(SelectedQuestion) ||
                e.PropertyName == nameof(Option1)          ||
                e.PropertyName == nameof(Option2)          ||
                e.PropertyName == nameof(Option3)          ||
                e.PropertyName == nameof(CorrectAnswer)    ||
                e.PropertyName == nameof(NewQuizTitle)     ||
                e.PropertyName == nameof(NewStatement)     ||
                e.PropertyName == nameof(Theme))
            {
                CreateQuizCommand.NotifyCanExecuteChanged();
                AddCommand.NotifyCanExecuteChanged();
                EditCommand.NotifyCanExecuteChanged();
                RemoveCommand.NotifyCanExecuteChanged();
            }

            _quizManager.SaveQuizzes(_quizManager._allQuizzes);
        }

        //Konstruktor
        public QuizEditorViewModel(NavigationStore navigationStore, QuizManager quizManager, ObservableCollection<Theme> themes)
        {
            _navigationStore = navigationStore;
            _quizManager = quizManager;
            _themes = themes;


            //CreateQuizCommand = new CreateQuizCommand(this, quizzes, quiz);

            AddCommand        = new RelayCommand(AddQuestionToQuiz, CanAddQuestionToQuiz);
            CreateQuizCommand = new RelayCommand(AddQuiz, CanAddQuiz);
            EditCommand       = new RelayCommand(EditQuestion, CanEditQuestion);
            RemoveCommand     = new RelayCommand(RemoveQuestion, CanRemoveQuestion);
            AddImageCommand   = new RelayCommand(AddImageToQuestion, CanAddImage);
            RemoveQuizCommand = new RelayCommand(RemoveQuiz, CanRemoveQuiz);
            ReturnCommand     = new RelayCommand(ReturnToStartMenu);

            PropertyChanged += OnViewModelPropertyChanged;
        }
    }
}
