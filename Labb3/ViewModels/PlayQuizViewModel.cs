using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private Queue<Question> _questionsQueue;
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
            set { _score = value; }
        }


        #endregion

        #region RelayCommands

        public RelayCommand CheckCommand { get; } 
        public RelayCommand QuitCommand { get; }

        private void CheckAnswer()
        {
            if (CurrentQuestion.Options[CurrentQuestion.CorrectAnswer] == SelectedOption)
            {
                SelectedOption = null;
                Score++;
                OnPropertyChanged(nameof(Score));
                //TODO Villkor som kollar om kön är tom inna Dequeue.
                CurrentQuestion = QuestionsQueue.Dequeue();
            }
            else
            {
                SelectedOption = null;
                OnPropertyChanged(nameof(Score));
                CurrentQuestion = QuestionsQueue.Dequeue();
            }
            
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
            _navigationStore.CurrentViewModel = new StartMenuViewModel(_navigationStore, new QuizManager(), new ObservableCollection<Theme>(_themes));
        }

        #endregion

        private Queue<Question> GenerateRandomQuestionQueue(Quiz quiz)
        {
            Queue<Question> tempQueue = new Queue<Question>();
            List<Question> questionsList = _selectedQuiz.Questions.ToList();

            while (questionsList.Count > 0)
            {
                Random r = new Random();

                var tempRandom = r.Next(0, questionsList.Count);

                tempQueue.Enqueue(questionsList[tempRandom]);
                questionsList.RemoveAt(tempRandom);

            }
            return tempQueue;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CheckCommand.NotifyCanExecuteChanged();
            //OnPropertyChanged(nameof(Score));

        }

        public PlayQuizViewModel(NavigationStore navigationStore, Quiz selectedQuiz, List<Theme> selectedThemes)
        {
            _selectedQuiz = selectedQuiz;
            _selectedThemes = selectedThemes;
            _navigationStore = navigationStore;

            _questionsQueue = GenerateRandomQuestionQueue(_selectedQuiz);
            _currentQuestion = QuestionsQueue.Dequeue();

            CheckCommand = new RelayCommand(CheckAnswer, CanCheckAnswer);
            QuitCommand = new RelayCommand(QuitToStart);

            PropertyChanged += OnViewModelPropertyChanged;
        }
    }
}