using EksamensopgaveDbContext;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Eksamensopgave_S3_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FuncLayer FuncLayer = new FuncLayer();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = FuncLayer;
            btnRedigereBog.IsEnabled = false;
            btnFjernBog.IsEnabled = false;
            btnRedigereUdlån.IsEnabled = false;
            btnFjernUdlån.IsEnabled = false;
        }

        private void ClearUserInterface_RegistreringAfbøger()
        {
            txbForfatter.Text = string.Empty;
            txbTitel.Text = string.Empty;
            txbUdgiver.Text = string.Empty;
            dpBogUdgivelseDato.Text = string.Empty;
            txbAntalEksemplarer.Text = string.Empty;
            txbISBN.Text = string.Empty;
            dgbBogListe.SelectedItem = null;
        }

        private void ClearUserInterface_RegistreringAfUdlån()
        {
            dpDatoUlåner.Text = string.Empty;
            txbLåner.Text = string.Empty;
            txbLånteBog.Text = string.Empty;
            txbAntalBøger.Text = string.Empty;
            dgbBogListeTilLånte.SelectedItem = null;
            dgbUdlånBogListe.SelectedItem = null;
        }

        private void TilføjBog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuncLayer.TilføjBog(txbForfatter.Text, txbTitel.Text, txbUdgiver.Text,
                    dpBogUdgivelseDato.SelectedDate, int.Parse(txbAntalEksemplarer.Text), txbISBN.Text);

                ClearUserInterface_RegistreringAfbøger();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fejl ved tilføjelse af bog: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RedigereBog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuncLayer.RedigereBog(txbForfatter.Text, txbTitel.Text, txbUdgiver.Text,
                    dpBogUdgivelseDato.SelectedDate, int.Parse(txbAntalEksemplarer.Text), txbISBN.Text);

                ClearUserInterface_RegistreringAfbøger();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FjernBog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuncLayer.FjernBog(dgbBogListe.SelectedItem as Bog);
                ClearUserInterface_RegistreringAfbøger();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgbBogListe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgbBogListe.SelectedItem is Bog selectedBog)
                {
                    txbForfatter.Text = selectedBog.Forfatter.ToString();
                    txbTitel.Text = selectedBog.Titel.ToString();
                    txbUdgiver.Text = selectedBog.Udgiver.ToString();
                    dpBogUdgivelseDato.Text = selectedBog.UdgivelseDato.ToString();
                    txbAntalEksemplarer.Text = selectedBog.AntalEksemplarer.ToString();
                    txbISBN.Text = selectedBog.ISBN.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txbISBN_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (FuncLayer.isbnExists(txbISBN.Text))
                {
                    btnRedigereBog.IsEnabled = true;
                    btnFjernBog.IsEnabled = true;
                    btnTilføjBog.IsEnabled = false;
                }
                else
                {
                    btnRedigereBog.IsEnabled = false;
                    btnFjernBog.IsEnabled = false;
                    btnTilføjBog.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUdlånBog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuncLayer.TilføjUdlånBog(dgbBogListeTilLånte.SelectedItem as Bog, dpBogUdgivelseDato.SelectedDate,
                    txbLåner.Text, int.Parse(txbAntalBøger.Text));
                ClearUserInterface_RegistreringAfUdlån();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRedigereUdlån_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuncLayer.RedigereUdlånBog(dpBogUdgivelseDato.SelectedDate,
                    txbLåner.Text, txbLånteBog.Text, int.Parse(txbAntalBøger.Text));
                ClearUserInterface_RegistreringAfUdlån();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnFjernUdlån_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuncLayer.FjernUdlånBog(dgbUdlånBogListe.SelectedItem as UdlånBog);
                ClearUserInterface_RegistreringAfUdlån();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgbUdlånBogListe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgbUdlånBogListe.SelectedItem is UdlånBog selectedUdlånBog)
                {
                    dpDatoUlåner.Text = selectedUdlånBog.UdlånDato.ToString();
                    txbLåner.Text = selectedUdlånBog.Låner;
                    txbLånteBog.Text = selectedUdlånBog.Bog.ISBN;
                    txbAntalBøger.Text = selectedUdlånBog.AntalBøger.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgbVælgBogTilUdlånListe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Bog selectedBog = dgbBogListeTilLånte.SelectedItem as Bog;
                if (selectedBog != null)
                {
                    txbLånteBog.Text = selectedBog.ISBN;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txbLånteBog_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (FuncLayer.isbnExists(txbLånteBog.Text))
                {
                    btnRedigereUdlån.IsEnabled = false;
                    btnFjernUdlån.IsEnabled = false;
                    btnUdlånBog.IsEnabled = true;
                }
                else
                {
                    btnRedigereUdlån.IsEnabled = true;
                    btnFjernUdlån.IsEnabled = true;
                    btnUdlånBog.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}