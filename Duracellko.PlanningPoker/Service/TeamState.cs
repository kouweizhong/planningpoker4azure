﻿// <copyright>
// Copyright (c) 2012 Rasto Novotny
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Duracellko.PlanningPoker.Service
{
    /// <summary>
    /// Specifies status if Scrum team.
    /// </summary>
    [Serializable]
    [DataContract(Name = "teamState", Namespace = Namespaces.PlanningPokerData)]
    public enum TeamState
    {
        /// <summary>
        /// Scrum team is initial state and estimation has not started yet.
        /// </summary>
        Initial,

        /// <summary>
        /// Estimation is in progress. Members can pick their estimations.
        /// </summary>
        EstimationInProgress,

        /// <summary>
        /// All members picked estimations and the estimation is finished.
        /// </summary>
        EstimationFinished,

        /// <summary>
        /// Estimation was canceled by Scrum master.
        /// </summary>
        EstimationCanceled
    }
}
