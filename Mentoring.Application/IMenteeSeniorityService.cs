using System.Collections.Generic;
using Mentoring.Core;

namespace Mentoring.Application
{
    public interface IMenteeSeniorityService
    {
        IEnumerable<MenteeSeniority> GetAll();
    }
}