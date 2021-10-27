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
        private ObservableCollection<Theme> _themes;

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
                    _selectedThemes.Clear();
                    _themes.Clear();

                    foreach (var theme in QuizManager.GetUniqueThemes(SelectedQuiz))
                    {
                        Theme tempTheme = new Theme(theme, false);
                        _themes.Add(tempTheme);
                    }
                }
            }
        }

        //TODO Gör så att Themes har en bool som sätts till true när den har selectats en gång. Iterera genom themes för att hitta vilka som var selected.
        private ObservableCollection<Theme> _listThemes;
        public ObservableCollection<Theme> ListThemes
        {
            get { return _listThemes = _themes; }
            set
            {
                SetProperty(ref _listThemes, value);
            }
        }

        private ObservableCollection<Theme> _selectedThemes = new ObservableCollection<Theme>();
        public ObservableCollection<Theme> SelectedThemes
        {
            get
            { return _selectedThemes; }
            set
            {
                SetProperty(ref _selectedThemes, value);
            }
        }

        #endregion

        #region RelayCommands
        public RelayCommand GoToQuizEditorCommand { get; }
        public RelayCommand GoToPlayQuizCommand { get; }
        public RelayCommand SetThemesCommand { get; }
        public RelayCommand QuitApplicationCommand { get; }

        private void GoToQuizEditor()
        {
            _navigationStore.CurrentViewModel = new QuizEditorViewModel(_navigationStore, _quizManager);
        }

        private void GoToPlayQuiz()
        {
            _navigationStore.CurrentViewModel = new PlayQuizViewModel(_navigationStore, _selectedQuiz, _selectedThemes.ToList());
        }
        private bool CanGoToPlayQuiz()
        {
            if (SelectedQuiz.Title != "TEMP" && SelectedThemes.Count != 0)
            {
                return true;
            }
            return false;
        }

        private void SetThemes()
        {
            _selectedThemes.Clear();
            foreach (var theme in _themes)
            {
                if (theme.Selected)
                {
                    _selectedThemes.Add(theme);
                }
            }
            GoToPlayQuizCommand.NotifyCanExecuteChanged();
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

        public StartMenuViewModel(NavigationStore navigationStore, QuizManager quizManager,
            ObservableCollection<Theme> themes)
        {
            _navigationStore = navigationStore;
            _quizManager = quizManager;
            _themes = themes;

            GoToQuizEditorCommand = new RelayCommand(GoToQuizEditor);
            GoToPlayQuizCommand = new RelayCommand(GoToPlayQuiz, CanGoToPlayQuiz);
            SetThemesCommand = new RelayCommand(SetThemes);
            QuitApplicationCommand = new RelayCommand(QuitApplication);

            PropertyChanged += OnViewModelPropertyChanged;
        }

    }
}
