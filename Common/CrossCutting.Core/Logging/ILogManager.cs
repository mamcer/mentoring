namespace CrossCutting.Core.Logging
{
    public interface ILogManager
    {
        IApplicationLogger DefaultLogger { get; }
    }
}