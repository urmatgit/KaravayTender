﻿using CleanArchitecture.Razor.Domain.Common;
using CleanArchitecture.Razor.Domain.Entities;

namespace CleanArchitecture.Razor.Domain.Events
{
    public class StageCompositionCreatedEvent : DomainEvent
    {
        public StageCompositionCreatedEvent(StageComposition item)
        {
            Item = item;
        }

        public StageComposition Item { get; }
    }
}
