using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.ViewModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb3.States
{
    public class NavigationState : ObservableObject
    {
        public static ObservableObject _currentViewModel;
        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                //MainViewModel.CurrentViewModel = value;
            }
        }

        public readonly QuizEditorViewModel QuizEditorViewModel;
        //public readonly PlayQuizViewModel PlayQuizViewModel;

        public RelayCommand NavigateCommand { get; }

        public NavigationState()
        {
            NavigateCommand = new RelayCommand(GoToEditCommand);


        }

        public void GoToEditCommand()
        {
            CurrentViewModel = new QuizEditorViewModel();
        }
    }
}
