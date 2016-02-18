using System;

namespace MyvarCraft.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Hook : Attribute
    {
        public string PacketName;
        public int State;

        public Hook(string aPacketName, int aState)
        {
            this.PacketName = aPacketName;
            this.State = aState;
        }
    }

   
}
