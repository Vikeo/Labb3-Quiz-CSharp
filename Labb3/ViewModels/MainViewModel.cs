using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.Managers;
using Labb3.Models;
using Labb3.Stores;
using Microsoft.Toolkit.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3.ViewModels
{
    class MainViewModel : ObservableObject
    {
        private readonly QuizManager _quizManager;
        private readonly NavigationStore _navigationManager;

        //HÄR ÄNDRAR MAN DATACONTEXT
        //public ObservableObject CurrentViewModel => new QuizEditorViewModel(); //Så som det funkar.

        public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel; //Så som det ska vara om det skall funka.

        public MainViewModel(NavigationStore navigationManager, QuizManager quizManager)
        {
            _navigationManager = navigationManager;
            _quizManager = quizManager;

            _navigationManager.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
