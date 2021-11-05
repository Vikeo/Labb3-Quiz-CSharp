using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Labb3.Managers;
using Labb3.Models;
using Labb3.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;

namespace Labb3.ViewModels
{
    public class QuizEditorViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly QuizManager _quizManager;
        private ObservableCollection<Theme> _themes;
        private readonly FileManager _fileManager;

        #region Properties

        private string _newStatement;
        public string NewStatement
        {
            get { return _newStatement; }
            set { SetProperty(ref _newStatement, value); }
        }

        //Kanske gör ett nytt objekt, "QuizOptions", mata in allt som behövs visas i den.
        private ObservableCollection<Quiz> _quizzes;
        public ObservableCollection<Quiz> Quizzes
        {
            get { return _quizzes = _quizManager.AllQuizzes; }
            set { SetProperty(ref _quizzes, value); }
        }

        private Quiz _selectedQuiz = new Quiz("TEMP", new List<Question>());
        public Quiz SelectedQuiz
        {
            get { return _selectedQuiz; }
            set
            {
                SetProperty(ref _selectedQuiz, value);

                if (_selectedQuiz == null)
                {
                    SelectedQuiz = DefaultSelectedQuiz();
                }
                ClearTextboxes();

                //Ett sätt att uppdatera vyernas information på
                Questions = new ObservableCollection<Question>(_selectedQuiz.Questions);
            }
        }

        private ObservableCollection<Question> _questions = new();
        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set
            {
                SetProperty(ref _questions, value);
                OnPropertyChanged(nameof(Questions));
            }
        }

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                SetProperty(ref _selectedQuestion, value);

                if (value != null)
                {
                    Image = null;
                    NewStatement = SelectedQuestion.Statement;
                    Options[0] = SelectedQuestion.Options[0];
                    Options[1] = SelectedQuestion.Options[1];
                    Options[2] = SelectedQuestion.Options[2];
                    Option1 = Options[0];
                    Option2 = Options[1];
                    Option3 = Options[2];
                    if (File.Exists(SelectedQuestion.ImagePath))
                    {
                        SetImageProperty(SelectedQuestion.ImagePath);
                        ChooseImageText = "Remove Image";
                    }
                    else
                    {
                        ChooseImageText = "Choose Image";
                    }
                    ThemeName = SelectedQuestion.Theme.ThemeName.ToString();
                    CorrectAnswer = SelectedQuestion.CorrectAnswer;
                }
            }
        }

        private string _newQuizTitle;

        public string NewQuizTitle
        {
            get { return _newQuizTitle; }
            set { SetProperty(ref _newQuizTitle, value); }
        }

        //CorrectAnswer comboboxen listar de tre alternativ som man har skrivit in.
        private int _correctAnswer;
        public int CorrectAnswer
        {
            get { return _correctAnswer; }
            set
            {
                SetProperty(ref _correctAnswer, value);
                EditCommand.NotifyCanExecuteChanged();
            }
        }

        private ObservableCollection<string> _options = new ObservableCollection<string>() { "", "", "" };
        public ObservableCollection<string> Options
        {
            get { return _options; }
            set { SetProperty(ref _options, value); }
        }

        private string _option1;
        public string Option1
        {
            get { return _option1; }
            set
            {
                SetProperty(ref _option1, value);
                _options[0] = _option1;
                EditCommand.NotifyCanExecuteChanged();
            }
        }

        private string _option2;
        public string Option2
        {
            get { return _option2; }
            set
            {
                SetProperty(ref _option2, value);
                _options[1] = _option2;
                EditCommand.NotifyCanExecuteChanged();
            }
        }

        private string _option3;
        public string Option3
        {
            get { return _option3; }
            set
            {
                SetProperty(ref _option3, value);
                _options[2] = _option3;
                EditCommand.NotifyCanExecuteChanged();
            }
        }

        private BitmapImage _image;
        public BitmapImage Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        private Theme _theme = new Theme("TEMP", false);
        public Theme Theme
        {
            get { return _theme; }
            set { SetProperty(ref _theme, value); }
        }

        private string _themeName;
        public string ThemeName
        {
            get { return _themeName; }
            set { SetProperty(ref _themeName, value); }
        }

        private string _chooseImageText;
        public string ChooseImageText
        {
            get { return _chooseImageText; }
            set { SetProperty(ref _chooseImageText, value); }
        }

        #endregion

        #region RelayCommands + Related Methods

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand RemoveQuizCommand { get; }
        public RelayCommand ReturnCommand { get; }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand CreateQuizCommand { get; }
        public RelayCommand EditQuizTitleCommand { get; }

        public void AddQuiz()
        {
            _quizManager.CreateNewQuiz(NewQuizTitle, new List<Question>());

            ClearTextboxes();

            //Gör så att Propertyn uppdateras
            SelectedQuiz = _quizManager.AllQuizzes[^1];

        }

        public bool CanAddQuiz()
        {
            //Kollar först det finns en quiz med samma Titel.
            if (_quizManager.AllQuizzes.All(q => q.Title != NewQuizTitle))
            {
                return !string.IsNullOrEmpty(NewQuizTitle);
            }
            return false;
        }

        public void AddQuestionToQuiz()
        {
            Theme newTheme = new Theme(ThemeName, false);

            SelectedQuiz.AddQuestion(NewStatement, newTheme, CorrectAnswer, false, null, Options.ToArray());

            Image = null;

            //Gör så att Propertyn uppdateras
            var tempQuiz = SelectedQuiz;
            SelectedQuiz = tempQuiz;
            SelectedQuestion = SelectedQuiz.Questions.ToList()[^1];

            _fileManager.SaveAllQuizzes(_quizManager.AllQuizzes);
        }

        public bool CanAddQuestionToQuiz()
        {
            if (SelectedQuiz == null)
            {
                return false;
            }
            else if (!string.IsNullOrEmpty(Option1) &&
                     !string.IsNullOrEmpty(Option2) &&
                     !string.IsNullOrEmpty(Option3) &&
                     CorrectAnswer != -1 &&
                     !string.IsNullOrEmpty(NewStatement) &&
                     !string.IsNullOrEmpty(Theme.ThemeName) &&
                     SelectedQuiz.Title != "TEMP" &&
                     Option1 != Option2 &&
                     Option1 != Option3 &&
                     Option2 != Option1 &&
                     Option2 != Option3 &&
                     Option3 != Option1 &&
                     Option3 != Option2)
            {
                if (SelectedQuiz.Questions.All(q => q.Statement != NewStatement))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public void EditQuizTitle()
        {
            //Visar en MessageBox asynkront
            Task.Run(() =>
            {
                var dialogResult = MessageBox.Show("Updating quiz title", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            SelectedQuiz.Title = NewQuizTitle;

            //Ändrar namnet på de sparade bilderna
            foreach (var question in SelectedQuiz.Questions)
            {
                string oldPath = question.ImagePath;
                string newPath = Path.Combine(_fileManager.ImageFolderPath,
                    $"{ReplaceInvalidChars(SelectedQuiz.Title)}{ReplaceInvalidChars(question.Statement)}.png");

                SetImageProperty(oldPath);
                SaveBitmapImage(Image, newPath);
                RemoveImage(oldPath, question);
                SetImageProperty(newPath);

                ChangeImagePathName(oldPath, newPath, question);
            }

            //TODO Combobox uppdateras inte direkt om jag inte gör såhär: (Hitta bättre sätt)
            int tempIndex = Quizzes.IndexOf(SelectedQuiz);
            if (Quizzes != null)
            {
                List<Quiz> tempQuizzes = Quizzes.ToList();

                Quizzes.Clear();
                foreach (var quiz in tempQuizzes)
                {
                    Quizzes.Add(quiz);
                }
            }
            SelectedQuiz = Quizzes[tempIndex];
        }

        public bool CanEditQuizTitle()
        {
            if (_quizManager.AllQuizzes.All(q => q.Title != NewQuizTitle) && _quizManager.AllQuizzes.Count > 0)
            {
                return !string.IsNullOrEmpty(NewQuizTitle);
            }
            return false;
        }

        public void EditQuestion()
        {
            SelectedQuestion.Statement = NewStatement;
            SelectedQuestion.Options[0] = Options[0];
            SelectedQuestion.Options[1] = Options[1];
            SelectedQuestion.Options[2] = Options[2];
            SelectedQuestion.Theme.ThemeName = ThemeName;

            //Gör så att jag kan ändra CorrectAnswer
            int tempIndex = SelectedQuiz.Questions.ToList().IndexOf(SelectedQuestion);
            SelectedQuiz.ChangeCorrectAnswer(tempIndex, CorrectAnswer);

            if (SelectedQuestion.ImagePath != null)
            {
                string oldPath = SelectedQuestion.ImagePath;
                string newPath = Path.Combine(_fileManager.ImageFolderPath,
                    $"{ReplaceInvalidChars(SelectedQuiz.Title)}{ReplaceInvalidChars(SelectedQuestion.Statement)}.png");
                ChangeImagePathName(oldPath, newPath, SelectedQuestion);
            }
            
            //Gör så att listan i vyn uppdateras.
            Questions = new ObservableCollection<Question>(SelectedQuiz.Questions);

            EditCommand.NotifyCanExecuteChanged();
        }

        private void ChangeImagePathName(string oldPath, string newPath, Question question)
        {
            SaveBitmapImage(Image, newPath);
            RemoveImage(oldPath, question);
            SetImageProperty(newPath);
            question.ImagePath = newPath;
        }

        public bool CanEditQuestion()
        {
            if (SelectedQuestion != null)
            {
                if (NewStatement != SelectedQuestion.Statement ||
                    SelectedQuestion.Theme.ThemeName != ThemeName ||
                    SelectedQuestion.Options[0] != Options[0] ||
                    SelectedQuestion.Options[1] != Options[1] ||
                    SelectedQuestion.Options[2] != Options[2] ||
                    SelectedQuestion.CorrectAnswer != CorrectAnswer)
                {
                    if (CorrectAnswer > -1 &&
                        !string.IsNullOrEmpty(Option1) &&
                        !string.IsNullOrEmpty(Option2) &&
                        !string.IsNullOrEmpty(Option3) &&
                        !string.IsNullOrEmpty(NewStatement) &&
                        !string.IsNullOrEmpty(Theme.ThemeName) &&
                        Option1 != Option2 &&
                        Option1 != Option3 &&
                        Option2 != Option1 &&
                        Option2 != Option3 &&
                        Option3 != Option1 &&
                        Option3 != Option2)
                    {
                        //Kollar om det finns någon fråga som har samma Statement som det man har skrivit in
                        return !(NewStatement != SelectedQuestion.Statement &&
                                 SelectedQuiz.Questions.Any(q => q.Statement == NewStatement));
                    }
                }
            }
            return false;
        }

        public void RemoveQuiz()
        {
            var messageBox = MessageBox.Show($"Are you sure you want to delete {SelectedQuiz.Title}?", "Message", MessageBoxButton.YesNo);
            if (messageBox == MessageBoxResult.Yes)
            {
                RemoveQuizImages(SelectedQuiz);
                Image = null;
                _quizManager.RemoveQuiz(SelectedQuiz);
                SelectedQuiz = DefaultSelectedQuiz();
            }
        }

        public bool CanRemoveQuiz()
        {
            if (SelectedQuiz != null)
            {
                return true;
            }
            return false;
        }

        public void RemoveQuestion()
        {
            int tempIndex = SelectedQuiz.Questions.ToList().IndexOf(SelectedQuestion);
            SelectedQuiz.RemoveQuestion(tempIndex);

            //ClearTextboxes();

            //Gör så att listan av frågor uppdateras i vyn.........
            if (Questions != null)
            {
                Questions = new ObservableCollection<Question>(SelectedQuiz.Questions);
            }
        }

        public bool CanRemoveQuestion()
        {
            if (SelectedQuestion != null)
            {
                return true;
            }
            return false;
        }

        public void AddImageToQuestion()
        {
            if (!File.Exists(SelectedQuestion.ImagePath))
            {
                OpenFileDialog openFileImageDialog = new OpenFileDialog();
                openFileImageDialog.Title = "Choose image";
                openFileImageDialog.Filter = "ImagePath Files|*.jpg;*.jpeg;*.png;|All files (*.*)|*.*";
                openFileImageDialog.FilterIndex = 1;

                if (openFileImageDialog.ShowDialog() == true)
                {
                    //Kopierar filen och byter namn på den så att den har samma namn som Quiz+Question.
                    if (SelectedQuiz.Title != "TEMP" && SelectedQuestion != null)
                    {
                        //Sparar en temporär kopia av bilden i images mappen:
                        File.Copy(openFileImageDialog.FileName,
                            Path.Combine(_fileManager.ImageFolderPath, $"{ReplaceInvalidChars(SelectedQuiz.Title)}{ReplaceInvalidChars(SelectedQuestion.Statement)}.pppl"));
                        SelectedQuestion.ImagePath =
                            Path.Combine(_fileManager.ImageFolderPath,
                                $"{ReplaceInvalidChars(SelectedQuiz.Title)}{ReplaceInvalidChars(SelectedQuestion.Statement)}.pppl");

                        //Sätter bilden.. 
                        SetImageProperty(SelectedQuestion.ImagePath);

                        //..och sparar undan den i filformatet PNG
                        SaveBitmapImage(Image, SelectedQuestion.ImagePath);

                        //Tar bort den temporära kopia
                        File.Delete(Path.Combine(_fileManager.ImageFolderPath, $"{ReplaceInvalidChars(SelectedQuiz.Title)}{ReplaceInvalidChars(SelectedQuestion.Statement)}.pppl"));

                        SelectedQuestion.ImagePath =
                            Path.Combine(_fileManager.ImageFolderPath,
                                $"{ReplaceInvalidChars(SelectedQuiz.Title)}{ReplaceInvalidChars(SelectedQuestion.Statement)}.png");

                        //Detta gör så att Image som visas i vyn inte är kopplad till den direkt filen
                        ChooseImageText = "Remove image";
                    }
                }
            }
            else if (File.Exists(SelectedQuestion.ImagePath))
            {
                ChooseImageText = "Choose image";
                RemoveImage(SelectedQuestion.ImagePath, SelectedQuestion);
            }
        }

        public void ReturnToStartMenu()
        {
            _fileManager.SaveAllQuizzes(_quizManager.AllQuizzes);
            _navigationStore.CurrentViewModel =
                new StartMenuViewModel(_navigationStore, _quizManager, _themes, _fileManager);
        }

        public bool CanAddImage()
        {
            if (SelectedQuestion != null)
            {
                return true;
            }
            return false;
        }


        private void RemoveImage(string path, Question question)
        {
            question.ImagePath = null;
            Image = null;
            if (path != null)
            {
                File.Delete(path);
            }
        }
        private void RemoveQuizImages(Quiz quiz)
        {
            foreach (var question in quiz.Questions)
            {
                string tempPath = question.ImagePath;
                question.ImagePath = null;
                Image = null;
                if (tempPath != null)
                {
                    File.Delete(tempPath);
                }
            }
        }
        #endregion

        #region Other Methods

        //Detta gör så att Image som visas i vyn inte är kopplad till den direkt filen
        private void SetImageProperty(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                BitmapImage tempBitmapImage = new BitmapImage();
                tempBitmapImage.BeginInit();
                tempBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                tempBitmapImage.UriSource = new Uri(imagePath);
                tempBitmapImage.EndInit();
                Image = tempBitmapImage;
            }
            else
            {
                Image = null;
            }
        }
        public void SaveBitmapImage(BitmapImage image, string filePath)
        {
            //TODO Om man lägger till en bild, tar bort och sen lägger till med en gång så blir det samma som den tidigare bilden.
            if (filePath != null)
            {
                filePath = filePath.Split('.')[0] + ".png";
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var fileStream = File.Create(filePath))
                {
                    encoder.Save(fileStream);
                }
            }
        }
        public string ReplaceInvalidChars(string filename)
        {
            filename = string.Join("_", filename.Split(" "));
            return string.Join("", filename.Split(Path.GetInvalidFileNameChars()));
        }

        private void ClearTextboxes()
        {
            Option1 = "";
            Option2 = "";
            Option3 = "";
            CorrectAnswer = 0;
            NewStatement = "";
            NewQuizTitle = "";
            ThemeName = "";
            Image = null;
        }

        private Quiz DefaultSelectedQuiz()
        {
            if (Quizzes.Count == 0)
            {
                return SelectedQuiz = new Quiz("TEMP", new List<Question>());
            }
            else
            {
                return SelectedQuiz = Quizzes.First();
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedQuiz) ||
                e.PropertyName == nameof(SelectedQuestion) ||
                e.PropertyName == nameof(Option1) ||
                e.PropertyName == nameof(Option2) ||
                e.PropertyName == nameof(Option3) ||
                e.PropertyName == nameof(CorrectAnswer) ||
                e.PropertyName == nameof(NewQuizTitle) ||
                e.PropertyName == nameof(NewStatement) ||
                e.PropertyName == nameof(Theme.ThemeName))
            {
                CreateQuizCommand.NotifyCanExecuteChanged();
                AddCommand.NotifyCanExecuteChanged();
                EditCommand.NotifyCanExecuteChanged();
                EditQuizTitleCommand.NotifyCanExecuteChanged();
                RemoveCommand.NotifyCanExecuteChanged();
                RemoveQuizCommand.NotifyCanExecuteChanged();
                AddImageCommand.NotifyCanExecuteChanged();
            }

            _fileManager.SaveAllQuizzes(_quizManager.AllQuizzes);
        }
        #endregion

        //Konstruktor
        public QuizEditorViewModel(NavigationStore navigationStore, QuizManager quizManager,
            ObservableCollection<Theme> themes, FileManager fileManager)
        {
            _navigationStore = navigationStore;
            _quizManager = quizManager;
            _themes = themes;
            _fileManager = fileManager;

            ChooseImageText = "Choose image";

            //CreateQuizCommand = new CreateQuizCommand(this, quizzes, quiz);

            AddCommand = new RelayCommand(AddQuestionToQuiz, CanAddQuestionToQuiz);
            CreateQuizCommand = new RelayCommand(AddQuiz, CanAddQuiz);
            EditCommand = new RelayCommand(EditQuestion, CanEditQuestion);
            RemoveCommand = new RelayCommand(RemoveQuestion, CanRemoveQuestion);
            AddImageCommand = new RelayCommand(AddImageToQuestion, CanAddImage);
            RemoveQuizCommand = new RelayCommand(RemoveQuiz, CanRemoveQuiz);
            ReturnCommand = new RelayCommand(ReturnToStartMenu);
            EditQuizTitleCommand = new RelayCommand(EditQuizTitle, CanEditQuizTitle);

            PropertyChanged += OnViewModelPropertyChanged;
        }
    }
}
