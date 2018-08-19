using System.ComponentModel;

namespace Mentoring.Core.Enums
{
    public enum MeetingMode
    {
        [Description("Face to Face")]
        FaceToFace,
        [Description("Online")]
        Online,
        [Description("Both")]
        Both
    }
}