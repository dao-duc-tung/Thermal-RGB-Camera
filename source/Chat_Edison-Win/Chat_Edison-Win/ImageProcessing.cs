using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chat_Edison_Win.MixImage;

namespace Chat_Edison_Win
{
    public class ImageProcessing
    {
        public static string RGBNameTemp = "UndistortedRGBImage.png";
        public static int numImage = 0;
        public const int NEW_HEIGHT = 660;
        public const int NEW_WIDTH = 880;
        public const int LCD_SCALE = 4;
        public static Pixels[] rgb = new Pixels[256];
        public static byte[,] raw_to_bmp = new byte[60, 80];
        public static ushort[,] rgb565 = new ushort[NEW_HEIGHT, NEW_WIDTH];
        public static byte[,] scaleimg = new byte[NEW_HEIGHT, NEW_WIDTH];
        public static ushort MaxVal, MinVal, SubVal;
        public static ushort[] imgFLIR16 = new ushort[4800];

        public static Mat _cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);
        public static Mat _distCoeffs = new Mat(1, 8, DepthType.Cv64F, 1);
        public static int width = 5;
        public static int height = 5;

        
        public static void InitIP()
        {
            make_color_table(rgb, 256);
            CalibrateRGBCAM();
        }

        #region Thermal Image
        public static byte[] ZoomIn()
        {
            double scale = 255.0 / SubVal;
            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 80; j++)
                {
                    raw_to_bmp[i, j] = (byte)((imgFLIR16[i * 80 + j] - MinVal) * scale);
                }
            }

            zoom_in_with_cubic(raw_to_bmp, scaleimg, 80, 60, NEW_WIDTH, NEW_HEIGHT);



            //convert to rgb565 and fill color
            //convertRGB565(scaleimg, rgb565, rgb);

            //byte[] temp = new byte[NEW_WIDTH * NEW_HEIGHT * 2];
            //int n = 0;
            //for (int i = 0; i < NEW_HEIGHT; i++)
            //{
            //    for (int j = 0; j < NEW_WIDTH; j++)
            //    {
            //        temp[n] = (byte)(rgb565[i, j]);
            //        temp[n + 1] = (byte)(rgb565[i, j] >> 8);
            //        n += 2;
            //    }
            //}

            byte[] temp = new byte[NEW_WIDTH * NEW_HEIGHT * 3];
            int n = 0;
            for (int i = 0; i < NEW_HEIGHT; i++)
            {
                for (int j = 0; j < NEW_WIDTH; j++)
                {
                    temp[n] = (byte)(rgb[scaleimg[i, j]].blue);
                    temp[n + 1] = (byte)(rgb[scaleimg[i, j]].green);
                    temp[n + 2] = (byte)(rgb[scaleimg[i, j]].red);
                    n += 3;
                }
            }

            return temp;
        }

        public static void zoom_in_with_cubic(byte[,] image, byte[,] outImg, int width, int height, int new_width, int new_height)
        {
            int x = 0, y = 0;
            double dx, dy;
            double[] t = new double[4];
            dx = (double)width / (double)(new_width - 1);
            dy = (double)height / (double)(new_height - 1);

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
                    double ix = (double)x * dx - 0.5;
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
                    t[0] = CubicHermite(image[oy - 1, ox - 1], image[oy - 1, ox], image[oy - 1, ox + 1], image[oy - 1, ox + 2], s);
                    t[1] = CubicHermite(image[oy, ox - 1], image[oy, ox], image[oy, ox + 1], image[oy, ox + 2], s);
                    t[2] = CubicHermite(image[oy + 1, ox - 1], image[oy + 1, ox], image[oy + 1, ox + 1], image[oy + 1, ox + 2], s);
                    t[3] = CubicHermite(image[oy + 2, ox - 1], image[oy + 2, ox], image[oy + 2, ox + 1], image[oy + 2, ox + 2], s);
                    int value = (int)CubicHermite(t[0], t[1], t[2], t[3], v);
                    outImg[y, x] = (byte)Clamp(value, 0, 255);
                }
            }
        }

        public static void make_color_table(Pixels[] rgb, long color)
        {
            for (int i = 0; i < 51; i++)
            {
                //white to yellow
                rgb[color - i - 1].red = 255;
                rgb[color - i - 1].green = (byte)(255 - i / 2);
                rgb[color - i - 1].blue = (byte)(255 - i * 5);
                //yellow to orange
                rgb[color - (i + 51)].red = (byte)(255 - i / 3);
                rgb[color - (i + 51)].green = (byte)(255 - i * 4);
                rgb[color - (i + 51)].blue = (byte)(i / 3);
                ////orange to viloet
                rgb[color - (i + 102)].red = (byte)(255 - i * 2);
                rgb[color - (i + 102)].green = (byte)(55 - i);
                rgb[color - (i + 102)].blue = (byte)(i * 3 / 2);
                ////violet to blue
                rgb[color - (i + 153)].red = (byte)(153 - i * 2);
                rgb[color - (i + 153)].green = 0;
                rgb[color - (i + 153)].blue = (byte)(55 + i);
                ////blue to black
                rgb[color - (i + 204)].red = (byte)(55 - i * 2 / 3);
                rgb[color - (i + 204)].green = 0;
                rgb[color - (i + 204)].blue = (byte)(100 - i * 2 / 3);
            }
            rgb[0].red = 0;
            rgb[0].green = 0;
            rgb[0].blue = 0;
        }

        public static double CubicHermite(double A, double B, double C, double D, double t)
        {
            double a = -A / 2 + 3 * B / 2 - 3 * C / 2 + D / 2;
            double b = A - 5 * B / 2 + 2 * C - D / 2;
            double c = -A / 2 + C / 2;
            double d = B;
            return a * t * t * t + b * t * t + c * t + d;
        }

        public static void convertRGB565(byte[,] img_in, UInt16[,] img_out, Pixels[] rgb)
        {
            for (int i = 0; i < NEW_HEIGHT; i++)
            {
                for (int j = 0; j < NEW_WIDTH; j++)
                {
                    img_out[i, j] = (ushort)(((rgb[img_in[i, j]].red >> 3) << 11) | ((rgb[img_in[i, j]].green >> 2) << 5) | (rgb[img_in[i, j]].blue >> 3));
                }
            }
        }

        public static bool CheckThermalImage(byte[] imageArr)
        {
            String s = "";
            MaxVal = 0;
            MinVal = 35565;
            int temp = 0;
            int n = 0;
            for (int i = 0; i < 60 * 80; i++)
            {
                imgFLIR16[n] = (ushort)(imageArr[i * 2] + (imageArr[i * 2 + 1] << 8));
                temp = imgFLIR16[n];
                n++;
            }
            MaxVal = imgFLIR16.Max();
            MinVal = imgFLIR16.Min();
            SubVal = (ushort)(MaxVal - MinVal);
            if (MaxVal >= 16258) return false;

            //Write file for debug
            //s += Convert.ToDouble(temperature.Trim());
            //s += "\n";
            //for(int i = 0; i < 60; i++)
            //{
            //    for (int j = 0; j < 80; j++)
            //    {
            //        s += imgFLIR16[i * 80 + j];
            //        s += " ";
            //    }
            //    s += "\n";
            //}
            //File.WriteAllText(@"C:\Users\Dao\Desktop\temperature VMIG\" + (++numImage).ToString() + ".txt", s);

            return true;
        }

        public static byte[] Convert16BitGrayScaleToRgb48(byte[] inBuffer, int width, int height)
        {
            int inBytesPerPixel = 2;
            int outBytesPerPixel = 6;

            byte[] outBuffer = new byte[width * height * outBytesPerPixel];
            int inStride = width * inBytesPerPixel;
            int outStride = width * outBytesPerPixel;

            // Step through the image by row
            for (int y = 0; y < height; y++)
            {
                // Step through the image by column
                for (int x = 0; x < width; x++)
                {
                    // Get inbuffer index and outbuffer index
                    int inIndex = (y * inStride) + (x * inBytesPerPixel);
                    int outIndex = (y * outStride) + (x * outBytesPerPixel);

                    byte hibyte = (byte)(inBuffer[inIndex + 1]);
                    byte lobyte = (byte)inBuffer[inIndex];

                    //R
                    outBuffer[outIndex] = lobyte;
                    outBuffer[outIndex + 1] = hibyte;

                    //G
                    outBuffer[outIndex + 2] = lobyte;
                    outBuffer[outIndex + 3] = hibyte;

                    //B
                    outBuffer[outIndex + 4] = lobyte;
                    outBuffer[outIndex + 5] = hibyte;
                }
            }
            return outBuffer;
        }

        public static Bitmap CreateBitmapFromBytes(byte[] pixelValues, int width, int height)
        {
            //Create an image that will hold the image data
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            //Bitmap bmp = new Bitmap(width, height, PixelFormat.Format16bppRgb565);

            //Get a reference to the images pixel data
            Rectangle dimension = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData picData = bmp.LockBits(dimension, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr pixelStartAddress = picData.Scan0;

            //Copy the pixel data into the bitmap structure
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, pixelStartAddress, pixelValues.Length);

            bmp.UnlockBits(picData);
            return bmp;
        }

        public static double Clamp(double val, double min, double max)
        {
            return Math.Min(Math.Max(val, min), max);
        }
        public static int Clamp(int val, int min, int max)
        {
            return Math.Min(Math.Max(val, min), max);
        }

        #endregion

        #region RGB Image

        public static double CalibrateRGBCAM()
        {
            Size patternSize = new Size(width, height);

            string[] fileEntries = Directory.GetFiles(@"..\..\..\..\rgb image", "*.jpg");

            Image<Gray, Byte>[] Frame_array_buffer = new Image<Gray, byte>[fileEntries.Length]; //number of images to calibrate camera over
            MCvPoint3D32f[][] corners_object_list = new MCvPoint3D32f[Frame_array_buffer.Length][];
            PointF[][] corners_points_list = new PointF[Frame_array_buffer.Length][];
            VectorOfPointF[] _cornersPointsVec = new VectorOfPointF[Frame_array_buffer.Length];
            Mat[] _rvecs, _tvecs;

            for (int k = 0; k < Frame_array_buffer.Length; k++)
            {
                Frame_array_buffer[k] = new Image<Gray, byte>(fileEntries[k]);
                _cornersPointsVec[k] = new VectorOfPointF();
                CvInvoke.FindChessboardCorners(Frame_array_buffer[k], patternSize, _cornersPointsVec[k], CalibCbType.AdaptiveThresh | CalibCbType.FilterQuads);
                //for accuracy
                CvInvoke.CornerSubPix(Frame_array_buffer[k], _cornersPointsVec[k], new Size(11, 11), new Size(-1, -1), new MCvTermCriteria(30, 0.1));

                //Fill our objects list with the real world mesurments for the intrinsic calculations
                List<MCvPoint3D32f> object_list = new List<MCvPoint3D32f>();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        object_list.Add(new MCvPoint3D32f(j * 20.0F, i * 20.0F, 0.0F));
                    }
                }
                corners_object_list[k] = object_list.ToArray();
                corners_points_list[k] = _cornersPointsVec[k].ToArray();
            }


            double error = CvInvoke.CalibrateCamera(corners_object_list, corners_points_list, Frame_array_buffer[0].Size,
                _cameraMatrix, _distCoeffs, CalibType.RationalModel, new MCvTermCriteria(30, 0.1), out _rvecs, out _tvecs);
            //If Emgu.CV.CvEnum.CALIB_TYPE == CV_CALIB_USE_INTRINSIC_GUESS and/or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of fx, fy, cx, cy must be initialized before calling the function
            //if you use FIX_ASPECT_RATIO and FIX_FOCAL_LEGNTH options, these values needs to be set in the intrinsic parameters before the CalibrateCamera function is called. Otherwise 0 values are used as default.
            Console.WriteLine("Intrinsci Calculation Error: " + error.ToString(), "Results"); //display the results to the user
            return error;
        }

        public static bool CheckRGBImage(byte[] rgbArr, int numBytes)
        {
            //Mat undistortedRGBImage = UndistortImage(rgbArr, numBytes);
            //do sth with undistorted RGB Image
            ByteArrayToJPG(RGBNameTemp, rgbArr, numBytes);

            return true;
        }

        public static void ByteArrayToJPG(string _FileName, byte[] _ByteArray, int numBytes)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(_FileName, FileMode.Create)))
            {
                for (int i = 0; i < numBytes; i++)
                {
                    writer.Write(_ByteArray[i]);
                }
            }
        }

        public static Pixels[,] UndistortedJPGToPixels()
        {
            Pixels[,] tempRGB = new Pixels[240, 320];

            Bitmap myImage = UndistortImage().Bitmap;
            byte[] rgbValues;
            BitmapData data = myImage.LockBits(new Rectangle(0, 0, myImage.Width, myImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);

            try
            {
                IntPtr ptr = data.Scan0;
                int bytes = Math.Abs(data.Stride) * myImage.Height;
                rgbValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            }
            finally
            {
                myImage.UnlockBits(data);
            }
            //if (arrImg.Length != 240 * 320 * 3) return null;

            int n = 0;
            for (int i = 0; i < 240; i++)
            {
                for (int j = 0; j < 320; j++)
                {
                    tempRGB[i, j].blue = rgbValues[n];
                    tempRGB[i, j].green = rgbValues[n + 1];
                    tempRGB[i, j].red = rgbValues[n + 2];
                    n += 4;
                }
            }

            return tempRGB;
        }

        public static Mat UndistortImage()
        {
            Image<Bgr, Byte> InputImage = new Image<Bgr, byte>(RGBNameTemp);
            Mat outFrame = new Mat();
            CvInvoke.Undistort(InputImage, outFrame, _cameraMatrix, _distCoeffs);
            return outFrame;
        }



        #endregion

    }
}
