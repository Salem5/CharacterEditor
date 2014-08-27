using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CharacterEditor.ViewModels.Command
{
    
    public class RelayCommand : ICommand//, INotifyPropertyChanged
    {
        #region Fields
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion
        // Fields 
        #region Constructors
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
            //this.CanExecuteChanged += RelayCommand_CanExecuteChanged;

        }

        //void RelayCommand_CanExecuteChanged(object sender, EventArgs e)
        //{
        //    RaisePropertyChanged("CanExecuteProperty"); 
        //}

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute"); 
            _execute = execute; _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        [System.Diagnostics.DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }


        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                
            }
            
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion
        // ICommand Members 

        //#region Properties
        //public bool CanExecuteProperty { get { return CanExecute(null); } }
        //#endregion
        //// Properties

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void RaisePropertyChanged(string argPropertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(argPropertyName));
        //    }
        //}
    }
}
