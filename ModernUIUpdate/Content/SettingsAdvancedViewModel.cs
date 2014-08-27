using FirstFloor.ModernUI.Presentation;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterEditor.Content
{
    /// <summary>
    /// A simple view model for configuring theme, font and accent colors.
    /// </summary>
    public class SettingsAdvancedViewModel
        : NotifyPropertyChanged
    {

        private RelayCommand changeDefaultPath;

        public RelayCommand ChangeDefaultPath
        {
            get
            {
                return changeDefaultPath ?? (changeDefaultPath = new RelayCommand(
                    (param) =>
                    {
                        CommonOpenFileDialog diag = new CommonOpenFileDialog(App.DocumentsPath);
                        diag.IsFolderPicker = true;
                        if (diag.ShowDialog() != CommonFileDialogResult.Ok)
                        {
                            return;
                        }
                        DocumentsPath = diag.FileName;
                    }
                    ));
            }
        }

        public string DocumentsPath
        {
            get { return App.DocumentsPath; }
            set { App.DocumentsPath = value;
            OnPropertyChanged("DocumentsPath");
            }
        }
    }
}
