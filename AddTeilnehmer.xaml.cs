using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Klassenliste
{
    /// <summary>
    /// Interaktionslogik für AddTeilnehmer.xaml
    /// </summary>
    public partial class AddTeilnehmer : Window
    {
        public AddTeilnehmer()
        {
            InitializeComponent();
            fillKlasse();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            using(var con = new KlassenlisteEntities1())
            {
                Teilnehmer t = new Teilnehmer();
                t.Name = name.Text;
                t.Vorname = vorname.Text;
                t.Email = email.Text;
                t.Geburtsdatum = geburtstag.SelectedDate;
                t.Klasse = klasse.SelectedItem as Klasse;

                con.Teilnehmer.Add(t);
                con.SaveChanges();
            }

            this.Close();
        }

        public void fillKlasse()
        {
            using(var con = new KlassenlisteEntities1())
            {
                List<Klasse> list = con.Klasse.ToList();
                klasse.ItemsSource = list;
            }
        }
    }
}
