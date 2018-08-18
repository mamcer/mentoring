namespace CrossCutting.Core.IOC
{
    using System;

    public interface IContainer
    {
        T Resolve<T>();

        object Resolve(Type type);
    }
}