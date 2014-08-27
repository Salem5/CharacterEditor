using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditor.Models
{
    public interface IAnimationBase : INotifyPropertyChanged
    {
        string Name
        {
            get;
            set;
        }

        IList<IFrameBase> Frames { get; set; }

    }
}
