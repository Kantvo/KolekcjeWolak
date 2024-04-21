using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Controls;

namespace kolekcjewolak
{
    public partial class MainPage : ContentPage
    {
        private string folderKolekcji = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Kolekcje");
        public ObservableCollection<Kolekcja> Kolekcje = new ObservableCollection<Kolekcja>();

        public MainPage()
        {
            InitializeComponent();

            if (!Directory.Exists(folderKolekcji))
                Directory.CreateDirectory(folderKolekcji);

            WczytajKolekcje();
            Debug.WriteLine(folderKolekcji);
        }

        private async void DodajKolekcje(object sender, EventArgs e)
        {
            string nazwa = await DisplayPromptAsync("Tworzenie", "Podaj nazwę nowej kolekcji", "Stwórz", "Anuluj");

            if (string.IsNullOrWhiteSpace(nazwa) || nazwa == "Anuluj")
                return;

            Kolekcja nowaKolekcja = new Kolekcja()
            {
                Nazwa = nazwa
            };

            ZapiszPlikKolekcji(nowaKolekcja);
            Kolekcje.Add(nowaKolekcja);
            await Navigation.PushAsync(new Kolekcja() { Nazwa = nowaKolekcja.Nazwa });
        }

        private void UsunKolekcje(object sender, EventArgs e)
        {
            Kolekcja wybranaKolekcja = wyborKolekcji.SelectedItem as Kolekcja;

            if (wybranaKolekcja == null)
                return;

            UsunPlikKolekcji(wybranaKolekcja);
            Kolekcje.Remove(wybranaKolekcja);
        }

        private async void PrzejdzDoKolekcji(object sender, EventArgs e)
        {
            if (wyborKolekcji.SelectedItem == null)
                return;

            await Navigation.PushAsync(new Kolekcja() { Nazwa = (wyborKolekcji.SelectedItem as Kolekcja).Nazwa });
        }

        private void WczytajKolekcje()
        {
            var pliki = Directory.GetFiles(folderKolekcji)
                                 .OrderBy(plik => new FileInfo(plik).CreationTime);

            foreach (string plik in pliki)
            {
                var nazwaPliku = Path.GetFileName(plik).Split(".")[0];

                Kolekcja kolekcja = new Kolekcja() { Nazwa = nazwaPliku };

                Kolekcje.Add(kolekcja);
            }

            wyborKolekcji.ItemsSource = Kolekcje;
        }

        private void WyborKolekcji_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wyborKolekcji.SelectedIndex != -1)
            {
                var wybranaKolekcja = wyborKolekcji.SelectedItem as Kolekcja;
                Debug.WriteLine("Wybrano kolekcję: " + wybranaKolekcja.Nazwa);
            }
        }

        private void ZapiszPlikKolekcji(Kolekcja kolekcja)
        {
            string sciezkaPliku = Path.Combine(folderKolekcji, $"{kolekcja.Nazwa}.txt");
            try
            {
                File.Create(sciezkaPliku).Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd w zapisywaniu kolekcji do pliku: {ex.Message}");
            }
        }

        private void UsunPlikKolekcji(Kolekcja kolekcja)
        {
            string sciezkaPliku = Path.Combine(folderKolekcji, $"{kolekcja.Nazwa}.txt");
            if (File.Exists(sciezkaPliku))
            {
                try
                {
                    File.Delete(sciezkaPliku);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd w usuwaniu kolekcji: {ex.Message}");
                }
            }
        }
    }
}
