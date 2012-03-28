using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;


namespace CameraFundamentals
{
    public class KinectMotion
    {

        public static bool leftHandStraight(Joint head, Joint lhand, Joint lshoulder, Joint rshoulder, Joint chip)
        {
            float dis_left_right_shoulder = rshoulder.Position.X - lshoulder.Position.X;
            float dis_left_to_shoulder = lshoulder.Position.X - lhand.Position.X;

            if (head.Position.Y > lhand.Position.Y && chip.Position.Y < lhand.Position.Y && dis_left_right_shoulder < dis_left_to_shoulder && lhand.Position.X < lshoulder.Position.X)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool rightHandStraight(Joint head, Joint rhand, Joint lshoulder, Joint rshoulder, Joint chip)
        {
            float dis_left_right_shoulder = rshoulder.Position.X - lshoulder.Position.X;
            float dis_right_to_shoulder = rhand.Position.X - rshoulder.Position.X;

            if (head.Position.Y > rhand.Position.Y && chip.Position.Y < rhand.Position.Y &&
                dis_left_right_shoulder < dis_right_to_shoulder && rhand.Position.X > rshoulder.Position.X)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool rightHandRaised(Joint head, Joint rhand)
        {
            if (head.Position.Y < rhand.Position.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool leftHandRaised(Joint head, Joint lhand)
        {
            if (head.Position.Y < lhand.Position.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool rightHandForward(Joint head, Joint rhand, Joint lshoulder, Joint rshoulder, Joint chip)
        {

            float dis_left_right_shoulder_ext = (rshoulder.Position.X - lshoulder.Position.X) * 0.30f;
            float dis_left_right_shoulder = (rshoulder.Position.X - lshoulder.Position.X);
            float dis_right_to_shoulder = rhand.Position.X - rshoulder.Position.X;
            float dis_rhand_rshoulder = rshoulder.Position.Z - rhand.Position.Z;

            if (rshoulder.Position.X - dis_left_right_shoulder_ext <= rhand.Position.X &&
                rhand.Position.X <= rshoulder.Position.X + dis_left_right_shoulder_ext &&

                rshoulder.Position.Y - dis_left_right_shoulder_ext <= rhand.Position.Y &&
                rhand.Position.Y <= rshoulder.Position.Y + dis_left_right_shoulder_ext * 2)
            {
                if (head.Position.Y > rhand.Position.Y && rhand.Position.Y > chip.Position.Y &&
                    dis_rhand_rshoulder > (dis_left_right_shoulder * 1.3f))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static bool rightHandBackward(Joint head, Joint rhand, Joint lshoulder, Joint rshoulder, Joint chip)
        {

            float dis_left_right_shoulder_ext = (rshoulder.Position.X - lshoulder.Position.X) * 0.30f;
            float dis_left_right_shoulder = (rshoulder.Position.X - lshoulder.Position.X);
            float dis_right_to_shoulder = rhand.Position.X - rshoulder.Position.X;
            float dis_rhand_rshoulder = rshoulder.Position.Z - rhand.Position.Z;

            if (rshoulder.Position.X - dis_left_right_shoulder_ext <= rhand.Position.X &&
                rhand.Position.X <= rshoulder.Position.X + dis_left_right_shoulder_ext &&

                rshoulder.Position.Y - dis_left_right_shoulder_ext <= rhand.Position.Y &&
                rhand.Position.Y <= rshoulder.Position.Y + dis_left_right_shoulder_ext * 2)
            {
                if (head.Position.Y > rhand.Position.Y && rhand.Position.Y > chip.Position.Y &&
                    dis_rhand_rshoulder < (dis_left_right_shoulder * 0.9f))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

    }
}
