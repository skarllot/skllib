// ExceptionExtension.cs
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
using System.Collections;
using stringb = System.Text.StringBuilder;

namespace SklLib
{
    /// <summary>
    /// Provides methods to <see cref="System.Exception"/> class.
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Creates a complete Exception dump to text.
        /// </summary>
        /// <param name="ex">The Exception to dump.</param>
        /// <returns>A human-readable text detailing Exception instance.</returns>
        public static string CreateDump(this Exception ex)
        {
            stringb message = new stringb();
            AppendDump(ex, message, 0);
            return message.ToString();
        }

        private static void AppendDump(Exception ex, stringb str, int level)
        {
            string indent = new string(' ', level);
            Type exType = ex.GetType();

            if (level > 0) {
                str.Append(indent);
                str.AppendLine("Inner exception:");
            }

            Action<string> append = (prop) => {
                var propInfo = exType.GetProperty(prop);
                if (propInfo != null) {
                    var val = propInfo.GetValue(ex, null);

                    if (val != null) {
                        str.AppendFormat("{0}* {1}: {2}",
                            indent, prop, val.ToString());
                        str.AppendLine();
                    }
                }
            };

            append("Message");
            append("HResult");
            append("HelpLink");
            append("Source");
            append("StackTrace");
            append("TargetSite");

            str.AppendFormat("{0}* Exception Type: {1}", indent, exType.ToString());
            str.AppendLine();

            foreach (DictionaryEntry de in ex.Data) {
                str.AppendFormat("{0} * {1} = {2}",
                    indent, de.Key, de.Value);
                str.AppendLine();
            }

            if (ex.InnerException != null) {
                AppendDump(ex.InnerException, str, ++level);
            }
        }
    }
}
