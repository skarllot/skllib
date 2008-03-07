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
