using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame4
{
    class KinectManager
    {
        Game1 mGame;
        KinectSensor mKinect;
        Color[] mLatestColorData;
        Texture2D mColorImage;

        public KinectManager(Game1 game)
        {

            mGame = game;

        }

        public string InitKinect()
        {
            if (KinectSensor.KinectSensors.Count == 0)
            {
                return "Error: No kinect sensors!";
            }

            mKinect = KinectSensor.KinectSensors[0];

            mKinect.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);

            mKinect.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(mKinect_ColorFrameReady);
            //mKinect.AllFramesReady
            mKinect.Start();

            return "";


        }



        void mKinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            ColorImageFrame frame = e.OpenColorImageFrame();

            if (frame == null)
            {
                return;
            }
            byte[] pixelData = new byte[frame.PixelDataLength];
            frame.CopyPixelDataTo(pixelData);

            mLatestColorData = new Color[pixelData.Length / 4];
            int offset = 0;
            for (int i = 0; i < mLatestColorData.Length; i++)
            {
                mLatestColorData[i] = new Color(pixelData[offset + 2], pixelData[offset + 1], pixelData[offset]);
                offset += 4;
            }
            frame.Dispose();
        }
        public void DrawColorImage(SpriteBatch batch, GraphicsDevice device, Rectangle bounds)
        {
            if (mLatestColorData == null)
            {
                return;
            }
            mColorImage = new Texture2D(device, 1280, 960);
            mColorImage.SetData<Color>(mLatestColorData);
            batch.Draw(mColorImage, bounds, Color.White);

        }
    }
}
