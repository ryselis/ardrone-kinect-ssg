using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Kinect;


namespace CameraFundamentals
{
    public class KinectCanvas
    {

        public static Dictionary<JointType, Brush> JointColors = new Dictionary<JointType, Brush>() { 
            {JointType.HipCenter, new SolidColorBrush(Color.FromRgb(169, 176, 155))},
            {JointType.Spine, new SolidColorBrush(Color.FromRgb(169, 176, 155))},
            {JointType.ShoulderCenter, new SolidColorBrush(Color.FromRgb(168, 230, 29))},
            {JointType.Head, new SolidColorBrush(Color.FromRgb(200, 0,   0))},
            {JointType.ShoulderLeft, new SolidColorBrush(Color.FromRgb(79,  84,  33))},
            {JointType.ElbowLeft, new SolidColorBrush(Color.FromRgb(84,  33,  42))},
            {JointType.WristLeft, new SolidColorBrush(Color.FromRgb(255, 126, 0))},
            {JointType.HandLeft, new SolidColorBrush(Color.FromRgb(215,  86, 0))},
            {JointType.ShoulderRight, new SolidColorBrush(Color.FromRgb(33,  79,  84))},
            {JointType.ElbowRight, new SolidColorBrush(Color.FromRgb(33,  33,  84))},
            {JointType.WristRight, new SolidColorBrush(Color.FromRgb(77,  109, 243))},
            {JointType.HandRight, new SolidColorBrush(Color.FromRgb(37,   69, 243))},
            {JointType.HipLeft, new SolidColorBrush(Color.FromRgb(77,  109, 243))},
            {JointType.KneeLeft, new SolidColorBrush(Color.FromRgb(69,  33,  84))},
            {JointType.AnkleLeft, new SolidColorBrush(Color.FromRgb(229, 170, 122))},
            {JointType.FootLeft, new SolidColorBrush(Color.FromRgb(255, 126, 0))},
            {JointType.HipRight, new SolidColorBrush(Color.FromRgb(181, 165, 213))},
            {JointType.KneeRight, new SolidColorBrush(Color.FromRgb(71, 222,  76))},
            {JointType.AnkleRight, new SolidColorBrush(Color.FromRgb(245, 228, 156))},
            {JointType.FootRight, new SolidColorBrush(Color.FromRgb(77,  109, 243))}
        };

        private readonly NUIcontroler _kinect;

        public KinectCanvas(NUIcontroler kinect)
        {
            _kinect = kinect;
        }

        public Point GetDisplayPosition(Joint joint)
        {
            int depthX, depthY;
            DepthImagePoint dp = _kinect.depthFrameFull.MapFromSkeletonPoint(joint.Position);

            depthX = Math.Max(0, Math.Min(dp.X, 320));  //convert to 320, 240 space
            depthY = Math.Max(0, Math.Min(dp.Y, 240));  //convert to 320, 240 space
            ColorImagePoint cp = _kinect.depthFrameFull.MapToColorImagePoint(depthX, depthY,
                ColorImageFormat.RgbResolution640x480Fps30);
            return new Point((int)(cp.X / 2), (int)(cp.Y / 2));
        }

        public Polyline GetBodySegment(JointCollection joints, Brush brush, params JointType[] ids)
        {
            var points = new PointCollection(ids.Length);
            foreach (var t in ids)
                points.Add(GetDisplayPosition(joints[t]));
            var polyline = new Polyline { Points = points, Stroke = brush, StrokeThickness = 6 };
            return polyline;
        }
    }
}
