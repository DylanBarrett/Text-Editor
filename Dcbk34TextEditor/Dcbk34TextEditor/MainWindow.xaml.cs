using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace Dcbk34TextEditor
{
    public partial class MainWindow : Window
    {
        const string APP_TITLE = "Dcbk34TextEditor";
        const string DEFAULT_FILENAME = "untitled";
        const string FILE_FILTER = "Text Files (*.txt)|*.txt";
        const string OPEN_ACTION_NAME = "Open";
        const string SAVE_AS_NAME = "Save As";
        
        TextDocument textFile = null;
        TextDocument backupFile = null;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void NewFileActionEvent(object sender, RoutedEventArgs e)
        {
            NewFileFunction();
        }
        private void OpenActionEvent(object sender, RoutedEventArgs e)
        {
            OpenFunction();
        }
        private void CloseCommandHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void SaveFileActionEvent(object sender, RoutedEventArgs e)
        {
            SaveFunction();
        }
        private void SaveAsActionEvent(object sender, RoutedEventArgs e)
        {
            SaveAsFunction();
        } 

        private void NewFileFunction()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FILE_FILTER;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.CheckFileExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = DEFAULT_FILENAME;
            saveFileDialog.Title = SAVE_AS_NAME;
            textFile = new TextDocument(TextEditor);
            if (backupFile == null)
            {
                if (!string.IsNullOrWhiteSpace(textFile.UserInput.Text))
                {
                    SaveWarning();
                }
                textFile.NewFile();
                ChangeApplicationTitle(DEFAULT_FILENAME);
                return;
            }
            if (System.String.Compare(textFile.UserInput.Text, backupFile.CurrentText) != 0 && !string.IsNullOrEmpty(textFile.UserInput.Text))
            {
                SaveWarning();
            }
            textFile.NewFile();
            ChangeApplicationTitle(DEFAULT_FILENAME);
            return;
        }

        private void OpenFunction()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = FILE_FILTER;
            openFileDialog.AddExtension = true;
            openFileDialog.Title = OPEN_ACTION_NAME;
            textFile = new TextDocument(TextEditor);

            if (!string.IsNullOrWhiteSpace(textFile.UserInput.Text) && backupFile == null)
            {
                SaveWarning();
                openFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    textFile = new TextDocument(openFileDialog.FileName, TextEditor);
                    textFile.OpenText();
                    backupFile = new TextDocument(openFileDialog.FileName, TextEditor.Text);
                    backupFile.ReadText();
                    ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textFile.FilePathName));
                    return;
                }
                else
                {
                    return;
                }
            }
            if (backupFile == null)
            {
                openFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    textFile = new TextDocument(openFileDialog.FileName, TextEditor);
                    backupFile = new TextDocument(openFileDialog.FileName, TextEditor.Text);
                    textFile.OpenText();
                    backupFile.ReadText();
                    ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textFile.FilePathName));
                    return;
                }
                else
                {
                    return;
                }
            }
            if (System.String.Compare(textFile.UserInput.Text, backupFile.CurrentText) != 0 && !string.IsNullOrEmpty(textFile.UserInput.Text))
            {
                SaveWarning();
                openFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    textFile = new TextDocument(openFileDialog.FileName, TextEditor);
                    backupFile = new TextDocument(openFileDialog.FileName, TextEditor.Text);
                    textFile.OpenText();
                    backupFile.ReadText();
                    ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textFile.FilePathName));
                    return;
                }
                else
                {
                    return;
                }
            }
            openFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                textFile = new TextDocument(openFileDialog.FileName, TextEditor);
                backupFile = new TextDocument(openFileDialog.FileName, TextEditor.Text);
                textFile.OpenText();
                backupFile.ReadText();
                ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textFile.FilePathName));
                return;
            }
            else
            {
                return;
            }
        }

        private void SaveFunction()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FILE_FILTER;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.AddExtension = true;
            if (textFile != null)
            {
                saveFileDialog.FileName = textFile.FilePathName ?? DEFAULT_FILENAME;
            }
            else
            {
                saveFileDialog.FileName = DEFAULT_FILENAME;
            }
            saveFileDialog.Title = SAVE_AS_NAME;
            textFile = new TextDocument(saveFileDialog.FileName, TextEditor);
            backupFile = new TextDocument(TextEditor.Text);
            if (textFile.FilePathName == DEFAULT_FILENAME)
            {
                saveFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    textFile = new TextDocument(saveFileDialog.FileName, TextEditor);
                    backupFile = new TextDocument(TextEditor.Text);
                    textFile.SaveText();
                    ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textFile.FilePathName));
                }
            }
            textFile.SaveText();
            return;
        }

        private void SaveAsFunction()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FILE_FILTER;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = SAVE_AS_NAME;
            if (textFile == null || string.IsNullOrWhiteSpace(textFile.FilePathName))
            {
                saveFileDialog.FileName = DEFAULT_FILENAME;
            }
            else
            {
                saveFileDialog.FileName = Path.GetFileNameWithoutExtension(textFile.FilePathName);
            }
            saveFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                textFile = new TextDocument(saveFileDialog.FileName, TextEditor);
                backupFile = new TextDocument(TextEditor.Text);
                textFile.SaveText();
                ChangeApplicationTitle(Path.GetFileNameWithoutExtension(textFile.FilePathName));
            }
            else
            {
                return;
            }
        }        

        private void SaveWarning()
        {
            MessageBoxResult result = MessageBox.Show("Do you wish to save this file?", "Save Warning", MessageBoxButton.YesNo, MessageBoxImage.None);
            if (result == MessageBoxResult.Yes)
            {
                SaveAsFunction();
            }
        }

        private void AboutMeActionEvent(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This textpad was created by Dylan Barrett for IT 4400.", "AboutMe", MessageBoxButton.OK, MessageBoxImage.None);
            return;
        }

        private void ChangeApplicationTitle(string PathName)
        {
            if (string.IsNullOrEmpty(PathName))
            {
                PathName = DEFAULT_FILENAME;
            }
            Title = APP_TITLE + " - " + PathName;
            return;
        }
        private void MaximizedScreenHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                return;
            }
            WindowState = WindowState.Normal;
            return;
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                textFile = new TextDocument(TextEditor);
                if (!string.IsNullOrWhiteSpace(textFile.UserInput.Text) && backupFile == null)
                {
                    SaveWarning();
                    return;
                }
                if (System.String.Compare(textFile.UserInput.Text, backupFile.CurrentText, false) != 0 && !string.IsNullOrEmpty(textFile.UserInput.Text))
                {
                    SaveWarning();
                    return;
                }
                if (string.IsNullOrWhiteSpace(textFile.UserInput.Text) && backupFile == null)
                {
                    return;
                }
                if (backupFile == null)
                {
                    return;
                }
            }
            catch (System.NullReferenceException)
            {
                return;
            }
        }
    }
}