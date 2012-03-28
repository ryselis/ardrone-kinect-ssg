using System;

namespace ARDroneKinect
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ARDroneKinect game = new ARDroneKinect())
            {
                game.Run();
            }
        }
    }
#endif
}

