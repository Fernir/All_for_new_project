namespace nManager.Plugins
{
    using System;

    public interface IPlugins
    {
        void CheckFields();
        void Dispose();
        void Initialize();
        void ResetConfiguration();
        void ShowConfiguration();

        string Author { get; }

        string Description { get; }

        bool IsStarted { get; }

        bool Loop { get; set; }

        string Name { get; }

        string TargetVersion { get; }

        string Version { get; }
    }
}

