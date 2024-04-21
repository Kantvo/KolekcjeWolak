using System;
using System.ComponentModel;

namespace kolekcjewolak
{
    public class Element : INotifyPropertyChanged
    {
        private string _nazwa;
        public string Nazwa
        {
            get => _nazwa;
            set
            {
                if (_nazwa != value)
                {
                    _nazwa = value;
                    OnPropertyChanged(nameof(Nazwa));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string nazwaWlasciwosci)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nazwaWlasciwosci));
        }
    }
}
