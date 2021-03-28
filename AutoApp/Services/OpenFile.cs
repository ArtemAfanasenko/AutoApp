using AutoApp.Interfaces;
using Microsoft.Win32;

namespace AutoApp.Services
{
    public class OpenFile : IOpenFile
    {
        public OpenFile()
        {

        }

        public string OpenFileDialogs()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.png)|*.png|(*.jpg)|*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return openFileDialog.FileName;
        }
    }
}
