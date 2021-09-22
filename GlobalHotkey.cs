﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Hmmm
{

    internal class GlobalHotkey
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private readonly int _modifier;
        private readonly int _key;
        private readonly IntPtr _hWnd;
        private readonly int _id;

        public GlobalHotkey(int modifier, Keys key, Form form)
        {
            this._modifier = modifier;
            this._key = (int)key;
            this._hWnd = form.Handle;
            _id = this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _modifier ^ _key ^ _hWnd.ToInt32();
        }

        public bool Register()
        {
            return RegisterHotKey(_hWnd, _id, _modifier, _key);
        }

        public bool Unregiser()
        {
            return UnregisterHotKey(_hWnd, _id);
        }
    }
}
