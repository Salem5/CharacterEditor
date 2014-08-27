using CharacterEditor.Models;
using CharacterEditor.Models.Services;
using CharacterEditor.ViewModels.Command;
using CharacterModelLib.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CharacterEditor.ViewModels
{
    class FrameSheetVM : BaseVM
    {
        private StorageService storageService = App.StorageService;
        private OpenFileDialog openFileDialog;
        private bool hideMargins;

        public bool HideMargins
        {
            get
            {
                return hideMargins; 
            }
            set
            {
                hideMargins = value;
                RaisePropertyChanged("HideMargins");
            }
        }

        public CharacterProject SelectedProject
        {
            get
            {
                return App.DUpdater.SelectedProject;
            }
            set
            {
                App.DUpdater.SelectedProject = value;
            }
        }

        private int maxPossibleFrame;

        public int MaxPossibleFrame
        {
            get { return maxPossibleFrame; }
            set { maxPossibleFrame = value;
            RaisePropertyChanged("MaxPossibleFrame");
            }
        }
        

        public FrameSheetVM()
        {
            Trace.WriteLine("FrameSheetVM initialized");
            App.DUpdater.PropertyChanged += DUpdater_PropertyChanged;
            openFileDialog = new OpenFileDialog();
        }

        void DUpdater_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }


        ICommand deleteAnimationCommand;

        public ICommand DeleteAnimationCommand
        {
            get
            {
                return deleteAnimationCommand ?? (deleteAnimationCommand = new RelayCommand(
                    (param) =>
                    {
                        SelectedProject.SelectedCharacter.AnimationCollection.Remove(SelectedProject.SelectedCharacter.SelectedAnimation);
                        SelectedProject.SelectedCharacter.SelectedAnimation = null;
                       App.DUpdater.LastStatus = "Animation succesfully deleted";
                    },
                    (param) =>
                    {
                        return (SelectedProject != null && SelectedProject.SelectedCharacter != null && SelectedProject.SelectedCharacter.SelectedAnimation != null);
                    }
                    ));
            }
        }

        private ICommand createSimpleAnimationCommand;

        public ICommand CreateSimpleAnimationCommand
        {
            get
            {
                return createSimpleAnimationCommand ?? (createSimpleAnimationCommand = new RelayCommand(
                    (param) =>
                    {
                        try
                        {
                            if (SelectedProject.SelectedCharacter.AnimationCollection.FirstOrDefault((an) => an.Name == AnimationNameField) != null)
                            {
                                App.DUpdater.LastStatus = "Simple animation with name already exists";
                                AnimationNameField = String.Empty;
                                return;
                            }
                            SelectedProject.SelectedCharacter.AnimationCollection.Add(new SimpleAnimation()
                            {
                                Name = AnimationNameField,
                                //StartIndex = int.Parse(AnimationStartIndexField),
                                //EndIndex = int.Parse(AnimationEndIndexField),
                                //Speed = int.Parse(AnimationSpeedField)
                            });
                            App.DUpdater.LastStatus = "Simple animation succesfully added";
                            AnimationNameField = String.Empty;
                        }
                        catch (Exception)
                        {
                            App.DUpdater.LastStatus = "Simple animation creation failed";
                        }
                    }
                    , (param) =>
                    { return SelectedProject != null && SelectedProject.SelectedCharacter != null && !String.IsNullOrEmpty(AnimationNameField); }
                    )
                    );
            }
        }

        private string animationNameField;

        public string AnimationNameField
        {
            get { return animationNameField; }
            set { 
                animationNameField = value;
                RaisePropertyChanged("AnimationNameField");
            }
        }

        private ICommand importFrameSheetCommand;
        
        
        public ICommand ImportFrameSheetCommand
        {
            get
            {
                return importFrameSheetCommand ?? (importFrameSheetCommand = new RelayCommand(
                    (param) =>
                    {
                        try
                        {
                        
                        if (openFileDialog.ShowDialog() != true)
                        {
                            App.DUpdater.LastStatus = "FrameSheet import interrupted";
                            return;
                        }

                        storageService.ImportFrameSheet(openFileDialog.FileName, SelectedProject);
                        
                        App.DUpdater.LastStatus = "FrameSheet successfully imported";

                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.Message);
                            //Debugger.Break();
                        }
                    }
                    , (param) =>
                    { return (SelectedProject != null && SelectedProject.SelectedCharacter != null); }
                    )
                    );
            }
        }
    }
}
