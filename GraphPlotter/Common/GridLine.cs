using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphPlotter.Common
{
    /// <summary>
    /// This class represents a grid line.
    /// </summary>
    public class GridLine
    {
        #region Properties
        /// <summary>
        /// The starting x coordinate of a grid line
        /// </summary>
        public double StartXPoint { get; set; }

        /// <summary>
        /// The starting y coordinate of the grid line.
        /// </summary>
        public double StartYPoint { get; set; }

        /// <summary>
        /// The ending x coordinate of the grid line.
        /// </summary>
        public double EndXPoint { get; set; }

        /// <summary>
        /// The ending y coordinate of the grid line.
        /// </summary>
        public double EndYPoint { get; set; }

        /// <summary>
        /// The thickness of the grid line.
        /// </summary>
        public double Thickness { get; set; }
        #endregion

    }
}
