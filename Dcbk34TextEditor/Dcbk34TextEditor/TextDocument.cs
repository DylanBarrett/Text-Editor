using System.IO;
using System.Windows.Controls;

namespace Dcbk34TextEditor
{
    class TextDocument
    {
        string userText;
        TextBox Input;
        string pathName;

        public TextDocument(string PathName, TextBox TextInput)
        {
            pathName = PathName;
            Input = TextInput;
        }

        public TextDocument(string PathName, string CurrentText)
        {
            pathName = PathName;
            userText = CurrentText;
        }

        public TextDocument(TextBox TextInput)
        {
            Input = TextInput;
        }

        public TextDocument(string CurrentText)
        {
            userText = CurrentText;
        }

        public void NewFile()
        {
            UserInput.Text = string.Empty;
        }

        public void OpenText()
        {
            UserInput.Text = File.ReadAllText(FilePathName);
        }
        public void SaveText()
        {
            File.WriteAllText(FilePathName, UserInput.Text);
        }

        public void ReadText()
        {
            userText = File.ReadAllText(FilePathName);
        }

        public string FilePathName
        {
            get { return pathName; }
            set { pathName = value; }
        }

        public TextBox UserInput
        {
            get { return Input; }
            set { Input = value; }
        }

        public string CurrentText
        {
            get { return userText; }
            set { userText = value; }
        }
    }
}