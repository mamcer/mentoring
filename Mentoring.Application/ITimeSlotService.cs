using System.Collections.Generic;
using Mentoring.Core;

namespace Mentoring.Application
{
    public interface ITimeSlotService
    {
        IEnumerable<TimeSlot> GetAll();
    }
}