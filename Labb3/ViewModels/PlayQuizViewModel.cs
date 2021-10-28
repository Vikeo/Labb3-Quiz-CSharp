using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Labb3.Managers;
using Labb3.Models;
using Labb3.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb3.ViewModels
{
    class PlayQuizViewModel : ObservableObject
    {
        private Quiz _selectedQuiz;
        private List<Theme> _selectedThemes;
        private NavigationStore _navigationStore;
        private FileManager _fileManager;
        private QuizManager _quizManager;

        #region Properties

        private List<Theme> _themes;
        public List<Theme> Themes
        {
            get { return _themes; }
            set { _themes = value; }
        }

        private Quiz _chosenQuiz;
        public Quiz ChosenQuiz
        {
            get { return _chosenQuiz; }
            set { SetProperty(ref _chosenQuiz, value); }
        }

        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set { _currentQuestion = value; }
        }

        private string _selectedOption;
        public string SelectedOption
        {
            get { return _selectedOption; }
            set { SetProperty(ref _selectedOption, value); }
        }

        private Theme _currentQuestionTheme;
        public Theme CurrentQuestionTheme
        {
            get { return _currentQuestionTheme = CurrentQuestion.Theme; }
            set { SetProperty(ref _currentQuestionTheme, value); }
        }

        private int _score;
        public int Score
        {
            get { return _score; }
            set { SetProperty(ref _score, value); }
        }

        private int _questionsCount;
        public int QuestionsCount
        {
            get { return _questionsCount; }
        }

        private int _questionCounter;
        public int QuestionCounter
        {
            get { return _questionCounter; }
            set { _questionCounter = value; }
        }

        #endregion

        #region RelayCommands

        public RelayCommand CheckCommand { get; }
        public RelayCommand QuitCommand { get; }
        public RelayCommand Answer1 { get; }
        public RelayCommand Answer2 { get; }
        public RelayCommand Answer3 { get; }

        private void CheckAnswer()
        {
            if (CurrentQuestion.Options[CurrentQuestion.CorrectAnswer] == SelectedOption)
            {
                Score++;
            }

            SelectedOption = null;
            OnPropertyChanged(nameof(Score));
        }
        private bool CanCheckAnswer()
        {
            if (!string.IsNullOrEmpty(SelectedOption))
            {
                return true;
            }
            return false;
        }

        private void QuitToStart()
        {
            //TODO Ändra QuizManager.
            _navigationStore.CurrentViewModel = new StartMenuViewModel(_navigationStore, _quizManager, new ObservableCollection<Theme>(_selectedThemes), _fileManager);
        }

        private void AnswerQuestion(int Option)
        {
            if (CurrentQuestion.CorrectAnswer == Option)
            {
                Score++;
            }

            if (_questionCounter < _questionsCount)
            {
                _questionCounter++;
            }
            OnPropertyChanged(nameof(QuestionCounter));

            CurrentQuestion = _chosenQuiz.GetRandomQuestion(_chosenQuiz);
            _chosenQuiz.RemoveQuestion(CurrentQuestion);

            if (CurrentQuestion == null)
            {
                MessageBox.Show($"Score {Score}", "Score", MessageBoxButton.OK);
                QuitToStart();
            }
            OnPropertyChanged(nameof(CurrentQuestion));
        }

        #endregion

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CheckCommand.NotifyCanExecuteChanged();
        }

        private void ShuffleCorrectAnswerIndex()
        {
            //Vill ha något som kan shuffla svaren, är tråkigt om de är på samma plats varje gång.
            string correctAnswerString = CurrentQuestion.Options[CurrentQuestion.CorrectAnswer];
            Random r = new Random();

            string[] tempOptions = CurrentQuestion.Options;

            int r1 = r.Next(1, 4);
            CurrentQuestion.Options[3] = tempOptions[r1];

            int r2 = r.Next(1, 4);
            while (r2 == r1)
            {
                r2 = r.Next(1, 4);
            }
            CurrentQuestion.Options[1] = tempOptions[r2];

            int r3 = r.Next(1, 4);
            while (r3 == r1 || r3 == r2)
            {
                r3 = r.Next(1, 4);
            }
            CurrentQuestion.Options[2] = tempOptions[r3];

            List<string> tempStringList = CurrentQuestion.Options.ToList();

        }

        public PlayQuizViewModel(NavigationStore navigationStore, QuizManager quizManager,  Quiz selectedQuiz, List<Theme> selectedThemes, FileManager fileManager)
        {
            _selectedQuiz = selectedQuiz;
            _selectedThemes = selectedThemes;
            _navigationStore = navigationStore;
            _quizManager = quizManager;
            _fileManager = fileManager;

            //TODO igen, skapar en kopia av ett objekt på ett konstigt sätt.
            _chosenQuiz = new Quiz(_selectedQuiz.Title, new List<Question>());
            foreach (var question in _selectedQuiz.Questions)
            {
                _chosenQuiz.Questions.Add(question);
            }

            _questionCounter = 0;
            _questionsCount = _selectedQuiz.Questions.Count();
            _themes = _selectedThemes;

            _currentQuestion = _chosenQuiz.GetRandomQuestion(_chosenQuiz);
            _chosenQuiz.RemoveQuestion(CurrentQuestion);
            _questionCounter++;

            CheckCommand = new RelayCommand(CheckAnswer, CanCheckAnswer);
            QuitCommand = new RelayCommand(QuitToStart);
            Answer1 = new RelayCommand(() => AnswerQuestion(1) );
            Answer2 = new RelayCommand(() => AnswerQuestion(2));
            Answer3 = new RelayCommand(() => AnswerQuestion(3));

            PropertyChanged += OnViewModelPropertyChanged;
        }
    }
}