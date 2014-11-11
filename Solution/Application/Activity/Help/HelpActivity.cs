
namespace Genlog
{
    public class HelpActivity : Activity
    {

        public HelpActivity(MainWindow context) : base(context)
        {
            AddView("home", new HelpView());

            Show("home");
        }
    }
}
