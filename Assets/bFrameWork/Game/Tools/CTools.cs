using System;

namespace bFrame.Game.Tools
{
    public static class CTools
    {
        public static int TickCount()
        {
            int tick = Environment.TickCount;//毫秒
            if (tick < 0)
            {
                tick += Int32.MaxValue;
            }

            return tick;
        }
    }
}