using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TIMLJSON.Classes
{
    public class TIML
    {
        public string magic { get => "timl"; set { } }
        public ulong hash { get => 0x1802080018020800; set { } } //probably not an actual hash, but we have no clue what it is
        public uint NULL0 { get => 0; set { } }
        public ulong Version { get => 32; set { } }
        public List<Time> times { get; set; }

        public TIML(BinaryReader br)
        {
            times = new List<Time>();
            byte[] nMagic = br.ReadBytes(4);
            if (br.ReadUInt64() != hash)
            {
                Console.WriteLine("Hash mismatch.");
                return;
            };
            br.ReadUInt32();
            if(br.ReadUInt64() != Version)
            {
                Console.WriteLine("Version mismatch.");
                return;
            }
            long timeCount = br.ReadInt64();
            for(int i = 0; i < timeCount; i++)
            {
                times.Add(new Time(br));
            }
        }
        public TIML()
        {
            times = new List<Time>();
        }
        public void Export(BinaryWriter bw)
        {
            bw.Write(magic.ToCharArray());
            bw.Write(hash);
            bw.Write(NULL0);
            bw.Write(Version);
            bw.Write((long)times.Count);
            //now for the bleh parts
            List<long> timeOffsets = new List<long>();
            for (int i = 0; i < times.Count; i++)
            {
                timeOffsets.Add(bw.BaseStream.Position);
                bw.Write((long)0);
            }
            for (int i = 0; i < times.Count; i++)
            {
                if (times[i].isDefined)
                {
                    long timeOff = bw.BaseStream.Position;
                    bw.BaseStream.Seek(timeOffsets[i], SeekOrigin.Begin);
                    bw.Write(timeOff);
                    bw.BaseStream.Seek(timeOff, SeekOrigin.Begin);
                    times[i].Export(bw);
                }
            }
        }
    }
}
