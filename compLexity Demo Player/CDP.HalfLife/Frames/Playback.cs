﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CDP.HalfLife.Frames
{
    public class Playback : Loading
    {
        public override byte Id
        {
            get { return (byte)FrameIds.Playback; }
        }
    }
}