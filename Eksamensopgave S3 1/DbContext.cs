using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamensopgaveDbContext
{
    public class Model : DbContext
    {
        public DbSet<Bog> BogTabel { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) { options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Eksamensopgave_S3_1; Trusted_Connection = True; "); }
    }

    public class Bog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Bog(int id, string forfatter, string titel, 
            string udgiver, int udgivelsesDag, int udgivelsesMåned, 
            int udgivelsesÅr,  int antalEksemplarer, string iSBN)
        {
            Id = id;
            Forfatter = forfatter;
            Titel = titel;
            Udgiver = udgiver;
            UdgivelsesDag = udgivelsesDag;
            UdgivelsesMåned = udgivelsesMåned;
            UdgivelsesÅr = udgivelsesÅr;
            AntalEksemplarer = antalEksemplarer;
            ISBN = iSBN;
        }

        public int Id { get; set; }
        public string Forfatter { get; set; }
        public string Titel { get; set; }
        public string Udgiver { get; set; }
        public int UdgivelsesDag { get; set; }
        public int UdgivelsesMåned { get; set; }
        public int UdgivelsesÅr { get; set; }
        public int AntalEksemplarer { get; set; }
        public string ISBN { get; set; }
    }
}
