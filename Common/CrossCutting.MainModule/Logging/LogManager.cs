namespace CrossCutting.MainModule.Logging
{
    using Core.Logging;

    public class LogManager : ILogManager
    {
        private readonly IApplicationLogger _logger;

        public LogManager(IApplicationLogger logger)
        {
            _logger = logger;
        }

        public IApplicationLogger DefaultLogger
        {
            get
            {
                return _logger;
            }
        }
    }
}