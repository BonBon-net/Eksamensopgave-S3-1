using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace EksamensopgaveDbContext
{
    public class Model : DbContext
    {
        public DbSet<Bog> BogTabel { get; set; }
        public DbSet<UdlånBog> UdlånBogTabel { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) { options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Eksamensopgave_S3_1; Trusted_Connection = True; "); }
    }

    public class Bog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Bog() { }
        public Bog(int id, string forfatter, string titel,
            string udgiver, DateTime udgivelseDato, int antalEksemplarer, string iSBN)
        {
            Id = id;
            Forfatter = forfatter;
            Titel = titel;
            Udgiver = udgiver;
            UdgivelseDato = udgivelseDato;
            AntalEksemplarer = antalEksemplarer;
            ISBN = iSBN;
        }

        public int Id { get; set; }
        public string Forfatter { get; set; }
        public string Titel { get; set; }
        public string Udgiver { get; set; }
        public DateTime UdgivelseDato { get; set; }
        public int AntalEksemplarer { get; set; }
        public string ISBN { get; set; }
    }

    public class UdlånBog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public UdlånBog() { }
        public UdlånBog(int id, DateTime udlånDato, string låner, Bog bog, int antalBøger)
        {
            Id = Id;
            UdlånDato = udlånDato;
            Låner = låner;
            Bog = bog;
            AntalBøger = antalBøger;
        }

        public int Id { get; set; }
        public DateTime UdlånDato { get; set; }
        public string Låner { get; set; }
        public Bog Bog { get; set; }
        public int AntalBøger { get; set; }
    }
}
