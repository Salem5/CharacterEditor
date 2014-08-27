using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace CharacterModelLib.Models
{
    public class Character : NotifyableBase, ICloneable //, IEditableObject
    {
        public Character()
        {
            PropertyChanged += Character_PropertyChanged;
            AnimationCollection = new ObservableCollection<IAnimationBase>();
            AnimationCollection.CollectionChanged += AnimationCollection_CollectionChanged;
        }

        private string characterPath;

        public string CharacterPath
        {
            get { return characterPath; }
            set
            {
                characterPath = value;
                RaisePropertyChanged("CharacterPath");
            }
        }

        private BitmapImage frameSheet;

        public BitmapImage FrameSheet
        {
            get {
                return frameSheet;
            }
            set
            {
                frameSheet = value;
                RaisePropertyChanged("FrameSheet");
            }
        }

        private BitmapImage portrait;

        public BitmapImage Portrait
        {
            get
            {
                return portrait;
            }
            set
            {
                portrait = value;
                RaisePropertyChanged("Portrait");
            }
        }
      
        void AnimationCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("AnimationCollection");
        }

        void selectedAnimation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("SelectedAnimation." + e.PropertyName);
        }

        private IAnimationBase selectedAnimation;

        public IAnimationBase SelectedAnimation
        {
            get { return selectedAnimation; }
            set
            {
                if (selectedAnimation != null)
                {
                    selectedAnimation.PropertyChanged -= selectedAnimation_PropertyChanged;
                }
                selectedAnimation = value;

                if (selectedAnimation != null)
                {
                    selectedAnimation.PropertyChanged += selectedAnimation_PropertyChanged;
                }
                RaisePropertyChanged("SelectedAnimation");
            }
        }

        private ObservableCollection<IAnimationBase> animationCollection;

        public ObservableCollection<IAnimationBase> AnimationCollection
        {
            get
            {
                return animationCollection;
            }
            set
            {
                if (animationCollection != null)
                {
                    animationCollection.CollectionChanged -= AnimationCollection_CollectionChanged;
                }
                animationCollection = value;
                if (animationCollection != null)
                {
                    animationCollection.CollectionChanged += AnimationCollection_CollectionChanged;
                }
                RaisePropertyChanged("AnimationCollection");
            }
        }

      

        void Character_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsDirty" && e.PropertyName != "IsRedirty")
            {
                IsDirty = true;
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

        public override string ToString()
        {
            return base.ToString() + ": Name = " + Name;
        }

        //private bool hasFrameSheet;

        //public bool HasFrameSheet
        //{
        //    get { return hasFrameSheet; }
        //    set
        //    {
              
        //        hasFrameSheet = value;
        //        RaisePropertyChanged("HasFrameSheet");
        //    }
        //}

        private bool isDirty;

        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
              
                isDirty = value;
                RaisePropertyChanged("IsDirty");
            }
        }


        private bool isRedirty;

        public bool IsRedirty
        {
            get { return isRedirty; }
            set
            {
                isRedirty = value;
                RaisePropertyChanged("IsRedirty");
            }
        }

        private int frameHeight = 24;

        [Category("Frame")]
        [DisplayName("Frame Height")]
        [Description("The height of a frame")]
        public int FrameHeight
        {
            get { return frameHeight; }
            set
            {
              
                frameHeight = value;
                RaisePropertyChanged("FrameHeight");
            }

        }

        private int frameWidth = 24 ;

        [Category("Frame")]
        [DisplayName("Frame Width")]
        [Description("The width of a frame")]
        public int FrameWidth
        {
            get { return frameWidth; }
            set
            {
                
                frameWidth = value;
                RaisePropertyChanged("FrameWidth");
            }
        }

        private int leftMargin;

        [Category("Margin")]
        [DisplayName("Left Margin")]
        [Description("The left space between the border and the frames")]
        public int LeftMargin
        {
            get { return leftMargin; }
            set
            {
               
                leftMargin = value;
                RaisePropertyChanged("LeftMargin");
            }
        }

        private int upperMargin;

        [Category("Margin")]
        [DisplayName("Upper Margin")]
        [Description("The upper space between the border and the frames")]
        public int UpperMargin
        {
            get { return upperMargin; }
            set
            {
               
                upperMargin = value;
                RaisePropertyChanged("UpperMargin");
            }
        }

        private int bottomMargin;

        [Category("Margin")]
        [DisplayName("Bottom Margin")]
        [Description("The bottom space between the border and the frames")]
        public int BottomMargin
        {
            get { return bottomMargin; }
            set
            {
               
                bottomMargin = value;
                RaisePropertyChanged("BottomMargin");
            }
        }

        private int rightMargin;

        [Category("Margin")]
        [DisplayName("Right Margin")]
        [Description("The right space between the border and the frames")]
        public int RightMargin
        {
            get { return rightMargin; }
            set
            {
              
                rightMargin = value;
                RaisePropertyChanged("RightMargin");
            }
        }

        //private bool isSaving;

        //public bool IsSaving
        //{
        //    get { return isSaving; }
        //    set
        //    {
        //        isSaving = value;
        //        RaisePropertyChanged("IsSaving");
        //    }
        //}



        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
