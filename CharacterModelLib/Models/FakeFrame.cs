using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterModelLib.Models
{
    public class FakeFrame : NotifyableBase, IFrameBase
    {
        private SimpleAnimation parent;

        public SimpleAnimation Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public int Index { set; get; }

        public FakeFrame(SimpleAnimation argParent, int argIndex)
        {
            parent = argParent;
            Index = argIndex;
        }

        public  int Position
        {
            get
            {
                //return Math.Abs(parent.StartIndex - parent.EndIndex) + Index;
                //return Index;
                if (parent.EndIndex > parent.StartIndex)
                {
                    return parent.StartIndex + Index;
                }
                else
                {
                    return parent.StartIndex - Index;
                }                
            }
            set
            { }

        }

        public  TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromMilliseconds(parent.Speed);
            }
            set
            { }
        }

        public  int Step
        {
            get
            {
                return Index;
            }
            set
            { }
        }
    }
}
