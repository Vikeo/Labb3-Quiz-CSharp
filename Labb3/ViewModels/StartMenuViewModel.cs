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
using Labb3.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb3.ViewModels
{
    class StartMenuViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly QuizManager _quizManager;
        private ObservableCollection<Theme> _themes;
        private readonly FileManager _fileManager;

        #region Properties

        private ObservableCollection<Quiz> _quizzes;
        public ObservableCollection<Quiz> Quizzes
        {
            get { return _quizzes = _quizManager.AllQuizzes; }
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

                    foreach (var theme in _quizManager.GetUniqueThemes(SelectedQuiz))
                    {
                        Theme tempTheme = new Theme(theme, false);
                        _themes.Add(tempTheme);
                    }
                }
            }
        }

        private ObservableCollection<Theme> _listThemes;
        public ObservableCollection<Theme> ListThemes
        {
            get { return _listThemes = _themes; }
            set => SetProperty(ref _listThemes, value);
        }

        private ObservableCollection<Theme> _selectedThemes = new ObservableCollection<Theme>();
        public ObservableCollection<Theme> SelectedThemes
        {
            get { return _selectedThemes; }
            set => SetProperty(ref _selectedThemes, value);
        }

        #endregion

        #region RelayCommands + Actions/Functions

        public RelayCommand GoToQuizEditorCommand { get; }
        public RelayCommand GoToPlayQuizCommand { get; }
        public RelayCommand QuitApplicationCommand { get; }
        public RelayCommand ExportQuizCommand { get; }
        public RelayCommand ImportQuizCommand { get; }
        public RelayCommand UpdateQuizzesCommand { get; }

        private void GoToQuizEditor()
        {
            _navigationStore.CurrentViewModel = new QuizEditorViewModel(_navigationStore, _quizManager, _themes, _fileManager);
        }

        private void GoToPlayQuiz()
        {
            //TODO När jag kommer tillbaka till StartMenu så finns de temana man valde kvar
            SetThemes();

            if (_selectedThemes.Count == 0)
            {
                MessageBox.Show($"Du har inte något tema valt, du måste välja minst ett.", "Score", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _navigationStore.CurrentViewModel = new PlayQuizViewModel(_navigationStore, _quizManager, _selectedQuiz, _selectedThemes.ToList(), _fileManager);
            }
        }
        private bool CanGoToPlayQuiz()
        {
            if (SelectedQuiz.Title != "TEMP")
            {
                return true;
            }
            return false;
        }

        private void SetThemes()
        {
            foreach (var theme in _themes)
            {
                if (theme.Selected)
                {
                    _selectedThemes.Add(theme);
                }
            }

            foreach (var question in SelectedQuiz.Questions)
            {
                foreach (var theme in _selectedThemes)
                {
                    if (question.Theme.ThemeName == theme.ThemeName)
                    {
                        question.Theme.Selected = true;
                    }
                }
            }
            GoToPlayQuizCommand.NotifyCanExecuteChanged();
        }

        private void QuitApplication()
        {
            _fileManager.SaveAllQuizzes(_quizManager.AllQuizzes);
            Application.Current.Shutdown();
        }

        private void ExportQuiz()
        {
            _fileManager.ExportQuiz(SelectedQuiz);
        }
        private bool CanExportQuiz()
        {
            if (SelectedQuiz.Title != "TEMP")
            {
                return true;
            }
            return false;
        }

        private async Task ImportQuiz()
        {
            _quizManager.AllQuizzes.Add(await _fileManager.ImportQuiz(_quizManager.AllQuizzes));
        }

        private void UpdateQuizzes()
        {
            Quizzes = _quizManager.AllQuizzes;
        }

        #endregion

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedQuiz))
            {
                GoToPlayQuizCommand.NotifyCanExecuteChanged();
                ExportQuizCommand.NotifyCanExecuteChanged();
            }
        }

        public StartMenuViewModel(NavigationStore navigationStore, QuizManager quizManager,
            ObservableCollection<Theme> themes, FileManager fileManager)
        {
            _navigationStore = navigationStore;
            _quizManager = quizManager;
            _themes = themes;
            _fileManager = fileManager;

            GoToQuizEditorCommand = new RelayCommand(GoToQuizEditor);
            GoToPlayQuizCommand = new RelayCommand(GoToPlayQuiz, CanGoToPlayQuiz);
            QuitApplicationCommand = new RelayCommand(QuitApplication);
            ExportQuizCommand = new RelayCommand(ExportQuiz, CanExportQuiz);
            ImportQuizCommand = new RelayCommand(() => ImportQuiz());
            UpdateQuizzesCommand = new RelayCommand(UpdateQuizzes);

            PropertyChanged += OnViewModelPropertyChanged;
        }
    }
}
