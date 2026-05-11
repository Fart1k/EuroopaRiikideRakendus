using EuroopaRiikideRakendus.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EuroopaRiikideRakendus;

public partial class CountryPage : ContentPage
{
	ObservableCollection<Riik> riigid;

	StackLayout textLayout;

	Grid grid;

    Label nimiLabel, pealinnLabel;

	ListView riigidListView;

	Entry nimiEntry;
	Entry pealinnEntry;
	Entry rahvaarvEntry;
	Entry lippEntry;

	Button lisaBtn, kustutaBtn, salvestaBtn, puhastaBtn;

	Image lippImage;

	Riik valitudRiik;
	public CountryPage()
	{
		riigid = new ObservableCollection<Riik>
		{
			new Riik
			{
				Nimi = "Eesti",
				Pealinn = "Tallinn",
				Rahvaarv = 1360000,
				Lipp = "estonia.png"
			},

			new Riik
			{
				Nimi = "Soome",
				Pealinn = "Helsingi",
				Rahvaarv = 5550000,
				Lipp = "finland.png"
			},

			new Riik
			{
				Nimi = "Rootsi",
				Pealinn = "Stocholm",
				Rahvaarv = 10500000,
				Lipp = "sweden.png"
			},
		};

		nimiEntry = new Entry
		{
			Placeholder = "Riigi nimetus"
		};
		pealinnEntry = new Entry
		{
			Placeholder = "Riigi pealinn"
		};
		rahvaarvEntry = new Entry
		{
			Placeholder = "Riigi rahvaarv",
			Keyboard = Keyboard.Numeric
		};
		lippEntry = new Entry
		{
			Placeholder = "Riigi lipp"
		};

		lisaBtn = new Button
		{
			Text = "Lisa",
			Command = new Command(OnLisaClicked)
		};

		kustutaBtn = new Button
		{
			Text = "Kustuta",
			Command = new Command(OnKustutaClicked)
		};

		salvestaBtn = new Button
		{
			Text = "Salvesta",
			Command = new Command(OnSalvestaClicked)
		};

		puhastaBtn = new Button
		{
			Text = "Puhasta tekst",
			Command = new Command(Puhasta)
		};

		riigidListView = new ListView
		{
			RowHeight = 70,
			ItemsSource = riigid,
			ItemTemplate = new DataTemplate(() =>
			{
				lippImage = new Image
				{
					WidthRequest = 60,
					HeightRequest = 40,
					Aspect = Aspect.AspectFill
				};
				lippImage.SetBinding(Image.SourceProperty, "Lipp");

				nimiLabel = new Label
				{
					FontAttributes = FontAttributes.Bold,
					FontSize = 18
				};
				nimiLabel.SetBinding(Label.TextProperty, "Nimi");

				pealinnLabel = new Label
				{
					FontSize = 14
				};
				pealinnLabel.SetBinding(Label.TextProperty, "Pealinn");

				textLayout = new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Children =
					{
						nimiLabel,
						pealinnLabel
					}
				};

				grid = new Grid
				{
					Padding = 5,
					ColumnDefinitions =
					{
						new ColumnDefinition { Width = 80 },
						new ColumnDefinition { Width = GridLength.Star }
					}
				};

				grid.Add(lippImage);
				grid.Add(textLayout, 1, 0);

				return new ViewCell
				{
					View = grid
				};
			})
		};

		riigidListView.ItemTapped += RiigidListView_ItemTapped;

		Content = new ScrollView
		{
			Content = new VerticalStackLayout
			{
				Padding = 10,
				Spacing = 10,

				Children =
				{
					nimiEntry, pealinnEntry, rahvaarvEntry, lippEntry,
                    puhastaBtn, lisaBtn, salvestaBtn, kustutaBtn,
					riigidListView
				}
			}
		};
	}

	// ListView vajutus
    private async void RiigidListView_ItemTapped(object? sender, ItemTappedEventArgs e)
    {
		if (e.Item == null)
		{
			return;
		}

		valitudRiik = (Riik)e.Item;

        nimiEntry.Text = valitudRiik.Nimi;
        pealinnEntry.Text = valitudRiik.Pealinn;
        rahvaarvEntry.Text = valitudRiik.Rahvaarv.ToString();
        lippEntry.Text = valitudRiik.Lipp;

        await DisplayAlertAsync(
			"Riigi info", 
			$"Riik: {valitudRiik.Nimi}\n"
			+ $"Pealinn: {valitudRiik.Pealinn}\n"
			+ $"Rahvaarv: {valitudRiik.Rahvaarv} inimest",
			"OK");
    }

	// Salvesta
    private async void OnSalvestaClicked(object obj)
    {
		if (valitudRiik == null)
		{
			await DisplayAlertAsync(
				"Viga",
				"Vali kõigepealt riik",
				"OK");

			return;
		}

        valitudRiik.Nimi = nimiEntry.Text;
        valitudRiik.Pealinn = pealinnEntry.Text;
        valitudRiik.Rahvaarv = int.Parse(rahvaarvEntry.Text);
        valitudRiik.Lipp = lippEntry.Text;

		riigidListView.ItemsSource = null;
		riigidListView.ItemsSource = riigid;
    }

	// Kustuta
    private async void OnKustutaClicked(object obj)
    {
		if (valitudRiik == null)
		{
			await DisplayAlertAsync(
				"Viga",
				"Vali kõigepealt riik",
				"OK");

			return;
		}

		riigid.Remove(valitudRiik);

		Puhasta();
    }

	//Puhasta
	private void Puhasta()
	{
        nimiEntry.Text = "";
        pealinnEntry.Text = "";
        rahvaarvEntry.Text = "";
        lippEntry.Text = "";
    }

	// Lisa
    private async void OnLisaClicked(object obj)
    {
		string uusNimi = nimiEntry.Text;

		bool olemas = riigid.Any(r => r.Nimi.Equals(uusNimi, StringComparison.OrdinalIgnoreCase));

		if (olemas)
		{
			await DisplayAlertAsync(
				"Viga",
				"See riik on juba nimekirjas",
				"OK");

			return;
		}

		Riik uusRiik = new Riik
		{
			Nimi = nimiEntry.Text,
			Pealinn = pealinnEntry.Text,
			Rahvaarv = int.Parse(rahvaarvEntry.Text),
			Lipp = lippEntry.Text
		};

		riigid.Add(uusRiik);
		Puhasta();
    }
}