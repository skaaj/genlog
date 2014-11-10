using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Genlog
{
    public class ViewChangedArgs : EventArgs
    {
        public UserControl RequestedView
        {
            get;
            set;
        }
    }

    public abstract class Activity
    {
        private MainWindow Context { get; set; }
        private Dictionary<string, UserControl> _views;

        public event ViewChangedHandler OnViewChanged;
        public delegate void ViewChangedHandler(Activity sender, ViewChangedArgs e);

        public UserControl View { get; private set; }

        public Activity(MainWindow context)
        {
            Context = context;
            _views = new Dictionary<string, UserControl>();

            Console.WriteLine("Activity::Create called");
        }

        public virtual void Stop()
        {
            Console.WriteLine("Activity::Stop called");
        }

        protected void AddView(string id, UserControl view)
        {
            _views.Add(id, view);
        }

        public void SetView(string id)
        {
            if (_views.ContainsKey(id))
            {
                View = _views[id];
                
                if (OnViewChanged != null)
                    OnViewChanged(this, new ViewChangedArgs() { RequestedView = View });
            }
        }
    }
}
