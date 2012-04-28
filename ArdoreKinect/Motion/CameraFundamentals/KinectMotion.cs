using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;


namespace CameraFundamentals
{
    public class KinectMotion
    {
        public static bool BothHandsStraight(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder) //right > left, up > down, back > front
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (RightHandDisplacementX < 1.5 && RightHandDisplacementX > -0.5 &&
                LeftHandDisplacementX < 1.5 && LeftHandDisplacementX > -0.5 &&
                RightHandDisplacementY < 0.7 && RightHandDisplacementY > -0.6 &&
                LeftHandDisplacementY < 0.7 && LeftHandDisplacementY > -0.6 &&
                RightHandDisplacementZ < -1.8 && LeftHandDisplacementZ < -1.8)
                return true;
            return false;
        }

        public static bool BothHandsMoreStraight(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder) //right > left, up > down, back > front
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (RightHandDisplacementX < 0.7 && LeftHandDisplacementX < 0.7)
                return true;
            return false;
        }

        public static bool WeGotABadassOverHere(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (RightHandDisplacementX < 1.8 && RightHandDisplacementX > -0.5 &&
                LeftHandDisplacementX < 1.8 && LeftHandDisplacementX > -0.5 &&
                RightHandDisplacementY < 0.6 && RightHandDisplacementY > -0.6 &&
                LeftHandDisplacementY < 0.6 && LeftHandDisplacementY > -0.6 &&
                RightHandDisplacementZ > -0.5 && LeftHandDisplacementZ > -0.5)
                return true;
            return false;
        }

        public static bool HandsClockwise(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (RightHandDisplacementX < 2.6 && RightHandDisplacementX > 0 &&
                LeftHandDisplacementX < 2.6 && LeftHandDisplacementX > 0 &&
                RightHandDisplacementY < 0.7 && RightHandDisplacementY > -1.6 &&
                LeftHandDisplacementY < 0.7 && LeftHandDisplacementY > -0.6 &&
                RightHandDisplacementZ > 0.5 && LeftHandDisplacementZ < -0.9)
                return true;
            return false;
        }

        public static bool HandsMoreClockwise(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (RightHandDisplacementX < 2.6 && RightHandDisplacementZ > 1.5 &&
                LeftHandDisplacementX < 2.6 && LeftHandDisplacementZ < -2.2)
                return true;
            return false;
        }

        public static bool HandsCounterclockwise(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (RightHandDisplacementX < 2.6 && RightHandDisplacementX > 0 &&
                LeftHandDisplacementX < 2.6 && LeftHandDisplacementX > 0 &&
                RightHandDisplacementY < 0.7 && RightHandDisplacementY > -0.6 &&
                LeftHandDisplacementY < 0.7 && LeftHandDisplacementY > -1.6 &&
                RightHandDisplacementZ < -0.9 && LeftHandDisplacementZ > 0.5)
                return true;
            return false;
        }

        public static bool HandsMoreCounterclockwise(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (LeftHandDisplacementX < 2.6 && LeftHandDisplacementZ > 1.5 &&
                RightHandDisplacementX < 2.6 && RightHandDisplacementZ < -2.2)
                return true;
            return false;
        }

        public static bool HandsRight(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (RightHandDisplacementX < 2.6 && LeftHandDisplacementX < 2.6 &&
                LeftHandDisplacementY > 0.2 && RightHandDisplacementY < -0.6 &&
                RightHandDisplacementZ > -1 && RightHandDisplacementZ < 0.5 &&
                LeftHandDisplacementZ > -1 && LeftHandDisplacementZ < 0.5)
                return true;
            return false;
        }

        public static bool HandsMoreRight(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            if (LeftHandDisplacementY > 1 && RightHandDisplacementY < -1)
                return true;
            return false;
        }

        public static bool HandsLeft(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (RightHandDisplacementX < 2.6 && LeftHandDisplacementX < 2.6 &&
                LeftHandDisplacementY < -0.6 && RightHandDisplacementY > 0.2 &&
                RightHandDisplacementZ > -1 && RightHandDisplacementZ < 0.5 &&
                LeftHandDisplacementZ > -1 && LeftHandDisplacementZ < 0.5)
                return true;
            return false;
        }

        public static bool HandsMoreLeft(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            if (LeftHandDisplacementY < -1 && RightHandDisplacementY > 1)
                return true;
            return false;
        }
        public static bool HandsRaised(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (RightHandDisplacementX < 2.7 && RightHandDisplacementX > 0 &&
                LeftHandDisplacementX < 2.7 && LeftHandDisplacementX > 0 &&
                RightHandDisplacementY > 0.71 && LeftHandDisplacementY > 0.71 &&
                RightHandDisplacementZ < 1 && RightHandDisplacementZ > -1.5 &&
                LeftHandDisplacementZ < 1 && LeftHandDisplacementZ > -1.5)
                return true;
            return false;
        }

        public static bool HandsMoreRaised(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            if (RightHandDisplacementY > 1.2 && LeftHandDisplacementY > 1.2)
                return true;
            return false;
        }

        public static bool HandsLaid(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            if (RightHandDisplacementX < 2.7 && RightHandDisplacementX > 0 &&
                LeftHandDisplacementX < 2.7 && LeftHandDisplacementX > 0 &&
                RightHandDisplacementY < -0.61 && LeftHandDisplacementY < -0.61 &&
                RightHandDisplacementY > -1.9 && LeftHandDisplacementY > -1.9 &&
                RightHandDisplacementZ < 1 && RightHandDisplacementZ > -3 &&
                LeftHandDisplacementZ < 1 && LeftHandDisplacementZ > -3)
                return true;
            return false;
        }

        public static bool HandsMoreLaid(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            if (RightHandDisplacementY < -1.7 && LeftHandDisplacementY < -1.7)
                return true;
            return false;
        }

        public static bool Emergency(Joint lhand, Joint rhand, Joint head)
        {
            return (head.Position.X - rhand.Position.X > -0.2 && head.Position.X - rhand.Position.X < 0.2 &&
                head.Position.X - lhand.Position.X > -0.2 && head.Position.X - lhand.Position.X < 0.2 &&
                head.Position.Y - rhand.Position.Y > -0.2 && head.Position.Y - rhand.Position.Y < 0.2 &&
                head.Position.Y - lhand.Position.Y > -0.2 && head.Position.Y - lhand.Position.Y < 0.2 &&
                head.Position.Z - rhand.Position.Z > -0.2 && head.Position.Z - rhand.Position.Z < 0.2 &&
                head.Position.Z - lhand.Position.Z > -0.2 && head.Position.Z - lhand.Position.Z < 0.2);
        }

        public static bool HandsDown(Joint lhand, Joint rhand, Joint chip)
        {
            if (lhand.Position.Y < chip.Position.Y && rhand.Position.Y < chip.Position.Y)
                return true;
            return false;
        }

        public static bool HandsNeutral(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder, Joint chip)
        {
            if (BothHandsStraight(lhand, rhand, lshoulder, rshoulder, cshoulder) ||
                WeGotABadassOverHere(lhand, rhand, lshoulder, rshoulder, cshoulder) ||
                HandsClockwise(lhand, rhand, lshoulder, rshoulder, cshoulder) ||
                HandsCounterclockwise(lhand, rhand, lshoulder, rshoulder, cshoulder) ||
                HandsLeft(lhand, rhand, lshoulder, rshoulder, cshoulder) ||
                HandsRight(lhand, rhand, lshoulder, rshoulder, cshoulder) ||
                HandsDown(lhand, rhand, chip))
                return false;
            return true;
        }

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

        public static float Gaz(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float gaz;
            gaz = (RightHandDisplacementZ + LeftHandDisplacementZ + 1) / 10;
            if (gaz < 0.02 && gaz > -0.02)
                gaz = 0;
            if (LeftHandDisplacementY > 1 && RightHandDisplacementY > 1 &&
                LeftHandDisplacementX < 1 && RightHandDisplacementX < 1)
                if (LeftHandDisplacementZ > 0 && RightHandDisplacementZ > 0)
                {
                    gaz = 0.01f;
                }
                else
                {
                    gaz = -0.01f;
                }
                
            if (LeftHandDisplacementY > 1 && RightHandDisplacementY > 1 &&
               LeftHandDisplacementX > 1 && RightHandDisplacementX > 1)
                gaz = 0.0f;
            if (gaz > 1f || gaz < -1f)
                gaz = 0;
            return gaz;
        }

        public static float Pitch(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float pitch;
            pitch = (RightHandDisplacementY + LeftHandDisplacementY + 1.0f) / 2;
            if (pitch < 0.2 && pitch > -0.2)
                pitch = 0;
            if (pitch > 2.5 || pitch < -2.5)
                pitch = 0;
            return pitch;
        }

        public static float Disp(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float disp;
            disp = 0;
            disp = (RightHandDisplacementY - LeftHandDisplacementY) / -20;
            if (disp > -0.02 && disp < 0.02)
                disp = 0;
           // if (disp > 0.25 || disp < -0.25)
            //    disp = 0;
            return disp;
        }

        public static float Rotate(Joint lhand, Joint rhand, Joint lshoulder, Joint rshoulder, Joint cshoulder)
        {
            float BasicDisplacement = rshoulder.Position.X - lshoulder.Position.X;
            float RightHandDisplacementX = (rhand.Position.X - cshoulder.Position.X) / BasicDisplacement;
            float RightHandDisplacementY = (rhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float RightHandDisplacementZ = (rhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float LeftHandDisplacementX = (cshoulder.Position.X - lhand.Position.X) / BasicDisplacement;
            float LeftHandDisplacementY = (lhand.Position.Y - cshoulder.Position.Y) / BasicDisplacement;
            float LeftHandDisplacementZ = (lhand.Position.Z - cshoulder.Position.Z) / BasicDisplacement;
            float rot;
            rot = (LeftHandDisplacementZ - RightHandDisplacementZ) / -1;
            if (rot > -0.2 && rot < 0.2)
                rot = 0;
            //if (rot > 2.5 || rot < -2.5)
              //  rot = 0;
            return rot;
        }
    }
}
