using CharacterEditor.ViewModels;
using CharacterModelLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditor.Models
{
    public class DetachedUpdater : BaseVM
    {
        private string lastStatus;

        public String LastStatus
        {
            get { return lastStatus; }
            set
            {
                lastStatus = value;
                RaisePropertyChanged("LastStatus");
            }
        }


        private CharacterProject selectedProject;

        public CharacterProject SelectedProject
        {
            get {
                return selectedProject;
            }
            set
            {
                if (selectedProject != null)
                {
                    selectedProject.PropertyChanged -= selectedProject_PropertyChanged;
                }
                selectedProject = value;

                if (selectedProject != null)
                {
                    selectedProject.PropertyChanged += selectedProject_PropertyChanged;
                }

                RaisePropertyChanged("SelectedProject");
            }
        }

        void selectedProject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("SelectedProject." + e.PropertyName);
            //RaisePropertyChanged("SelectedProject");
        }
    }
}
