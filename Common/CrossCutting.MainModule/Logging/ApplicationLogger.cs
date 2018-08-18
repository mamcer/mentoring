namespace CrossCutting.MainModule.Logging
{
    using System.Diagnostics;
    using Core.Logging;

    public class ApplicationLogger : IApplicationLogger
    {
        private ILogWriter _critical;
        private ILogWriter _error;
        private ILogWriter _warning;
        private ILogWriter _information;

        public ILogWriter Critical
        {
            get
            {
                if (_critical == null)
                {
                    _critical = new MelLogWriter(TraceEventType.Critical);
                }

                return _critical;
            }
        }

        public ILogWriter Error
        {
            get
            {
                if (_error == null)
                {
                    _error = new MelLogWriter(TraceEventType.Error);
                }

                return _error;
            }
        }

        public ILogWriter Warning
        {
            get
            {
                if (_warning == null)
                {
                    _warning = new MelLogWriter(TraceEventType.Warning);
                }

                return _warning;
            }
        }

        public ILogWriter Information
        {
            get
            {
                if (_information == null)
                {
                    _information = new MelLogWriter(TraceEventType.Information);
                }

                return _information;
            }
        }

        public string Name
        {
            get
            {
                return "MS Enterprise Library Application Logger";
            }
        }
    }
}