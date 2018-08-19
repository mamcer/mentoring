using System;
using System.Collections.Generic;
using CrossCutting.Core.Logging;
using Mentoring.Core;
using Mentoring.Data;

namespace Mentoring.Application
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;

        public TimeSlotService(IUnitOfWork unitOfWork, ILogManager logManager)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
        }

        public IEnumerable<TimeSlot> GetAll()
        {
            try
            {
                return _unitOfWork.TimeSlotRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.TimeSlotService.GetAll", ex);
                return null;
            }
        }
    }
}