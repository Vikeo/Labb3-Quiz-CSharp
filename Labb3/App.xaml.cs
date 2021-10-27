﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Labb3.Managers;
using Labb3.Models;
using Labb3.Stores;
using Labb3.ViewModels;
using Labb3.Views;

namespace Labb3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        private readonly QuizManager _quizManager;
        private ObservableCollection<Theme> _themes;


        public App()
        {
            _navigationStore = new NavigationStore();
            _themes = new ObservableCollection<Theme>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            QuizManager._allQuizzes = QuizManager.LoadQuizzes();

            base.OnStartup(e);

            //Där som man ska starta.
            _navigationStore.CurrentViewModel = new StartMenuViewModel(_navigationStore, _quizManager, _themes);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore, _quizManager)
            };

            MainWindow.Show();

        }
    }
}
