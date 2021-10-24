using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb3.Managers;
using Labb3.Models;
using Microsoft.Toolkit.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Labb3.ViewModels
{
    class MainViewModel : ObservableObject
    {

        public ObservableObject CurrentViewModel { get; }

        public MainViewModel()
        {
            CurrentViewModel = new QuizEditorViewModel();
        }
    }
}
