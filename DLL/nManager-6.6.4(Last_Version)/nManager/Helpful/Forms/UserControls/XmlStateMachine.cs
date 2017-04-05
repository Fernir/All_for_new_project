namespace nManager.Helpful.Forms.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;

    public class XmlStateMachine
    {
        private string _cujahegijaXawaebi = string.Empty;
        private string _qiujaejuto = "";
        private Stack<XmlTokenType> _unudurokexo = new Stack<XmlTokenType>();
        public XmlTokenType CurrentState = XmlTokenType.Unknown;

        private string AleukiIgeusef(string uleavacaom, int qaikiepiceutOjepoku)
        {
            string str = "";
            int index = uleavacaom.IndexOf('=');
            if (index != -1)
            {
                str = uleavacaom.Substring(0, index);
            }
            return str;
        }

        private string EcausuodekuitNoatoi(string uleavacaom, int qaikiepiceutOjepoku)
        {
            string str = "";
            int index = uleavacaom.IndexOf('<');
            if (index != -1)
            {
                str = uleavacaom.Substring(0, index);
            }
            return str;
        }

        public string GetNextToken(string s, int location, out XmlTokenType ttype)
        {
            ttype = XmlTokenType.Unknown;
            string str = this.IgitauciOmoduici(s, location);
            if (!string.IsNullOrEmpty(str))
            {
                location += str.Length;
            }
            this._qiujaejuto = s.Substring(location, s.Length - location);
            this._cujahegijaXawaebi = string.Empty;
            if (this.CurrentState == XmlTokenType.CDataStart)
            {
                if (this._qiujaejuto.StartsWith("]]>"))
                {
                    this.CurrentState = XmlTokenType.CDataEnd;
                    this._cujahegijaXawaebi = "]]>";
                }
                else
                {
                    this.CurrentState = XmlTokenType.CDataValue;
                    int index = this._qiujaejuto.IndexOf("]]>");
                    this._cujahegijaXawaebi = this._qiujaejuto.Substring(0, index);
                }
            }
            else if (this.CurrentState == XmlTokenType.DocTypeStart)
            {
                this.CurrentState = XmlTokenType.DocTypeName;
                this._cujahegijaXawaebi = "DOCTYPE";
            }
            else if (this.CurrentState == XmlTokenType.DocTypeName)
            {
                this.CurrentState = XmlTokenType.DocTypeDeclaration;
                int length = this._qiujaejuto.IndexOf("[");
                this._cujahegijaXawaebi = this._qiujaejuto.Substring(0, length);
            }
            else if (this.CurrentState == XmlTokenType.DocTypeDeclaration)
            {
                this.CurrentState = XmlTokenType.DocTypeDefStart;
                this._cujahegijaXawaebi = "[";
            }
            else if (this.CurrentState == XmlTokenType.DocTypeDefStart)
            {
                if (this._qiujaejuto.StartsWith("]>"))
                {
                    this.CurrentState = XmlTokenType.DocTypeDefEnd;
                    this._cujahegijaXawaebi = "]>";
                }
                else
                {
                    this.CurrentState = XmlTokenType.DocTypeDefValue;
                    int num3 = this._qiujaejuto.IndexOf("]>");
                    this._cujahegijaXawaebi = this._qiujaejuto.Substring(0, num3);
                }
            }
            else if (this.CurrentState == XmlTokenType.DocTypeDefValue)
            {
                this.CurrentState = XmlTokenType.DocTypeDefEnd;
                this._cujahegijaXawaebi = "]>";
            }
            else if (this.CurrentState == XmlTokenType.DoubleQuotationMarkStart)
            {
                if (this._qiujaejuto[0] == '"')
                {
                    this.CurrentState = XmlTokenType.DoubleQuotationMarkEnd;
                    this._cujahegijaXawaebi = "\"";
                }
                else
                {
                    this.CurrentState = XmlTokenType.AttributeValue;
                    int num4 = this._qiujaejuto.IndexOf("\"");
                    this._cujahegijaXawaebi = this._qiujaejuto.Substring(0, num4);
                }
            }
            else if (this.CurrentState == XmlTokenType.SingleQuotationMarkStart)
            {
                if (this._qiujaejuto[0] == '\'')
                {
                    this.CurrentState = XmlTokenType.SingleQuotationMarkEnd;
                    this._cujahegijaXawaebi = "'";
                }
                else
                {
                    this.CurrentState = XmlTokenType.AttributeValue;
                    int num5 = this._qiujaejuto.IndexOf("'");
                    this._cujahegijaXawaebi = this._qiujaejuto.Substring(0, num5);
                }
            }
            else if (this.CurrentState == XmlTokenType.CommentStart)
            {
                if (this._qiujaejuto.StartsWith("-->"))
                {
                    this.CurrentState = XmlTokenType.CommentEnd;
                    this._cujahegijaXawaebi = "-->";
                }
                else
                {
                    this.CurrentState = XmlTokenType.CommentValue;
                    this._cujahegijaXawaebi = this.Pomuav(this._qiujaejuto, location);
                }
            }
            else if (this.CurrentState == XmlTokenType.NodeStart)
            {
                this.CurrentState = XmlTokenType.NodeName;
                this._cujahegijaXawaebi = this.Ucouta(this._qiujaejuto, location);
            }
            else if (this.CurrentState == XmlTokenType.XmlDeclarationStart)
            {
                this.CurrentState = XmlTokenType.NodeName;
                this._cujahegijaXawaebi = this.Ucouta(this._qiujaejuto, location);
            }
            else if (this.CurrentState == XmlTokenType.NodeName)
            {
                if ((this._qiujaejuto[0] != '/') && (this._qiujaejuto[0] != '>'))
                {
                    this.CurrentState = XmlTokenType.AttributeName;
                    this._cujahegijaXawaebi = this.AleukiIgeusef(this._qiujaejuto, location);
                }
                else
                {
                    this.Keoceuhiag();
                }
            }
            else if (this.CurrentState == XmlTokenType.NodeEndValueStart)
            {
                if (this._qiujaejuto[0] == '<')
                {
                    this.Keoceuhiag();
                }
                else
                {
                    this.CurrentState = XmlTokenType.NodeValue;
                    this._cujahegijaXawaebi = this.EcausuodekuitNoatoi(this._qiujaejuto, location);
                }
            }
            else if (this.CurrentState == XmlTokenType.DoubleQuotationMarkEnd)
            {
                this.Uwairitubaijoe(location);
            }
            else if (this.CurrentState == XmlTokenType.SingleQuotationMarkEnd)
            {
                this.Uwairitubaijoe(location);
            }
            else
            {
                this.Keoceuhiag();
            }
            if (this._cujahegijaXawaebi != string.Empty)
            {
                ttype = this.CurrentState;
                return (str + this._cujahegijaXawaebi);
            }
            return string.Empty;
        }

        public Color GetTokenColor(XmlTokenType ttype)
        {
            Color.FromArgb(0xee, 0x95, 0x44);
            switch (ttype)
            {
                case XmlTokenType.XmlDeclarationStart:
                case XmlTokenType.XmlDeclarationEnd:
                case XmlTokenType.NodeStart:
                case XmlTokenType.NodeEnd:
                case XmlTokenType.NodeEndValueStart:
                case XmlTokenType.AttributeValue:
                case XmlTokenType.CommentStart:
                case XmlTokenType.CommentEnd:
                case XmlTokenType.CDataStart:
                case XmlTokenType.CDataEnd:
                case XmlTokenType.DocTypeStart:
                case XmlTokenType.DocTypeDefStart:
                case XmlTokenType.DocTypeDefEnd:
                case XmlTokenType.DocTypeEnd:
                    return Color.Blue;

                case XmlTokenType.NodeName:
                case XmlTokenType.DocTypeName:
                    return Color.Blue;

                case XmlTokenType.NodeValue:
                case XmlTokenType.EqualSignStart:
                case XmlTokenType.EqualSignEnd:
                case XmlTokenType.DoubleQuotationMarkStart:
                case XmlTokenType.DoubleQuotationMarkEnd:
                case XmlTokenType.SingleQuotationMarkStart:
                case XmlTokenType.SingleQuotationMarkEnd:
                    return Color.Black;

                case XmlTokenType.AttributeName:
                case XmlTokenType.DocTypeDeclaration:
                    return Color.Red;

                case XmlTokenType.CommentValue:
                    return Color.Green;

                case XmlTokenType.CDataValue:
                case XmlTokenType.DocTypeDefValue:
                    return Color.Gray;
            }
            return Color.Orange;
        }

        public string GetXmlDeclaration(string s)
        {
            int index = s.IndexOf("<?");
            int num2 = s.IndexOf("?>");
            if ((index > -1) && (num2 > index))
            {
                return s.Substring(index, (num2 - index) + 2);
            }
            return string.Empty;
        }

        private string IgitauciOmoduici(string uleavacaom, int qaikiepiceutOjepoku)
        {
            bool flag = false;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; (qaikiepiceutOjepoku + i) < uleavacaom.Length; i++)
            {
                char c = uleavacaom[qaikiepiceutOjepoku + i];
                if (!char.IsWhiteSpace(c))
                {
                    break;
                }
                flag = true;
                builder.Append(c);
            }
            if (flag)
            {
                return builder.ToString();
            }
            return string.Empty;
        }

        private void Keoceuhiag()
        {
            if (this._qiujaejuto.StartsWith("<![CDATA["))
            {
                this.CurrentState = XmlTokenType.CDataStart;
                this._cujahegijaXawaebi = "<![CDATA[";
            }
            else if (this._qiujaejuto.StartsWith("<!DOCTYPE"))
            {
                this.CurrentState = XmlTokenType.DocTypeStart;
                this._cujahegijaXawaebi = "<!";
            }
            else if (this._qiujaejuto.StartsWith("</"))
            {
                this.CurrentState = XmlTokenType.NodeStart;
                this._cujahegijaXawaebi = "</";
            }
            else if (this._qiujaejuto.StartsWith("<!--"))
            {
                this.CurrentState = XmlTokenType.CommentStart;
                this._cujahegijaXawaebi = "<!--";
            }
            else if (this._qiujaejuto.StartsWith("<?"))
            {
                this.CurrentState = XmlTokenType.XmlDeclarationStart;
                this._cujahegijaXawaebi = "<?";
            }
            else if (this._qiujaejuto.StartsWith("<"))
            {
                this.CurrentState = XmlTokenType.NodeStart;
                this._cujahegijaXawaebi = "<";
            }
            else if (this._qiujaejuto.StartsWith("="))
            {
                this.CurrentState = XmlTokenType.EqualSignStart;
                if (this.CurrentState == XmlTokenType.AttributeValue)
                {
                    this.CurrentState = XmlTokenType.EqualSignEnd;
                }
                this._cujahegijaXawaebi = "=";
            }
            else if (this._qiujaejuto.StartsWith("?>"))
            {
                this.CurrentState = XmlTokenType.XmlDeclarationEnd;
                this._cujahegijaXawaebi = "?>";
            }
            else if (this._qiujaejuto.StartsWith(">"))
            {
                this.CurrentState = XmlTokenType.NodeEndValueStart;
                this._cujahegijaXawaebi = ">";
            }
            else if (this._qiujaejuto.StartsWith("-->"))
            {
                this.CurrentState = XmlTokenType.CommentEnd;
                this._cujahegijaXawaebi = "-->";
            }
            else if (this._qiujaejuto.StartsWith("]>"))
            {
                this.CurrentState = XmlTokenType.DocTypeEnd;
                this._cujahegijaXawaebi = "]>";
            }
            else if (this._qiujaejuto.StartsWith("]]>"))
            {
                this.CurrentState = XmlTokenType.CDataEnd;
                this._cujahegijaXawaebi = "]]>";
            }
            else if (this._qiujaejuto.StartsWith("/>"))
            {
                this.CurrentState = XmlTokenType.NodeEnd;
                this._cujahegijaXawaebi = "/>";
            }
            else if (this._qiujaejuto.StartsWith("\""))
            {
                if (this.CurrentState == XmlTokenType.AttributeValue)
                {
                    this.CurrentState = XmlTokenType.DoubleQuotationMarkEnd;
                }
                else
                {
                    this.CurrentState = XmlTokenType.DoubleQuotationMarkStart;
                }
                this._cujahegijaXawaebi = "\"";
            }
            else if (this._qiujaejuto.StartsWith("'"))
            {
                this.CurrentState = XmlTokenType.SingleQuotationMarkStart;
                if (this.CurrentState == XmlTokenType.AttributeValue)
                {
                    this.CurrentState = XmlTokenType.SingleQuotationMarkEnd;
                }
                this._cujahegijaXawaebi = "'";
            }
        }

        private string Pomuav(string uleavacaom, int qaikiepiceutOjepoku)
        {
            string str = "";
            int index = uleavacaom.IndexOf("-->");
            if (index != -1)
            {
                str = uleavacaom.Substring(0, index);
            }
            return str;
        }

        private List<string> Seateqa(string uleavacaom)
        {
            List<string> list = new List<string>();
            string[] strArray = uleavacaom.Split(new char[] { ' ' });
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = strArray[i].Trim();
                if (strArray[i].Length > 0)
                {
                    list.Add(strArray[i]);
                }
            }
            return list;
        }

        private string Ucouta(string uleavacaom, int qaikiepiceutOjepoku)
        {
            string str = "";
            for (int i = 0; i < uleavacaom.Length; i++)
            {
                if (((uleavacaom[i] == '/') || (uleavacaom[i] == ' ')) || (uleavacaom[i] == '>'))
                {
                    return str;
                }
                str = str + uleavacaom[i].ToString();
            }
            return str;
        }

        private void Uwairitubaijoe(int qaikiepiceutOjepoku)
        {
            if (this._qiujaejuto.StartsWith(">"))
            {
                this.Keoceuhiag();
            }
            else if (this._qiujaejuto.StartsWith("/>"))
            {
                this.Keoceuhiag();
            }
            else if (this._qiujaejuto.StartsWith("?>"))
            {
                this.Keoceuhiag();
            }
            else
            {
                this.CurrentState = XmlTokenType.AttributeName;
                this._cujahegijaXawaebi = this.AleukiIgeusef(this._qiujaejuto, qaikiepiceutOjepoku);
            }
        }
    }
}

