// Swap.cs
//
//  Copyright (C) 2008 Fabrício Godoy
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

namespace SklLib
{
    
    /// <summary>
    /// Provides variables swapping.
    /// </summary>
    public static class Swap
    {
        /// <summary>
        /// Swaps the values from variables.
        /// </summary>
        /// <param name="var1">First variable.</param>
        /// <param name="var2">Second variable.</param>
        public static void Do<T>(ref T var1, ref T var2)
        {
            T temp = var1;
            var1 = var2;
            var2 = temp;
        }
        
        /// <summary>
        /// Swaps the values from variables.
        /// </summary>
        /// <param name="var1">First variable.</param>
        /// <param name="var2">Second variable.</param>
        public static void Do(ref object var1, ref object var2)
        {
            object temp = var1;
            var1 = var2;
            var2 = temp;
        }
    }
}
