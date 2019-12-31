using TesteFernando.Views;
using Xamarin.Forms;

namespace TesteFernando
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new CharactersPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
