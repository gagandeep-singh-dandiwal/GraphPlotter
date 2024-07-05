using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotter.Common
{
    /// <summary>
    /// This class reperesents a number on the x axis and y axis
    /// </summary>
    public class Number
    {
        /// <summary>
        /// The WPF x coordinates of the number
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// The WPF y coordinates of the number
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// The value of the number
        /// </summary>
        public string Value { get; set; }
    }
}
