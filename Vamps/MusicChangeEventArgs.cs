using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagement
{
  public class MusicChangeEventArgs : EventArgs
    {
        string musicName;
        public string MusicName
        {
            get { return musicName; }
            private set { musicName = value; }
        }

        public MusicChangeEventArgs(string musicName)
        {
            MusicName = musicName;
        }
    }
}
