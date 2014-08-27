using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditor.Helper
{
    [Serializable]
    public delegate void PropertyDataChangedEventHandler(object sender, PropertyDataChangedEventArgs e);


    public class PropertyDataChangedEventArgs : EventArgs
    {
        private string changedDataName;
        public PropertyDataChangedEventArgs(string argChangedDataName)
        {
            changedDataName = argChangedDataName;
        }

        public string ChangedDataName
        {
            get { return changedDataName; }
            private set
            {
                changedDataName = value;
            }
        }
    }
}
