﻿using NServiceBus;

namespace Common.PubSub.Messages
{
    public class EnergyBalanceChanged : IEvent
    {
        public long EnergySourceGid { get; set; }
        public float ImportedEnergy { get; set; }
        public float ProducedEnergy { get; set; }
        public float DemandEnergy { get; set; }
    }
}
