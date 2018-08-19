using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Core.Logging;
using Mentoring.Core;
using Mentoring.Data;

namespace Mentoring.Application
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;

        public TopicService(IUnitOfWork unitOfWork, ILogManager logManager)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
        }

        public Topic GetById(int id)
        {
            try
            {
                return _unitOfWork.TopicRepository.Search(t => t.Id == id, null, "Mentors").FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.TopicService.GetById", ex);
                return null;
            }
        }

        public IEnumerable<Topic> GetAll()
        {
            try
            {
                return _unitOfWork.TopicRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.TopicService.GetAll", ex);
                return null;
            }
        }
    }
}