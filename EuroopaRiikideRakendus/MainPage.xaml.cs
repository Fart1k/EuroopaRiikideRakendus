namespace EuroopaRiikideRakendus
{
    public partial class MainPage : ContentPage
    {

        Label titleLabel;
        Button startBtn;


        public MainPage()
        {
            titleLabel = new Label
            {
                Text = "Euroopa Riikid",
                FontSize = 34,
                HorizontalOptions = LayoutOptions.Center
            };

            startBtn = new Button
            {
                Text = "Alusta",
                Command = new Command(async () =>
                {
                    await Navigation.PushAsync(new CountryPage());
                })
            };

            Content = new VerticalStackLayout
            {
                Padding = 30,
                Spacing = 20,
                Children =
                {
                    titleLabel, startBtn
                }
            };

        }

    }
}
