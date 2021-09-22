using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hmmm
{
    public partial class Form1 : Form
    {
        private readonly GlobalHotkey _ghk;


        public Form1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;

            //must be after show in taskbar is set false - as this changes handle
            _ghk = new GlobalHotkey(Program.Constants.NOMOD, Keys.Delete, this);

            this.Load += (o, e) =>
            {
                var reg = _ghk.Register();

                if (!reg)
                    System.Diagnostics.Debug.WriteLine("Failed to register");

                this.Size = Size.Empty;
            };

            this.Closing += (o, e) =>
            {
                _ghk.Unregiser();
            };
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Program.Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }

        private int _keyCounter = 0;
        private int cooldownMultiplier = 1;
        private int cooldown = 5;//seconds 
        private int playCount = 0;
        private DateTime _lastRun;
        private void HandleHotkey()
        {
            _keyCounter++;

            var stamp = DateTime.Now;

            if (_keyCounter > 10 && Math.Abs((_lastRun - stamp).Seconds) > (cooldown * cooldownMultiplier))
            {
                //play sound;
                System.Diagnostics.Debug.WriteLine("Hotkey pressed");
                var audio = new SoundPlayer(Resource2.ResourceManager.GetStream("Meow"));
                audio.Play();

                _lastRun = stamp;
                playCount++;

                if (playCount > 7)
                    cooldownMultiplier = 60;//no more than once every 5 minutes
            }

            //takes longer between keypress for less annoyingness
            //if (_keyCounter > 10)
            //    if (_keyCounter % 30 == 0) cooldownMultiplier++;

        }

    }
}
