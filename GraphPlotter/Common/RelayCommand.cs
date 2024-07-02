using System;
using System.Windows.Input;

namespace GraphPlotter.Common
{
    /// <summary>
    /// This class implements the ICommand interface which helping in binding with 
    /// commands in the views.
    /// </summary>
    internal class RelayCommand : ICommand
    {
        #region Constructor
        /// <summary>
        /// The RelayCommand constructor
        /// </summary>
        /// <param name="execute">The delegate to functoin which is executed.</param>
        /// <param name="canExecute">The delegate to the function which tell if the command should be execute or not.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region Private variables
        /// <summary>
        /// The local private field for action delegate.
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// The local private field for predicate delegate.
        /// </summary>
        private readonly Predicate<object> _canExecute;
        #endregion

        #region Properties
        /// <summary>
        /// This event is raised when the result of CanExuecte changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method executes the method passed on as delegate.
        /// </summary>
        /// <param name="parameter">the parameter of the method passed</param>
        /// <returns>If the method should be executed or not.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        /// <summary>
        /// This method executes the method passed on as delegate.
        /// </summary>
        /// <param name="parameter">the parameter of the method passed</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion
    }
}
