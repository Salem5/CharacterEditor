using CharacterEditor.ViewModels;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CharacterEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private MainVM viewModel;
        private Timer previewTimer;
        private FirstFloor.ModernUI.Presentation.Link framesheetLink;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            Trace.WriteLine(String.Format("=== {0} ; {1} === Application started", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString()));
            App.Initialize();
            base.OnInitialized(e);
        }

        

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            Trace.WriteLine("Application ended");
            Trace.Close();
        }

        public static readonly RoutedEvent CharacterSwitchedEvent = EventManager.RegisterRoutedEvent(
            "CharacterSwitched", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ModernWindow));

        // Provide CLR accessors for the event 
        public event RoutedEventHandler CharacterSwitched
        {
            add { AddHandler(CharacterSwitchedEvent, value); }
            remove { RemoveHandler(CharacterSwitchedEvent, value); }
        }

        private void RaiseCharacterSwitchedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(MainWindow.CharacterSwitchedEvent);
            RaiseEvent(newEventArgs);
        }


        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = DataContext as MainVM;
            viewModel.PropertyChanged += viewModel_PropertyChanged;

            framesheetLink = new FirstFloor.ModernUI.Presentation.Link() { DisplayName = "framesheet", Source = new Uri("/Pages/FramesheetPage.xaml",UriKind.RelativeOrAbsolute) };
        }


        void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedProject.SelectedCharacter":
                    if (viewModel.SelectedProject != null && viewModel.SelectedProject.SelectedCharacter != null)
                    {
                        LinkGroupBig.DisplayName = viewModel.SelectedProject.SelectedCharacter.Name;
                        if (!LinkGroupBig.Links.Contains(framesheetLink))
                        {
                            LinkGroupBig.Links.Add(framesheetLink);
                        }
                    }
                    else
                    {
                        LinkGroupBig.DisplayName = "[Not selected]";
                        LinkGroupBig.Links.Remove(framesheetLink);
                        //<mui:Link DisplayName="framesheet" Source="/Pages/FramesheetPage.xaml" />
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
