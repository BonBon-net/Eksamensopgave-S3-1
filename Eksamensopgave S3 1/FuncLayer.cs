//using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using EksamensopgaveDbContext;
using Microsoft.EntityFrameworkCore;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


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
        }
        public ObservableCollection<Bog> BogListe
        {
            get
            {
                return model.BogTabel.Local.ToObservableCollection();
            }
        }

        public bool isbnExists(string søgISBN)
        {
            for (int i = 0; i < BogListe.Count; i++)
            {
                if (BogListe[i].ISBN == søgISBN)
                {
                    return true;
                }
            }
            return false;
        }

        public Bog TilføjBog(string Forfatter, string Titel, string Udgiver,
            int UdgivelsesDag, int UdgivelsesMåned, int UdgivelsesÅr, int AntalEksemplarer, string ISBN)
        {
            // Check if any of the required book details are missing or invalid
            if (string.IsNullOrEmpty(Forfatter) || string.IsNullOrEmpty(Titel) ||
                string.IsNullOrEmpty(Udgiver) || UdgivelsesDag < 1 ||
                UdgivelsesMåned < 1 || UdgivelsesÅr < 1 ||
                AntalEksemplarer < 1 || string.IsNullOrEmpty(ISBN))
            {
                // If they are, throw an exception with an error message
                throw new ArgumentException("One or more of the provided arguments are invalid.");
            }

            // Create a new 'Bog' (Book) object using the provided details
            Bog bog = new Bog(
                0, // Assuming Entity Framework will assign a unique ID
                Forfatter,
                Titel,
                Udgiver,
                UdgivelsesDag,
                UdgivelsesMåned,
                UdgivelsesÅr, // Create a new DateOnly object from the publication year
                AntalEksemplarer,
                ISBN
            );

            // Add the newly created book object to the 'BogTabel' (BookTable) in the database context
            model.BogTabel.Add(bog);
            // Save the changes to the database
            model.SaveChanges();
            // Notify the UI or other parts of the application that the 'BogListe' (BookList) has changed
            RaisePropertyChanged(nameof(BogListe));
            // Return the newly added book object
            return bog;
        }

        public Bog RedigereBog(string Forfatter, string Titel, string Udgiver,
            int UdgivelsesDag, int UdgivelsesMåned, int Udgivelsesår, int AntalEksemplarer, string ISBN)
        {
            // Validate the input arguments to ensure none are null, empty, or less than 1.
            // An ArgumentException is thrown if any of the provided values are invalid.
            if (string.IsNullOrEmpty(Forfatter) || string.IsNullOrEmpty(Titel) ||
                string.IsNullOrEmpty(Udgiver) || UdgivelsesDag < 1 ||
                UdgivelsesMåned < 1 || Udgivelsesår < 1 ||
                AntalEksemplarer < 1 || string.IsNullOrEmpty(ISBN))
            {
                throw new ArgumentException("One or more of the provided arguments are invalid.");
            }

            // Find the book in the database table ('BogTabel') that matches the given ISBN.
            var bog = model.BogTabel.FirstOrDefault(b => b.ISBN == ISBN);

            // If no book is found with the specified ISBN, throw an InvalidOperationException.
            if (bog == null)
            {
                throw new InvalidOperationException("Bogen med det angivne ISBN blev ikke fundet.");
            }

            var existingBog = BogListe.First(v => v.ISBN == ISBN);
            if (existingBog != null)
            {
                // Update the properties of the found book object with the new values provided as arguments.
                bog.Forfatter = Forfatter;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.Forfatter)));
                bog.Titel = Titel;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.Titel)));
                bog.Udgiver = Udgiver;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.Udgiver)));
                bog.UdgivelsesDag = UdgivelsesDag;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.UdgivelsesDag)));
                bog.UdgivelsesMåned = UdgivelsesMåned;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.UdgivelsesMåned)));
                bog.UdgivelsesÅr = Udgivelsesår;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.UdgivelsesÅr)));
                bog.AntalEksemplarer = AntalEksemplarer;
                PropertyChanged?.Invoke(existingBog, new PropertyChangedEventArgs(nameof(existingBog.AntalEksemplarer)));
            }
            else
            {
                throw new Exception("bog Eksisterer ikke");
            }

            // Save the changes made to the book object back to the database.
            model.SaveChanges();
            // Notify the UI or other parts of the application that the book list property has changed.
            RaisePropertyChanged(nameof(BogListe));
            // Return the updated book object.
            return bog;
        }

        public Bog FjernBog(Bog? bog, bool)
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
    }
}
