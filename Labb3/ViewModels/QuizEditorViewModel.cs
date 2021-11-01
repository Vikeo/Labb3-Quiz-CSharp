﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Labb3.Managers;
using Labb3.Models;
using Labb3.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;

namespace Labb3.ViewModels
{
    public class QuizEditorViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly QuizManager _quizManager;
        private ObservableCollection<Theme> _themes;
        private readonly FileManager _fileManager;

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
            }
        }

        private Quiz _selectedQuiz = new Quiz("TEMP", new List<Question>());
        public Quiz SelectedQuiz
        {
            get { return _selectedQuiz; }
            set
            {
                SetProperty(ref _selectedQuiz, value);

                if (_selectedQuiz == null)
                {
                    SelectedQuiz = DefaultSelectedQuiz();
                }

                //TODO SÅHÄR UPPDATERAT MAN TYDLIGEN VYER
                Questions = new ObservableCollection<Question>(_selectedQuiz.Questions);
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
                    NewStatement  = SelectedQuestion.Statement;
                    Options[1]    = SelectedQuestion.Options[1];
                    Options[2]    = SelectedQuestion.Options[2];
                    Options[3]    = SelectedQuestion.Options[3];
                    Option1 = Options[1];
                    Option2 = Options[2];
                    Option3 = Options[3];
                    ImagePath = SelectedQuestion.ImagePath;

                    ThemeName = SelectedQuestion.Theme.ThemeName.ToString();
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

        //TODO Implement
        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set { SetProperty(ref _imagePath, value); }
        }

        private Theme _theme = new Theme("TEMP", false);
        public Theme Theme
        {
            get { return _theme; }
            set
            {
                SetProperty(ref _theme, value);
            }
        }

        private string _themeName;
        public string ThemeName
        {
            get { return _themeName; }
            set
            {
                SetProperty(ref _themeName, value);
            }
        }

        #endregion

        #region RelayCommand
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand RemoveQuizCommand { get; }
        public RelayCommand ReturnCommand { get; }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand CreateQuizCommand { get; }
        public RelayCommand EditQuizTitleCommand { get; }

        public void AddQuiz()
        {
            _quizManager.CreateNewQuiz(NewQuizTitle, new List<Question>());

            Option1 = "";
            Option2 = "";
            Option3 = "";
            CorrectAnswer = 0;
            NewStatement = "";
            NewQuizTitle = "";

            //TODO Gör så att Propertyn uppdateras?
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
            Theme newTheme = new Theme(ThemeName.ToString(), false);
            SelectedQuiz.AddQuestion(NewStatement, newTheme, CorrectAnswer, false, ImagePath, Options.ToArray());

            //Gör så att Propertyn uppdateras

            var tempQuiz = SelectedQuiz;
            SelectedQuiz = tempQuiz;
            SelectedQuestion = SelectedQuiz.Questions.ToList()[^1];

            _fileManager.SaveAllQuizzes(_quizManager._allQuizzes);
        }

        //Kollar om man kan lägga till frågor till quizen.
        public bool CanAddQuestionToQuiz()
        {
            if (SelectedQuiz == null)
            {
                return false;
            }
            else if (!string.IsNullOrEmpty(Option1)         &&
                     !string.IsNullOrEmpty(Option2)         &&
                     !string.IsNullOrEmpty(Option3)         &&
                     CorrectAnswer != 0                     &&
                     !string.IsNullOrEmpty(NewStatement)    &&
                     !string.IsNullOrEmpty(Theme.ThemeName) &&
                     SelectedQuiz.Title != "TEMP"           &&
                     Option1 != Option2                     &&
                     Option1 != Option3                     &&
                     Option2 != Option1                     &&
                     Option2 != Option3                     &&
                     Option3 != Option1                     &&
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

        public void EditQuizTitle()
        {
            SelectedQuiz.Title = NewQuizTitle;

            //Quizzes = new ObservableCollection<Quiz>();

            //TODO Combobox uppdateras inte direkt om jag inte gör såhär:

            int tempIndex = Quizzes.IndexOf(SelectedQuiz);
            if (Quizzes != null)
            {
                List<Quiz> tempQuizzes = Quizzes.ToList();

                Quizzes.Clear();
                foreach (var quiz in tempQuizzes)
                {
                    Quizzes.Add(quiz);
                }
            }
            SelectedQuiz = Quizzes[tempIndex];
        }

        public bool CanEditQuizTitle()
        {
            //TODO NÄSTAN Exakt samma kod som CanAddQuiz
            if (_quizManager._allQuizzes.All(q => q.Title != NewQuizTitle) && _quizManager._allQuizzes.Count > 0)
            {
                return !string.IsNullOrEmpty(NewQuizTitle);
            }
            return false;
        }

        //TODO Kan inte edita om jag ändrar på en Option
        public void EditQuestion()
        {
            SelectedQuestion.Statement       = NewStatement;
            SelectedQuestion.Options[1]      = Options[1];
            SelectedQuestion.Options[2]      = Options[2];
            SelectedQuestion.Options[3]      = Options[3];
            Question.ChangeCorrectAnswer(SelectedQuestion, CorrectAnswer);
            SelectedQuestion.Theme.ThemeName = ThemeName;

            //Gör så att listan i vyn uppdateras.
            Questions = new ObservableCollection<Question>(SelectedQuiz.Questions);

            EditCommand.NotifyCanExecuteChanged();
        }
        public bool CanEditQuestion()
        {
            bool tempBool = false;
            //TODO Bättre sätt att göra detta på??????
            if (SelectedQuestion != null)
            {
                if (NewStatement != SelectedQuestion.Statement          ||
                    SelectedQuestion.Theme.ThemeName != ThemeName       ||
                    SelectedQuestion.Options[1] != Options[1]           ||
                    SelectedQuestion.Options[2] != Options[2]           ||
                    SelectedQuestion.Options[3] != Options[3]           ||
                    SelectedQuestion.CorrectAnswer != CorrectAnswer) 
                {
                    if (CorrectAnswer > 0                      &&
                        !string.IsNullOrEmpty(Option1)         &&
                        !string.IsNullOrEmpty(Option2)         &&
                        !string.IsNullOrEmpty(Option3)         &&
                        !string.IsNullOrEmpty(NewStatement)    &&
                        !string.IsNullOrEmpty(Theme.ThemeName) &&
                        Option1 != Option2                     &&
                        Option1 != Option3                     &&
                        Option2 != Option1                     &&
                        Option2 != Option3                     &&
                        Option3 != Option1                     &&
                        Option3 != Option2)
                    {
                        //Har denna här så att jag kan sätta det jag vill returnera till True här, men sen kolla ett till vilkor.
                        tempBool = true;

                        if (NewStatement != SelectedQuestion.Statement && SelectedQuiz.Questions.Any(q => q.Statement == NewStatement))
                        {
                            tempBool = false;
                        }
                        return tempBool;
                    }
                }
            }
            return false;
        }
        public void RemoveQuiz()
        {
            //TODO Popup-Är du säker?
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
                Questions = new ObservableCollection<Question>(SelectedQuiz.Questions);
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

        //TODO Kanske ska göra så att bilderna lagras i en mapp i applikationen. 
        //TODO Kanske ha som async?
        public void AddImageToQuestion()
        {
            OpenFileDialog openFileImageDialog = new OpenFileDialog();

            openFileImageDialog.Title = "Choose image";
            openFileImageDialog.Filter = "ImagePath Files|*.jpg;*.jpeg;*.png;|All files (*.*)|*.*";
            openFileImageDialog.FilterIndex = 1;

            if (openFileImageDialog.ShowDialog() == true)
            {
                //TODO ändra så att det är path istället för en fil.
                BitmapImage bitImage = new BitmapImage(new Uri(openFileImageDialog.FileName));
                ImagePath = openFileImageDialog.FileName;

                //Kopierar filen och byter namn på den så att den har samma namn som Quiz+Question.
                if (SelectedQuiz.Title != "TEMP" && SelectedQuestion != null)
                {
                    //TODO Det kan finnas flera bilder som hejter QuizTitle+Statement, men att de har olika filändelser.
                    File.Copy(openFileImageDialog.FileName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{SelectedQuiz.Title}{SelectedQuestion.Statement}.{openFileImageDialog.FileName.Split('.')[1]}"));
                    SelectedQuestion.ImagePath =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            $"{SelectedQuiz.Title}{SelectedQuestion.Statement}.{openFileImageDialog.FileName.Split('.')[1]}");
                }
            }
        }

        public bool CanAddImage()
        {
            if (SelectedQuestion != null)
            {
                return true;
            }
            return false;
        }
        public void ReturnToStartMenu()
        {
            _navigationStore.CurrentViewModel = new StartMenuViewModel(_navigationStore, _quizManager, _themes, _fileManager);
        }
        private void ClearTextboxes()
        {
            //Option1 = "";
            //Option2 = "";
            //Option3 = "";
            //CorrectAnswer = 0;
            //NewStatement = "";
            //ThemeName = null;
        }
        private Quiz DefaultSelectedQuiz()
        {
            if (Quizzes.Count == 0)
            {
                return SelectedQuiz = new Quiz("TEMP", new List<Question>());
            }
            else
            {
                return SelectedQuiz = Quizzes.First();
            }
        }
        #endregion

        private async Task SetImage()
        {
            await Task.Run(() => ImagePath = SelectedQuestion.ImagePath);
        }

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
                e.PropertyName == nameof(Theme.ThemeName)  ||
                e.PropertyName == nameof(ImagePath))
            {
                CreateQuizCommand.NotifyCanExecuteChanged();
                AddCommand.NotifyCanExecuteChanged();
                EditCommand.NotifyCanExecuteChanged();
                EditQuizTitleCommand.NotifyCanExecuteChanged();
                RemoveCommand.NotifyCanExecuteChanged();
                RemoveQuizCommand.NotifyCanExecuteChanged();
                AddImageCommand.NotifyCanExecuteChanged();
            }

            _fileManager.SaveAllQuizzes(_quizManager._allQuizzes);
        }

        //Konstruktor
        public QuizEditorViewModel(NavigationStore navigationStore, QuizManager quizManager, ObservableCollection<Theme> themes, FileManager fileManager)
        {
            _navigationStore = navigationStore;
            _quizManager = quizManager;
            _themes = themes;
            _fileManager = fileManager;

            //CreateQuizCommand = new CreateQuizCommand(this, quizzes, quiz);

            AddCommand        = new RelayCommand(AddQuestionToQuiz, CanAddQuestionToQuiz);
            CreateQuizCommand = new RelayCommand(AddQuiz, CanAddQuiz);
            EditCommand       = new RelayCommand(EditQuestion, CanEditQuestion);
            RemoveCommand     = new RelayCommand(RemoveQuestion, CanRemoveQuestion);
            AddImageCommand   = new RelayCommand(() => AddImageToQuestion(), CanAddImage);
            RemoveQuizCommand = new RelayCommand(RemoveQuiz, CanRemoveQuiz);
            ReturnCommand     = new RelayCommand(ReturnToStartMenu);
            EditQuizTitleCommand = new RelayCommand(EditQuizTitle, CanEditQuizTitle);

            PropertyChanged += OnViewModelPropertyChanged;
        }
    }
}
