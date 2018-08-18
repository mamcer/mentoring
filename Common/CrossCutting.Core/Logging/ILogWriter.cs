namespace CrossCutting.Core.Logging
{
    using System;

    public interface ILogWriter
    {
        void Write(string message);
       
        void Write(string message, Exception ex);
    }
}