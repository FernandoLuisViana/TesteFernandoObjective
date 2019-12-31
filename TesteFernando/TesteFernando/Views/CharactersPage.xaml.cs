using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TesteFernando.Custom;
using TesteFernando.Models;
using TesteFernando.Services;
using TesteFernando.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TesteFernando.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharactersPage : ContentPage, INotifyPropertyChanged
    {
        #region Ctor
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CharactersPage()
        {
            InitializeComponent();
            BindingContext = this;            
            Loading();
        }
        #endregion

        #region Methods
        public async void Loading()
        {
            Load();
            await GetCharacter("4", QtdCharacters.ToString());
        }

        public void Load()
        {            
            TwoSelected = false;
            ActualPage = 1;
            QtdCharacters = 0;
            TxtButtonStart = ActualPage.ToString();
            TxtButtonCenter = (ActualPage + 1).ToString();
            TxtButtonEnd = (ActualPage + 2).ToString();
            StartButtonClicked();
            CenterButton();
            EndButton();
        }

        private async void listCharacter_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var character = e.SelectedItem as Character;

            await Navigation.PushModalAsync(new CharactersDetailPage(character));
        }

        private void SyncItensTapped(object sender, System.EventArgs e)
        {
            Loading();
            navigatiomBar.IsVisible = true;
            btnSync.IsVisible = false;
        }

        private async void SearchBar_Unfocused(object sender, FocusEventArgs e)
        {
            if (ConnectionUtils.IsConnected())
            {
                var nameCharacters = sender as Entry;
                if (!string.IsNullOrEmpty(nameCharacters.Text))
                {
                    navigatiomBar.IsVisible = false;
                    btnSync.IsVisible = true;
                    await Navigation.PushModalAsync(new CustomGIFLoader());

                    ApiService apiService = new ApiService();

                    var charactersList = await apiService.GetCharactersByName(nameCharacters.Text, "4", "0");

                    foreach (var item in charactersList.Data.Results)
                    {
                        if (!string.IsNullOrEmpty(item.Thumbnail.Path))
                        {
                            var uriHttps = item.Thumbnail.Path.Replace("http", "https");
                            item.ImageUri = uriHttps + "/detail.jpg";
                        }
                    }

                    var pages = charactersList.Data.Total / 4;

                    listCharacter.ItemsSource = charactersList.Data.Results;

                    txtSearchBar.Text = null;

                    Load();

                    await Navigation.PopModalAsync();
                }
            }
            else
            {
                btnSync.IsVisible = true;
                await DisplayAlert("Atenção", "Verifique sua conexão", "OK");
            }
        }
        public async Task GetCharacter(string limit, string offset)
        {
            if (ConnectionUtils.IsConnected())
            {
                await Navigation.PushModalAsync(new CustomGIFLoader());

                ApiService apiService = new ApiService();

                var charactersList = await apiService.GetCharacters(limit, offset);

                foreach (var item in charactersList.Data.Results)
                {
                    if (!string.IsNullOrEmpty(item.Thumbnail.Path))
                    {
                        var uriHttps = item.Thumbnail.Path.Replace("http", "https");
                        item.ImageUri = uriHttps + "/detail.jpg";
                    }
                }

                var pages = charactersList.Data.Total / 4;

                listCharacter.ItemsSource = charactersList.Data.Results;

                await Navigation.PopModalAsync();
            }
            else
            {
                btnSync.IsVisible = true;
                await DisplayAlert("Atenção", "Verifique sua conexão", "OK");
            }
        }
        #endregion

        #region Prop
        private bool twoSelected;
        public bool TwoSelected
        {
            get { return twoSelected; }
            set { twoSelected = value; NotifyPropertyChanged(); }
        }
        private int actualPage;
        public int ActualPage
        {
            get { return actualPage; }
            set { actualPage = value; NotifyPropertyChanged(); }
        }

        private int qtdCharacters;
        public int QtdCharacters
        {
            get { return qtdCharacters; }
            set { qtdCharacters = value; NotifyPropertyChanged(); }
        }

        private string txtButtonStart;
        public string TxtButtonStart
        {
            get { return txtButtonStart; }
            set
            {
                txtButtonStart = value;
                NotifyPropertyChanged();
            }
        }

        private string txtButtonCenter;
        public string TxtButtonCenter
        {
            get { return txtButtonCenter; }
            set
            {
                txtButtonCenter = value;
                NotifyPropertyChanged();
            }
        }

        private string txtButtonEnd;
        public string TxtButtonEnd
        {
            get { return txtButtonEnd; }
            set
            {
                txtButtonEnd = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Clicked
        private async void NextPageCharacters_Clicked(object sender, System.EventArgs e)
        {
            if (ActualPage == 1)
            {
                ActualPage++;

                StartButton();

                CenterButtonClicked();

                EndButton();
            }
            else
            {
                StartButton();

                CenterButton();

                EndButtonClicked();
            }
            await GetCharacter("4", QtdCharacters.ToString());
        }

        private async void FrontPageCharacters_Clicked(object sender, EventArgs e)
        {
            if (ActualPage > 1)
            {
                ActualPage = ActualPage - 1;
                
                if (ActualPage == 1)
                {
                    StartButtonClicked();

                    CenterButton();

                    EndButton();
                }
                else
                {
                    ControlPage();

                    StartButton();

                    CenterButtonClicked();

                    EndButton();
                }

                await GetCharacter("4", QtdCharacters.ToString());
            }
            else
                await DisplayAlert("Atenção", "Você já esta na primeira página", "OK");

        }

        private async void StartButton_Clicked(object sender, EventArgs e)
        {
            ActualPage--;
            if (ActualPage > 1)
            {
                StartButton();

                ControlPage();

                CenterButtonClicked();

                EndButton();
                await GetCharacter("4", QtdCharacters.ToString());
            }
            else
            if (ActualPage == 1)
            {
                StartButtonClicked();

                CenterButton();

                EndButton();
                await GetCharacter("4", QtdCharacters.ToString());
            }
            else
                await DisplayAlert("Atenção", "Você já esta na primeira página", "OK");

        }

        private async void CenterButton_Clicked(object sender, EventArgs e)
        {
            StartButton();

            ActualPage++;

            CenterButtonClicked();

            EndButton();

            await GetCharacter("4", QtdCharacters.ToString());
        }

        private async void EndButton_Clicked(object sender, EventArgs e)
        {
            StartButton();

            CenterButton();

            EndButtonClicked();

            await GetCharacter("4", QtdCharacters.ToString());
        }
        #endregion

        #region ControlButtons
        public void StartButtonClicked()
        {            
            QtdCharacters = (ActualPage - 1) * 4 ;
            btnstart.BackgroundColor = Color.FromHex("#D42026");
            btnstart.TextColor = Color.FromHex("#FFFFFF");
        }

        public void StartButton()
        {
            btnstart.BackgroundColor = Color.FromHex("#FFFFFF");
            btnstart.TextColor = Color.FromHex("#D42026");
        }

        public void CenterButtonClicked()
        {
            QtdCharacters = (ActualPage - 1) * 4;
            btncenter.BackgroundColor = Color.FromHex("#D42026");
            btncenter.TextColor = Color.FromHex("#FFFFFF");
        }

        public void CenterButton()
        {
            btncenter.BackgroundColor = Color.FromHex("#FFFFFF");
            btncenter.TextColor = Color.FromHex("#D42026");
        }

        public void EndButtonClicked()
        {
            btnend.BackgroundColor = Color.FromHex("#D42026");
            btnend.TextColor = Color.FromHex("#FFFFFF");
            ActualPage++;

            ControlPage();

            CenterButtonClicked();

            EndButton();

            
        }

        public void EndButton()
        {
            btnend.BackgroundColor = Color.FromHex("#FFFFFF");
            btnend.TextColor = Color.FromHex("#D42026");
        }

        public void ControlPage()
        {
            TxtButtonStart = (ActualPage - 1).ToString();
            TxtButtonCenter = ActualPage.ToString();
            TxtButtonEnd = (ActualPage + 1).ToString();
        }
        #endregion
    }
}