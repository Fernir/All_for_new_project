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
        private List<TnbExpendablePanel> _dedoufeo = new List<TnbExpendablePanel>();

        public TnbRibbonManager()
        {
            this._dedoufeo = new List<TnbExpendablePanel>();
            base.Size = new Size(0x23d, 370);
            base.Location = new Point(0x23d, 370);
            base.BorderStyle = BorderStyle.None;
            base.AutoScroll = false;
            base.ControlAdded += new ControlEventHandler(this.Facaetieniaqak);
            base.ControlRemoved += new ControlEventHandler(this.Tavea);
            base.Click += new EventHandler(this.Relocator);
            this.NupemiutDaefer();
        }

        private void Facaetieniaqak(object moleileucucisUgofe, ControlEventArgs afaixaquOkul)
        {
            this.NupemiutDaefer();
            foreach (Control control in base.Controls)
            {
                if ((control is TnbExpendablePanel) && !this._dedoufeo.Contains(control as TnbExpendablePanel))
                {
                    TnbExpendablePanel item = control as TnbExpendablePanel;
                    if (this._dedoufeo.Count > 1)
                    {
                        if (item.OrderIndex == -1)
                        {
                            item.OrderIndex = this._dedoufeo[this._dedoufeo.Count - 1].OrderIndex + 1;
                        }
                        item.Location = this.NupemiutDaefer();
                    }
                    else if (item.OrderIndex == -1)
                    {
                        item.OrderIndex = 0;
                    }
                    item.OnStatusChanged = (EventHandler) Delegate.Combine(item.OnStatusChanged, new EventHandler(this.Relocator));
                    item.OnOrderChanged = (EventHandler) Delegate.Combine(item.OnOrderChanged, new EventHandler(this.Relocator));
                    this._dedoufeo.Add(item);
                }
            }
            this.NupemiutDaefer();
        }

        private Point NupemiutDaefer()
        {
            if (CS$<>9__CachedAnonymousMethodDelegate1 == null)
            {
                CS$<>9__CachedAnonymousMethodDelegate1 = new Func<TnbExpendablePanel, int>(TnbRibbonManager.<Relocator>b__0);
            }
            this._dedoufeo = this._dedoufeo.OrderBy<TnbExpendablePanel, int>(CS$<>9__CachedAnonymousMethodDelegate1).ToList<TnbExpendablePanel>();
            int height = 0;
            int y = 0;
            for (int i = 0; i < this._dedoufeo.Count; i++)
            {
                if (i != 0)
                {
                    Point point = new Point(0, y + height);
                    this._dedoufeo[i].Location = point;
                    height += this._dedoufeo[i].Height - height;
                    y = this._dedoufeo[i].Location.Y;
                }
                else
                {
                    this._dedoufeo[i].Location = (this._dedoufeo[i].Location.Y <= 0) ? new Point(0, this._dedoufeo[i].Location.Y) : new Point(0, 0);
                    height = this._dedoufeo[i].Height;
                    y = this._dedoufeo[i].Location.Y;
                }
            }
            base.Invalidate();
            return new Point(0, y + height);
        }

        [Category("Appearance")]
        public void Relocator(object sender, EventArgs eventArgs)
        {
            this.NupemiutDaefer();
        }

        private void Tavea(object moleileucucisUgofe, ControlEventArgs afaixaquOkul)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this._dedoufeo.Count; i++)
            {
                TnbExpendablePanel control = this._dedoufeo[i];
                if (!base.Controls.Contains(control))
                {
                    list.Add(i);
                }
            }
            foreach (int num2 in list)
            {
                this._dedoufeo.RemoveAt(num2);
            }
            this.NupemiutDaefer();
        }
    }
}

