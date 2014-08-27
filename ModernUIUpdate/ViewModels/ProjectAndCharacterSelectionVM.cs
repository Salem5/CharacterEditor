using CharacterEditor.Models;
using CharacterEditor.Models.Services;
using CharacterEditor.ViewModels;
using CharacterEditor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Input;
using CharacterEditor.ViewModels.Command;
using CharacterEditor.Enum;
using System.Windows;
using System.Diagnostics;
using CharacterModelLib.Models;

namespace CharacterEditor.ViewModels
{
    class ProjectAndCharacterSelectionVM : BaseVM
    {
        private StorageService storageService = App.StorageService;

        private ICommand createProjectCommand;

        public ICommand CreateProjectCommand
        {
            get
            {
                return createProjectCommand ?? (createProjectCommand = new RelayCommand(
                    (param) =>
                    {
                        try
                        {
                            CloseProject();
                            CharacterProject nextProject = storageService.CreateProject(projectNameField, "Standard Author");

                            App.DUpdater.SelectedProject = nextProject;
                            App.DUpdater.LastStatus = "Project successfully created";
                            //TODO: modern Ui fixes
                            //switch (storageService.CreateProject(selectedDirectory, "Standard Author"))
                            //{
                            //    case CreationResult.Success:
                            //        ProjectName = createProjectDiag.ProjectName.Text;
                            //        IsProjectLoaded = true;
                            //        LastStatus = "Project successfully created";
                            //        break;
                            //    case CreationResult.FolderCreationFailed:
                            //        MessageBox.Show("Folder creation Failed", "Creation Failed", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                            //        LastStatus = "Folder creation Failed";
                            //        break;
                            //    case CreationResult.FolderAlreadyExists:
                            //        MessageBox.Show("Folder already existed", "Folder exists", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                            //        LastStatus = "Folder already existed";
                            //        break;
                            //    case CreationResult.FileCreationFailed:
                            //        MessageBox.Show("Project File creation Failed", "Creation Failed", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                            //        LastStatus = "Project File creation Failed";
                            //        break;
                            //    default:
                            //        throw new Exception();
                            //}
                            ProjectNameField = string.Empty;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            App.DUpdater.LastStatus = "Project creation failed";
                        }
                    },
                       (param) =>
                       {
                           return (!String.IsNullOrEmpty(ProjectNameField));
                       }
                    ));
            }
        }


        private ICommand deleteCharacterCommand;

        public ICommand DeleteCharacterCommand
        {
            get
            {
                return deleteCharacterCommand ?? (deleteCharacterCommand = new RelayCommand(
                    (param) =>
                    {
                        try
                        {
                            storageService.DeleteCharacter(SelectedProject,SelectedProject.SelectedCharacter);
                            SelectedProject.CharacterCollection.Remove(SelectedProject.SelectedCharacter);
                            SelectedProject.SelectedCharacter = null;
                            App.DUpdater.LastStatus = "Character sucessfully deleted";
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            App.DUpdater.LastStatus = "Character deletion failed";
                        }
                    },
                       (param) =>
                       {
                           return (SelectedProject != null && SelectedProject.SelectedCharacter != null);
                       }
                    ));
            }
        }


        private ICommand deleteProjectCommand;

        public ICommand DeleteProjectCommand
        {
            get
            {
                return deleteProjectCommand ?? (deleteProjectCommand = new RelayCommand(
                    (param) =>
                    {
                        storageService.DeleteProject(SelectedProject);
                        SelectedProject = null;
                        App.DUpdater.LastStatus = "Project sucessfully deleted";
                    },
                       (param) =>
                       {
                           return (SelectedProject != null );
                       }
                    ));
            }
        }

        private void CloseProject()
        {
            if (SelectedProject != null && SelectedProject.SelectedCharacter != null && SelectedProject.SelectedCharacter.IsDirty)
            {
                storageService.SaveCharacter(SelectedProject.SelectedCharacter, SelectedProject);
            }
        }

        private String projectNameField;

        public String ProjectNameField
        {
            get { return projectNameField; }
            set
            {
                projectNameField = value;
                RaisePropertyChanged("ProjectNameField");
            }
        }

        private String characterNameField;

        public String CharacterNameField
        {
            get { return characterNameField; }
            set { 
                characterNameField = value;
                RaisePropertyChanged("CharacterNameField");
            }
        }

        private ICommand createCharacterCommand;

        public ICommand CreateCharacterCommand
        {
            get
            {
                return createCharacterCommand ?? (createCharacterCommand = new RelayCommand(
                    (param) =>
                    {
                        if (String.IsNullOrEmpty(characterNameField))
                        {
                            MessageBox.Show("No character name specified", "Missing character name", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);

                            App.DUpdater.LastStatus = "Missing character name";
                            return;
                        }
                        storageService.CreateCharacter(SelectedProject, characterNameField);
                        App.DUpdater.LastStatus = "Character successfully created";

                        //{
                        //    case CreationResult.Success:
                        //        CharacterCollection.Add(new Character() { Name = CharacterNameField });
                        //        CharacterNameField = String.Empty;
                        //        LastStatus = "Character successfully created";
                        //        break;
                        //    case CreationResult.FileCreationFailed:
                        //    case CreationResult.FileAlreadyExists:
                        //    case CreationResult.FolderCreationFailed:
                        //        LastStatus = "Character creation failed";
                        //        break;
                        //    case CreationResult.FolderAlreadyExists:
                        //        LastStatus = "Character already exists";
                        //        break;
                        //    default:
                        //        throw new Exception();
                        //}
                        CharacterNameField = string.Empty;
                    }
                    , (param) =>
                    { return App.DUpdater.SelectedProject != null && !String.IsNullOrEmpty(CharacterNameField); }
                    )
                    );
            }
        }

        public ProjectAndCharacterSelectionVM()
        {
            Trace.WriteLine("ProjectAndCharacterSelectionVM initialized");
            App.DUpdater.PropertyChanged += DUpdater_PropertyChanged;
            App.StorageService.PropertyDataChanged += StorageService_PropertyDataChanged;
            
        }
        void StorageService_PropertyDataChanged(object sender, Helper.PropertyDataChangedEventArgs e)
        {
            RaisePropertyChanged(e.ChangedDataName);
        }

        void DUpdater_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        public CharacterProject SelectedProject
        {
            get
            {
                return App.DUpdater.SelectedProject;
            }
            set
            {
                if (App.DUpdater.SelectedProject != null)
                {
                    App.DUpdater.SelectedProject.SelectedCharacter = null;
                }
                App.DUpdater.SelectedProject = value;
            }
        }


        public IEnumerable<CharacterProject> ProjectCollection
        {
            get
            {
                return storageService.ProjectCollection;
            }
        }
    }
}
