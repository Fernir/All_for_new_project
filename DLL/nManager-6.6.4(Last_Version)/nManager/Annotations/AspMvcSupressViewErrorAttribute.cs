namespace nManager.Annotations
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class AspMvcSupressViewErrorAttribute : Attribute
    {
    }
}

