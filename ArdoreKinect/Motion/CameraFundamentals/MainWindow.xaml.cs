/////////////////////////////////////////////////////////////////////////
//
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// This code is licensed under the terms of the Microsoft Kinect for
// Windows SDK (Beta) License Agreement:
// http://kinectforwindows.org/KinectSDK-ToU
//
/////////////////////////////////////////////////////////////////////////
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf; 
//using System.Windows.Forms;  

namespace CameraFundamentals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer timerUpdate;
        private DispatcherTimer timerVideoUpdate;
        private DispatcherTimer timerColorUpdate;
        private DispatcherTimer timerDepthUpdate;
        private DispatcherTimer timerSkellUpdate;


        private NUIcontroler _nui;
        private DroneControler _drone;

        private KinectCanvas _kinectCanvas;

        private Timer _timer;
        private Timer _trotleft;
        private Timer _trotright;
        private Timer _tforward;
        private Timer _tbackward;
        private Timer _tflyleft;
        private Timer _tflyright;
        private Timer _takingOff;

        private bool takeOffCom;
        private bool conMassage;
        private bool islanding;
        private bool command;
        private bool motion;

        private bool imgNormal;
        private bool imgTakeOff;

        private Stopwatch stopWatch;

        public MainWindow()
        {
            stopWatch = new Stopwatch();
            takeOffCom = false;
            conMassage = false;
            islanding = false;
            command = false;
            motion = true;
            imgNormal = true;
            imgTakeOff = false;


            InitializeDrone();
            InitializeKinect();
            InitializeTimers();
        }

        
        
        private void InitializeDrone()
        {
            _drone = new DroneControler(ref label1);
            //_drone.ConnectToNetwork();
        }

        private void InitializeKinect()
        {

            _nui = new NUIcontroler(800, 600);
            _nui.init();
            _kinectCanvas = new KinectCanvas(_nui);
        }

        private void InitializeTimers()
        {
            timerUpdate = new DispatcherTimer();
            timerUpdate.Interval = new TimeSpan(0, 0, 0, 0, 33);
            timerUpdate.Tick += new EventHandler(timerStatusUpdate_Tick);
            timerUpdate.Start();

            timerVideoUpdate = new DispatcherTimer();
            timerVideoUpdate.Interval = new TimeSpan(0, 0, 0, 0, 33);
            timerVideoUpdate.Tick += new EventHandler(timerVideoUpdate_Tick);
            timerVideoUpdate.Start();

            timerColorUpdate = new DispatcherTimer();
            timerColorUpdate.Interval = new TimeSpan(0, 0, 0, 0, 33);
            timerColorUpdate.Tick += new EventHandler(timerCUpdate_Tick);
            timerColorUpdate.Start();

            timerDepthUpdate = new DispatcherTimer();
            timerDepthUpdate.Interval = new TimeSpan(0, 0, 0, 0, 33);
            timerDepthUpdate.Tick += new EventHandler(timerDUpdate_Tick);
            timerDepthUpdate.Start();

            timerSkellUpdate = new DispatcherTimer();
            timerSkellUpdate.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timerSkellUpdate.Tick += new EventHandler(timerSUpdate_Tick);
            timerSkellUpdate.Start();
        }

        private void UpdateStatus()
        {
            stopWatch.Stop();
            float gameTime = stopWatch.Elapsed.Milliseconds;
            stopWatch.Reset();
            stopWatch.Start();

            if (motion)
            {
                UpdateMotion(gameTime);
            }
            else
            {
                UpdateVoice();
            }

          
        }


        private void SkeletonDraw()
        { 
            if (_nui.skelet != null)
            {
                Skeleton.Children.Clear();
                PaintBones(_nui.skelet);
                PaintJoints(_nui.skelet);
            }
            else
            {
                Skeleton.Children.Clear();
            }

        }

        private void PaintJoints(Skeleton skeleton)
        {
            foreach (Joint joint in skeleton.Joints)
            {
                var jointPos = _kinectCanvas.GetDisplayPosition(joint);
                var jointLine = new Line
                {
                    X1 = jointPos.X - 3
                };
                jointLine.X2 = jointLine.X1 + 8;
                jointLine.Y1 = jointLine.Y2 = jointPos.Y;
                jointLine.Stroke = KinectCanvas.JointColors[joint.JointType];
                jointLine.StrokeThickness = 8;
                Skeleton.Children.Add(jointLine);
            }
        }

        private void PaintBones(Skeleton skeleton)
        {
         
            var brush = new SolidColorBrush(Colors.Blue);
            Skeleton.Children.Add(_kinectCanvas.GetBodySegment
                (skeleton.Joints, brush, JointType.HipCenter,
                JointType.Spine, JointType.ShoulderCenter, JointType.Head));
            Skeleton.Children.Add(_kinectCanvas.GetBodySegment
                (skeleton.Joints, brush, JointType.ShoulderCenter,
                JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft));
            Skeleton.Children.Add(_kinectCanvas.GetBodySegment
                (skeleton.Joints, brush, JointType.ShoulderCenter,
                JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight));
            Skeleton.Children.Add(_kinectCanvas.GetBodySegment
                (skeleton.Joints, brush, JointType.HipCenter, JointType.HipLeft,
                JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft));
            Skeleton.Children.Add(_kinectCanvas.GetBodySegment
                (skeleton.Joints, brush, JointType.HipCenter, JointType.HipRight,
                JointType.KneeRight, JointType.AnkleRight, JointType.FootRight));
        }



        private void UpdateVoice()
        {
            //label3.Content = "Sound: " + _nui.msg;
            if (_nui.voiceTakingOff && !_drone.droneControl.IsFlying)
            {
                _drone.Takeoff();
                _nui.voiceTakingOff = false;
                label1.Content = "Voice -> Take off";
                label2.Content = "Balsas - Pakilti";
            }
            if (_drone.droneControl.IsConnected && _drone.droneControl.IsFlying)
            {
                if (_nui.voiceLanding)
                {
                    _drone.Land();
                    label1.Content = "Voice -> Landing";
                    label2.Content = "Balsas - Nusileisti";
                    _nui.voiceLanding = false;
                } else if (_nui.voiceLeft)
                {
                    _drone.Navigate(0, 0, -0.5f, 0);
                    label1.Content = "Voice -> Rot Left";
                    label2.Content = "Balsas - Pasisukti Kairėn";
                    _nui.voiceLeft = false;
                } else if (_nui.voiceRight)
                {
                    _drone.Navigate(0, 0, 0.5f, 0);
                    label1.Content = "Voice -> Rot Right";
                    label2.Content = "Balsas - Pasisukti Desinėn";
                    _nui.voiceRight = false;
                } else if (_nui.voiceStop)
                {
                    _drone.Navigate(0, 0, 0, 0);
                    label1.Content = "Voice -> Stop";
                    label2.Content = "Balsas - Stop!";
                    _nui.voiceStop = false;
                }
            }
        }

        
        // judesiai ir drono elgesys
        private void UpdateMotion(float gameTime)
        {
            
            // bandom kad parasytu tiesiog koki veiksma rodai

            if (KinectMotion.leftHandStraight(_nui.head, _nui.lhand, _nui.lshoulder,
                _nui.rshoulder, _nui.chip))
            {
                label2.Content = "leftHandStraight";
            }

            if (KinectMotion.rightHandStraight(_nui.head, _nui.rhand, _nui.lshoulder,
                _nui.rshoulder, _nui.chip))
            {
                label2.Content = "rightHandStraight";
            }

            if (KinectMotion.rightHandRaised(_nui.head, _nui.rhand))
            {
                label2.Content = "rightHandRaised";
            }
            
            if (KinectMotion.leftHandRaised(_nui.head, _nui.lhand))
            {
                label2.Content = "leftHandRaised";
            }

            if (KinectMotion.rightHandForward(_nui.head, _nui.rhand, _nui.lshoulder,
                _nui.rshoulder, _nui.chip))
            {
                label2.Content = "rightHandForward";
            }

            if (KinectMotion.rightHandBackward(_nui.head, _nui.rhand, _nui.lshoulder,
                _nui.rshoulder, _nui.chip))
            {
                label2.Content = "rightHandBackward";
            }

            // ----------------------------------------------------------------


            
            
           /* if (_drone.droneControl.IsConnected)
            {
                if (!_drone.droneControl.IsFlying)
                {
                    updateTakeOff(gameTime);
                }

                if (!conMassage)
                {
                    conMassage = true;
                    _drone.FlatTrim();
                }

                if (takeOffCom && !_drone.droneControl.IsFlying)
                {
                    label1.Content = "Motion - Take off";
                    label2.Content = "Judesys - Pakilti";
                    _drone.Takeoff();
                    takeOffCom = false;
                    islanding = false;
                    _takingOff = new Timer(1500);
                }
            }




            if (_drone.droneControl.IsFlying && takeOffCom == false)
            {
                if ((KinectMotion.leftHandStraight(_nui.head, _nui.lhand, _nui.lshoulder, _nui.rshoulder, _nui.chip)) &&
                    (KinectMotion.rightHandStraight(_nui.head, _nui.rhand, _nui.lshoulder, _nui.rshoulder, _nui.chip)))
                {
                    _drone.Land();
                    islanding = true;
                    label2.Content = "Judesys - Nusileisti";
                }
            }

            if (_drone.droneControl.IsFlying && UpdateTimer(ref _takingOff, _drone.droneControl.IsFlying, gameTime))
            {
                if (UpdateTimer(ref _trotleft, KinectMotion.leftHandRaised(_nui.head, _nui.lhand) && !KinectMotion.rightHandStraight(_nui.head, _nui.rhand, _nui.lshoulder, _nui.rshoulder, _nui.chip), gameTime))
                {

                    _drone.Navigate(0, 0, -1, 0);
                    label2.Content = "Judesys - Suktis kairėn";
                    command = true;
                    return;
                }

                if (UpdateTimer(ref _trotright, KinectMotion.rightHandRaised(_nui.head, _nui.rhand) && !KinectMotion.leftHandStraight(_nui.head, _nui.lhand, _nui.lshoulder, _nui.rshoulder, _nui.chip), gameTime))
                {
                    _drone.Navigate(0, 0, 1, 0);
                    label2.Content = "Judesys - Suktis dešinėn";
                    command = true;
                    return;
                }

                if (UpdateTimer(ref _tforward, KinectMotion.rightHandForward(_nui.head, _nui.rhand, _nui.lshoulder, _nui.rshoulder, _nui.chip), gameTime))
                {
                    _drone.Navigate(0, -0.1f, 0, 0);
                    label2.Content = "Judesys - Pirmyn";
                    command = true;
                    return;
                }
                if (UpdateTimer(ref _tbackward, KinectMotion.rightHandBackward(_nui.head, _nui.rhand, _nui.lshoulder, _nui.rshoulder, _nui.chip), gameTime))
                {
                    _drone.Navigate(0, 0.1f, 0, 0);
                    label2.Content = "Judesys - Atgal";
                    command = true;
                    return;
                }
                if (UpdateTimer(ref _tflyleft, (KinectMotion.rightHandRaised(_nui.head, _nui.rhand)) &&
                        (KinectMotion.leftHandStraight(_nui.head, _nui.lhand, _nui.lshoulder, _nui.rshoulder, _nui.chip)), gameTime))
                {
                    _drone.Navigate(-0.1f, 0, 0, 0);
                    label2.Content = "Judesys - Skristi kairėn";
                    command = true;
                    return;
                }
                if (UpdateTimer(ref _tflyright, KinectMotion.leftHandRaised(_nui.head, _nui.lhand) &&
                        KinectMotion.rightHandStraight(_nui.head, _nui.rhand, _nui.lshoulder, _nui.rshoulder, _nui.chip), gameTime))
                {
                    _drone.Navigate(0.1f, 0, 0, 0);
                    label2.Content = "Judesys - Skristi dešinėn";
                    command = true;
                    return;
                }
                if (islanding == false && command == true)
                {
                    _drone.Navigate(0, 0, 0, 0);
                    command = false;
                }

            }*/
        }


        private bool UpdateTimer(ref Timer t, bool isacc, float gt)
        {
            if (t != null)
            {
                if (isacc)
                {
                    t.update(gt);
                    if (t.finnish)
                        return true;
                    else
                        return false;
                }
                else
                {
                    t = null;
                    return false;
                }
            }
            else
            {
                if (isacc)
                {
                    t = new Timer(750);
                    return false;
                }
                return false;
            }
        }

        private void updateTakeOff(float dt)
        {
            if (_nui.isSkeleton && !takeOffCom)
            {
                if (_timer == null)
                    _timer = new Timer(3000);
                else
                {
                    if ((KinectMotion.leftHandRaised(_nui.head, _nui.lhand)) &&
                    (KinectMotion.rightHandRaised(_nui.head, _nui.rhand)))
                    {
                        _timer.update(dt);
                        if (_timer.finnish)
                        {
                            takeOffCom = true;
                            _timer = null;
                        }
                    }
                    else
                        _timer = null;
                }
            }
        }


       /* private void SetNewDroidImage()
        {
            if (_drone.droneControl.ImageSourceImage != null)
               image3.Source = _drone.droneControl.ImageSourceImage;
        }*/

        private void timerStatusUpdate_Tick(object sender, EventArgs e)
        {

           
            label1.Content = _drone.msg;

            UpdateStatus();
            
        }

        private void timerSUpdate_Tick(object sender, EventArgs e)
        {
            //Title = _nui.FrameRate.ToString();
            SkeletonDraw();
        }

        private void timerDUpdate_Tick(object sender, EventArgs e)
        {
            //label1.Content = _drone.msg;
            if (_nui.depthBitmap != null && image1.Source == null)
            {
                image1.Source = _nui.depthBitmap;
                timerDepthUpdate.Interval = new TimeSpan(0, 0, 0, 7, 0);
            }

        }

        private void timerCUpdate_Tick(object sender, EventArgs e)
        {
            //label1.Content = _drone.msg;
            //UpdateStatus();

            if (_nui.colorBitmap != null && image2.Source == null)
            {
                image2.Source = _nui.colorBitmap;
                timerColorUpdate.Interval = new TimeSpan(0, 0, 0, 7, 0);
            }
        }
        
        private void timerVideoUpdate_Tick(object sender, EventArgs e)
        {
         //   SetNewDroidImage();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // per čia eina keist 'paveiksliukų' dydį
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _drone.Disconnect();
            _nui.StopKinect();
        }
        
        protected override void OnKeyDown(KeyEventArgs keyEvent)
        {
            if (keyEvent.Key == Key.V)
            {
             //   _nui.Start();
             //   motion = false;
             //   Title = "Balso sąsaja";
            }
            else if (keyEvent.Key == Key.M)
            {
             //   motion = true;
             //   Title = "Judesio sąsaja";
            }
            else if (keyEvent.Key == Key.Space)
            {
                _drone.Land();
            }
        }
    }
}
