using System;
using System.Collections.Generic;
using CrossCutting.Core.Logging;
using Mentoring.Core;
using Mentoring.Data;

namespace Mentoring.Application
{
    public class MenteeSeniorityService : IMenteeSeniorityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;

        public MenteeSeniorityService(IUnitOfWork unitOfWork, ILogManager logManager)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
        }

        public IEnumerable<MenteeSeniority> GetAll()
        {
            try
            {
                return _unitOfWork.MenteeSeniorityRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.MenteeSeniorityService.GetAll", ex);
                return null;
            }
        }
    }
}