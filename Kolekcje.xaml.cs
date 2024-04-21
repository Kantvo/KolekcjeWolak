using System.Collections.ObjectModel;

namespace kolekcjewolak;

public partial class Kolekcja : ContentPage
{
    public string Nazwa { get; set; }

    public ObservableCollection<Element> Elementy = new ObservableCollection<Element>();

    public Kolekcja()
    {
        InitializeComponent();

        elementy.ItemsSource = Elementy;
        WczytajElementyZPliku();
    }

    private async void DodajElement(object sender, EventArgs e)
    {
        string nazwa = await DisplayPromptAsync("Nowy element", "Podaj nazwê elementu", "Dodaj", "Anuluj");
        if (string.IsNullOrWhiteSpace(nazwa) || nazwa == "Anuluj")
            return;

        Element element = new Element { Nazwa = nazwa };
        Elementy.Add(element);
        ZapiszElementyDoPliku();
    }

    private void UsunElement(object sender, EventArgs e)
    {
        if (elementy.SelectedItem is Element wybranyElement)
        {
            Elementy.Remove(wybranyElement);
            ZapiszElementyDoPliku();
        }
    }

    private async void EdytujElement(object sender, EventArgs e)
    {
        if (elementy.SelectedItem is Element wybranyElement)
        {
            string nowaNazwa = await DisplayPromptAsync("Edycja elementu", "Nowa nazwa elementu", initialValue: wybranyElement.Nazwa, accept: "Edytuj", cancel: "Anuluj");
            if (!string.IsNullOrWhiteSpace(nowaNazwa) && nowaNazwa != "Anuluj")
            {
                wybranyElement.Nazwa = nowaNazwa;
                ZapiszElementyDoPliku();
            }
        }
    }

    private void WczytajElementyZPliku()
    {
        string sciezkaPliku = PobierzSciezkePliku();
        if (File.Exists(sciezkaPliku))
        {
            var linie = File.ReadAllLines(sciezkaPliku);
            foreach (var linia in linie)
            {
                Elementy.Add(new Element { Nazwa = linia });
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        WczytajElementyZPliku();
    }

    private void ZapiszElementyDoPliku()
    {
        string sciezkaPliku = PobierzSciezkePliku();
        File.WriteAllLines(sciezkaPliku, Elementy.Select(e => e.Nazwa));
    }

    private string PobierzSciezkePliku()
    {
        return Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Kolekcje"), $"{Nazwa}.txt");
    }
}
