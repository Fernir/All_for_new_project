namespace nManager.Wow.Class
{
    using System;

    [Serializable]
    public class Node
    {
        private int _epuaxeuxouAfiefovu;
        private int _iqearaweoxear;
        private bool _irakoroipuQaecodi = true;
        private string _loedacItuoj = "";
        private string _witiweMiv = "";

        public bool Actived
        {
            get
            {
                return this._irakoroipuQaecodi;
            }
            set
            {
                this._irakoroipuQaecodi = value;
            }
        }

        public int Id
        {
            get
            {
                return this._iqearaweoxear;
            }
            set
            {
                this._iqearaweoxear = value;
            }
        }

        public string Name
        {
            get
            {
                return this._witiweMiv;
            }
            set
            {
                this._witiweMiv = value;
            }
        }

        public int Skill
        {
            get
            {
                return this._epuaxeuxouAfiefovu;
            }
            set
            {
                this._epuaxeuxouAfiefovu = value;
            }
        }

        public string Type
        {
            get
            {
                return this._loedacItuoj;
            }
            set
            {
                this._loedacItuoj = value;
            }
        }
    }
}

