using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Labb3.Models;
using Labb3.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3.ViewModels
{
    class PlayQuizViewModel : ObservableObject
    {
        private Quiz _selectedQuiz;
        private List<string> _selectedThemes;
        private NavigationStore _navigationStore;

        public PlayQuizViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public PlayQuizViewModel(NavigationStore navigationStore, Quiz selectedQuiz, List<string> selectedThemes)
        {
            _selectedQuiz = selectedQuiz;
            _selectedThemes = selectedThemes;
            _navigationStore = navigationStore;
        }
    }
}