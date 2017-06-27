using System;
using System.Windows;
using DRectangle = System.Drawing.Rectangle;

namespace ClipMirror.Single
{
    public static class Convert
    {
        public static Int32Rect ToInt32Rect(this DRectangle r)
        {
            return new Int32Rect(r.X, r.Y, r.Width, r.Height);
        }

        public static DRectangle ToRectangle(this Int32Rect r)
        {
            return new DRectangle(r.X, r.Y, r.Width, r.Height);
        }
    }
}
