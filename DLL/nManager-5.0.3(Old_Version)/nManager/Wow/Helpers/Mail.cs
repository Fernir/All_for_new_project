namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Mail
    {
        public static int GetNumAttachments()
        {
            try
            {
                return Others.ToInt32(Lua.LuaDoString("numAttachments = 0; for i=1, ATTACHMENTS_MAX_SEND do local itemName, itemTexture, stackCount, quality = GetSendMailItem(i); if itemName then numAttachments = numAttachments + 1; end end", "numAttachments", false));
            }
            catch (Exception exception)
            {
                Logging.WriteError("Mail > GetNumAttachments(): " + exception, true);
                return 0;
            }
        }

        public static void SendMessage(string target, string titleMsg, string txtMsg, List<string> itemSend, List<string> itemNoSend, List<WoWItemQuality> itemQuality, out bool mailSendingCompleted)
        {
            try
            {
                string str = itemSend.Aggregate<string, string>("", (current, s) => current + " or namei == \"" + s + "\" ");
                string str2 = itemQuality.Aggregate<WoWItemQuality, string>(" 1 == 2 ", (current, s) => string.Concat(new object[] { current, " or r == ", (uint) s, " " }));
                string str3 = "";
                string str4 = "";
                if (itemNoSend.Count > 0)
                {
                    str4 = " end ";
                    str3 = itemNoSend.Aggregate<string, string>(" if ", (current, s) => (current + " and namei ~= \"" + s + "\" ")).Replace("if  and", "if ") + " then ";
                }
                string command = "";
                command = ((command + "MailFrameTab_OnClick(0,2) " + "local c,l,r,_=0 ") + "for b=0,4 do " + "for s=1,40 do  ") + "local l=GetContainerItemLink(b,s) " + "if l then namei,_,r=GetItemInfo(l) ";
                command = (((((command + "if " + str2 + " " + str + " then ") + str3) + "UseContainerItem(b,s) " + str4) + " end " + "end ") + "end " + "end ") + " numAttachments = 0; for i=1, ATTACHMENTS_MAX_SEND do local itemName, itemTexture, stackCount, quality = GetSendMailItem(i); if itemName then numAttachments = numAttachments + 1; end end " + "if numAttachments>0 then ";
                if ((titleMsg != "") && (txtMsg != ""))
                {
                    command = command + "SendMail(\"" + target + "\", \"" + titleMsg + "\", \"" + txtMsg + "\") ";
                }
                else
                {
                    command = command + "SendMail(\"" + target + "\", \"" + titleMsg + " \", \"" + titleMsg + " \") ";
                }
                command = command + "end ";
                mailSendingCompleted = Others.ToInt32(Lua.LuaDoString(command, "numAttachments", false)) <= 0;
                Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
            }
            catch (Exception exception)
            {
                mailSendingCompleted = true;
                Logging.WriteError("Mail > SendMessage(string target, string titleMsg, string txtMsg, List<String> itemSend, List<string> itemNoSend, List<Enums.WoWItemQuality> itemQuality): " + exception, true);
            }
        }
    }
}

