namespace nManager.Wow.Class
{
    using System;

    [Serializable]
    public class Digsite
    {
        private bool _ciocise;
        private float _conaoxoewMidegCoabeboug;
        private int _iqearaweoxear;
        private Point _roefi;
        private string _witiweMiv;

        public bool Active
        {
            get
            {
                return this._ciocise;
            }
            set
            {
                this._ciocise = value;
            }
        }

        public Point Center
        {
            get
            {
                return this._roefi;
            }
            set
            {
                this._roefi = value;
            }
        }

        public int id
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

        public string name
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

        public float PriorityDigsites
        {
            get
            {
                return this._conaoxoewMidegCoabeboug;
            }
            set
            {
                this._conaoxoewMidegCoabeboug = value;
            }
        }
    }
}

