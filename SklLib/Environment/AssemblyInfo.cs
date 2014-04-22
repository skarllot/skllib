// AssemblyInfo.cs
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
using System.Reflection;

namespace SklLib.Environment
{
    /// <summary>
    /// Provides assembly information from running entrypoint.
    /// </summary>
    public static class AssemblyInfo
    {
        #region Fields

        static readonly string company;
        static readonly string version;
        static readonly string product;
        static readonly string copyright;
        static readonly string title;
        static readonly string description;

        #endregion

        #region Constructors

        static AssemblyInfo()
        {
            Assembly currentAssembly = Assembly.GetEntryAssembly();
            if (currentAssembly == null)
                return;

            AssemblyCompanyAttribute aCompany = GetCustomAttribute<AssemblyCompanyAttribute>(currentAssembly);
            if (aCompany != null)
                company = aCompany.Company;
            AssemblyVersionAttribute aVersion = GetCustomAttribute<AssemblyVersionAttribute>(currentAssembly);
            if (aVersion != null)
                version = aVersion.Version;
            AssemblyProductAttribute aProduct = GetCustomAttribute<AssemblyProductAttribute>(currentAssembly);
            if (aProduct != null)
                product = aProduct.Product;
            AssemblyCopyrightAttribute aCopyright = GetCustomAttribute<AssemblyCopyrightAttribute>(currentAssembly);
            if (aCopyright != null)
                copyright = aCopyright.Copyright;
            AssemblyTitleAttribute aTitle = GetCustomAttribute<AssemblyTitleAttribute>(currentAssembly);
            if (aTitle != null)
                title = aTitle.Title;
            AssemblyDescriptionAttribute aDescription = GetCustomAttribute<AssemblyDescriptionAttribute>(currentAssembly);
            if (aDescription != null)
                description = aDescription.Description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the company property from assembly information.
        /// </summary>
        public static string Company { get { return company; } }
        /// <summary>
        /// Gets the version property from assembly information.
        /// </summary>
        public static string Version { get { return version; } }
        /// <summary>
        /// Gets the product property from assembly information.
        /// </summary>
        public static string Product { get { return product; } }
        /// <summary>
        /// Gets the copyright property from assembly information.
        /// </summary>
        public static string Copyright { get { return copyright; } }
        /// <summary>
        /// Gets the title property from assembly information.
        /// </summary>
        public static string Title { get { return title; } }
        /// <summary>
        /// Gets the description property from assembly information.
        /// </summary>
        public static string Description { get { return description; } }

        #endregion

        #region Methods

        static T GetCustomAttribute<T>(Assembly assembly) where T : class
        {
            object[] cAttrib = assembly.GetCustomAttributes(typeof(T), false);
            if (cAttrib == null || cAttrib.Length == 0)
                return null;

            return cAttrib[0] as T;
        }

        #endregion
    }
}
