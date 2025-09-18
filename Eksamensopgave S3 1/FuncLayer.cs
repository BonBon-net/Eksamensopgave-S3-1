using EksamensopgaveDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;


namespace Eksamensopgave_S3_1
{
    internal class FuncLayer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        //RaisePropertyChanged(nameof());
        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        Model model { get; set; } = new Model();
        public FuncLayer()
        {
            model.BogTabel.Load();
            RaisePropertyChanged(nameof(BogListe));
            model.UdlånBogTabel.Load();
            RaisePropertyChanged(nameof(UdlånBogListe));
        }
        public ObservableCollection<Bog> BogListe
        {
            get
            {
                return model.BogTabel.Local.ToObservableCollection();
            }
        }
        public ObservableCollection<UdlånBog> UdlånBogListe
        {
            get
            {
                return model.UdlånBogTabel.Local.ToObservableCollection();
            }
        }

        public bool isbnExists(string søgISBN)
        {
            if (BogListe.FirstOrDefault(b => b.ISBN == søgISBN) != null)
            {
                return true;
            }
            return false;
        }

        public Bog TilføjBog(string Forfatter, string Titel, string Udgiver,
            DateTime? date, int AntalEksemplarer, string ISBN)
        {
            ValiderBogData(Forfatter, Titel, Udgiver, date, AntalEksemplarer, ISBN);

            Bog bog = new Bog(
                0,
                Forfatter,
                Titel,
                Udgiver,
                date.Value,
                AntalEksemplarer,
                ISBN
                
            );

            model.BogTabel.Add(bog);
            model.SaveChanges();
            RaisePropertyChanged(nameof(BogListe));
            return bog;
        }

        public Bog RedigereBog(string Forfatter, string Titel, string Udgiver,
            DateTime? date, int AntalEksemplarer, string ISBN)
        {
            ValiderBogData(Forfatter, Titel, Udgiver, date, AntalEksemplarer, ISBN);

            var bog = model.BogTabel.FirstOrDefault(b => b.ISBN == ISBN);

            if (bog == null)
            {
                throw new InvalidOperationException("Bogen med det angivne ISBN blev ikke fundet.");
            }

            var existingBog = BogListe.First(v => v.ISBN == ISBN);
            if (existingBog != null)
            {
                bog.Forfatter = Forfatter;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.Forfatter)));
                bog.Titel = Titel;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.Titel)));
                bog.Udgiver = Udgiver;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.Udgiver)));
                bog.UdgivelseDato = date.Value;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.UdgivelseDato)));
                bog.AntalEksemplarer = AntalEksemplarer;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.AntalEksemplarer)));
            }
            else
            {
                throw new Exception("bog Eksisterer ikke");
            }

            model.SaveChanges();
            RaisePropertyChanged(nameof(BogListe));
            return bog;
        }

        public Bog FjernBog(Bog? bog)
        {
            if (bog == null)
            {
                throw new Exception("Den valgte bog findes ikke");
            }

            model.Remove(bog);
            model.SaveChanges();
            RaisePropertyChanged(nameof(BogListe));
            return bog;
        }

        public UdlånBog TilføjUdlånBog(Bog bog, DateTime? date, string Låner, int AntalBøger)
        {
            if (bog == null)
            {
                throw new Exception("Den valgte bog findes ikke");
            }

            ValiderUdlånBogData(date, Låner, AntalBøger);

            UdlånBog udlånBog = new UdlånBog(
                0,
                date.Value,
                Låner,
                bog,
                AntalBøger
            );

            model.Add(udlånBog);
            RaisePropertyChanged(nameof(UdlånBogListe));

            return udlånBog;
        }

        public UdlånBog RedigereUdlånBog(DateTime? date, string låner, string ISBN, int antalBøger)
        {
            ValiderUdlånBogData(date, låner, antalBøger);

            var existingUdlånBog = model.UdlånBogTabel.FirstOrDefault(b => b.Bog.ISBN == ISBN);
            if (existingUdlånBog != null)
            {
                existingUdlånBog.UdlånDato = date.Value;
                PropertyChanged?.Invoke(existingUdlånBog, new PropertyChangedEventArgs(nameof(existingUdlånBog.UdlånDato)));
                existingUdlånBog.Låner = låner;
                PropertyChanged?.Invoke(existingUdlånBog, new PropertyChangedEventArgs(nameof(existingUdlånBog.Låner)));
                existingUdlånBog.Bog.ISBN = ISBN;
                PropertyChanged?.Invoke(existingUdlånBog, new PropertyChangedEventArgs(nameof(existingUdlånBog.Bog.ISBN)));
                existingUdlånBog.AntalBøger = antalBøger;
                PropertyChanged?.Invoke(existingUdlånBog, new PropertyChangedEventArgs(nameof(existingUdlånBog.AntalBøger)));
            }
            else
            {
                throw new Exception("bog Eksisterer ikke");
            }

            model.SaveChanges();

            return existingUdlånBog;
        }

        public UdlånBog FjernUdlånBog(UdlånBog? udlånBog)
        {
            if (udlånBog == null)
            {
                throw new Exception("Den valgte bog findes ikke");
            }

            model.Remove(udlånBog);
            model.SaveChanges();
            RaisePropertyChanged(nameof(UdlånBogListe));
            return udlånBog;
        }

        private void ValiderBogData(string forfatter, string titel, string udgiver, DateTime? date, int antalEksemplarer, string isbn)
        {
            if (date == null)
            {
                throw new ArgumentException("Udgivelses Dag skal være valgt.");
            }

            if (string.IsNullOrEmpty(forfatter))
            {
                throw new ArgumentException("Forfatter kan ikke være null eller tom.");
            }

            if (string.IsNullOrEmpty(titel))
            {
                throw new ArgumentException("Titel må ikke være tom eller null.");
            }

            if (string.IsNullOrEmpty(udgiver))
            {
                throw new ArgumentException("Udgiver kan ikke være nul eller tom.");
            }

            if (date.Value.Day < 1 || date.Value.Day > 30)
            {
                throw new ArgumentException("Udgivelses Dag skal være mellem 1 og 30.");
            }

            if (date.Value.Month < 1 || date.Value.Month > 12)
            {
                throw new ArgumentException("Udgivelses Måned skal være mellem 1 og 12.");
            }

            if (date.Value.Year < 1)
            {
                throw new ArgumentException("Udgivelses År skal være et positivt tal.");
            }

            if (antalEksemplarer < 1)
            {
                throw new ArgumentException("Antal Eksemplarer skal være et positivt tal.");
            }

            if (string.IsNullOrEmpty(isbn))
            {
                throw new ArgumentException("ISBN-nummeret må ikke være null eller tomt.");
            }
        }

        private void ValiderUdlånBogData(DateTime? date, string Låner, int AntalBøger)
        {
            if (date == null)
            {
                throw new ArgumentException("Udgivelses Dag skal være valgt.");
            }

            if (date.Value.Day < 0 || date.Value.Day > 30)
            {
                throw new ArgumentException("Udgivelses Dag skal være mellem 1 og 30");
            }

            if (date.Value.Month < 0 || date.Value.Month > 12)
            {
                throw new ArgumentException("Release Month must be between 1 and 12.");
            }

            if (date.Value.Year < 0)
            {
                throw new ArgumentException("Låneår skal være et positivt tal.");
            }

            if (string.IsNullOrEmpty(Låner))
            {
                throw new ArgumentException("Låntagere kan ikke være nul eller tomme.");
            }

            if (AntalBøger < 1)
            {
                throw new ArgumentException("Antal Bøger skal være et positivt tal.");
            }
        }
    }
}
