// Simple.cs
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
using System.Security.Cryptography;

namespace Root.Security.Cryptography
{
    /// <summary>
    /// Provides methods to make more easily works with cryptography.
    /// </summary>
    public static class Simple
    {
        /// <summary>
        /// Encrypt a text using a TripleDES symmetric encryptor and MD5 hashed key.
        /// </summary>
        /// <param name="text">Specifies a text to encrypt.</param>
        /// <param name="key">Specifies a key to encrypt.</param>
        /// <returns>Encrypted text.</returns>
        /// <exception cref="ArgumentNullException"><c>text</c> or <c>key</c> is a null reference.</exception>
        public static string EncryptText(string text, string key)
        {
            if (text == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "text"));
            if (key == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "key"));

            TripleDESCryptoServiceProvider prov = ProcessProvider(key);
            ICryptoTransform transformer = prov.CreateEncryptor();
            byte[] buff = System.Text.Encoding.Unicode.GetBytes(text);
            return Convert.ToBase64String(transformer.TransformFinalBlock(buff, 0, buff.Length));
        }

        /// <summary>
        /// Decrypt a text using a TripleDES symmetric encryptor and MD5 hashed key.
        /// </summary>
        /// <param name="text">Specifies a text to decrypt.</param>
        /// <param name="key">Specifies a key to decrypt.</param>
        /// <returns>Decrypted text.</returns>
        /// <exception cref="ArgumentNullException"><c>text</c> or <c>key</c> is a null reference.</exception>
        public static string DecryptText(string text, string key)
        {
            if (text == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "text"));
            if (key == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "key"));

            TripleDESCryptoServiceProvider prov = ProcessProvider(key);
            ICryptoTransform transformer = prov.CreateDecryptor();
            byte[] buff = Convert.FromBase64String(text);
            return System.Text.Encoding.Unicode.GetString(transformer.TransformFinalBlock(buff, 0, buff.Length));
        }

        private static TripleDESCryptoServiceProvider ProcessProvider(string key)
        {
            TripleDESCryptoServiceProvider cripter = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

            cripter.Key = hasher.ComputeHash(System.Text.Encoding.Unicode.GetBytes(key));
            cripter.Mode = CipherMode.ECB;

            return cripter;
        }

    }
}
