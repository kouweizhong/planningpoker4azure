﻿// <copyright>
// Copyright (c) 2012 Rasto Novotny
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duracellko.PlanningPoker.Configuration
{
    /// <summary>
    /// Defines configuration of planning poker.
    /// </summary>
    public interface IPlanningPokerConfiguration
    {
        /// <summary>
        /// Gets the time, after which is client disconnected when he/she does not check for new messages.
        /// </summary>
        /// <value>The client inactivity timeout.</value>
        TimeSpan ClientInactivityTimeout { get; }

        /// <summary>
        /// Gets the interval, how often is executed job to search for and disconnect inactive clients.
        /// </summary>
        /// <value>The client inactivity check interval.</value>
        TimeSpan ClientInactivityCheckInterval { get; }

        /// <summary>
        /// Gets the time, how long can client wait for new message. Empty message collection is sent to the client after the specified time.
        /// </summary>
        /// <value>The wait for message timeout.</value>
        TimeSpan WaitForMessageTimeout { get; }
    }
}
