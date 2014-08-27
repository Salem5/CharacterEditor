using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditor.Models
{
    public class ComplexAnimation : NotifyableBase , IAnimationBase
    {
        private ObservableCollection<IFrameBase> frames;
        private string name;

        public ComplexAnimation()
        {
            frames = new ObservableCollection<IFrameBase>();
            frames.CollectionChanged += Frames_CollectionChanged;
        }

        void Frames_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null) {
            foreach (object item in e.OldItems)
            {
                (item as IFrameBase).PropertyChanged -= FrameCollectionItem_PropertyChanged;
            }
                }
            if (e.NewItems != null)
            {
                foreach (object item in e.NewItems)
                {
                    (item as IFrameBase).PropertyChanged += FrameCollectionItem_PropertyChanged;
                }
            }

            RaisePropertyChanged("Frames.Collection");
        }

        private void FrameCollectionItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("Frames.Collection[" + frames.IndexOf(sender as IFrameBase) + "]." + e.PropertyName);
        }


        

    
        private IFrameBase selectedFrame;

        public IFrameBase SelectedFrame
        {
            get { return selectedFrame; }
            set
            {
                selectedFrame = value;
                RaisePropertyChanged("SelectedFrame");
            }
        }


        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public IList<IFrameBase> Frames
        {
            get
            {
                return frames;
            }
            set
            {
                frames = new ObservableCollection<IFrameBase>(value);
                RaisePropertyChanged("Frames");
            }
        }
    }
}
