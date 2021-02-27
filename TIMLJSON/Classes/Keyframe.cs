using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TIMLJSON.Classes
{
    public class FloatObj
    {
        public float value { get; set; }
        public FloatObj(float val)
        {
            value = val;
        }
    }
    public class IntObj
    {
        public int value { get; set; }
        public IntObj(int val)
        {
            value = val;
        }
    }
    public class Color
    {
        public byte red { get; set; }
        public byte green { get; set; }
        public byte blue { get; set; }
        public byte alpha { get; set; }

        public Color(byte r, byte g, byte b, byte a)
        {
            red = r;
            green = g;
            blue = b;
            alpha = a;
        }
    }
    [KeyframeConverter.JsonInterfaceConverter(typeof(KeyframeConverter))]
    public interface Keyframe
    {
        public int keyframeType { get; set; }
        public object MainData { get; set; }
        public float bounceForwardLimit { get; set; }
        public float bounceBackLimit { get; set; }
        public float frameTime { get; set; }
        public short easingMethod { get; set; }
        public short interpolationType { get; set; }
        public void Export(BinaryWriter bw);
        public int Length()
        {
            return 20;
        }

    }
    public class FloatKey : Keyframe 
    {
        //its apparently possible for this to be a different dataType, but we'll get to that when we get to that
        public int keyframeType { get => 2; set { } }
        public FloatObj MainData { get; set; }
        public float bounceForwardLimit { get; set; }
        public float bounceBackLimit { get; set; }
        public float frameTime { get; set; }
        public short easingMethod { get; set; }
        public short interpolationType { get; set; }//should pretty much always be 2
        object Keyframe.MainData { get => MainData; set => MainData = (FloatObj)value; }

        public FloatKey(BinaryReader br)
        {
            MainData = new FloatObj(br.ReadSingle());
            bounceForwardLimit = br.ReadSingle();
            bounceBackLimit = br.ReadSingle();
            frameTime = br.ReadSingle();
            easingMethod = br.ReadInt16();
            interpolationType = br.ReadInt16();
        }
        public FloatKey()
        {
            MainData = new FloatObj(0);
            bounceForwardLimit = 0;
            bounceBackLimit = 0;
            frameTime = 0;
            easingMethod = 3;
            interpolationType = 2;
        }
        public void Export(BinaryWriter bw)
        {
            bw.Write(MainData.value);
            bw.Write(bounceForwardLimit);
            bw.Write(bounceBackLimit);
            bw.Write(frameTime);
            bw.Write(easingMethod);
            bw.Write(interpolationType);
        }
    }

    public class IntKey : Keyframe
    {
        public int keyframeType { get => 1; set { } }
        public IntObj MainData { get; set; }
        public float bounceForwardLimit { get; set; }
        public float bounceBackLimit { get; set; }
        public float frameTime { get; set; }
        public short easingMethod { get; set; }
        public short interpolationType { get; set; }//should pretty much always be 2
        object Keyframe.MainData { get => MainData; set => MainData = (IntObj)value; }

        public IntKey(BinaryReader br)
        {
            MainData = new IntObj(br.ReadInt32());
            bounceForwardLimit = br.ReadSingle();
            bounceBackLimit = br.ReadSingle();
            frameTime = br.ReadSingle();
            easingMethod = br.ReadInt16();
            interpolationType = br.ReadInt16();
        }
        public IntKey()
        {
            MainData = new IntObj(0);
            bounceForwardLimit = 0;
            bounceBackLimit = 0;
            frameTime = 0;
            easingMethod = 3;
            interpolationType = 2;
        }
        public void Export(BinaryWriter bw)
        {
            bw.Write(MainData.value);
            bw.Write(bounceForwardLimit);
            bw.Write(bounceBackLimit);
            bw.Write(frameTime);
            bw.Write(easingMethod);
            bw.Write(interpolationType);
        }
    }
    public class ColorKey : Keyframe
    {
        public int keyframeType { get => 3; set { } }
        public Color MainData;
        public float bounceForwardLimit { get; set; }
        public float bounceBackLimit { get; set; }
        public float frameTime { get; set; }
        public short easingMethod { get; set; }
        public short interpolationType { get; set; }//should pretty much always be 2
        object Keyframe.MainData { get => MainData; set => MainData = (Color)value; }

        public ColorKey(BinaryReader br)
        {
            MainData = new Color(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
            bounceForwardLimit = br.ReadSingle();
            bounceBackLimit = br.ReadSingle();
            frameTime = br.ReadSingle();
            easingMethod = br.ReadInt16();
            interpolationType = br.ReadInt16();
        }
        public ColorKey()
        {
            MainData = new Color(255,255,255,255);
            bounceForwardLimit = 0;
            bounceBackLimit = 0;
            frameTime = 0;
            easingMethod = 3;
            interpolationType = 2;
        }
        public void Export(BinaryWriter bw)
        {
            bw.Write(MainData.red);
            bw.Write(MainData.green);
            bw.Write(MainData.blue);
            bw.Write(MainData.alpha);
            bw.Write(bounceForwardLimit);
            bw.Write(bounceBackLimit);
            bw.Write(frameTime);
            bw.Write(easingMethod);
            bw.Write(interpolationType);
        }
    }
}
