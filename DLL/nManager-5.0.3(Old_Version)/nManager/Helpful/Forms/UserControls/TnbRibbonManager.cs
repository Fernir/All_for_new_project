namespace nManager.Helpful.Forms.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public class TnbRibbonManager : Panel
    {
        private List<TnbExpendablePanel> _panelAttached = new List<TnbExpendablePanel>();

        public TnbRibbonManager()
        {
            this._panelAttached = new List<TnbExpendablePanel>();
            base.Size = new Size(0x23d, 370);
            base.Location = new Point(0x23d, 370);
            base.BorderStyle = BorderStyle.None;
            base.AutoScroll = false;
            base.ControlAdded += new ControlEventHandler(this.OnControlAdd);
            base.ControlRemoved += new ControlEventHandler(this.OnControlDel);
            base.Click += new EventHandler(this.Relocator);
            this.Relocator();
        }

        private void OnControlAdd(object sender, ControlEventArgs controlEventArgs)
        {
            this.Relocator();
            foreach (Control control in base.Controls)
            {
                if ((control is TnbExpendablePanel) && !this._panelAttached.Contains(control as TnbExpendablePanel))
                {
                    TnbExpendablePanel item = control as TnbExpendablePanel;
                    if (this._panelAttached.Count > 1)
                    {
                        if (item.OrderIndex == -1)
                        {
                            item.OrderIndex = this._panelAttached[this._panelAttached.Count - 1].OrderIndex + 1;
                        }
                        item.Location = this.Relocator();
                    }
                    else if (item.OrderIndex == -1)
                    {
                        item.OrderIndex = 0;
                    }
                    item.OnStatusChanged = (EventHandler) Delegate.Combine(item.OnStatusChanged, new EventHandler(this.Relocator));
                    item.OnOrderChanged = (EventHandler) Delegate.Combine(item.OnOrderChanged, new EventHandler(this.Relocator));
                    this._panelAttached.Add(item);
                }
            }
            this.Relocator();
        }

        private void OnControlDel(object sender, ControlEventArgs controlEventArgs)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this._panelAttached.Count; i++)
            {
                TnbExpendablePanel control = this._panelAttached[i];
                if (!base.Controls.Contains(control))
                {
                    list.Add(i);
                }
            }
            foreach (int num2 in list)
            {
                this._panelAttached.RemoveAt(num2);
            }
            this.Relocator();
        }

        private Point Relocator()
        {
            this._panelAttached = (from o in this._panelAttached
                orderby o.OrderIndex
                select o).ToList<TnbExpendablePanel>();
            int height = 0;
            int y = 0;
            for (int i = 0; i < this._panelAttached.Count; i++)
            {
                if (i != 0)
                {
                    Point point = new Point(0, y + height);
                    this._panelAttached[i].Location = point;
                    height += this._panelAttached[i].Height - height;
                    y = this._panelAttached[i].Location.Y;
                }
                else
                {
                    this._panelAttached[i].Location = (this._panelAttached[i].Location.Y <= 0) ? new Point(0, this._panelAttached[i].Location.Y) : new Point(0, 0);
                    height = this._panelAttached[i].Height;
                    y = this._panelAttached[i].Location.Y;
                }
            }
            base.Invalidate();
            return new Point(0, y + height);
        }

        [Category("Appearance")]
        public void Relocator(object sender, EventArgs eventArgs)
        {
            this.Relocator();
        }
    }
}

