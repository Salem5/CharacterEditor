using CharacterEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CharacterEditor.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class ProjectAndCharacterSelectionPage : UserControl
    {
        private System.Threading.Timer timer;
        private ProjectAndCharacterSelectionVM viewModel;

        public ProjectAndCharacterSelectionPage()
        {
            InitializeComponent();
            Trace.WriteLine("ProjectAndCharacterSelection page initialized");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("ProjectAndCharacterSelectionPage loaded");

            viewModel = DataContext as ProjectAndCharacterSelectionVM;
            viewModel.PropertyChanged += viewModel_PropertyChanged;

            timer = new System.Threading.Timer(
                (vm) =>
                {
                    if (vm == null)
                    {
                        return;
                    }

                    if (App.DUpdater.SelectedProject != null && App.DUpdater.SelectedProject.SelectedCharacter != null && App.DUpdater.SelectedProject.SelectedCharacter.IsDirty)
                    {
                        lock (App.DUpdater.SelectedProject.SelectedCharacter)
                        {
                            App.StorageService.SaveCharacter(App.DUpdater.SelectedProject.SelectedCharacter, (vm as ProjectAndCharacterSelectionVM).SelectedProject);
                        }
                    }
                }, viewModel, 1000, 5000);
        }

        private void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        { }

        private void addCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            addCharacterPopup.IsOpen = true;
        }

        private void addProjectButton_Click(object sender, RoutedEventArgs e)
        {
            addProjectPopup.IsOpen = true;
        }


        private void addProjectConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            addProjectPopup.IsOpen = false;
        }

        private void addCharacterConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            addCharacterPopup.IsOpen = false;
        }

        private void deleteCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SystemSounds.Exclamation.Play();
            deleteCharacterPopup.IsOpen = true;
        }

        private void deleteCharacterConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            deleteCharacterPopup.IsOpen = false;
        }

        private void deleteProjectButton_Click(object sender, RoutedEventArgs e)
        {
            deleteProjectPopup.IsOpen = true;
        }

        private void deleteProjectConfirmButton_Click(object sender, RoutedEventArgs e)
        {

            deleteProjectPopup.IsOpen = false;
        }
    }
}