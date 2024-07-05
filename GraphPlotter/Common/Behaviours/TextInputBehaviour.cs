using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GraphPlotter.Common.Behaviours
{
    /// <summary>
    /// This class is the behaviour for the textboxes in the application.
    /// </summary>
    public class TextInputBehaviour:Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += PreviewTextInput;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= PreviewTextInput;

        }

        /// <summary>
        /// This is called when a user inputs in the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed((sender as TextBox).Text + e.Text);
        }

        /// <summary>
        /// Here the regex calculates if the input is valid or not.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsTextAllowed(string text)
        {
            //Regex for textboxes, 5 whole digits and 4 decimal digits.
            Regex regex = new Regex(@"^-?\d{0,5}(\,\d{0,4})?$");
            return regex.IsMatch(text);
        }
    }
}
