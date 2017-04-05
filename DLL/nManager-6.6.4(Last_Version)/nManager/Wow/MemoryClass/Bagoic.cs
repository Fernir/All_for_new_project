namespace nManager.Wow.MemoryClass
{
    using nManager.Wow.MemoryClass.Magic;
    using SlimDX;
    using SlimDX.Direct3D11;
    using SlimDX.DXGI;
    using SlimDX.Windows;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal class Bagoic
    {
        private static uint _afusudu;
        private static uint _ataogeofim;

        public static uint Hatesojaitaive(int koixabAwi)
        {
            BlackMagic magic = new BlackMagic(koixabAwi);
            System.Diagnostics.Process processById = System.Diagnostics.Process.GetProcessById(koixabAwi);
            uint num = magic.ReadUInt((uint) (((int) magic.GetModule(processById.ProcessName + ".exe").BaseAddress) + 0xd88f20));
            uint dwAddress = magic.ReadUInt(num + 0x256c);
            if (dwAddress == 0)
            {
                return 0;
            }
            uint num3 = magic.ReadUInt(dwAddress);
            _afusudu = magic.ReadUInt(num3 + 0xa8);
            return _afusudu;
        }

        public static bool OvaijGuir(int koixabAwi)
        {
            return (Hatesojaitaive(koixabAwi) == 0);
        }

        public static uint Uboeqe()
        {
            if (_ataogeofim <= 0)
            {
                using (RenderForm form = new RenderForm())
                {
                    SlimDX.Direct3D11.Device device;
                    SwapChain chain;
                    SwapChainDescription swapChainDescription = new SwapChainDescription {
                        BufferCount = 1,
                        Flags = SwapChainFlags.None,
                        IsWindowed = true,
                        ModeDescription = new ModeDescription(100, 100, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                        OutputHandle = form.Handle,
                        SampleDescription = new SampleDescription(1, 0),
                        SwapEffect = SwapEffect.Discard,
                        Usage = Usage.RenderTargetOutput
                    };
                    if (SlimDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapChainDescription, out device, out chain).IsSuccess)
                    {
                        using (device)
                        {
                            using (chain)
                            {
                                BlackMagic magic = new BlackMagic(System.Diagnostics.Process.GetCurrentProcess().Id);
                                _ataogeofim = magic.ReadUInt(magic.ReadUInt((uint) ((int) chain.ComPointer)) + 0x20);
                            }
                        }
                    }
                }
            }
            return _ataogeofim;
        }

        public static byte[] _jouweReoxul
        {
            [CompilerGenerated]
            get
            {
                return <OriginalBytes>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <OriginalBytes>k__BackingField = value;
            }
        }

        public static byte[] _qecaigepe
        {
            [CompilerGenerated]
            get
            {
                return <OriginalBytesDX>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <OriginalBytesDX>k__BackingField = value;
            }
        }
    }
}

