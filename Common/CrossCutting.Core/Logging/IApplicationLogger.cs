namespace CrossCutting.Core.Logging
{
    public interface IApplicationLogger
    {
        ILogWriter Critical { get; }

        ILogWriter Error { get; }
        
        ILogWriter Warning { get; }
        
        ILogWriter Information { get; }
        
        string Name { get; }
    }
}