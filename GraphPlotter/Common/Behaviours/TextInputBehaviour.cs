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
        private void PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed((sender as TextBox).Text + e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            //Regex for textboxes, 5 whole digits and 4 decimal digits.
            Regex regex = new Regex(@"^-?\d{1,5}(\,\d{0,4})?$");
            return regex.IsMatch(text);
        }
    }
}
