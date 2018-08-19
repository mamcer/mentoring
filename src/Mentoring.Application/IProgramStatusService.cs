using Mentoring.Core;
using Mentoring.Core.Enums;

namespace Mentoring.Application
{
    public interface IProgramStatusService
    {
        ProgramStatus GetCurrentProgramStatus();

        void SaveProgramStatus(ProgramStatusCode programStatusCode, string programStatusDescription);
    }
}