using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARDrone.Control;
using ARDrone.Control.Commands;
using ARDrone.Control.Data;
using ARDrone.Control.Events;

namespace ARDroneKinect
{
    public class DroneControler
    {

        public DroneControl droneControl;
        private DroneConfig currentDroneConfig;
        private LogManager log;

        public DroneControler(LogManager lg)
        {
            log = lg;

            currentDroneConfig = new DroneConfig();
            currentDroneConfig.Load();

            droneControl = new DroneControl(currentDroneConfig);


            droneControl.Error += droneControl_Error_Async;
            droneControl.ConnectionStateChanged += droneControl_ConnectionStateChanged_Async;
            droneControl.NetworkConnectionStateChanged += droneControl_NetworkConnectionStateChanged_Async;
        }

        // Event handlers
        private void droneControl_NetworkConnectionStateChanged_Async(object sender, DroneNetworkConnectionStateChangedEventArgs e)
        {
            //Dispatcher.BeginInvoke(new DroneNetworkConnectionStateChangedEventHandler(droneControl_NetworkConnectionStateChanged_Sync), sender, e);
            if (e.State == DroneNetworkConnectionState.ScanningForNewNetworks)
            {
                UpdateUISync("Drone network scan");
            }
            if (e.State == DroneNetworkConnectionState.TryingToConnect)
            {
                UpdateUISync("Trying to connect to network");
            }
            if (e.State == DroneNetworkConnectionState.PingSuccesful)
            {
                UpdateUISync("Ping succesful");
                Connect();
            }
            if (e.State == DroneNetworkConnectionState.NotConnected)
            {
                UpdateUISync("Not Connected");
                Connect();
            }
        }


        private void droneControl_Error_Async(object sender, DroneErrorEventArgs e)
        {
            // Dispatcher.BeginInvoke(new DroneErrorEventHandler(droneControl_Error_Sync), sender, e);
        }

        private void droneControl_Error_Sync(object sender, DroneErrorEventArgs e)
        {
            //HandleError(e);
        }

        private void droneControl_ConnectionStateChanged_Async(object sender, DroneConnectionStateChangedEventArgs e)
        {
            int x;
            //Dispatcher.BeginInvoke(new DroneConnectionStateChangedEventHandler(droneControl_ConnectionStateChanged_Sync), sender, e);
        }

        private void droneControl_ConnectionStateChanged_Sync(object sender, DroneConnectionStateChangedEventArgs e)
        {
            int x;
            //HandleConnectionStateChange(e);
        }

        private void UpdateUIAsync(String message)
        {
            log.addLog(message);
            //Dispatcher.BeginInvoke(new OutputEventHandler(UpdateUISync), message);
        }

        private void UpdateUISync(String message)
        {
            log.addLog(message);
        }

        public void ConnectToNetwork()
        {
            droneControl.ConnectToDroneNetwork();
            //UpdateUISync("Connecting to the drone network");
        }

        public void Connect()
        {
            droneControl.ConnectToDrone();
            UpdateUISync("Connecting to the drone");

        }

        public void Disconnect()
        {
            droneControl.Disconnect();
            UpdateUISync("Disconnecting from drone");
        }


        public void Takeoff()
        {
            Command takeOffCommand = new FlightModeCommand(DroneFlightMode.TakeOff);

            if (!droneControl.IsCommandPossible(takeOffCommand))
                return;

            droneControl.SendCommand(takeOffCommand);
            UpdateUIAsync("Taking off");
        }

        public void Land()
        {
            Command landCommand = new FlightModeCommand(DroneFlightMode.Land);

            if (!droneControl.IsCommandPossible(landCommand))
                return;

            droneControl.SendCommand(landCommand);
            UpdateUIAsync("Landing");
        }

        public void Emergency()
        {
            Command emergencyCommand = new FlightModeCommand(DroneFlightMode.Emergency);

            if (!droneControl.IsCommandPossible(emergencyCommand))
                return;

            droneControl.SendCommand(emergencyCommand);
            UpdateUIAsync("Sending emergency signal");
        }

        public void FlatTrim()
        {
            Command resetCommand = new FlightModeCommand(DroneFlightMode.Reset);
            Command flatTrimCommand = new FlatTrimCommand();

            if (!droneControl.IsCommandPossible(resetCommand) || !droneControl.IsCommandPossible(flatTrimCommand))
                return;

            droneControl.SendCommand(resetCommand);
            droneControl.SendCommand(flatTrimCommand);
            UpdateUIAsync("Sending flat trim");
        }

        public void EnterHoverMode()
        {
            Command enterHoverModeCommand = new HoverModeCommand(DroneHoverMode.Hover);

            if (!droneControl.IsCommandPossible(enterHoverModeCommand))
                return;

            droneControl.SendCommand(enterHoverModeCommand);
            UpdateUIAsync("Entering hover mode");
        }

        public void LeaveHoverMode()
        {
            Command leaveHoverModeCommand = new HoverModeCommand(DroneHoverMode.StopHovering);

            if (!droneControl.IsCommandPossible(leaveHoverModeCommand))
                return;

            droneControl.SendCommand(leaveHoverModeCommand);
            UpdateUIAsync("Leaving hover mode");
        }

        public void Navigate(float roll, float pitch, float yaw, float gaz)
        {
            FlightMoveCommand flightMoveCommand = new FlightMoveCommand(roll, pitch, yaw, gaz);

            if (droneControl.IsCommandPossible(flightMoveCommand))
                droneControl.SendCommand(flightMoveCommand);
        }



    }
}
