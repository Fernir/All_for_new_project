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

    internal class D3D
    {
        private static uint d3d11Adresse;
        private static uint d3d9Adresse;

        public static uint D3D11Adresse()
        {
            if (d3d11Adresse <= 0)
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
                                d3d11Adresse = magic.ReadUInt(magic.ReadUInt((uint) ((int) chain.ComPointer)) + 0x20);
                            }
                        }
                    }
                }
            }
            return d3d11Adresse;
        }

        public static uint D3D9Adresse(int processId)
        {
            BlackMagic magic = new BlackMagic(processId);
            System.Diagnostics.Process processById = System.Diagnostics.Process.GetProcessById(processId);
            uint num = magic.ReadUInt((uint) (((int) magic.GetModule(processById.ProcessName + ".exe").BaseAddress) + 0xc12248));
            uint dwAddress = magic.ReadUInt(num + 0x28b8);
            if (dwAddress == 0)
            {
                return 0;
            }
            uint num3 = magic.ReadUInt(dwAddress);
            d3d9Adresse = magic.ReadUInt(num3 + 0xa8);
            return d3d9Adresse;
        }

        public static bool IsD3D11(int processId)
        {
            return (D3D9Adresse(processId) == 0);
        }

        public static byte[] OriginalBytes
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
    }
}

