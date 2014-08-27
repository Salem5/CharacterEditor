using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditor.Models
{
    public class Frame : NotifyableBase, IFrameBase
    {
        private int step;
        public   int Step
        {
            get { return step; }
            set
            {
                step = value;
                RaisePropertyChanged("Step");
            }
        }

        private int position;

        public  int Position
        {
            get { return position; }
            set
            {
                position = value;
                RaisePropertyChanged("Position");
            }
        }
        private TimeSpan duration;

        public  TimeSpan Duration
        {
            get { return duration; }
            set { duration = value;
            RaisePropertyChanged("Duration");
            }
        }

    }
}
