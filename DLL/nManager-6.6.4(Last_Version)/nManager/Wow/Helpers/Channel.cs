namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using System;
    using System.Runtime.InteropServices;

    public class Channel
    {
        public int CurrentMsg;

        public Channel()
        {
            try
            {
                this.AfijoxoxLuemied();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Channel(): " + exception, true);
            }
        }

        private void AfijoxoxLuemied()
        {
            try
            {
                this.CurrentMsg = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xff990c0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetCurrentMsg(): " + exception, true);
            }
        }

        private string Iputehoite(string ajevugieviw, string ajetivagideput, string efuaboapuvaidViqiweob)
        {
            try
            {
                int startIndex = ajevugieviw.IndexOf(ajetivagideput, StringComparison.Ordinal) + ajetivagideput.Length;
                int num2 = ajevugieviw.IndexOf(efuaboapuvaidViqiweob, startIndex, StringComparison.Ordinal);
                if ((num2 - startIndex) <= 0)
                {
                    return "";
                }
                return ajevugieviw.Substring(startIndex, num2 - startIndex);
            }
            catch (Exception exception)
            {
                Logging.WriteError("string stringBetween(string chaine, string debut, string fin): " + exception, true);
                return "";
            }
        }

        private Message NouqoebuxiavuEvuivoaji()
        {
            try
            {
                if (this.CurrentMsg > 0x3b)
                {
                    this.CurrentMsg = 0;
                }
                if ((this.CurrentMsg >= Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xff990c0)) && ((this.CurrentMsg - Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xff990c0)) <= 2))
                {
                    return new Message();
                }
                string ajevugieviw = Memory.WowMemory.Memory.ReadUTF8String(((Memory.WowProcess.WowModule + 0xf3f660) + 0x65) + ((uint) (0x17e8 * this.CurrentMsg)));
                Message message = new Message();
                if (ajevugieviw != "")
                {
                    this.CurrentMsg++;
                    message.Channel = Others.ToInt32(this.Iputehoite(ajevugieviw, "Type: [", "]"));
                    message.Nickname = Others.ToUtf8(this.Iputehoite(ajevugieviw, "Name: [", "]"));
                    message.Msg = Others.ToUtf8(this.Iputehoite(ajevugieviw, "Text: [", "]"));
                }
                return message;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Chat > ReadMsg(): " + exception, true);
                return new Message();
            }
        }

        private string Quaciu(int deqoiqa)
        {
            try
            {
                switch (deqoiqa)
                {
                    case 0:
                        return "[Addon]";

                    case 1:
                        return "[Say]";

                    case 2:
                        return "[Party]";

                    case 3:
                        return "[Raid]";

                    case 4:
                        return "[Guild]";

                    case 5:
                        return "[Officers]";

                    case 6:
                        return "[Yell]";

                    case 7:
                        return "[Whisper]";

                    case 8:
                        return "[Monster Whisper]";

                    case 9:
                        return "[To]";

                    case 10:
                        return "[Emote]";

                    case 0x11:
                        return "[General]";

                    case 0x1b:
                        return "[Loot]";

                    case 0x33:
                        return "[Real Id Whisper]";

                    case 0x34:
                        return "[Real Id To]";
                }
                return ("[Channel " + deqoiqa + "]");
            }
            catch (Exception exception)
            {
                Logging.WriteError("getChannel(int Channel): " + exception, true);
                return "";
            }
        }

        public string ReadAllChannel()
        {
            try
            {
                Message message = this.NouqoebuxiavuEvuivoaji();
                if (message.Msg != null)
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.Quaciu(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadAllChannel(): " + exception, true);
            }
            return "";
        }

        public string ReadGuildChannel()
        {
            try
            {
                Message message = this.NouqoebuxiavuEvuivoaji();
                if ((message.Msg != null) && (message.Channel == 4))
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.Quaciu(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadGuildChannel(): " + exception, true);
            }
            return "";
        }

        public static string ReadLastMsg()
        {
            try
            {
                return Memory.WowMemory.Memory.ReadUTF8String(((Memory.WowProcess.WowModule + 0xf3f660) + 0x65) + ((uint) (0x17e8 * Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xff990c0))));
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadLastMsg(): " + exception, true);
            }
            return "";
        }

        public string ReadLootChannel()
        {
            try
            {
                Message message = this.NouqoebuxiavuEvuivoaji();
                if ((message.Msg != null) && (message.Channel == 0x1b))
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.Quaciu(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadWhisperChannel(): " + exception, true);
            }
            return "";
        }

        public string ReadSayChannel()
        {
            try
            {
                Message message = this.NouqoebuxiavuEvuivoaji();
                if ((message.Msg != null) && (message.Channel == 1))
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.Quaciu(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadSayChannel(): " + exception, true);
            }
            return "";
        }

        public string ReadWhisperAndRealIdChannel()
        {
            try
            {
                Message message = this.NouqoebuxiavuEvuivoaji();
                if ((message.Msg != null) && ((message.Channel == 7) || (message.Channel == 0x33)))
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.Quaciu(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadWhisperChannel(): " + exception, true);
            }
            return "";
        }

        public string ReadWhisperChannel()
        {
            try
            {
                Message message = this.NouqoebuxiavuEvuivoaji();
                if ((message.Msg != null) && (message.Channel == 7))
                {
                    return string.Concat(new object[] { DateTime.Now, " - ", message.Nickname, " ", this.Quaciu(message.Channel), " : ", message.Msg, "\r\n" });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadWhisperChannel(): " + exception, true);
            }
            return "";
        }

        public int GetCurrentMsgInWow
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + 0xff990c0);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetCurrentMsgInWow: " + exception, true);
                }
                return 0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public int Channel;
            public string Msg;
            public string Nickname;
        }
    }
}

