using System;
using System.Collections.Generic;
using System.Text;

namespace SklLib
{
    /// <summary>
    /// Defines a object that can be validated.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Returns whether current instance is valid.
        /// </summary>
        /// <returns>True whether is valid; otherwise false.</returns>
        bool IsValid();
    }
}
