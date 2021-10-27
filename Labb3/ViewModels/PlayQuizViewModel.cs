using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            get
            {
                return _chosenQuiz = _selectedQuiz;
            }
            set { SetProperty(ref _chosenQuiz, value); }
        }

        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set { SetProperty(ref _questions, value); }
        }

        private readonly Queue<Question> _questionsQueue;
        public Queue<Question> QuestionsQueue
        {
            get { return _questionsQueue; }
        }

        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set { SetProperty(ref _currentQuestion, value); }
        }

        private List<Question> _presentedQuestions;
        public List<Question> PresentedQuestions
        {
            get { return _presentedQuestions; }
            set { SetProperty(ref _presentedQuestions, value); }
        }

        private string _selectedOption;
        public string SelectedOption
        {
            get { return _selectedOption; }
            set { SetProperty(ref _selectedOption, value); }
        }

        private string _currentQuestionTheme;
        public string CurrentQuestionTheme
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
            GetNextQuestionInQueue();
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
            _navigationStore.CurrentViewModel = new StartMenuViewModel(_navigationStore, new QuizManager(), new ObservableCollection<Theme>(_selectedThemes));
        }

        private void AnswerQuestion(int Option)
        {
            if (CurrentQuestion.CorrectAnswer == Option)
            {
                Score++;
            }

            GetNextQuestionInQueue();

        }

        #endregion

        private void GetNextQuestionInQueue()
        {
            if (QuestionsQueue.Count > 0)
            {
                CurrentQuestion = QuestionsQueue.Dequeue();
            }
            else
            {
                //TODO Pop-up. Du fick såhär många poäng, gå till menyn.
                MessageBox.Show($"Score {Score}", "Score", MessageBoxButton.OK);
                QuitToStart();
            }
        }

        //TODO Har lagt in denna i Quiz-klassen, men kanske kommer behöva den här istället.
        //private Queue<Question> GenerateRandomQuestionQueue(Quiz quiz)
        //{
        //    Queue<Question> tempQueue = new Queue<Question>();
        //    List<Question> questionsList = _selectedQuiz.Questions.ToList();

        //    while (questionsList.Count > 0)
        //    {
        //        Random r = new Random();

        //        var tempRandom = r.Next(0, questionsList.Count);

        //        tempQueue.Enqueue(questionsList[tempRandom]);
        //        questionsList.RemoveAt(tempRandom);

        //    }
        //    return tempQueue;
        //}

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CheckCommand.NotifyCanExecuteChanged();
            //OnPropertyChanged(nameof(Score));
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

        public PlayQuizViewModel(NavigationStore navigationStore, Quiz selectedQuiz, List<Theme> selectedThemes)
        {
            _selectedQuiz = selectedQuiz;
            _selectedThemes = selectedThemes;
            _navigationStore = navigationStore;

            _questionsQueue = _selectedQuiz.GenerateRandomQuestionQueue(_selectedQuiz);
            _currentQuestion = QuestionsQueue.Dequeue();
            //ShuffleCorrectAnswerIndex();

            CheckCommand = new RelayCommand(CheckAnswer, CanCheckAnswer);
            QuitCommand = new RelayCommand(QuitToStart);
            Answer1 = new RelayCommand(() => AnswerQuestion(1) );
            Answer2 = new RelayCommand(() => AnswerQuestion(2));
            Answer3 = new RelayCommand(() => AnswerQuestion(3));

            PropertyChanged += OnViewModelPropertyChanged;
        }
    }
}