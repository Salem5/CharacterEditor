using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CharacterModelLib.Models
{
    public interface IFrameBase : INotifyPropertyChanged
    {
         int Step
        {
            get;
            set;
        }
     

         int Position
        {
            get;
            set;
        }

         TimeSpan Duration
        {
            get;
            set;
        }

         //BitmapImage FrameSheetPart
         //{
         //    get;
         //    set;
         //}
    }
}
