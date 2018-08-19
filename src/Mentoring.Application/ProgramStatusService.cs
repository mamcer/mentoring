using System;
using System.Linq;
using CrossCutting.Core.Logging;
using Mentoring.Core;
using Mentoring.Core.Enums;
using Mentoring.Data;

namespace Mentoring.Application
{
    public class ProgramStatusService : IProgramStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;

        public ProgramStatusService(IUnitOfWork unitOfWork, ILogManager logManager)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
        }

        public void SaveProgramStatus(ProgramStatusCode programStatusCode, string programStatusDescription)
        {
            try
            {
                var programStatus = new ProgramStatus
                {
                    CreationDate = DateTime.Now,
                    StatusCode = (int)programStatusCode,
                    StatusDescription = programStatusDescription
                };

                _unitOfWork.ProgramStatusRepository.Insert(programStatus);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.ProgramStatusService.SaveStatus", ex);
            }
        }

        public ProgramStatus GetCurrentProgramStatus()
        {
            try
            {
                var lastProgramStatus = _unitOfWork.ProgramStatusRepository.Search(null, q => q.OrderByDescending(p => p.CreationDate)).FirstOrDefault();
                return lastProgramStatus;
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.ProgramStatusService.GetCurrentProgramStatus", ex);
            }

            return null;
        }
    }
}