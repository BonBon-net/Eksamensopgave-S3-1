using EksamensopgaveDbContext;
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
            txbFjernAntalEksemplarer.IsEnabled = false;
            btnTilføjBog.IsEnabled = true;
        }

        private void ClearUserInterface_RegistreringAfbøger()
        {
            txbForfatter.Clear();
            txbTitel.Clear();
            txbUdgiver.Clear();
            txbUdgivelsesDag.Clear();
            txbUdgivelsesMåned.Clear();
            txbUdgivelsesÅr.Clear();
            txbAntalEksemplarer.Clear();
            txbISBN.Clear();
            dgbBogListe.SelectedItem = null;
        }

        private void TilføjBog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuncLayer.TilføjBog(txbForfatter.Text, txbTitel.Text, txbUdgiver.Text,
                    int.Parse(txbUdgivelsesDag.Text), int.Parse(txbUdgivelsesMåned.Text), 
                    int.Parse(txbUdgivelsesÅr.Text), int.Parse(txbAntalEksemplarer.Text), txbISBN.Text);

                MessageBox.Show("Bogen blev tilføjet!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

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
                    int.Parse(txbUdgivelsesDag.Text), int.Parse(txbUdgivelsesMåned.Text),
                    int.Parse(txbUdgivelsesÅr.Text), int.Parse(txbAntalEksemplarer.Text), txbISBN.Text);

                MessageBox.Show("Bogen blev Redigeret!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

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
                FuncLayer.FjernBog(dgbBogListe.SelectedItem as Bog, txbFjernAntalEksemplarer.IsEnabled);

                MessageBox.Show("Bogen blev Fjernet!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

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
                //Bog bog = (Bog)sender;
                if (dgbBogListe.SelectedItem is Bog selectedBog)
                {
                    txbForfatter.Text = selectedBog.Forfatter.ToString();
                    txbTitel.Text = selectedBog.Titel.ToString();
                    txbUdgiver.Text = selectedBog.Udgiver.ToString();
                    txbUdgivelsesDag.Text = selectedBog.UdgivelsesDag.ToString();
                    txbUdgivelsesMåned.Text = selectedBog.UdgivelsesMåned.ToString();
                    txbUdgivelsesÅr.Text = selectedBog.UdgivelsesÅr.ToString();
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
                    btnFjernBog.IsEnabled= false;
                    btnTilføjBog.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "skift knap Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cheackboxFjernEksemplarer_Click(object sender, RoutedEventArgs e)
        {
            if (txbFjernAntalEksemplarer.IsEnabled == false)
            {
                txbFjernAntalEksemplarer.IsEnabled = true;
            }
            else
            {
                txbFjernAntalEksemplarer.IsEnabled = false;
            }
        }
    }
}