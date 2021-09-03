using System;

using Wyn.Data.Abstractions.Descriptors;

namespace Wyn.Data.Core.Descriptors
{
    public class RepositoryDescriptor : IRepositoryDescriptor
    {
        public Type InterfaceType { get; }

        public Type ImplementType { get; }

        public RepositoryDescriptor(Type interfaceType, Type implementType)
        {
            InterfaceType = interfaceType;
            ImplementType = implementType;
        }
    }
}
