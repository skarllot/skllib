// GrammarNumberWriteInfo.cs
//
//  Copyright (C) 2014 Fabrício Godoy
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SklLib.Formatting
{
    /// <summary>
    /// Defines how to writes grammatical number of any type.
    /// </summary>
    public struct GrammarNumberWriteInfo
    {
        #region Fields

        private string _singular;
        private string _plural;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="GrammarNumberWriteInfo"/> and
        /// sets the singular and plural text.
        /// </summary>
        /// <param name="singular">The singular text.</param>
        /// <param name="plural">The plural text.</param>
        public GrammarNumberWriteInfo(string singular, string plural)
        {
            _singular = singular;
            _plural = plural;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets how to writes the singular of type.
        /// </summary>
        public string Plural { get { return _plural; } }

        /// <summary>
        /// Gets how to writes the plural of type.
        /// </summary>
        public string Singular { get { return _singular; } }

        #endregion
    }
}
