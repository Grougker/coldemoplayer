﻿using System;
using System.IO;

namespace CDP.HalfLife.Messages
{
    public class SvcSignOnNum : EngineMessage
    {
        public override byte Id
        {
            get { return (byte)EngineMessageIds.svc_signonnum; }
        }

        public override string Name
        {
            get { return "svc_signonnum"; }
        }

        public override bool CanSkipWhenWriting
        {
            get { return true; }
        }

        public byte Number { get; set; }

        public override void Skip(BitReader buffer)
        {
            buffer.SeekBytes(1);
        }

        public override void Read(BitReader buffer)
        {
            Number = buffer.ReadByte(); // Always 1.
        }

        public override void Write(BitWriter buffer)
        {
            buffer.WriteByte(Number);
        }

        public override void Log(StreamWriter log)
        {
            log.WriteLine("Number: {0}", Number);
        }
    }
}