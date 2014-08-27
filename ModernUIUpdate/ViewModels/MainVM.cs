using CharacterEditor.Models;
using CharacterEditor.ViewModels;
using CharacterEditor.ViewModels.Command;
using CharacterModelLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CharacterEditor.ViewModels
{
    public class MainVM :BaseVM
    {
        public MainVM()
        {
            App.DUpdater.PropertyChanged += DUpdater_PropertyChanged;
        }
        
        public CharacterProject SelectedProject
        {
            get {
              
                return App.DUpdater.SelectedProject; 
            }
        }

        void DUpdater_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        ICommand selectPreviousCharacterCommand;

        public ICommand SelectPreviousCharacterCommand
        {
            get
            {
                return selectPreviousCharacterCommand ?? (selectPreviousCharacterCommand = new RelayCommand(
                   (param) =>
                   {
                       App.DUpdater.SelectedProject.SelectedCharacter = App.DUpdater.SelectedProject.PreviousCharacter;
                   }
                    , (param) =>
                    { return (App.DUpdater.SelectedProject != null && (App.DUpdater.SelectedProject.SelectedCharacter != null || App.DUpdater.SelectedProject.CharacterCollection.Count >= 1)); }
                    )
                    );
            }
        }

        private ICommand selectNextCharacterCommand;

        public ICommand SelectNextCharacterCommand
        {
            get
            {
                return selectNextCharacterCommand ?? (selectNextCharacterCommand = new RelayCommand(
                    (param) =>
                    {
                        App.DUpdater.SelectedProject.SelectedCharacter = App.DUpdater.SelectedProject.NextCharacter;
                    }
                    , (param) =>
                    { return (App.DUpdater.SelectedProject != null && (App.DUpdater.SelectedProject.SelectedCharacter != null || App.DUpdater.SelectedProject.CharacterCollection.Count >= 1)); }
                    )
                    );
            }
        }

        

    }
}
