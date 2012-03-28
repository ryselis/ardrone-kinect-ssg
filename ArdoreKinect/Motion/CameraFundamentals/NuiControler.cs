using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;


namespace CameraFundamentals
{
    //Initializes NUI runtime and handles every Skeleton Frame
    public class NUIcontroler
    {
        //Kinect Runtime
        public KinectSensor nui;

        private int sw;
        private int sh;

        public bool isSkeleton;

        public Joint head;
        public Joint rhand;
        public Joint lhand;
        public Joint chip;
        public Joint lshoulder;
        public Joint rshoulder;

        public bool hasImg;

        public bool voiceTakingOff = false;
        public bool voiceLanding = false;
        public bool voiceLeft = false;
        public bool voiceStop = false;
        public bool voiceRight = false;

        public int xMax = 640;
        public int yMax = 480;

        public Skeleton skelet;

        const float MaxDepthDistance = 4095; // max value returned
        const float MinDepthDistance = 850; // min value returned
        const float MaxDepthDistanceOffset = MaxDepthDistance - MinDepthDistance;


        private SpeechRecognitionEngine speechRecognizer;
        bool speechNotRecognized;
        private EnergyCalculatingPassThroughStream stream;

        bool closing = false;
        const int skeletonCount = 6;
        Skeleton[] allSkeletons = new Skeleton[skeletonCount];

        //public Color[] color;
        /**
         * Constructor
         * @param sw - scale width
         * @param sh - scale height
         **/
        public NUIcontroler(int sw, int sh)
        {
            isSkeleton = false;
            this.sw = sw;
            this.sh = sh;
            hasImg = false;
        }

        public void init()
        {
            SetupKinectManually();
        }

        private void SetupKinectManually()
        {
            if (KinectSensor.KinectSensors.Count > 0)
            {
                //use first Kinect
                nui = KinectSensor.KinectSensors[0];


                var parameters = new TransformSmoothParameters
                {
                    Smoothing = 0.3f,
                    Correction = 0.0f,
                    Prediction = 0.0f,
                    JitterRadius = 1.0f,
                    MaxDeviationRadius = 0.5f
                };

                //nui.SkeletonStream.Enable(parameters);


                //register for event and enable Kinect sensor features you want
                nui.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                nui.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                nui.SkeletonStream.Enable();
                nui.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(nui_DepthFrameReady);
                nui.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(nui_ColorFrameReady);
                nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
                
                nui.Start();
                //Start();

            }
        }


        public void Start()
        {
            this.speechRecognizer = this.CreateSpeechRecognizer();
            var audioSource = this.nui.AudioSource;
            audioSource.BeamAngleMode = BeamAngleMode.Adaptive;
            var kinectStream = audioSource.Start();
            this.stream = new EnergyCalculatingPassThroughStream(kinectStream);
            this.speechRecognizer.SetInputToAudioStream(
                this.stream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            this.speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
        }


        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        private SpeechRecognitionEngine CreateSpeechRecognizer()
        {
            RecognizerInfo ri = GetKinectRecognizer();
            if (ri == null)
            {
                return null;
            }

            SpeechRecognitionEngine sre;
            try
            {
                sre = new SpeechRecognitionEngine(ri.Id);
            }
            catch
            {
                return null;
            }

            var choices = new Choices(); 
            choices.Add("fly");
            choices.Add("down");
            choices.Add("left");
            choices.Add("right");
            choices.Add("stop");

            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(choices);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            var g = new Grammar(gb);

            sre.LoadGrammar(g);
            sre.SpeechRecognized += this.sre_SpeechRecognized;
            sre.SpeechHypothesized += this.sre_SpeechHypothesized;
            sre.SpeechRecognitionRejected += this.sre_SpeechRecognitionRejected;

            return sre;
        }

        public string msg = "";

        void sre_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            speechNotRecognized = true;
        }

        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            msg = "Hypothesized " + e.Result.Text + " with confidence " + e.Result.Confidence;
            //
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            msg = "Recognized " + e.Result.Text + " with confidence " + e.Result.Confidence;

            if (e.Result.Text == "fly" && e.Result.Confidence > 0.8f)
            {
                resetsound();
                voiceTakingOff = true;
                return;
            }

            if (e.Result.Text == "down" && e.Result.Confidence > 0.9f)
            {
                resetsound();
                voiceLanding = true;
                return;
            }

            if (e.Result.Text == "left" && e.Result.Confidence > 0.9f)
            {
                resetsound();
                voiceLeft = true;
                return;
            }
            if (e.Result.Text == "right" && e.Result.Confidence > 0.9f)
            {
                resetsound();
                voiceRight = true;
                return;
            }
            if (e.Result.Text == "stop" && e.Result.Confidence > 0.9f)
            {
                resetsound();
                voiceStop = true;
                return;
            }
        }

        private void resetsound()
        {
            speechNotRecognized = false;

            voiceTakingOff = false;
            voiceLanding = false;
            voiceLeft = false;
            voiceRight = false;
            voiceStop = false;

        }


        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            UpdateFrameRate();
            if (closing)
            {
                return;
            }
          
            //Get a skeleton
            Skeleton first = GetFirstSkeleton(e);

            if (first == null)
            {
                skelet = null;
                isSkeleton = false; 
                return;
            }
            isSkeleton = true;
            skelet = first;

            head = first.Joints[JointType.Head];
            rhand = first.Joints[JointType.HandRight];
            lhand = first.Joints[JointType.HandLeft];
            chip = first.Joints[JointType.HipCenter];
            lshoulder = first.Joints[JointType.ShoulderLeft];
            rshoulder = first.Joints[JointType.ShoulderRight];
            

        }

        Skeleton GetFirstSkeleton(SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame();
            if (skeletonFrameData == null)
            {
                return null;
            }


            skeletonFrameData.CopySkeletonDataTo(allSkeletons);

            //get the first tracked skeleton
            Skeleton first = (from s in allSkeletons
                                where s.TrackingState == SkeletonTrackingState.Tracked
                                select s).FirstOrDefault();


            return first;
        }


        private static readonly int Bgr32BytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
        private ColorImageFormat lastImageFormat = ColorImageFormat.Undefined;
        private byte[] pixelData;
        public WriteableBitmap colorBitmap;
        
        void nui_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            
            ColorImageFrame imageFrame = e.OpenColorImageFrame();
            if (imageFrame != null)
            {
                // We need to detect if the format has changed.
                bool haveNewFormat = this.lastImageFormat != imageFrame.Format;

                if (haveNewFormat)
                {
                    this.pixelData = new byte[imageFrame.PixelDataLength];
                }

                imageFrame.CopyPixelDataTo(this.pixelData);

                // A WriteableBitmap is a WPF construct that enables resetting the Bits of the image.
                // This is more efficient than creating a new Bitmap every frame.
                if (haveNewFormat)
                {
                    this.colorBitmap = new WriteableBitmap(
                        imageFrame.Width,
                        imageFrame.Height,
                        96,  // DpiX
                        96,  // DpiY
                        PixelFormats.Bgr32,
                        null);
                }

                this.colorBitmap.WritePixels(
                    new Int32Rect(0, 0, imageFrame.Width, imageFrame.Height),
                    this.pixelData,
                    imageFrame.Width * Bgr32BytesPerPixel,
                    0);

                this.lastImageFormat = imageFrame.Format;
                
            }

                
        }

        public int frameRate = -1;
        private bool collectFrameRate = true;
        private DateTime lastTime = DateTime.MaxValue;

        public bool CollectFrameRate
        {
            get
            {
                return this.collectFrameRate;
            }

            set
            {
                if (value != this.collectFrameRate)
                {
                    this.collectFrameRate = value;
                    //this.NotifyPropertyChanged("CollectFrameRate");
                }
            }
        }

        public int FrameRate
        {
            get
            {
                return this.frameRate;
            }

            private set
            {
                if (this.frameRate != value)
                {
                    this.frameRate = value;
                    //this.NotifyPropertyChanged("FrameRate");
                }
            }
        }

        protected int TotalFrames { get; set; }

        protected int LastFrames { get; set; }

        

        protected void UpdateFrameRate()
        {
            
                ++this.TotalFrames;

                DateTime cur = DateTime.Now;
                var span = cur.Subtract(this.lastTime);
                if (this.lastTime == DateTime.MaxValue || span >= TimeSpan.FromSeconds(1))
                {
                    // A straight cast will truncate the value, leading to chronic under-reporting of framerate.
                    // rounding yields a more balanced result
                    this.FrameRate = (int)Math.Round((this.TotalFrames - this.LastFrames) / span.TotalSeconds);
                    this.LastFrames = this.TotalFrames;
                    this.lastTime = cur;
                }
            
        }

        private static readonly int[] IntensityShiftByPlayerR = { 1, 2, 0, 2, 0, 0, 2, 0 };
        private static readonly int[] IntensityShiftByPlayerG = { 1, 2, 2, 0, 2, 0, 0, 1 };
        private static readonly int[] IntensityShiftByPlayerB = { 1, 0, 2, 2, 0, 2, 0, 2 };

        private const int RedIndex = 2;
        private const int GreenIndex = 1;
        private const int BlueIndex = 0;
        
        private DepthImageFormat lastDImageFormat;
        private short[] pixelDData;

        // We want to control how depth data gets converted into false-color data
        // for more intuitive visualization, so we keep 32-bit color frame buffer versions of
        // these, to be updated whenever we receive and process a 16-bit frame.
        private byte[] depthFrame32;
        public WriteableBitmap depthBitmap;
        public DepthImageFrame depthFrameFull;

        void nui_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            DepthImageFrame depthFrame = e.OpenDepthImageFrame();

            if (depthFrame != null)
            {
                depthFrameFull = depthFrame;
                
                // We need to detect if the format has changed.
                bool haveNewFormat = this.lastDImageFormat != depthFrame.Format;

                if (haveNewFormat)
                {
                    this.pixelDData = new short[depthFrame.PixelDataLength];
                    this.depthFrame32 = new byte[depthFrame.Width * depthFrame.Height * Bgr32BytesPerPixel];
                }

                depthFrame.CopyPixelDataTo(this.pixelDData);

                byte[] convertedDepthBits = this.ConvertDepthFrame(this.pixelDData, ((KinectSensor)sender).DepthStream);

                // A WriteableBitmap is a WPF construct that enables resetting the Bits of the image.
                // This is more efficient than creating a new Bitmap every frame.
                if (haveNewFormat)
                {
                    this.depthBitmap = new WriteableBitmap(
                        depthFrame.Width,
                        depthFrame.Height,
                        96,  // DpiX
                        96,  // DpiY
                        PixelFormats.Bgr32,
                        null);
                }

                this.depthBitmap.WritePixels(
                    new Int32Rect(0, 0, depthFrame.Width, depthFrame.Height),
                    convertedDepthBits,
                    depthFrame.Width * Bgr32BytesPerPixel,
                    0);

                this.lastDImageFormat = depthFrame.Format;
            }
                 
        }

        private byte[] ConvertDepthFrame(short[] depthFrame, DepthImageStream depthStream)
        {
            int tooNearDepth = depthStream.TooNearDepth;
            int tooFarDepth = depthStream.TooFarDepth;
            int unknownDepth = depthStream.UnknownDepth;

            for (int i16 = 0, i32 = 0; i16 < depthFrame.Length && i32 < this.depthFrame32.Length; i16++, i32 += 4)
            {
                int player = depthFrame[i16] & DepthImageFrame.PlayerIndexBitmask;
                int realDepth = depthFrame[i16] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                // transform 13-bit depth information into an 8-bit intensity appropriate
                // for display (we disregard information in most significant bit)
                byte intensity = (byte)(~(realDepth >> 4));

                if (player == 0 && realDepth == 0)
                {
                    this.depthFrame32[i32 + RedIndex] = 100;
                    this.depthFrame32[i32 + GreenIndex] = 0;
                    this.depthFrame32[i32 + BlueIndex] = 0;
                }
                else if (player == 0 && realDepth == tooFarDepth)
                {
                    this.depthFrame32[i32 + RedIndex] = 30;
                    this.depthFrame32[i32 + GreenIndex] = 0;
                    this.depthFrame32[i32 + BlueIndex] = 100;
                }
                else if (player == 0 && realDepth == unknownDepth)
                {
                    this.depthFrame32[i32 + RedIndex] = 100;
                    this.depthFrame32[i32 + GreenIndex] = 180;
                    this.depthFrame32[i32 + BlueIndex] = 40;
                }
                else
                {
                    /*
                    this.depthFrame32[i32 + RedIndex] = 5;
                    this.depthFrame32[i32 + GreenIndex] = 5;
                    this.depthFrame32[i32 + BlueIndex] = 40;
                     */
                    // tint the intensity by dividing by per-player values
                    
                    this.depthFrame32[i32 + RedIndex] = (byte)(intensity >> IntensityShiftByPlayerR[player]);
                    this.depthFrame32[i32 + GreenIndex] = (byte)(intensity >> IntensityShiftByPlayerG[player]);
                    this.depthFrame32[i32 + BlueIndex] = (byte)(intensity >> IntensityShiftByPlayerB[player]);
                     
                }
            }

            return this.depthFrame32;
        }

        public static byte CalculateIntensityFromDepth(int distance)
        {
            //formula for calculating monochrome intensity for histogram
            return (byte)(255 - (255 * Math.Max(distance - MinDepthDistance, 0)
                / (MaxDepthDistanceOffset)));
        }



        public void StopKinect()
        {
            closing = true;
            if (nui != null)
            {
                if (nui.IsRunning)
                {
                    //stop sensor 
                    nui.Stop();

                    if (this.speechRecognizer != null)
                    {
                        this.speechRecognizer.RecognizeAsyncCancel();
                        this.speechRecognizer.RecognizeAsyncStop();
                    }

                    //stop audio if not null
                    if (nui.AudioSource != null)
                    {
                        nui.AudioSource.Stop();
                    }


                }
            }
        }
       
    }


    class EnergyCalculatingPassThroughStream : Stream
    {
        private const int SamplesPerPixel = 10;
        private const int WaveImageWidth = 500;
        private const int WaveImageHeight = 100;

        private readonly double[] energy = new double[WaveImageWidth];
        private readonly object syncRoot = new object();
        private readonly Stream baseStream;

        private int index;
        private int sampleCount;
        private double avgSample;

        public EnergyCalculatingPassThroughStream(Stream stream)
        {
            this.baseStream = stream;
        }

        public override long Length
        {
            get { return this.baseStream.Length; }
        }

        public override long Position
        {
            get { return this.baseStream.Position; }
            set { this.baseStream.Position = value; }
        }

        public override bool CanRead
        {
            get { return this.baseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.baseStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.baseStream.CanWrite; }
        }

        public override void Flush()
        {
            this.baseStream.Flush();
        }

        public void GetEnergy(double[] energyBuffer)
        {
            lock (this.syncRoot)
            {
                int energyIndex = this.index;
                for (int i = 0; i < this.energy.Length; i++)
                {
                    energyBuffer[i] = this.energy[energyIndex];
                    energyIndex++;
                    if (energyIndex >= this.energy.Length)
                    {
                        energyIndex = 0;
                    }
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int retVal = this.baseStream.Read(buffer, offset, count);
            const double A = 0.3;
            lock (this.syncRoot)
            {
                for (int i = 0; i < retVal; i += 2)
                {
                    short sample = BitConverter.ToInt16(buffer, i + offset);
                    this.avgSample += sample * sample;
                    this.sampleCount++;

                    if (this.sampleCount == SamplesPerPixel)
                    {
                        this.avgSample /= SamplesPerPixel;

                        this.energy[this.index] = .2 + ((this.avgSample * 11) / (int.MaxValue / 2));
                        this.energy[this.index] = this.energy[this.index] > 10 ? 10 : this.energy[this.index];

                        if (this.index > 0)
                        {
                            this.energy[this.index] = (this.energy[this.index] * A) + ((1 - A) * this.energy[this.index - 1]);
                        }

                        this.index++;
                        if (this.index >= this.energy.Length)
                        {
                            this.index = 0;
                        }

                        this.avgSample = 0;
                        this.sampleCount = 0;
                    }
                }
            }

            return retVal;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.baseStream.Write(buffer, offset, count);
        }
    }
}