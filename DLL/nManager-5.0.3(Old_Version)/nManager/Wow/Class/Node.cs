namespace nManager.Wow.Class
{
    using System;

    [Serializable]
    public class Node
    {
        private bool _actived = true;
        private int _id;
        private string _name = "";
        private int _skill;
        private string _type = "";

        public bool Actived
        {
            get
            {
                return this._actived;
            }
            set
            {
                this._actived = value;
            }
        }

        public int Id
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

        public string Name
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

        public int Skill
        {
            get
            {
                return this._skill;
            }
            set
            {
                this._skill = value;
            }
        }

        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

