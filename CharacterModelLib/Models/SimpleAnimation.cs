using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModelLib.Models
{
    public class SimpleAnimation : NotifyableBase, IAnimationBase
    {
        private int startIndex;
        private int speed;
        private int endIndex;
        private string name;
        private FakeFrameCollection frames;

        public SimpleAnimation()
        {
            frames = new FakeFrameCollection(this);
        }

        public int RangeStart
        {
            get {
                if (startIndex <= endIndex)
                {
                    return startIndex;     
                }
                else
                {
                    return endIndex;
                }
            }
        }

        public int RangeEnd
        {
            get
            {
                if (startIndex <= endIndex)
                {
                    return endIndex;
                }
                else
                {
                    return startIndex;
                }
            }
            
        }


        public int StartIndex
        {
            get { return startIndex; }
            set
            {
                startIndex = value;
                RaisePropertyChanged("StartIndex");
                RaisePropertyChanged("RangeEnd");
                RaisePropertyChanged("RangeStart");
                
            }
        }

        public int EndIndex
        {
            get { return endIndex; }
            set
            {
                endIndex = value;
                RaisePropertyChanged("EndIndex");
                RaisePropertyChanged("RangeEnd");
                RaisePropertyChanged("RangeStart");
            }
        }

        /// <summary>
        /// Speed in Milliseconds
        /// </summary>
        public int Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                RaisePropertyChanged("Speed");
            }
        }

        public bool IsReverse
        {
            get
            {
                return (endIndex < startIndex);
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

        private bool bounce;

        public bool Bounce
        {
            get { return bounce; }
            set { bounce = value;
            RaisePropertyChanged("Bounce");
            }
        }

        private bool loop;

        public bool Loop
        {
            get { return loop; }
            set
            {
                loop = value;
                RaisePropertyChanged("Loop");
            }
        }

        public IList<IFrameBase> Frames
        {
            get
            {
                return frames.ToList<IFrameBase>();
            }
            set
            {

            }
        }
    }
}
