using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Edison_Win
{
    public partial class MixImage
    {
        //public struct bmpfileheader
        //{
        //    public ushort filetype;
        //    public int filesize;
        //    public ushort reserved1;
        //    public ushort reserved2;
        //    public uint bitmapoffset;
        //};
        //public struct bitmapheader
        //{
        //    public uint size;
        //    public int width;
        //    public int height;
        //    public ushort planes;
        //    public ushort bitsperpixel;
        //    public uint compression;
        //    public int sizeofbitmap;
        //    public int horzres;
        //    public int vertres;
        //    public uint colorsused;
        //    public uint colorsimp;
        //};
        public struct Pixels
        {
            public int blue;
            public int green;
            public int red;
        }

        public short[,] g7 = new short[7, 7] {
    { 0, 0, -1, -1, -1, 0, 0 },
    { 0, -2, -3, -3, -3, -2, 0 },
    { -1, -3, 5, 5, 5, -3, -1 },
    { -1, -3, 5, 16, 5, -3, -1 },
    { -1, -3, 5, 5, 5, -3, -1 },
    { 0, -2, -3, -3, -3, -2, 0 },
    { 0, 0, -1, -1, -1, 0, 0 } };
        public short[,] g9 = new short[9, 9] {
    { 0, 0, 0, -1, -1, -1, 0, 0, 0 },
    { 0, -2, -3, -3, -3, -3, -3, -2, 0 },
    { 0, -3, -2, -1, -1, -1, -2, -3, 0 },
    { -1, -3, -1, 9, 9, 9, -1, -3, -1 },
    { -1, -3, -1, 9, 19, 9, -1, -3, -1 },
    { -1, -3, -1, 9, 9, 9, -1, -3, -1 },
    { 0, -3, -2, -1, -1, -1, -2, -3, 0 },
    { 0, -2, -3, -3, -3, -3, -3, -2, 0 },
    { 0, 0, 0, -1, -1, -1, 0, 0, 0 } };
        public short[,] e_mask = new short[3, 3] {
    { -9, 0, -9 },
    { 0, 36, 0 },
    { -9, 0, -9 } };
        public short[,] contrast = new short[3, 3] {
    { 1, 1, 1 },
    { 1, 1, 1 },
    { 1, 1, 1 } };
        public short[,] enhance_mask = new short[3, 3] {
    { -1, 0, -1 },
    { 0, 4, 0 },
    { -1, 0, -1 } };
        public short[,] quick_mask = new short[3, 3] {
    { -1, 0, -1 },
    { 0, 4, 0 },
    { -1, 0, -1 } };
        public short[,] kirsch_mask_0 = new short[3, 3] {
    { 5, 5, 5 },
    { -3, 0, -3 },
    { -3, -3, -3 }
};
        public short[,] kirsch_mask_1 = new short[3, 3] {
    { -3, 5, 5 },
    { -3, 0, 5 },
    { -3, -3, -3 } };
        public short[,] kirsch_mask_2 = new short[3, 3] {
    { -3, -3, 5 },
    { -3, 0, 5 },
    { -3, -3, 5 } };
        public short[,] kirsch_mask_3 = new short[3, 3] {
    { -3, -3, -3 },
    { -3, 0, 5 },
    { -3, 5, 5 } };
        public short[,] kirsch_mask_4 = new short[3, 3] {
    { -3, -3, -3 },
    { -3, 0, -3 },
    { 5, 5, 5 } };
        public short[,] kirsch_mask_5 = new short[3, 3] {
    { -3, -3, -3 },
    { 5, 0, -3 },
    { 5, 5, -3 } };
        public short[,] kirsch_mask_6 = new short[3, 3] {
    { 5, -3, -3 },
    { 5, 0, -3 },
    { 5, -3, -3 } };
        public short[,] kirsch_mask_7 = new short[3, 3] {
    { 5, 5, -3 },
    { 5, 0, -3 },
    { -3, -3, -3 } };
        public short[,] prewitt_mask_0 = new short[3, 3] {
    { 1, 1, 1 },
    { 1, -2, 1 },
    { -1, -1, -1 } };
        public short[,] prewitt_mask_1 = new short[3, 3] {
    { 1, 1, 1 },
    { 1, -2, -1 },
    { 1, -1, -1 } };
        public short[,] prewitt_mask_2 = new short[3, 3] {
    { 1, 1, -1 },
    { 1, -2, -1 },
    { 1, 1, -1 } };
        public short[,] prewitt_mask_3 = new short[3, 3] {
    { 1, -1, -1 },
    { 1, -2, -1 },
    { 1, 1, 1 } };
        public short[,] prewitt_mask_4 = new short[3, 3] {
    { -1, -1, -1 },
    { 1, -2, 1 },
    { 1, 1, 1 } };
        public short[,] prewitt_mask_5 = new short[3, 3] {
    { -1, -1, 1 },
    { -1, -2, 1 },
    { 1, 1, 1 } };
        public short[,] prewitt_mask_6 = new short[3, 3] {
    { -1, 1, 1 },
    { -1, -2, 1 },
    { -1, 1, 1 } };
        public short[,] prewitt_mask_7 = new short[3, 3] {
    { 1, 1, 1 },
    { -1, -2, 1 },
    { -1, -1, 1 } };
        public short[,] sobel_mask_0 = new short[3, 3] {
    { 1, 2, 1 },
    { 0, 0, 0 },
    { -1, -2, -1 } };
        public short[,] sobel_mask_1 = new short[3, 3] {
    { 2, 1, 0 },
    { 1, 0, -1 },
    { 0, -1, -2 } };
        public short[,] sobel_mask_2 = new short[3, 3] {
    { 1, 0, -1 },
    { 2, 0, -2 },
    { 1, 0, -1 } };
        public short[,] sobel_mask_3 = new short[3, 3] {
    { 0, -1, -2 },
    { 1, 0, -1 },
    { 2, 1, 0 } };
        public short[,] sobel_mask_4 = new short[3, 3] {
    { -1, -2, -1 },
    { 0, 0, 0 },
    { 1, 2, 1 } };
        public short[,] sobel_mask_5 = new short[3, 3] {
    { -2, -1, 0 },
    { -1, 0, 1 },
    { 0, 1, 2 } };
        public short[,] sobel_mask_6 = new short[3, 3] {
    { -1, 0, 1 },
    { -2, 0, 2 },
    { -1, 0, 1 } };
        public short[,] sobel_mask_7 = new short[3, 3] {
    { 0, 1, 2 },
    { -1, 0, 1 },
    { -2, -1, 0 } };


    }
}
