// LockedMultiAccess.cs
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

namespace Root
{
    /// <summary>
    /// Stores a object protected by multiaccess.
    /// </summary>
    /// <typeparam name="T">Type of object to protect.</typeparam>
    public class LockedMultiAccess<T>
    {
        #region Fields

        private T userVar;
        private object isLocked;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes LockedMultiAccess.
        /// </summary>
        public LockedMultiAccess()
        {
            isLocked = (object)false;
        }

        /// <summary>
        /// Initializes LockedMultiAccess and stores a initial value.
        /// </summary>
        /// <param name="value"></param>
        public LockedMultiAccess(T value)
        {
            userVar = value;
            isLocked = (object)false;
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
                LockObject();
                T obj = userVar;
                UnLockObject();
                return obj;
            }
            set
            {
                LockObject();
                userVar = value;
                UnLockObject();
            }
        }

        /// <summary>
        /// Gets a boolean indicating whether current object is locked.
        /// </summary>
        public bool IsLocked
        {
            get
            {
                if (!System.Threading.Monitor.TryEnter(isLocked))
                    return true;
                System.Threading.Monitor.Exit(isLocked);
                return false;
                //System.Threading.Thread.Sleep(1);
                //bool b1 = (bool)isLocked;
                //return b1;
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
            System.Threading.Monitor.Enter(this.isLocked);
        //inicio:
        //    while ((bool)isLocked)
        //    {
        //        System.Threading.Thread.Sleep(1);
        //    }
        //    if (!internalIsLocked(true))
        //        goto inicio;
        }

        /// <summary>
        /// Unlocks stored object to be accessed by <see cref="SafeAccess"/>.
        /// </summary>
        public void UnLockObject()
        {
            System.Threading.Monitor.Exit(this.isLocked);
            //internalIsLocked(false);
            //System.Threading.Thread.Sleep(1);
        }

        //private bool internalIsLocked(bool value)
        //{
        //    lock (isLocked)
        //    {
        //        if ((bool)isLocked && value)
        //            return false;

        //        isLocked = (object)value;
        //        return true;
        //    }
        //}

        #endregion
    }
    
}
