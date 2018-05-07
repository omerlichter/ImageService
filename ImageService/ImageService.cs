using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Server;
using ImageService.Modal;
using ImageService.Controller;
using System.Configuration;

namespace ImageService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };

    /// <summary>
    /// Image service class.
    /// </summary>
    public partial class ImageService : ServiceBase
    {
        #region Members
        private ImageServer m_imageServer;          // The Image Server
        private IImageServiceModal modal;
        private IImageController controller;
        private ILoggingService logging;
        private int eventId = 1;
        #endregion

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="args">args from command line</param>
        public ImageService(string[] args)
        {
            // initialize
            InitializeComponent();
            // get values from app.config
            string eventSourceName = ConfigurationManager.AppSettings.Get("SourceName");
            string logName = ConfigurationManager.AppSettings.Get("LogName");
            string outputFolder = ConfigurationManager.AppSettings.Get("OutputDir");
            int thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
           
            // create logger and logger history
            logging = new LoggingService();
            ILoggingHistory loggingHistory = new LoggingHistory();
            logging.MessageRecieved += eventLogMessage;
            logging.MessageRecieved += loggingHistory.AddLog;

            // create image service model
            modal = new ImageServiceModal(outputFolder, thumbnailSize);

            // create image controller
            controller = new ImageController(modal, loggingHistory);
        }

        /// <summary>
        /// on start of the service.
        /// </summary>
        /// <param name="args">args from command line</param>
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // write start to event log
            this.logging.Log("In OnStart", MessageTypeEnum.INFO);

            // create image server
            m_imageServer = new ImageServer(controller, logging, 12345);
            this.controller.SetCloseCommand(this.m_imageServer);

            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /// <summary>
        /// the function called in event log.
        /// </summary>
        /// <param name="sender">the sender event object</param>
        /// <param name="e">args for the event, message and type</param>
        private void eventLogMessage(Object sender, MessageRecievedEventArgs e)
        {
            // get the type of the message
            EventLogEntryType type;
            switch (e.Status)
            {
                case MessageTypeEnum.WARNING: type = EventLogEntryType.Warning; break;
                case MessageTypeEnum.FAIL: type = EventLogEntryType.Error; break;
                case MessageTypeEnum.INFO: type = EventLogEntryType.Information; break;
                default: type = EventLogEntryType.Information; break;
            }
            eventLog1.WriteEntry(e.Message, type);
        }

        /// <summary>
        /// on continue of the service.
        /// </summary>
        protected override void OnContinue()
        {
            // write continue to event log
            this.logging.Log("In OnContinue", MessageTypeEnum.INFO);
        }

        /// <summary>
        /// on stop of the service
        /// </summary>
        protected override void OnStop()
        {
            // Update the service state to stop Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // write stop to event log
            this.logging.Log("In OnStop", MessageTypeEnum.INFO);

            // close the server
            this.m_imageServer.CloseServer();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /// <summary>
        /// the function called every minute, to monitoring the service.
        /// </summary>
        /// <param name="sender">the sender event object</param>
        /// <param name="args">args for the event</param>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }
    }
}
