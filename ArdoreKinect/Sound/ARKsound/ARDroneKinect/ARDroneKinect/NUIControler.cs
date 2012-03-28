/**
 * Copyright 2011 Justas Salkevicius
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 **/
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;



namespace ARDroneKinect
{
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
        LogManager log;

        public NUIcontroler(int sw, int sh, LogManager l)
        {
            isSkeleton = false;
            log = l;
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

                this.speechRecognizer = this.CreateSpeechRecognizer();

                nui.Start();
                Start();

            }
        }


        private void Start()
        {
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

        void sre_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {

            log.addLog("Rejected " + e.Result.Text + " with confidence " + e.Result.Confidence);
            speechNotRecognized = true;
        }

        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            log.addLog("Hypothesized " + e.Result.Text + " with confidence " + e.Result.Confidence);
            
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            log.addLog("Recognized " + e.Result.Text + " with confidence " + e.Result.Confidence);
            if (e.Result.Text == "fly" && e.Result.Confidence > 0.6f)
            {
                resetsound();
                voiceTakingOff = true;
                return;
            }

            if (e.Result.Text == "down" && e.Result.Confidence > 0.6f)
            {
                resetsound();
                voiceLanding = true;
                return;
            }

            if (e.Result.Text == "left" && e.Result.Confidence > 0.6f)
            {
                resetsound();
                voiceLeft = true;
                return;
            }
            if (e.Result.Text == "right" && e.Result.Confidence > 0.6f)
            {
                resetsound();
                voiceRight = true;
                return;
            }
            if (e.Result.Text == "stop" && e.Result.Confidence > 0.6f)
            {
                resetsound();
                voiceStop = true;
                return;
            }
        }

        public void resetsound()
        {
            //speechNotRecognized = false;

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
