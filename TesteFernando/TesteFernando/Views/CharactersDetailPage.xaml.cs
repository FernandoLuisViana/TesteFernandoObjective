
using TesteFernando.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TesteFernando.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharactersDetailPage : ContentPage
    {
        public CharactersDetailPage(Character character)
        {
            InitializeComponent();
            Loading(character);
        }

        public async void Loading(Character character)
        {
            foreach (var item in character.Urls)
            {
                if (item.type.Contains("wiki"))
                {
                    var url = item.url.Replace("http", "https");
                    web.Source = url;
                }
            }
        }
    }
}