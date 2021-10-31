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
        }

        private Quiz _chosenQuiz;
        public Quiz ChosenQuiz
        {
            get { return _chosenQuiz; }
        }

        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set { _currentQuestion = value; }
        }

        //TODO Fixa denna property. Kanske inte behöver eftersom CurrentQuestion.Theme.ThemeName redan finns.
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

        private readonly int _questionsCount;
        public int QuestionsCount
        {
            get { return _questionsCount; }
        }

        private int _questionCounter;
        public int QuestionCounter
        {
            get { return _questionCounter; }
        }

        #endregion

        #region RelayCommands

        public RelayCommand CheckCommand { get; }
        public RelayCommand QuitCommand { get; }
        public RelayCommand Answer1 { get; }
        public RelayCommand Answer2 { get; }
        public RelayCommand Answer3 { get; }

        #endregion

        #region Actions/Functions

        private void QuitToStart()
        {
            //TODO Ändra QuizManager.
            _chosenQuiz.ResetQuestionsAsked();
            _navigationStore.CurrentViewModel = new StartMenuViewModel(_navigationStore, _quizManager, new ObservableCollection<Theme>(_selectedThemes), _fileManager);
        }

        //Kollar om svaret är rätt, om nästa fråga är == null så avslutas spelet.
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

            CurrentQuestion = _chosenQuiz.GetRandomQuestion();

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
            //TODO Tom här i PlayView, kanske inte ens behöver denna här
        }

        private void ShuffleCorrectAnswerIndex()
        {
            //TODO Vill ha något som kan shuffla svaren, är tråkigt om de är på samma plats varje gång.
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
            //TODO Behöver jag TVÅ selectedQuiz och themes??
            _selectedQuiz = selectedQuiz;
            _chosenQuiz = selectedQuiz;
            _selectedThemes = selectedThemes;
            _themes = selectedThemes;
            _navigationStore = navigationStore;
            _quizManager = quizManager;
            _fileManager = fileManager;

            _questionCounter = 0;
            _questionsCount = _chosenQuiz.Questions.Count();

            _currentQuestion = _chosenQuiz.GetRandomQuestion();

            _questionCounter++;

            QuitCommand = new RelayCommand(QuitToStart);
            Answer1 = new RelayCommand(() => AnswerQuestion(1) );
            Answer2 = new RelayCommand(() => AnswerQuestion(2));
            Answer3 = new RelayCommand(() => AnswerQuestion(3));

            PropertyChanged += OnViewModelPropertyChanged;
        }
    }
}