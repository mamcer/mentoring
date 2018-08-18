using System.ComponentModel;

namespace Mentoring.Core.Enums
{
    public enum ProgramStatusCode
    {
        [Description("Program In Progress")]
        ProgramInProgress = 0,
        [Description("Program Renewal")]
        ProgramRenewal = 2
    }
}