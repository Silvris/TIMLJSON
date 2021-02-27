using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TIMLJSON.Classes
{
    public class TimelineParam
    {
        public long timelineParamHash { get; set; }
        public List<ParamMember> paramMembers { get; set; }
        public TimelineParam(BinaryReader br)
        {
            paramMembers = new List<ParamMember>();
            long paramMemberOff = br.ReadInt64();
            long paramMemberCount = br.ReadInt64();
            timelineParamHash = br.ReadInt64();
            long returnAdd = br.BaseStream.Position;
            br.BaseStream.Seek(paramMemberOff, SeekOrigin.Begin);
            for(int i = 0; i < paramMemberCount; i++)
            {
                paramMembers.Add(new ParamMember(br));
            }
            br.BaseStream.Seek(returnAdd, SeekOrigin.Begin);
        }
        public TimelineParam()
        {
            paramMembers = new List<ParamMember>();
            timelineParamHash = 0x3AC1EACA;//UberMaterial
        }
        public int Length()
        {
            return 24;
        }
        public void Export(BinaryWriter bw, long memberOffset)
        {
            bw.Write((long)memberOffset);
            bw.Write((long)paramMembers.Count);
            bw.Write(timelineParamHash);
        }
    }
}
