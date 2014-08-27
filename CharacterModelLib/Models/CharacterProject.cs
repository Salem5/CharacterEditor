using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModelLib.Models
{
    public class CharacterProject : NotifyableBase
    {
        public CharacterProject()
        {
            characterCollection = new ObservableCollection<Character>();
        }

        void characterCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {


            RaisePropertyChanged("CharacterCollection");
        }

        Character selectedCharacter;

        public Character SelectedCharacter
        {
            get { return selectedCharacter; }
            set
            {
                if (selectedCharacter != null)
                {
                    selectedCharacter.PropertyChanged -= selectedCharacter_PropertyChanged;
                }
                selectedCharacter = value;

                if (selectedCharacter != null)
                {
                    selectedCharacter.PropertyChanged += selectedCharacter_PropertyChanged;
                }

                RaisePropertyChanged("SelectedCharacter");
                RaisePropertyChanged("NextCharacter");
                RaisePropertyChanged("PreviousCharacter");
            }
        }

        public Character NextCharacter
        {
            get
            {
                if (selectedCharacter == null)
                {
                    return CharacterCollection[0];
                }


                int index = CharacterCollection.IndexOf(selectedCharacter);
                if (index >= CharacterCollection.Count - 1)
                {
                    return CharacterCollection[0];
                }
                else
                {
                    return CharacterCollection[index + 1];
                }
            }
        }

        public Character PreviousCharacter
        {
            get
            {
                if (selectedCharacter == null)
                {
                    return CharacterCollection[CharacterCollection.Count - 1];
                }

                int index = CharacterCollection.IndexOf(selectedCharacter);
                if (index <= 0)
                {
                    return CharacterCollection[CharacterCollection.Count - 1];
                }
                else
                {
                    return CharacterCollection[index - 1];
                }
            }
        }

        private void selectedCharacter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
             RaisePropertyChanged("SelectedCharacter." + e.PropertyName);
             //RaisePropertyChanged(e.PropertyName);
            //RaisePropertyChanged("SelectedCharacter");
        }

        private ObservableCollection<Character> characterCollection;

        public ObservableCollection<Character> CharacterCollection
        {
            get { return characterCollection; }
            set
            {
                if (characterCollection != null)
                {
                    characterCollection.CollectionChanged -= characterCollection_CollectionChanged;
                }
                characterCollection = value;

                if (characterCollection != null)
                {
                    characterCollection.CollectionChanged += characterCollection_CollectionChanged;
                }
                RaisePropertyChanged("CharacterCollection");
            }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string projectPath;

        public string ProjectPath
        {
            get { return projectPath; }
            set
            {
                projectPath = value;
                RaisePropertyChanged("ProjectPath");
            }
        }

        public override string ToString()
        {
            return base.ToString() + ": Name=" + Name;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return ((obj as CharacterProject).Name == this.Name);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
