using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TIMLJSON.Classes
{
    public class Time
    {
        //uint unkn0 { get; set; } TimelineParamCount
        public bool isDefined { get; set; }
        public uint unkn1 { get; set; }
        public uint unkn2 { get; set; }
        public float animationLength { get; set; }
        public float loopStart { get; set; }
        public uint loopValue { get; set; }
        public uint unknHash { get; set; }
        public List<TimelineParam> timelineParams { get; set; }
        public Time(BinaryReader br)
        {
            timelineParams = new List<TimelineParam>();
            long offset = br.ReadInt64();
            if (offset != 0)
            {
                isDefined = true;
                long returnAdd = br.BaseStream.Position;
                br.BaseStream.Seek(offset, SeekOrigin.Begin);
                long timelineParamOff = br.ReadInt64();
                long timelineParamCount = br.ReadInt64();
                unkn1 = br.ReadUInt32();
                unkn2 = br.ReadUInt32();
                animationLength = br.ReadSingle();
                loopStart = br.ReadSingle();
                loopValue = br.ReadUInt32();
                unknHash = br.ReadUInt32();
                br.BaseStream.Seek(timelineParamOff, SeekOrigin.Begin);
                for (int i = 0; i < timelineParamCount; i++)
                {
                    timelineParams.Add(new TimelineParam(br));
                }
                br.BaseStream.Seek(returnAdd, SeekOrigin.Begin);
            }
            else
            {
                isDefined = false;
                unkn1 = 0;
                unkn2 = 0;
                animationLength = 0;
                loopStart = 0;
                loopValue = 0;
                unknHash = 0;
            }
        }
        public Time()
        {
            timelineParams = new List<TimelineParam>();
            isDefined = false;
            unkn1 = 0;
            unkn2 = 0;
            animationLength = 0;
            loopStart = 0;
            loopValue = 0;
            unknHash = 0;
        }
        public void Export(BinaryWriter bw)
        {
            long offOff = bw.BaseStream.Position;
            bw.Write((long)0);
            bw.Write((long)timelineParams.Count);
            bw.Write(unkn1);
            bw.Write(unkn2);
            bw.Write(animationLength);
            bw.Write(loopStart);
            bw.Write(loopValue);
            bw.Write(unknHash);
            long paramOff = bw.BaseStream.Position;
            bw.BaseStream.Seek(offOff, SeekOrigin.Begin);
            bw.Write(paramOff);
            bw.BaseStream.Seek(paramOff, SeekOrigin.Begin);
            long runningTotal = paramOff;
            List<long> memberOffs = new List<long>();
            List<long> keyframeOffs = new List<long>();
            for(int i = 0; i < timelineParams.Count; i++)
            {
                runningTotal += timelineParams[i].Length();
                memberOffs.Add(runningTotal);
            }
            for(int i = 0; i < timelineParams.Count; i++)
            {
                timelineParams[i].Export(bw,memberOffs[i]);
                for (int j = 0; j < timelineParams[i].paramMembers.Count; j++)
                {
                    runningTotal += timelineParams[i].paramMembers[j].Length();
                }
            }
            for(int i = 0; i < timelineParams.Count; i++)
            {
                for(int j = 0; j < timelineParams[i].paramMembers.Count; j++)
                {
                    keyframeOffs.Add(runningTotal);
                    runningTotal += timelineParams[i].paramMembers[j].KeyframeLength();
                }
            }
            int currentIndex = 0;
            for (int k = 0; k < timelineParams.Count; k++)
            {
                for(int l = 0; l < timelineParams[k].paramMembers.Count; l++)
                {
                    timelineParams[k].paramMembers[l].Export(bw, keyframeOffs[currentIndex]);
                    currentIndex++;
                }
            }
            bw.BaseStream.Seek(0, SeekOrigin.End);

        }
    }
}
