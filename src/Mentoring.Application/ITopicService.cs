using System.Collections.Generic;
using Mentoring.Core;

namespace Mentoring.Application
{
    public interface ITopicService
    {
        IEnumerable<Topic> GetAll();

        Topic GetById(int id);
    }
}