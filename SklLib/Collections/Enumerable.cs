// Enumerable.cs
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

namespace SklLib.Collections
{
    /// <summary>
    /// Provides methods to extend <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/> interface.
    /// </summary>
    public static class Enumerable
    {
        /// <summary>
        /// Converts the elements in the current <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>
        /// to another type, and returns a list containing the converted elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/> to convert.</param>
        /// <param name="converter">
        /// A <see cref="System.Converter&lt;T1,T2&gt;"/> delegate that converts each element from one type to another type.
        /// </param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/> of the target type containing the converted
        /// elements from the current <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">source is null</exception>
        /// <exception cref="ArgumentNullException">converter is null</exception>
        public static IEnumerable<TResult> ConvertAll<TSource, TResult>(
            this IEnumerable<TSource> source,
            System.Converter<TSource, TResult> converter)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (converter == null)
                throw new ArgumentNullException("converter");

            List<TResult> result = new List<TResult>(source.Count());
            foreach (TSource item in source) {
                result.Add(converter(item));
            }

            return result;
        }

        /// <summary>
        /// Performs the specified action on each element of the <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/> to iterate over.</param>
        /// <param name="action">
        /// The <see cref="System.Action&lt;T&gt;"/> delegate to perform on each element of the
        /// <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>.
        /// </param>
        /// <returns>The reference of especified source.</returns>
        /// <exception cref="ArgumentNullException">source is null</exception>
        /// <exception cref="ArgumentNullException">action is null</exception>
        public static IEnumerable<TSource> ForEach<TSource>(
            this IEnumerable<TSource> source,
            Action<TSource> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            foreach (TSource item in source) {
                action(item);
            }

            return source;
        }

        /// <summary>
        /// Performs the specified action on each element of the <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/> to iterate over.</param>
        /// <param name="action">
        /// The <see cref="System.Action&lt;T&gt;"/> delegate to perform on each element of the
        /// <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>.
        /// </param>
        /// <returns>The reference of especified source.</returns>
        /// <exception cref="ArgumentNullException">source is null</exception>
        /// <exception cref="ArgumentNullException">action is null</exception>
        public static IEnumerable<TSource> ForEach<TSource>(
            this IEnumerable<TSource> source,
            Action<TSource, int> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            int index = 0;
            foreach (TSource item in source) {
                action(item, index);
                index++;
            }

            return source;
        }
    }
}
