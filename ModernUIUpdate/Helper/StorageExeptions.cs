using CharacterEditor.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditor.Helper
{
    public class CreationException : Exception
    {
        public CreationException(CreationResult result)
        {
            this.Result = result;
        }
        public CreationResult Result { set; get; }
    }
    public class LoadingException : Exception
    {
        public LoadingException(LoadingResult result)
        {
            this.Result = result;
        }
        public LoadingResult Result { set; get; }
    }
    public class SavingException : Exception
    {
        public SavingException(SavingResult result)
        {
            this.Result = result;
        }
        public SavingResult Result { set; get; }
    }

    public class DeletionException : Exception
    {
        public DeletionException(DeletionResult result)
        {
            this.Result = result;
        }
        public DeletionResult Result { set; get; }
    }
}
