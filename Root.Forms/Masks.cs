// Masks.cs
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
using System.Windows.Forms;

namespace SklLib.Forms
{
	/// <summary>
	/// Provides input restrictions to TextBoxBases and ComboBoxes.
	/// </summary>
	public class Masks
	{
		#region Fields

		SklLib.IMaskeable _type;
		string _mask;
		Strings.IndexedChar[] _autoChars;
		System.Text.StringBuilder _tempMask;
		System.Text.StringBuilder _base;
		UserInputBasedControl _uibCtrl;
		bool _isBack;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance and defines type of restriction.
		/// </summary>
		/// <param name="type">Type of restriction.</param>
		public Masks(SklLib.IMaskeable type)
		{
			if (type == null)
				throw new ArgumentException(resExceptions.ArgumentNull.Replace("%var", "type"));
			if (!type.IsReadOnly)
				throw new ArgumentException(resExceptions.MustReadOnlyArg.Replace("%var", "type"));

			_type = type;
			_mask = type.Mask.Replace("!", "");
			_tempMask = new System.Text.StringBuilder(_mask.Length);
			_base = new System.Text.StringBuilder(_mask.Length);

			_autoChars = Strings.AdditionalChars(type.Mask, _mask);

			for (int i = 0; i < _autoChars.Length; i++)
			{
				_autoChars[i].Character = type.Mask[_autoChars[i].Position + 1];
				_autoChars[i].Position -= i;
			}
			_uibCtrl = new UserInputBasedControl();
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the input focus leaves the control and the text is valid.
		/// </summary>
		public event EventHandler ValidLeave;

		/// <summary>
		/// Occurs when the input focus leaves the control and the text not is valid.
		/// </summary>
		public event EventHandler InvalidLeave;

		#endregion

		#region Public Methods

		/// <summary>
		/// Applies input restriction for the specified TextBoxBase.
		/// </summary>
		/// <param name="txtControl">A TextBoxBase control.</param>
		/// <returns>True whether txtControl not is null; otherwise false.</returns>
		public bool ApplyMask(TextBoxBase txtControl)
		{
			if (txtControl == null)
				return false;

			AddEvents(txtControl);
			return true;
		}

		/// <summary>
		/// Applies input restriction for the specified ComboBox.
		/// </summary>
		/// <param name="cmbControl">A ComboBox control</param>
		/// <returns>True whether cmbControl not is null; otherwise false.</returns>
		public bool ApplyMask(ComboBox cmbControl)
		{
			if (cmbControl == null)
				return false;

			AddEvents(cmbControl);
			return true;
		}

		/// <summary>
		/// Applies input restrictions for the specified Control or for its child controls.
		/// </summary>
		/// <param name="ctrl">A Control.</param>
		/// <returns>
		/// <para>True whether ctrl is a supported control; otherwise false.</para>
		/// <para>If the ctrl has child controls a true value means that all its child controls are valid.</para>
		/// </returns>
		public bool ApplyMask(Control ctrl)
		{
			bool ret = true;
			if (ctrl.HasChildren)
			{
				foreach (Control ct in ctrl.Controls)
					if (!this.ApplyMask(ct))
						ret = false;
				return ret;
			}
			else if (ctrl is TextBoxBase)
				return this.ApplyMask((TextBoxBase)ctrl);
			else if (ctrl is ComboBox)
				return this.ApplyMask((ComboBox)ctrl);
			else
				return false;
		}

		#endregion

		#region Private Methods

		private void AddEvents(Control ctrl)
		{
			ctrl.KeyPress += new KeyPressEventHandler(ctrl_KeyPress);
			ctrl.TextChanged += new EventHandler(ctrl_TextChanged);
			ctrl.Leave += new EventHandler(ctrl_Leave);
			ctrl.Text = string.Empty;
		}

		private void ctrl_Leave(object sender, EventArgs e)
		{
			_uibCtrl.SetControl(sender);
			if (_type.IsMatch(_uibCtrl.Text))
			{
				if (ValidLeave != null)
					this.ValidLeave(sender, e);
			}
			else
			{
				if (InvalidLeave != null)
					this.InvalidLeave(sender, e);
			}
		}

		private void ctrl_KeyPress(object sender, KeyPressEventArgs e)
		{
			_uibCtrl.SetControl(sender);

			int txtSel = _uibCtrl.SelectionStart;

			_base.Remove(0, _base.Length);	// Clears string
			_base.Append(_uibCtrl.Text);		// Stores textbase text contents

			// Verifies the key pressed by user
			if (e.KeyChar == (char)Keys.Back && txtSel > 0)
			{ _base.Remove(txtSel - 1, 1); _isBack = true; }
			else if (char.IsLetterOrDigit(e.KeyChar) || char.IsSymbol(e.KeyChar))
			{ _base.Insert(txtSel, e.KeyChar); _isBack = false; }
			else
			{
				e.Handled = true;
				return;
			}

			// Removes all auto-chars stored
			for (int i = 0; i < _autoChars.Length; i++)
				_base.Replace(_autoChars[i].Character.ToString(), "");

			// Inserts all auto-chars
			for (int i = 0; i < _autoChars.Length; i++)
			{
				if (_autoChars[i].Position == txtSel)
					txtSel++;
				if (_base.Length >= _autoChars[i].Position)
					_base.Insert(_autoChars[i].Position, _autoChars[i].Character.ToString());
				else
					break;
			}
			
			_tempMask.Remove(0, _tempMask.Length);	// Clears string
			_tempMask.Append(_mask);				// Stores _mask
			if (_mask.Length >= _base.Length)
				_tempMask.Remove(0, _base.Length);	// Keeps only that needs to match
			else
				_tempMask.Remove(0, _tempMask.Length);	// Clears string
			_tempMask.Insert(0, _base.ToString());	// Join the _base text with _tempMask

			// Verifies if is a valid text
			if (_type.IsMatch(_tempMask.ToString()))
			{
				_uibCtrl.Text = _base.ToString();

				if (_isBack)
					_uibCtrl.SelectionStart = txtSel - 1;
				else
				{
					txtSel++;
					for (int i = 0; i < _autoChars.Length; i++)
						if (_autoChars[i].Position == txtSel)
							txtSel++;
					_uibCtrl.SelectionStart = txtSel;
				}
			}
			
			e.Handled = true;	// Cancels the key pressed by user
		}

		private void ctrl_TextChanged(object sender, EventArgs e)
		{
			_uibCtrl.SetControl(sender);

			_tempMask.Remove(0, _tempMask.Length);	// Clears string
			_tempMask.Append(_mask);				// Stores _mask
			if (_mask.Length >= _uibCtrl.Text.Length)
				_tempMask.Remove(0, _uibCtrl.Text.Length);	// Keeps only that needs to match
			else
				_tempMask.Remove(0, _tempMask.Length);		//Clears string
			_tempMask.Insert(0, _uibCtrl.Text);				// Join the _base text with _tempMask

			if (!_type.IsMatch(_tempMask.ToString()))
			{
				int txtSel = _uibCtrl.SelectionStart;

				_base.Remove(0, _base.Length);	// Clears string
				_base.Append(_uibCtrl.Text);		// Stores textbase text contents

				// Removes all auto-chars stored
				for (int i = 0; i < _autoChars.Length; i++)
					_base.Replace(_autoChars[i].Character.ToString(), "");

				// Inserts all auto-chars
				for (int i = 0; i < _autoChars.Length; i++)
				{
					if (_base.Length >= _autoChars[i].Position)
						_base.Insert(_autoChars[i].Position, _autoChars[i].Character.ToString());
					else
						break;
				}

				_uibCtrl.Text = _base.ToString();

				for (int i = 0; i < _autoChars.Length; i++)
					if (_autoChars[i].Position <= txtSel)
						txtSel++;
				_uibCtrl.SelectionStart = txtSel;
			}
		}

		#endregion

		// Class to aid between control types, working in clean way
		private class UserInputBasedControl
		{
			#region Fields
			
			private TextBoxBase txt;
			private ComboBox cmb;
			private Control ctrl;
			
			#endregion

			#region Method
			
			public void SetControl(object obj)
			{
				if (!(obj is Control))
					throw new Exception("Invalid object");

				if (obj is TextBoxBase)
					this.txt = (TextBoxBase)obj;
				if (obj is ComboBox)
					this.cmb = (ComboBox)obj;
				this.ctrl = (Control)obj;
			}
			
			#endregion

			#region Properties
			
			public string Text
			{
				get { return ctrl.Text; }
				set { ctrl.Text = value; }
			}

			public int SelectionStart
			{
				get
				{
					if (txt != null)
						return txt.SelectionStart;
					else
						return cmb.SelectionStart;
				}
				set
				{
					if (txt != null)
						txt.SelectionStart = value;
					else
						cmb.SelectionStart = value;
				}
			}
			
			#endregion
			
		}
	}
}
