﻿using SuperRocket.Orchard.Application.Services.Enums;
using SuperRocket.Orchard.Core.SharePoint;
using SuperRocket.Orchard.Core.SharePoint.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperRocket.Orchard.Application.Services.Args
{
    internal class SyncServiceProgressEventArgs : EventArgs
    {
        public SyncConfiguration Configuration { get; set; }
        public SyncEventType EventType { get; }
        public ProgressStatus Status { get; }
        public int Percent { get; }
        public string Message { get; }
        public Exception InnerException { get; }

        public SyncServiceProgressEventArgs(SyncConfiguration configuration, SyncEventType syncEventType, ProgressStatus status, int percent, string message, Exception innerException = null)
        {
            this.Configuration = configuration;
            this.EventType = syncEventType;
            this.Status = status;
            this.Percent = percent;
            this.Message = message;
            this.InnerException = innerException;
        }
    }
}
