namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using System;

    public class Chat
    {
        public static void SendChatMessage(string message)
        {
            try
            {
                Lua.LuaDoString("SendChatMessage(\"" + message + "\");", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("SendChatMessage(String message): " + exception, true);
            }
        }

        public static void SendChatMessageGuild(string message)
        {
            try
            {
                Lua.LuaDoString("SendChatMessage(\"" + message + "\", \"GUILD\");", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("SendChatMessageGuild(String message): " + exception, true);
            }
        }

        public static void SendChatMessageWhisper(string message, string playerName)
        {
            try
            {
                Lua.LuaDoString("SendChatMessage(\"" + message + "\", \"whisper\", nil, \"" + playerName + "\");", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("SendChatMessageWhisper(String message, String playerName): " + exception, true);
            }
        }

        public static void SendChatMessageWhisperAtTarget(string message)
        {
            try
            {
                Lua.LuaDoString("SendChatMessage(\"" + message + "\", \"whisper\", nil, UnitName(\"target\"));", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("SendChatMessageWhisperAtTarget(String message): " + exception, true);
            }
        }
    }
}

