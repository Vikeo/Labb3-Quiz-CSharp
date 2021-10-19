using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Labb3.Models;
using Labb3.ViewModels;

namespace Labb3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly Quiz _quiz;


        public App()
        {
            _quiz = new Quiz("Big quiz", new List<Question>());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Här ska man instansiera allt som man vill kunna visa. Flera olika view, om de ska leva över applikationens livstid.

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_quiz)
            };

            MainWindow.Show();

            
        }
    }
}
