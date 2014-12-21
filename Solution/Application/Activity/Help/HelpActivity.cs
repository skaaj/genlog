
namespace Genlog
{
    public class HelpActivity : Activity///Détermine l'Activity utilisée pour la View Help
    {

        public HelpActivity(MainWindow context) : base(context)
        {
            Reload();

            Show("home");
        }

        public override void Reload()
        {
            base.Reload();

            AddView("home", new HelpView());
        }
    }
}
