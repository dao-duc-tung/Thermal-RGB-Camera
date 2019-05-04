using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Edison_Win
{
    public partial class MixImage
    {

        static double M_PI = 3.14159265358979323846;


        public static Bitmap MixBothImage()
        {
            int i = 0, j = 0;
            /************Tao mot mang IMG 60 * 80***************/
            //ushort[,] img_arr_pmg = new ushort[60, 80];

            /***********Chuyen Mang du lieu doc duoc thanh mang anh 256 cap xam***************/
            ushort minval = ImageProcessing.MinVal;
            byte[,] raw_to_bmp = new byte[60, 80];
            double scale = 255.0 / ImageProcessing.SubVal;
            for (i = 0; i < 60; i++)
            {
                for (j = 0; j < 80; j++)
                {
                    //int temp = img_arr_pmg[i, j] - minval;
                    //raw_to_bmp[60 - i - 1, j] = (byte)((img_arr_pmg[i, j] - minval) * scale);
                    raw_to_bmp[60 - i - 1, 80 - j - 1] = (byte)((ImageProcessing.imgFLIR16[i * 80 + j] - minval) * scale);
                }
            }

            /*********Tao mang 240 x 320 image scaling *********************/
            byte[,] scaleimg = new byte[240, 320];
            scaleimg = zoom_in_with_cubic(raw_to_bmp, 80, 60, 320, 240);

            /****************Tao mang 240 x 320 image rotating******************/
            //byte[,] rotateimg = new byte[240, 320];
            //rotateImg(scaleimg, rotateimg, -90);

            /*********Anh RGB******************/
            Pixels[,] img_arr_rgb = ImageProcessing.UndistortedJPGToPixels();//mang anh rgb input
            Pixels[,] img_cutted = new Pixels[180, 252];
            for (i = 0; i < 180; i++)
            {
                for (j = 0; j < 252; j++)
                {
                    img_cutted[i, j] = img_arr_rgb[i + 17, j + 36];
                }
            }
            Pixels[,] img_zooming_rgb = new Pixels[240, 320];
            img_zooming_rgb = zoom_in_with_cubic_rgb(img_cutted, 252, 180, 320, 240);
            //average_overlay_rgb(img_zooming_rgb, ImageProcessing.rgb, rotateimg);
            average_overlay_rgb(img_zooming_rgb, ImageProcessing.rgb, scaleimg);

            int n = 0;
            byte[] arrImg = new byte[240 * 320 * 3];
            for (i = 239; i >=0; i--)
            {
                for (j = 319; j >= 0; j--)
                {
                    arrImg[n] = (byte)img_zooming_rgb[i, j].blue;
                    arrImg[n + 1] = (byte)img_zooming_rgb[i, j].green;
                    arrImg[n + 2] = (byte)img_zooming_rgb[i, j].red;
                    n += 3;
                }
            }
            Bitmap bmpTemp = ImageProcessing.CreateBitmapFromBytes(arrImg, 320, 240);

            return bmpTemp;
        }


        static void make_color_table(Pixels[] rgb, long color)
        {
            for (int i = 0; i < 51; i++)
            {
                //white to yellow
                rgb[color - i - 1].red = 255;
                rgb[color - i - 1].green = 255 - i / 2;
                rgb[color - i - 1].blue = 255 - i * 5;
                //yellow to orange
                rgb[color - (i + 51)].red = 255 - i / 3;
                rgb[color - (i + 51)].green = 255 - i * 4;
                rgb[color - (i + 51)].blue = i / 3;
                ////orange to viloet
                rgb[color - (i + 102)].red = 255 - i * 2;
                rgb[color - (i + 102)].green = 55 - i;
                rgb[color - (i + 102)].blue = i * 3 / 2;
                ////violet to blue
                rgb[color - (i + 153)].red = 153 - i * 2;
                rgb[color - (i + 153)].green = 0;
                rgb[color - (i + 153)].blue = 55 + i;
                ////blue to black
                rgb[color - (i + 204)].red = 55 - i * 2 / 3;
                rgb[color - (i + 204)].green = 0;
                rgb[color - (i + 204)].blue = 100 - i * 2 / 3;
            }
            rgb[0].red = 0;
            rgb[0].green = 0;
            rgb[0].blue = 0;
        }
        static byte[,] zoom_in_with_cubic(byte[,] image, int width, int height, int new_width, int new_height)
        {
            int x = 0, y = 0, m = 0, n = 0;
            float dx, dy;
            float[] t = new float[4];
            dx = (float)width / (float)(new_width - 1);
            dy = (float)height / (float)(new_height - 1);
            byte[,] _zoomImg = new byte[new_height, new_width];

            for (y = 0; y < new_height; y++)
            {
                double iy = y * dy - 0.5;
                double v = iy - Math.Floor(iy);
                int oy = (int)iy;
                if (oy - 1 < 0)
                {
                    oy = 1;
                }
                if (((oy + 1) > (height - 1)) && ((oy + 2) > (height - 1)))
                {
                    oy = height - 4;
                }
                else if (((oy + 1) <= (height - 1)) && ((oy + 2) > (height - 1)))
                {
                    oy = height - 3;
                }
                for (x = 0; x < new_width; x++)
                {
                    double ix = x * dx - 0.5;
                    double s = ix - Math.Floor(ix);
                    int ox = (int)ix;
                    if (ox - 1 < 0)
                    {
                        ox = 1;
                    }
                    if ((ox + 1 > width - 1) && ((ox + 2) > (width - 1)))
                    {
                        ox = width - 4;
                    }
                    else if (((ox + 1) <= (width - 1)) && ((ox + 2) > (width - 1)))
                    {
                        ox = width - 3;
                    }
                    t[0] = CubicHermite(image[oy - 1, ox - 1], image[oy - 1, ox], image[oy - 1, ox + 1], image[oy - 1, ox + 2], (float)s);
                    t[1] = CubicHermite(image[oy, ox - 1], image[oy, ox], image[oy, ox + 1], image[oy, ox + 2], (float)s);
                    t[2] = CubicHermite(image[oy + 1, ox - 1], image[oy + 1, ox], image[oy + 1, ox + 1], image[oy + 1, ox + 2], (float)s);
                    t[3] = CubicHermite(image[oy + 2, ox - 1], image[oy + 2, ox], image[oy + 2, ox + 1], image[oy + 2, ox + 2], (float)s);
                    //
                    //t[0] = CubicHermite(image[ox - 1,oy - 1], image[ox,oy - 1], image[ox + 1,oy - 1], image[ox + 2,oy - 1], s);
                    //t[1] = CubicHermite(image[ox - 1,oy], image[ox,oy], image[ox + 1,oy], image[ox + 2,oy], s);
                    //t[2] = CubicHermite(image[ox - 1,oy + 1], image[ox,oy + 1], image[ox + 1,oy + 1], image[ox + 2,oy + 1], s);
                    //t[3] = CubicHermite(image[ox - 1,oy + 2], image[ox,oy + 2], image[ox + 1,oy + 2], image[ox + 2,oy + 2], s);
                    int value = (int)CubicHermite(t[0], t[1], t[2], t[3], (float)v);
                    _zoomImg[y, x] = (byte)clamp(value, 0, 255);
                }
            }
            return _zoomImg;
        }
        static Pixels[,] zoom_in_with_cubic_rgb(Pixels[,] image, int width, int height, int new_width, int new_height)
        {
            int x = 0, y = 0, m = 0, n = 0;
            float dx, dy;
            float[] t_blue = new float[4];
            float[] t_red = new float[4];
            float[] t_green = new float[4];
            dx = (float)width / (float)(new_width - 1);
            dy = (float)height / (float)(new_height - 1);
            Pixels[,] _zoomImg = new Pixels[new_height, new_width];

            for (y = 0; y < new_height; y++)
            {
                double iy = y * dy - 0.5;
                double v = iy - Math.Floor(iy);
                int oy = (int)iy;
                if (oy - 1 < 0)
                {
                    oy = 1;
                }
                if (((oy + 1) > (height - 1)) && ((oy + 2) > (height - 1)))
                {
                    oy = height - 4;
                }
                else if (((oy + 1) <= (height - 1)) && ((oy + 2) > (height - 1)))
                {
                    oy = height - 3;
                }
                for (x = 0; x < new_width; x++)
                {
                    double ix = x * dx - 0.5;
                    double s = ix - Math.Floor(ix);
                    int ox = (int)ix;
                    if (ox - 1 < 0)
                    {
                        ox = 1;
                    }
                    if ((ox + 1 > width - 1) && ((ox + 2) > (width - 1)))
                    {
                        ox = width - 4;
                    }
                    else if (((ox + 1) <= (width - 1)) && ((ox + 2) > (width - 1)))
                    {
                        ox = width - 3;
                    }
                    t_blue[0] = CubicHermite(image[oy - 1, ox - 1].blue, image[oy - 1, ox].blue, image[oy - 1, ox + 1].blue, image[oy - 1, ox + 2].blue, (float)s);
                    t_blue[1] = CubicHermite(image[oy, ox - 1].blue, image[oy, ox].blue, image[oy, ox + 1].blue, image[oy, ox + 2].blue, (float)s);
                    t_blue[2] = CubicHermite(image[oy + 1, ox - 1].blue, image[oy + 1, ox].blue, image[oy + 1, ox + 1].blue, image[oy + 1, ox + 2].blue, (float)s);
                    t_blue[3] = CubicHermite(image[oy + 2, ox - 1].blue, image[oy + 2, ox].blue, image[oy + 2, ox + 1].blue, image[oy + 2, ox + 2].blue, (float)s);
                    //
                    //
                    //t[0] = CubicHermite(image[ox - 1,oy - 1], image[ox,oy - 1], image[ox + 1,oy - 1], image[ox + 2,oy - 1], s);
                    //t[1] = CubicHermite(image[ox - 1,oy], image[ox,oy], image[ox + 1,oy], image[ox + 2,oy], s);
                    //t[2] = CubicHermite(image[ox - 1,oy + 1], image[ox,oy + 1], image[ox + 1,oy + 1], image[ox + 2,oy + 1], s);
                    //t[3] = CubicHermite(image[ox - 1,oy + 2], image[ox,oy + 2], image[ox + 1,oy + 2], image[ox + 2,oy + 2], s);
                    int valueblue = (int)CubicHermite(t_blue[0], t_blue[1], t_blue[2], t_blue[3], (float)v);
                    _zoomImg[y, x].blue = clamp(valueblue, 0, 255);

                    //red
                    t_red[0] = CubicHermite(image[oy - 1, ox - 1].red, image[oy - 1, ox].red, image[oy - 1, ox + 1].red, image[oy - 1, ox + 2].red, (float)s);
                    t_red[1] = CubicHermite(image[oy, ox - 1].red, image[oy, ox].red, image[oy, ox + 1].red, image[oy, ox + 2].red, (float)s);
                    t_red[2] = CubicHermite(image[oy + 1, ox - 1].red, image[oy + 1, ox].red, image[oy + 1, ox + 1].red, image[oy + 1, ox + 2].red, (float)s);
                    t_red[3] = CubicHermite(image[oy + 2, ox - 1].red, image[oy + 2, ox].red, image[oy + 2, ox + 1].red, image[oy + 2, ox + 2].red, (float)s);
                    int valuered = (int)CubicHermite(t_red[0], t_red[1], t_red[2], t_red[3], (float)v);
                    _zoomImg[y, x].red = clamp(valuered, 0, 255);

                    //green
                    t_green[0] = CubicHermite(image[oy - 1, ox - 1].green, image[oy - 1, ox].green, image[oy - 1, ox + 1].green, image[oy - 1, ox + 2].green, (float)s);
                    t_green[1] = CubicHermite(image[oy, ox - 1].green, image[oy, ox].green, image[oy, ox + 1].green, image[oy, ox + 2].green, (float)s);
                    t_green[2] = CubicHermite(image[oy + 1, ox - 1].green, image[oy + 1, ox].green, image[oy + 1, ox + 1].green, image[oy + 1, ox + 2].green, (float)s);
                    t_green[3] = CubicHermite(image[oy + 2, ox - 1].green, image[oy + 2, ox].green, image[oy + 2, ox + 1].green, image[oy + 2, ox + 2].green, (float)s);
                    int valuegreen = (int)CubicHermite(t_green[0], t_green[1], t_green[2], t_green[3], (float)v);
                    _zoomImg[y, x].green = clamp(valuegreen, 0, 255);
                }
            }
            return _zoomImg;
        }
        static float CubicHermite(float A, float B, float C, float D, float t)
        {
            float a = -A / 2 + 3 * B / 2 - 3 * C / 2 + D / 2;
            float b = A - 5 * B / 2 + 2 * C - D / 2;
            float c = -A / 2 + C / 2;
            float d = B;
            return a * t * t * t + b * t * t + c * t + d;
        }
        static int clamp(int v, int min, int max)
        {
            if (v < min)
                return min;
            else if (v > max)
                return max;
            return v;
        }
        static void convertRGB565(byte[,] img_in, ushort[,] img_out, Pixels[] rgb)
        {
            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 80; j++)
                {
                    img_out[i, j] = (ushort)(((rgb[img_in[i, j]].red >> 3) << 11) | ((rgb[img_in[i, j]].green >> 2) << 5) | (rgb[img_in[i, j]].blue >> 3));
                }
            }
        }
        static void rotateImg(byte[,] img_in, byte[,] img_out, double angle)
        {
            for (int i = 0; i < 240; i++)
            {
                for (int j = 0; j < 320; j++)
                {
                    int hwidth = 160;
                    int hheight = 120;
                    int yt = j - hwidth;
                    int xt = i - hheight;
                    double sinma = Math.Sin(-angle * M_PI / 180);
                    double cosma = Math.Cos(-angle * M_PI / 180);
                    int ys = (int)Math.Round((cosma * xt - sinma * yt) + hwidth);
                    int xs = (int)Math.Round((sinma * xt + cosma * yt) + hheight);
                    if (xs >= 0 && xs < 240 && ys >= 0 && ys < 320)
                    {
                        /* set target pixel (x,y) to color at (xs,ys) */
                        img_out[i, 320 - j - 1] = img_in[xs, ys];
                    }
                    else
                    {
                        /* set target pixel (x,y) to some default background */
                        img_out[i, 320 - j - 1] = 0;
                    }
                }
            }
        }
        static void average_overlay_rgb(Pixels[,] img_data_in, Pixels[] rgb, byte[,] img_data_out)
        {
            int i, j;
            for (i = 0; i < 240; i++)
            {
                for (j = 0; j < 320; j++)
                {
                    //if (img_data_out[i,j] < 90)
                    //{
                    //	img_data_in[240 - i - 1,320 - j - 1].red = 10;
                    //	img_data_in[240 - i - 1,320 - j - 1].blue = 10;
                    //	img_data_in[240 - i - 1,320 - j - 1].green = 10;
                    //}
                    if (img_data_out[i, j] > 79)
                    {
                        img_data_in[i, j].red = (img_data_in[i, j].red + rgb[img_data_out[i, j]].red) / 2;
                        img_data_in[i, j].blue = (img_data_in[i, j].blue + rgb[img_data_out[i, j]].blue) / 2;
                        img_data_in[i, j].green = (img_data_in[i, j].green + rgb[img_data_out[i, j]].green) / 2;
                    }
                }
            }
        }

        //void homogeneity(Pixels[,] the_image, Pixels** out_image, long rows, long cols, long bits_per_pixel, int threshold, int high)
        //{
        //    int a, b, absdiff, absmax, diff, i, j, length, max, max_diff, new_hi, new_low, width;
        //    new_hi = 250;
        //    new_low = 16;
        //    if (bits_per_pixel == 4)
        //    {
        //        new_hi = 10;
        //        new_low = 3;
        //    }
        //    max = 255;
        //    if (bits_per_pixel == 4)
        //        max = 16;
        //    for (i = 0; i < rows; i++)
        //    {
        //        for (j = 0; j < cols; j++)
        //        {
        //            out_image[i,j].blue = 0;
        //            out_image[i,j].red = 0;
        //            out_image[i,j].green = 0;
        //        }
        //    }
        //    for (i = 1; i < rows - 1; i++)
        //    {
        //        if ((i % 10) == 0)
        //        {
        //        }
        //        for (j = 1; j < cols - 1; j++)
        //        {
        //            max_diff = 0;
        //            for (a = -1; a <= 1; a++)
        //            {
        //                for (b = -1; b <= 1; b++)
        //                {
        //                    diff = the_image[i,j].blue - the_image[i + a,j + b].blue;
        //                    absdiff = Math.Abs(diff);
        //                    if (absdiff > max_diff)
        //                        max_diff = absdiff;
        //                }
        //            }
        //            out_image[i,j].blue = max_diff;
        //            out_image[i,j].green = max_diff;
        //            out_image[i,j].red = max_diff;
        //        }
        //    }
        //}
        //void difference_edge(Pixels** the_image, Pixels** out_image, long rows, long cols, long bits_per_pixel, int threshold, int high)
        //{
        //    int a, b, absdiff, absmax, diff, i, j,
        //        length, max, max_diff, new_hi, new_low, width;
        //    new_hi = 250;
        //    new_low = 16;
        //    if (bits_per_pixel == 4)
        //    {
        //        new_hi = 10;
        //        new_low = 3;
        //    }
        //    max = 255;
        //    if (bits_per_pixel == 4)
        //        max = 16;
        //    for (i = 0; i < rows; i++)
        //        for (j = 0; j < cols; j++)
        //        {
        //            out_image[i,j].blue = 0;
        //            out_image[i,j].red = 0;
        //            out_image[i,j].green = 0;
        //        }
        //    for (i = 1; i < rows - 1; i++)
        //    {
        //        for (j = 1; j < cols - 1; j++)
        //        {
        //            max_diff = 0;
        //            absdiff = Math.Abs(the_image[i - 1,j - 1].blue - the_image[i + 1,j + 1].blue);
        //            if (absdiff > max_diff) max_diff = absdiff;
        //            absdiff = Math.Abs(the_image[i - 1,j + 1].blue - the_image[i + 1,j - 1].blue);
        //            if (absdiff > max_diff) max_diff = absdiff;
        //            absdiff = Math.Abs(the_image[i,j - 1].blue - the_image[i,j + 1].blue);
        //            if (absdiff > max_diff) max_diff = absdiff;
        //            absdiff = Math.Abs(the_image[i - 1,j].blue - the_image[i + 1,j].blue);
        //            if (absdiff > max_diff) max_diff = absdiff;
        //            {
        //                out_image[i,j].blue = max_diff;
        //                out_image[i,j].green = max_diff;
        //                out_image[i,j].red = max_diff;
        //            }
        //        }
        //    }
        //    if (threshold == 1)
        //    {
        //        for (i = 0; i < rows; i++)
        //        {
        //            for (j = 0; j < cols; j++)
        //            {
        //                if (out_image[i,j].blue > high)
        //                {
        //                    out_image[i,j].blue = new_hi;
        //                }
        //                else
        //                {
        //                    out_image[i,j].blue = new_low;
        //                }
        //            }
        //        }
        //    }
        //}
        //void gaussian_edge(Pixels** the_image, Pixels** out_image, int rows, int cols, int bits_per_pixel, int size, int threshold, int high)
        //{
        //    char[] response = new char[80];
        //    int sum = 0;
        //    int a, b, absdiff, absmax, diff, i, j,
        //        length, lower = 0, max, new_hi, new_low,
        //        scale, starti = 0, stopi = 0, startj = 0, stopj = 0,
        //        upper = 0, width;
        //    new_hi = 250;
        //    new_low = 16;
        //    if (bits_per_pixel == 4)
        //    {
        //        new_hi = 10;
        //        new_low = 3;
        //    }
        //    max = 255;
        //    if (bits_per_pixel == 4)
        //        max = 16;
        //    if (size == 7)
        //    {
        //        lower = -3;
        //        upper = 4;
        //        starti = 3;
        //        startj = 3;
        //        stopi = rows - 3;
        //        stopj = cols - 3;
        //        scale = 2;
        //    }
        //    if (size == 9)
        //    {
        //        lower = -4;
        //        upper = 5;
        //        starti = 4;
        //        startj = 4;
        //        stopi = rows - 4;
        //        stopj = cols - 4;
        //        scale = 2;
        //    }
        //    for (i = 0; i < rows; i++)
        //        for (j = 0; j < cols; j++)
        //        {
        //            out_image[i,j].blue = 0;
        //            out_image[i,j].red = 0;
        //            out_image[i,j].green = 0;
        //        }
        //    for (i = starti; i < stopi; i++)
        //    {
        //        //if ((i % 10) == 0) printf(" i=%d", i);
        //        for (j = startj; j < stopj; j++)
        //        {
        //            sum = 0;
        //            for (a = lower; a < upper; a++)
        //            {
        //                for (b = lower; b < upper; b++)
        //                {
        //                    if (size == 7)
        //                        sum = sum + the_image[i + a,j + b].blue *
        //                        g7[a + 3, b + 3];
        //                    if (size == 9)
        //                        sum = sum + the_image[i + a,j + b].blue *
        //                        g9[a + 4, b + 4];
        //                }
        //            }
        //            if (sum < 0) sum = 0;
        //            if (sum > max) sum = max;
        //            out_image[i,j].blue = sum;
        //            out_image[i,j].green = sum;
        //            out_image[i,j].red = sum;
        //        }
        //    }
        //    if (threshold == 1)
        //    {
        //        for (i = 0; i < rows; i++)
        //        {
        //            for (j = 0; j < cols; j++)
        //            {
        //                if (out_image[i,j].blue > high)
        //                {
        //                    out_image[i,j].blue = new_hi;
        //                }
        //                else
        //                {
        //                    out_image[i,j].blue = new_low;
        //                }
        //            }
        //        }
        //    }
        //}
        //void contrast_edge(Pixels** the_image, Pixels** out_image, long rows, long cols, long bits_per_pixel, int threshold, int high)
        //{
        //    int ad, d;
        //    int a, b, absdiff, absmax, diff, i, j,
        //        length, max, new_hi, new_low,
        //        sum_d, sum_n, width;
        //    new_hi = 250;
        //    new_low = 16;
        //    if (bits_per_pixel == 4)
        //    {
        //        new_hi = 10;
        //        new_low = 3;
        //    }
        //    max = 255;
        //    if (bits_per_pixel == 4)
        //        max = 16;
        //    for (i = 0; i < rows; i++)
        //        for (j = 0; j < cols; j++)
        //        {
        //            out_image[i,j].blue = 0;
        //            out_image[i,j].red = 0;
        //            out_image[i,j].green = 0;
        //        }
        //    for (i = 1; i < rows - 1; i++)
        //    {
        //        if ((i % 10) == 0) Console.WriteLine("{0}", i);
        //        for (j = 1; j < cols - 1; j++)
        //        {
        //            sum_n = 0;
        //            sum_d = 0;
        //            for (a = -1; a < 2; a++)
        //            {
        //                for (b = -1; b < 2; b++)
        //                {
        //                    sum_n = sum_n + the_image[i + a,j + b].blue *
        //                        e_mask[a + 1, b + 1];
        //                    sum_d = sum_d + the_image[i + a,j + b].blue *
        //                        contrast[a + 1, b + 1];
        //                }
        //            }
        //            d = sum_d / 9;
        //            if (d == 0)
        //                d = 1;
        //            out_image[i,j].blue = sum_n / d;
        //            out_image[i,j].red = sum_n / d;
        //            out_image[i,j].green = sum_n / d;
        //            if (out_image[i,j].blue > max)
        //                out_image[i,j].blue = max;
        //            if (out_image[i,j].blue < 0)
        //                out_image[i,j].blue = 0;
        //        }
        //    }
        //    if (threshold == 1)
        //    {
        //        for (i = 0; i < rows; i++)
        //        {
        //            for (j = 0; j < cols; j++)
        //            {
        //                if (out_image[i,j].blue > high)
        //                {
        //                    out_image[i,j].blue = new_hi;
        //                }
        //                else
        //                {
        //                    out_image[i,j].blue = new_low;
        //                }
        //            }
        //        }
        //    }
        //}
        //int RGB2Gray(byte Red, byte Blue, byte Green)
        //{
        //    return (int)(0.299 * Red + 0.587 * Green + 0.144 * Blue);
        //}

        //void make_bmp_file_header(bmpfileheader* file_header, int height, int width)
        //{
        //    file_header->filetype = 0x4d42;
        //    file_header->filesize = height * width + 1078;
        //    file_header->reserved1 = 0;
        //    file_header->reserved2 = 0;
        //    file_header->bitmapoffset = 1078;
        //}
        //void make_bitmap_header(bitmapheader* bitmap_header, int height, int witdh)
        //{
        //    bitmap_header->size = 40;
        //    bitmap_header->height = height;
        //    bitmap_header->width = witdh;
        //    bitmap_header->planes = 1;
        //    bitmap_header->compression = 0;
        //    bitmap_header->sizeofbitmap = 80;
        //    bitmap_header->bitsperpixel = 8;
        //    bitmap_header->colorsimp = 256;
        //    bitmap_header->colorsused = 256;
        //    bitmap_header->compression = 0;
        //    bitmap_header->vertres = 2835;
        //    bitmap_header->horzres = 2835;
        //}
        //void make_bmp_file_header_rgb(bmpfileheader* file_header, int height, int width)
        //{
        //    file_header->filetype = 0x4d42;
        //    file_header->filesize = height * width * 3 + 54;
        //    file_header->reserved1 = 0;
        //    file_header->reserved2 = 0;
        //    file_header->bitmapoffset = 54;
        //}
        //void make_bitmap_header_rgb(bitmapheader* bitmap_header, int height, int witdh)
        //{
        //    bitmap_header->size = 40;
        //    bitmap_header->height = height;
        //    bitmap_header->width = witdh;
        //    bitmap_header->planes = 1;
        //    bitmap_header->compression = 0;
        //    bitmap_header->sizeofbitmap = height * witdh * 3;
        //    bitmap_header->bitsperpixel = 24;
        //    bitmap_header->colorsimp = 0;
        //    bitmap_header->colorsused = 0;
        //    bitmap_header->vertres = 3778;
        //    bitmap_header->horzres = 3778;
        //}
        //int free_image_array(struct ctstruct ** the_array, long length)
        //{
        //	int i;
        //	for (i = 0; i<length; i++)

        //        free(the_array[i]);
        //	return(1);
        //}
        //int free_image_array_8bit(byte** array_in, long length)
        //{
        //    int i;
        //    for (i = 0; i < length; i++)
        //        free(array_in[i]);
        //    return (1);
        //}

    }
}
