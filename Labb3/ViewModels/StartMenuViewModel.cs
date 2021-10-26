using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Labb3.Managers;
using Labb3.Models;
using Labb3.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb3.ViewModels
{
    class StartMenuViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly QuizManager _quizManager;

        #region Properties

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

                //TODO Det måste finnas ett bättre sätt att updatera vyn på.....
                if (_themes != null && _selectedQuiz != null)
                {
                    _themes.Clear();

                    foreach (var theme in QuizManager.GetUniqueThemes(SelectedQuiz))
                    {
                        _themes.Add(theme);
                    }
                }
            }
        }

        private ObservableCollection<string> _themes = new ObservableCollection<string>();
        public ObservableCollection<string> Themes
        {
            get { return _themes; }
            set
            {
                SetProperty(ref _themes, value);
            }
        }

        private string _selectedThemes;
        public string SelectedThemes
        {
            get { return _selectedThemes; }
            set
            {
                SetProperty(ref _selectedThemes, value);
                _selectedThemes = value;
            }
        }
        #endregion

        #region RelayCommands
        public RelayCommand GoToQuizEditorCommand { get; }
        public RelayCommand GoToPlayQuizCommand { get; }
        public RelayCommand GetThemesCommand { get; }
        public RelayCommand QuitApplicationCommand { get; }

        private void GoToQuizEditor()
        {
            _navigationStore.CurrentViewModel = new QuizEditorViewModel(_navigationStore, _quizManager);
        }

        private void GoToPlayQuiz()
        {
            _navigationStore.CurrentViewModel = new PlayQuizViewModel(_navigationStore);
        }
        private bool CanGoToPlayQuiz()
        {
            return true;
            //throw new NotImplementedException();
        }

        private void GetThemes()
        {
            //throw new NotImplementedException();
        }

        private void QuitApplication()
        {
            Application.Current.Shutdown();
        }

        #endregion
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedQuiz))
            {
                GoToPlayQuizCommand.NotifyCanExecuteChanged();
            }

            QuizManager.SaveQuizzes(QuizManager._allQuizzes);
        }

        public StartMenuViewModel(NavigationStore navigationStore, QuizManager quizManager)
        {
            _navigationStore = navigationStore;
            _quizManager = quizManager;

            GoToQuizEditorCommand = new RelayCommand(GoToQuizEditor);
            GoToPlayQuizCommand = new RelayCommand(GoToPlayQuiz, CanGoToPlayQuiz);
            GetThemesCommand = new RelayCommand(GetThemes);
            QuitApplicationCommand = new RelayCommand(QuitApplication);

            PropertyChanged += OnViewModelPropertyChanged;
        }
    }
}
