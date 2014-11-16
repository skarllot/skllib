// LockedMultiAccess.cs
//
//  Copyright (C) 2008, 2014 Fabrício Godoy
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
using System.Threading;

namespace SklLib.Threading
{
    /// <summary>
    /// Stores a object protected by multiaccess.
    /// </summary>
    /// <typeparam name="T">Type of object to protect.</typeparam>
    public class LockedMultiAccess<T>
    {
        #region Fields

        private T userVar;
        private AutoResetEvent evLocked;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes LockedMultiAccess.
        /// </summary>
        public LockedMultiAccess()
            : this(default(T))
        {
        }

        /// <summary>
        /// Initializes LockedMultiAccess and stores a initial value.
        /// </summary>
        /// <param name="value">The value.</param>
        public LockedMultiAccess(T value)
        {
            userVar = value;
            evLocked = new AutoResetEvent(true);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the object stored by this instance directly.
        /// </summary>
        public T DirectAccess
        {
            get { return userVar; }
            set { userVar = value; }
        }

        /// <summary>
        /// Gets or sets the object stored by this instance; If the object is locked then
        /// waits to unlock.
        /// </summary>
        public T SafeAccess
        {
            get
            {
                evLocked.WaitOne();
                T result = userVar;
                evLocked.Set();
                return result;
            }
            set
            {
                evLocked.WaitOne();
                userVar = value;
                evLocked.Set();
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether current object is locked.
        /// </summary>
        public bool IsLocked
        {
            get
            {
                if (evLocked.WaitOne(0)) {
                    evLocked.Set();
                    return false;
                }

                return true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Locks stored object to be accessed by <see cref="SafeAccess"/>, if the object is locked then
        /// waits to unlock.
        /// </summary>
        public void LockObject()
        {
            evLocked.WaitOne();
        }

        /// <summary>
        /// Unlocks stored object to be accessed by <see cref="SafeAccess"/>.
        /// </summary>
        public void UnLockObject()
        {
            evLocked.Set();
        }

        #endregion
    }
    
}
