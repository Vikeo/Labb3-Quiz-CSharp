using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
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

        public List<Theme> Themes { get; set; }

        public Quiz ChosenQuiz { get; }

        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get => _currentQuestion;
            set { SetProperty(ref _currentQuestion, value); }
        }

        //TODO Ta bort denna property. Kanske inte behöver eftersom CurrentQuestion.Theme.ThemeName redan finns.
        private Theme _currentQuestionTheme;
        public Theme CurrentQuestionTheme
        {
            get { return _currentQuestionTheme = CurrentQuestion.Theme; }
            set { SetProperty(ref _currentQuestionTheme, value); }
        }

        private int _score;
        public int Score
        {
            get => _score;
            set { SetProperty(ref _score, value); }
        }

        public int QuestionsCount { get; }
        public int QuestionCounter { get; private set; }

        #endregion

        #region RelayCommands
        public RelayCommand QuitCommand { get; }
        public RelayCommand Answer1 { get; }
        public RelayCommand Answer2 { get; }
        public RelayCommand Answer3 { get; }

        #endregion

        #region Actions/Functions
        private void QuitToStart()
        {
            ChosenQuiz.ResetThemeSelected();
            ChosenQuiz.ResetQuestionsAsked();
            _navigationStore.CurrentViewModel = new StartMenuViewModel(_navigationStore, _quizManager, new ObservableCollection<Theme>(_selectedThemes), _fileManager);
        }

        //Kollar om svaret är rätt, om nästa fråga är == null så avslutas spelet.
        private void AnswerQuestion(int option)
        {
            if (CurrentQuestion.CorrectAnswer == option)
            {
                Score++;
            }
            if (QuestionCounter < QuestionsCount)
            {
                QuestionCounter++;
            }

            OnPropertyChanged(nameof(QuestionCounter));
            CurrentQuestion = ChosenQuiz.GetRandomQuestion();

            if (CurrentQuestion == null)
            {
                MessageBox.Show($"Score {Score}", "Score", MessageBoxButton.OK);
                QuitToStart();
            }
            OnPropertyChanged(nameof(CurrentQuestion));
        }
        private void ShuffleCorrectAnswerIndex()
        {
            //TODO Vill ha något som kan shuffla svaren, är tråkigt om de är på samma plats varje gång.
            string correctAnswerString = CurrentQuestion.Options[CurrentQuestion.CorrectAnswer];
            Random r = new Random();

            string[] tempOptions = CurrentQuestion.Options;

            int r1 = r.Next(1, 4);
            CurrentQuestion.Options[2] = tempOptions[r1];

            int r2 = r.Next(1, 4);
            while (r2 == r1)
            {
                r2 = r.Next(1, 4);
            }
            CurrentQuestion.Options[0] = tempOptions[r2];

            int r3 = r.Next(1, 4);
            while (r3 == r1 || r3 == r2)
            {
                r3 = r.Next(1, 4);
            }
            CurrentQuestion.Options[1] = tempOptions[r3];

            List<string> tempStringList = CurrentQuestion.Options.ToList();
        }
        #endregion

        public PlayQuizViewModel(NavigationStore navigationStore, QuizManager quizManager,  Quiz selectedQuiz, List<Theme> selectedThemes, FileManager fileManager)
        {
            _selectedQuiz = selectedQuiz;
            ChosenQuiz = selectedQuiz;
            _selectedThemes = selectedThemes;
            Themes = selectedThemes; 
            _navigationStore = navigationStore;
            _quizManager = quizManager;
            _fileManager = fileManager;

            QuestionCounter = 0;

            QuestionsCount = ChosenQuiz.Questions.Count();

            _currentQuestion = ChosenQuiz.GetRandomQuestion();

            QuestionCounter++;

            QuitCommand = new RelayCommand(QuitToStart);
            Answer1 = new RelayCommand(() => AnswerQuestion(0));
            Answer2 = new RelayCommand(() => AnswerQuestion(1));
            Answer3 = new RelayCommand(() => AnswerQuestion(2));
        }
    }
}