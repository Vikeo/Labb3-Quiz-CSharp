using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.Managers;
using Labb3.Models;
using Labb3.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb3.ViewModels
{
    class MainMenuViewModel : ObservableObject
    {

        private readonly NavigationStore _navigationStore;
        private readonly QuizManager _quizManager;
        
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
                SetProperty(ref _selectedQuiz, value);
                _selectedQuiz = value;
            }
        }

        private List<string> _themes = QuizManager.GetUniqueThemes();
        public List<string> Themes
        {
            get { return _themes; }
            set
            {
                SetProperty(ref _themes, value);
                _themes = value;
            }
        }

        private string _selectedTheme;
        public string SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                SetProperty(ref _selectedTheme, value);
                _selectedTheme = value;
            }
        }

        public RelayCommand GoToQuizEditorCommand { get; }
        public RelayCommand GoToPlayQuizCommand { get; }
        public RelayCommand QuitCommand { get; }

        public MainMenuViewModel(Stores.NavigationStore navigationStore)
        {
            GoToQuizEditorCommand = new RelayCommand(GoToQuizEditor);
            GoToPlayQuizCommand = new RelayCommand(GoToPlayQuiz, CanGoToPlayQuiz);
            QuitCommand = new RelayCommand(Quit);
            _navigationStore = navigationStore;
        }

        private void GoToPlayQuiz()
        {
            //throw new NotImplementedException();
        }
        private bool CanGoToPlayQuiz()
        {
            return true;
            //throw new NotImplementedException();
        }

        private void GoToQuizEditor()
        {
            _navigationStore.CurrentViewModel = new QuizEditorViewModel(_navigationStore, _quizManager);
        }
        private void Quit()
        {
            //throw new NotImplementedException();
        }
    }
}
