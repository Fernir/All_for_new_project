namespace nManager.Wow.Class
{
    using System;

    [Serializable]
    public class Digsite
    {
        private bool _active;
        private Point _center;
        private int _id;
        private string _name;
        private float _priorityDigsites;

        public bool Active
        {
            get
            {
                return this._active;
            }
            set
            {
                this._active = value;
            }
        }

        public Point Center
        {
            get
            {
                return this._center;
            }
            set
            {
                this._center = value;
            }
        }

        public int id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public float PriorityDigsites
        {
            get
            {
                return this._priorityDigsites;
            }
            set
            {
                this._priorityDigsites = value;
            }
        }
    }
}

