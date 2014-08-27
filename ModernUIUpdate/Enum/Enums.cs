using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditor.Enum
{
    [Flags]
    public enum CreationResult
    {
        Success = 0,
        FolderCreationFailed = 1,
        FolderAlreadyExists = 2,
        FileCreationFailed = 3,
        FileAlreadyExists = 4
    }
    [Flags]
    public enum LoadingResult
    {
        Success = 0,
        FolderLoadingFailed = 1,
        FolderNotExisting = 2,
        FileLoadingFailed = 3,
        FileNotExisting = 4
    }
    [Flags]
    public enum SavingResult
    {
        Success = 0,
        FolderAccessFailed = 1,
        FolderNotExisting = 2,
        FileOveridingFailed = 3,
        FileSavingFailed = 4,
    }
    [Flags]
    public enum DeletionResult
    {
        Success = 0,
        FolderAccessFailed = 1,
        FolderNotExisting = 2,
        FileNotExisting = 3,
        FileDeletionFailed = 4,
        FolderDeletionFailed = 5,
    }
}
