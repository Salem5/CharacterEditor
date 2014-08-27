using CharacterEditor.Models;
using CharacterEditor.Models.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CharacterEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static DetachedUpdater dUpdater = new DetachedUpdater();

        //public static TraceSource Logger = new TraceSource("projectBetaCharacterEditLogger");

        public static DetachedUpdater DUpdater
        {
            get { return dUpdater; }
        }

        private static StorageService storageService;
        public static StorageService StorageService
        {
            get
            {
                return storageService ?? (storageService = new StorageService());
            }
        }

        private static string documentsPath;

        public static string DocumentsPath
        {
            get
            {

                //return documentsPath;
                return CharacterEditor.Properties.Settings.Default.DefaultStoragePath;
            }
            set
            {

                //CharacterEditor.Properties.Settings.Default.DefaultStoragePath = documentsPath = value;
                CharacterEditor.Properties.Settings.Default.DefaultStoragePath = value;
                CharacterEditor.Properties.Settings.Default.Save();
                CloseProject();
                LoadStuff();
            }
        }

        private static string projectsPath;

        public static string ProjectsPath
        {
            get
            {
                //if (String.IsNullOrEmpty(projectsPath))
                //{
                //    LoadStuff();
                //}
                return projectsPath;
            }
            private set { projectsPath = value; }
        }

        private static string templatesPath;

        public static string TemplatesPath
        {
            get
            {
                //if (String.IsNullOrEmpty(templatesPath))
                //{
                //    LoadStuff();
                //}
                return templatesPath;
            }
            private set { templatesPath = value; }
        }

        private static void LoadStuff()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");

            if (String.IsNullOrEmpty(DocumentsPath))
            {
                DocumentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ProjBCharaEdit");
            }

            ////DocumentsPath = CharacterEditor.Properties.Settings.Default.DefaultStoragePath;

            ProjectsPath = Path.Combine(DocumentsPath, "Projects");
            TemplatesPath = Path.Combine(DocumentsPath, "Templates");

            if (!Directory.Exists(DocumentsPath))
            {
                Directory.CreateDirectory(DocumentsPath);
            }

            if (!Directory.Exists(ProjectsPath))
            {
                Directory.CreateDirectory(ProjectsPath);
                
            }
            if (!Directory.Exists(TemplatesPath))
            {
                Directory.CreateDirectory(TemplatesPath);
                File.Copy(Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "Material", "FramesheetMissing.png"), Path.Combine(TemplatesPath, FileTemplateProvider.FrameSheetFileName), true);
            }
            StorageService.RaisePropertyDataChanged("ProjectCollection");
        }

        private static void CloseProject()
        {
            if (DUpdater.SelectedProject != null && DUpdater.SelectedProject.SelectedCharacter != null)
            {
                StorageService.SaveCharacter(DUpdater.SelectedProject.SelectedCharacter, DUpdater.SelectedProject);    
            }
            if (DUpdater.SelectedProject != null)
            {
                DUpdater.SelectedProject.SelectedCharacter = null;
                DUpdater.SelectedProject = null;    
            }
        }

        public static void Initialize()
        {
            LoadStuff();
        }
    }
}