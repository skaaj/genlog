using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Genlog.View
{
    public class ViewManager
    {
        private static ViewManager instance;
        private Dictionary<string, UserControl> _viewList;

        public static ViewManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ViewManager();

                return instance;
            }
        }

        private ViewManager()
        {
            _viewList = new Dictionary<string, UserControl>();

            _viewList.Add("home", new HomeView());
            _viewList.Add("memtest", new MemoryTestView());
            _viewList.Add("foctest", new FocusTestView());
            _viewList.Add("stats", new StatisticsView());
            _viewList.Add("help", new HelpView());
        }

        public UserControl this[string key]
        {
            get
            {
                if (!_viewList.ContainsKey(key)) return null;

                return _viewList[key];
            }
        }
    }
}
