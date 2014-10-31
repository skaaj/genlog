using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Genlog
{
    public class Activity
    {
        private MainWindow Context { get; set; }
        private Dictionary<string, UserControl> _views;

        public event ViewChangedHandler OnViewChanged;
        public delegate void ViewChangedHandler(UserControl sender, EventArgs e);

        public UserControl View { get; private set; }

        public Activity(MainWindow context)
        {
            Context = context;
            _views = new Dictionary<string, UserControl>();

            Console.WriteLine("Activity::Create called");
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

                if(OnViewChanged != null)
                    OnViewChanged(View, null);
            }
        }
    }
}
