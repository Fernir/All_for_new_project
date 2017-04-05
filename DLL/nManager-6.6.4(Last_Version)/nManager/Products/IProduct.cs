namespace nManager.Products
{
    using System;

    public interface IProduct
    {
        void Dispose();
        void Initialize();
        void RemoteStart(string[] args);
        void Settings();
        void Start();
        void Stop();

        bool IsStarted { get; }
    }
}

