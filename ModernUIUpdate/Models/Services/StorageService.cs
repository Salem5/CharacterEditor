using CharacterEditor.Enum;
using CharacterEditor.Helper;
using CharacterEditor.Models;
using Microsoft.VisualBasic.FileIO;
using CharacterEditor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using CharacterModelLib.Models;

namespace CharacterEditor.Models.Services
{

    public class StorageService : IDisposable
    {
        public event PropertyDataChangedEventHandler PropertyDataChanged;
        public event FileSystemEventHandler StorageDataChanged;

        //private ObservableCollection<Character> characterCollection;
        //private string projectPath;

        //public string ProjectPath
        //{
        //    get { return projectPath; }
        //    set
        //    {
        //        projectPath = value;
        //        RaisePropertyDataChanged("ProjectPath");
        //    }
        //}

        //public ObservableCollection<Character> CharacterCollection
        //{
        //    get { return characterCollection; }
        //    set
        //    {
        //        characterCollection = value;
        //        RaisePropertyDataChanged("CharacterCollection");
        //    }
        //}

        ObservableCollection<CharacterProject> delayedProjectCollection;

        private ObservableCollection<CharacterProject> DelayedProjectCollection
        {
            get { return delayedProjectCollection; }
            set
            {
                delayedProjectCollection = value;
                RaisePropertyDataChanged("ProjectCollection");
            }
        }

        public IEnumerable<CharacterProject> ProjectCollection
        {
            get
            {
                try
                {
                    ObservableCollection<CharacterProject> tempList = new ObservableCollection<CharacterProject>();
                    foreach (string inDirectory in Directory.EnumerateDirectories(App.ProjectsPath))
                    {
                        CharacterProject nextProject = new CharacterProject() { Name = Path.GetFileName(inDirectory), ProjectPath = inDirectory };
                        LoadCharacters(nextProject);
                        tempList.Add(nextProject);
                    }
                    
                    DelayedProjectCollection.Clear();

                    foreach (CharacterProject inProject in tempList)
                    {
                        //if (!DelayedProjectCollection.Contains(inProject))
                        {
                            DelayedProjectCollection.Add(inProject);
                        }
                    }

                    return DelayedProjectCollection;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    //Debugger.Break();
                    return null;
                }
            }
        }

        public StorageService()
        {
            DelayedProjectCollection = new ObservableCollection<CharacterProject>();
            DelayedProjectCollection.CollectionChanged += DelayedProjectCollection_CollectionChanged;
        }

        void DelayedProjectCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyDataChanged("ProjectCollection.Items");
        }

        public CharacterProject CreateProject(string projectName, string argAuthor)
        {
            try
            {
                string selectedDirectory = Path.Combine(
                               App.ProjectsPath,
                               projectName
                           );

                if (Directory.Exists(selectedDirectory))
                {
                    throw new CreationException(CreationResult.FolderAlreadyExists);
                }
                if (!Directory.CreateDirectory(selectedDirectory).Exists)
                {
                    throw new CreationException(CreationResult.FolderCreationFailed);
                }

                CharacterProject nextProject = new CharacterProject() { Name = projectName, ProjectPath = selectedDirectory };
                LoadCharacters(nextProject);
                FileTemplateProvider.CreateProjectFile(selectedDirectory, argAuthor);
                RaisePropertyDataChanged("ProjectCollection");
                return nextProject;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                return null;
            }
        }

        private ObservableCollection<Character> LoadCharacters(CharacterProject argProject)
        {
            try
            {
                ObservableCollection<Character> tempList = new ObservableCollection<Character>();
                foreach (string inDirectory in Directory.EnumerateDirectories(argProject.ProjectPath))
                {
                    XDocument characterDoc = XDocument.Load(Path.Combine(inDirectory, FileTemplateProvider.CharacterFileName));
                    Character charTemp = new Character()
                    {
                        Name = Path.GetFileName(inDirectory),
                        FrameWidth = int.Parse(characterDoc.Root.Element("frameSheet").Attribute("frameWidth").Value),
                        FrameHeight = int.Parse(characterDoc.Root.Element("frameSheet").Attribute("frameHeight").Value),
                        LeftMargin = int.Parse(characterDoc.Root.Element("frameSheet").Attribute("leftMargin").Value),
                        UpperMargin = int.Parse(characterDoc.Root.Element("frameSheet").Attribute("upperMargin").Value),
                        RightMargin = int.Parse(characterDoc.Root.Element("frameSheet").Attribute("rightMargin").Value),
                        BottomMargin = int.Parse(characterDoc.Root.Element("frameSheet").Attribute("bottomMargin").Value),
                        //HasFrameSheet = bool.Parse(characterDoc.Root.Element("frameSheet").Attribute("hasFrameSheet").Value)
                    };

                    XDocument animationDoc = XDocument.Load(Path.Combine(inDirectory, FileTemplateProvider.AnimationsFileName));
                    ObservableCollection<IAnimationBase> tempAnimationCollection = new ObservableCollection<IAnimationBase>();

                    foreach (XElement inAnimationElement in animationDoc.Root.Elements("animation"))
                    {
                        if (inAnimationElement.Attribute("type").Value == typeof(ComplexAnimation).ToString())
                        {
                            ObservableCollection<IFrameBase> tempFrameCollection = new ObservableCollection<IFrameBase>();
                            foreach (XElement inFrameElement in inAnimationElement.Elements("frame"))
                            {
                                tempFrameCollection.Add(new Frame()
                                {
                                    Duration = TimeSpan.Parse(inFrameElement.Attribute("duration").Value),
                                    Position = int.Parse(inFrameElement.Attribute("position").Value),
                                    Step = int.Parse(inFrameElement.Attribute("step").Value)
                                });
                            }
                            tempAnimationCollection.Add(new ComplexAnimation()
                            {
                                Frames = new ObservableCollection<IFrameBase>(tempFrameCollection),
                                Name = inAnimationElement.Attribute("name").Value
                            });
                        }
                        else if (inAnimationElement.Attribute("type").Value == typeof(SimpleAnimation).ToString())
                        {
                            tempAnimationCollection.Add(new SimpleAnimation()
                                {
                                    Name = inAnimationElement.Attribute("name").Value,
                                    StartIndex = int.Parse(inAnimationElement.Attribute("startIndex").Value),
                                    EndIndex = int.Parse(inAnimationElement.Attribute("endIndex").Value),
                                    Speed = int.Parse(inAnimationElement.Attribute("speed").Value),
                                    Bounce = (inAnimationElement.Attribute("bounce") != null ? bool.Parse(inAnimationElement.Attribute("bounce").Value): false),
                                    Loop = (inAnimationElement.Attribute("loop") != null ? bool.Parse(inAnimationElement.Attribute("loop").Value) : false),
                                });
                        }
                        else
                        {
                            throw new Exception("Unknown animation type in file");
                        }
                    }
                    charTemp.AnimationCollection = tempAnimationCollection;
                    charTemp.CharacterPath = Path.Combine(argProject.ProjectPath, charTemp.Name);
                    LoadFrameSheet(charTemp);
                    tempList.Add(charTemp);
                }
                argProject.CharacterCollection = tempList;
                return tempList;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                return null;
            }
        }

        public void LoadFrameSheet(Character argCharacter)
        {
            try
            {
                string filePath = Path.Combine(argCharacter.CharacterPath, FileTemplateProvider.FrameSheetFileName);

                if ( File.Exists(filePath))
                {
                    BitmapImage tempFrameSheet = new BitmapImage();

                    tempFrameSheet.BeginInit();
                    tempFrameSheet.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
                    tempFrameSheet.CacheOption = BitmapCacheOption.OnLoad;
                    tempFrameSheet.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    tempFrameSheet.EndInit();

                     argCharacter.FrameSheet = tempFrameSheet;
                }
                else
                {
                    BitmapImage tempFrameSheet = new BitmapImage();

                    tempFrameSheet.BeginInit();
                    tempFrameSheet.UriSource = new Uri(Path.Combine(App.TemplatesPath, FileTemplateProvider.FrameSheetFileName), UriKind.RelativeOrAbsolute);
                    tempFrameSheet.EndInit();

                    argCharacter.FrameSheet = tempFrameSheet;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                //App.DUpdater.LastStatus = "Framesheet loading failed";
                
            }
        }

        public void SaveCharacter(Character argCharacter, CharacterProject argProject)
        {
            try
            {
                XDocument characterDoc = XDocument.Load(Path.Combine(argProject.ProjectPath, argCharacter.Name, FileTemplateProvider.CharacterFileName));
                do
                {
                    argCharacter.IsRedirty = false;

                    Character charCopy = argCharacter;
                    characterDoc.Root.Element("frameSheet").Attribute("frameWidth").Value = charCopy.FrameWidth.ToString();
                    characterDoc.Root.Element("frameSheet").Attribute("frameHeight").Value = charCopy.FrameHeight.ToString();
                    characterDoc.Root.Element("frameSheet").Attribute("leftMargin").Value = charCopy.LeftMargin.ToString();
                    characterDoc.Root.Element("frameSheet").Attribute("upperMargin").Value = charCopy.UpperMargin.ToString();
                    characterDoc.Root.Element("frameSheet").Attribute("rightMargin").Value = charCopy.RightMargin.ToString();
                    characterDoc.Root.Element("frameSheet").Attribute("bottomMargin").Value = charCopy.BottomMargin.ToString();
                    //characterDoc.Root.Element("frameSheet").Attribute("hasFrameSheet").Value = charCopy.HasFrameSheet.ToString();
                    characterDoc.Save(Path.Combine(argProject.ProjectPath, charCopy.Name, FileTemplateProvider.CharacterFileName));

                    XDocument animationDoc = XDocument.Load(Path.Combine(argProject.ProjectPath, argCharacter.Name, FileTemplateProvider.AnimationsFileName));
                    animationDoc.Root.RemoveNodes();
                    foreach (IAnimationBase inAnimation in argCharacter.AnimationCollection)
                    {
                        XElement animationElement = new XElement("animation",
                                    new XAttribute("name", inAnimation.Name),
                                    new XAttribute("type", inAnimation.GetType().ToString())
                                );

                        if (inAnimation.GetType() == typeof(ComplexAnimation))
                        {
                            foreach (IFrameBase inFrame in (inAnimation as ComplexAnimation).Frames)
                            {
                                XElement framesElement = new XElement("frame");
                                framesElement.Add(
                                    new XAttribute("step", inFrame.Step.ToString()),
                                    new XAttribute("position", inFrame.Position.ToString()),
                                    new XAttribute("duration", inFrame.Duration.ToString())
                                    );
                                animationElement.Add(framesElement);
                            }
                        }
                        else if (inAnimation.GetType() == typeof(SimpleAnimation))
                        {
                            SimpleAnimation simpleAnim = inAnimation as SimpleAnimation;
                            animationElement.Add(
                                new XAttribute("startIndex", simpleAnim.StartIndex.ToString()),
                                new XAttribute("endIndex", simpleAnim.EndIndex.ToString()),
                                new XAttribute("speed", simpleAnim.Speed.ToString()),
                                new XAttribute("bounce", simpleAnim.Bounce.ToString()),
                                new XAttribute("loop", simpleAnim.Loop.ToString())
                                    );
                        }
                        else
                        {
                            throw new Exception("Unknown animation type to be saved");
                        }

                        animationDoc.Root.Add(animationElement);
                    }

                    animationDoc.Save(Path.Combine(argProject.ProjectPath, argCharacter.Name, FileTemplateProvider.AnimationsFileName));
                    argCharacter.IsDirty = false;
                } while (argCharacter.IsRedirty);
                //return SaveResult.Success;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                
                
            }
        }

        public Character CreateCharacter(CharacterProject project, string characterName)
        {
            try
            {
                string selectedDirectory = Path.Combine(project.ProjectPath, characterName);

                if (Directory.Exists(selectedDirectory))
                {
                    throw new CreationException(CreationResult.FolderAlreadyExists);
                }
                if (!Directory.CreateDirectory(selectedDirectory).Exists)
                {
                    throw new CreationException(CreationResult.FolderCreationFailed);
                }

                var res = FileTemplateProvider.CreateCharacterFile(selectedDirectory, characterName);

                if (res != CreationResult.Success)
                {//TODO: better exception handling
                    throw new CreationException(res);
                }
                FileTemplateProvider.CreateAnimationsFile(selectedDirectory);
                Character nextCharacter = new Character() { Name = characterName, CharacterPath = selectedDirectory};
                LoadFrameSheet(nextCharacter);
                project.CharacterCollection.Add(nextCharacter);
                project.SelectedCharacter = nextCharacter;
                return nextCharacter;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                return null;
            }
        }

        public void Dispose()
        {

        }

        public void RaisePropertyDataChanged(string argChangedDataName)
        {
            if (PropertyDataChanged != null)
            {
                PropertyDataChanged(this, new PropertyDataChangedEventArgs(argChangedDataName));
            }
        }

        private void RaiseStorageDataChanged(object sender, FileSystemEventArgs e)
        {
            if (StorageDataChanged != null)
            {
                StorageDataChanged(sender, e);
            }
        }

        internal void ImportFrameSheet(string fileToImportPath, CharacterProject project)
        {
            try
            {
                string sourceFileEnding = Path.GetExtension(fileToImportPath).ToLower();

                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream fs = File.OpenRead(fileToImportPath))
                    {
                        BitmapDecoder decoder;

                        switch (sourceFileEnding)
                        {
                            case ".png":
                                decoder = new PngBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                                break;
                            case ".jpg":
                                decoder = new JpegBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                                break;
                            case ".bmp":
                                decoder = new BmpBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                                break;
                            case ".gif":
                                decoder = new GifBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                                break;
                            default:
                                throw new CharacterEditor.Helper.CreationException(CreationResult.FileCreationFailed);
                        }

                        if (decoder != null)
                        {
                            BitmapSource sourceToResize = decoder.Frames.First();
                            {
                                double dpi = 96;
                                int width = sourceToResize.PixelWidth;
                                int height = sourceToResize.PixelHeight;

                                int stride = width * 4; // 4 bytes per pixel
                                byte[] pixelData = new byte[stride * height];
                                sourceToResize.CopyPixels(pixelData, stride, 0);

                                sourceToResize = BitmapSource.Create(width, height, dpi, dpi, sourceToResize.Format, sourceToResize.Palette, pixelData, stride);

                                FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                                newFormatedBitmapSource.BeginInit();
                                newFormatedBitmapSource.Source = sourceToResize;
                                newFormatedBitmapSource.DestinationFormat = System.Windows.Media.PixelFormats.Pbgra32;
                                newFormatedBitmapSource.EndInit();

                                using (FileStream destinationStream = new FileStream(Path.Combine(project.ProjectPath, project.SelectedCharacter.Name, FileTemplateProvider.FrameSheetFileName), FileMode.Create))
                                {
                                    BitmapEncoder encoder;

                                    switch (sourceFileEnding)
                                    {
                                        case ".png":
                                            encoder = new PngBitmapEncoder();
                                            break;
                                        case ".jpg":
                                            encoder = new JpegBitmapEncoder();
                                            break;
                                        case ".bmp":
                                            encoder = new BmpBitmapEncoder();
                                            break;
                                        case ".gif":
                                            encoder = new GifBitmapEncoder();
                                            break;
                                        default:
                                            throw new CharacterEditor.Helper.CreationException(CreationResult.FileCreationFailed);
                                    }
                                    encoder.Frames.Add(BitmapFrame.Create(newFormatedBitmapSource));
                                    encoder.Save(destinationStream);
                                }
                            }
                        }
                    }
                }
                App.StorageService.LoadFrameSheet(project.SelectedCharacter);
            }
            catch (Exception ex)
            {
                //Debugger.Break();
                Trace.WriteLine(ex.Message);
                
            }
        }

        internal void DeleteCharacter(CharacterProject argProject, Character argCharacter)
        {
            try
            {
                FileSystem.DeleteDirectory(Path.Combine(argProject.ProjectPath, argCharacter.Name), UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                
            }
        }

        internal void DeleteProject(CharacterProject SelectedProject)
        {
            try
            {
                FileSystem.DeleteDirectory(SelectedProject.ProjectPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                DelayedProjectCollection.Remove(SelectedProject);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                
            }
        }
    }
}
