using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TIMLJSON.Classes
{
    public class ParamMember
    {
        public uint type { get; set; }
        public int keyframeType { get; set; }
        public List<Keyframe> keyframes { get; set; }

        //created constructor, probably useless but I'm probably gonna use this elsewhere
        ParamMember()
        {
            type = 0x8F198226;//mBaseMapFactorIntensity
            keyframeType = 2;
            keyframes = new List<Keyframe>();
            keyframes.Add(new FloatKey());

        }
        public ParamMember(BinaryReader br)
        {
            keyframes = new List<Keyframe>();
            long keyframeOff = br.ReadInt64();
            long keyframeCount = br.ReadInt64();
            type = br.ReadUInt32();
            keyframeType = br.ReadInt32();
            long returnAdd = br.BaseStream.Position;
            br.BaseStream.Seek(keyframeOff, SeekOrigin.Begin);
            for(int i = 0; i < keyframeCount; i++)
            {
                switch (keyframeType)
                {
                    case 0:
                        keyframes.Add(new IntKey(br));
                        break;
                    case 1:
                        keyframes.Add(new IntKey(br));
                        break;
                    case 2:
                        keyframes.Add(new FloatKey(br));
                        break;
                    case 3:
                        keyframes.Add(new ColorKey(br));
                        break;
                    default:
                        Console.WriteLine("New keyframe type: " + keyframeType.ToString());
                        keyframes.Add(new FloatKey(br));
                        break;
                }
            }
            br.BaseStream.Seek(returnAdd, SeekOrigin.Begin);
        }
        public int Length()
        {
            return 24;
        }
        public int KeyframeLength()
        {
            return keyframes.Count * 20;
        }
        public void Export(BinaryWriter bw, long keyframeOffset)
        {
            bw.Write(keyframeOffset);
            bw.Write((long)keyframes.Count);
            bw.Write(type);
            bw.Write(keyframeType);
            long returnAdd = bw.BaseStream.Position;
            bw.BaseStream.Seek(keyframeOffset, SeekOrigin.Begin);
            for(int currentKey = 0; currentKey < keyframes.Count; currentKey++)
            {
                keyframes[currentKey].Export(bw);
            }
            bw.BaseStream.Seek(returnAdd, SeekOrigin.Begin);
        }
    }
}
