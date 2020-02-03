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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Globalization;

namespace Klassenliste
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // fills the treeView
            fillTreeView();
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            // opens the new window
            AddTeilnehmer ad = new AddTeilnehmer();
            ad.Show();
            fillTreeView();
        }

        private void Button_delete(object sender, RoutedEventArgs e)
        {
            // connect to the db
            using (var con = new DBKlassenliste())
            {
                // if something is selected in the TreeView
                if(treeView.SelectedItem != null)
                {
                    // if the selected item isn't null
                    Teilnehmer t = treeView.SelectedItem as Teilnehmer;
                    if (t != null)
                    {
                        // delete the Teilnehmer
                        con.Teilnehmer.Remove(t);
                        con.SaveChanges();

                        fillTreeView();
                    }
                }
            }
        }

        private void Button_import(object sender, RoutedEventArgs e)
        {
            // gets all the seperatly parts of the CSV
            String[][] results = readCSV(getFile());
            
            Klasse k = null;
            Teilnehmer t = null;

            // if i can read the CSV
            if (results != null) {
                // connect to the db
                using (var con = new DBKlassenliste())
                {
                    // get all the Klassen
                    List<Klasse> list = con.Klasse.ToList();

                    // for each line, save the entries
                    for (int i = 0; i < results.GetLength(0); i++)
                    {
                        // make a new Klasse
                        k = new Klasse();
                        k.Bezeichnung = results[i][4];
                        k.Kuerzel = results[i][5];
                        k.Jahr = Int32.Parse(results[i][6]);

                        // check if its already in the db
                        if(!list.Contains(k))
                        {
                            // if its not in the db, insert it
                            con.Klasse.Add(k);
                        }

                        // make a new Teilnehmer
                        t = new Teilnehmer();
                        t.Name = results[i][0];
                        t.Vorname = results[i][1];      
                        t.Email = results[i][2];
                        t.Geburtsdatum = DateTime.ParseExact(results[i][3], "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        t.Klasse = k;

                        // save the Teilnehmer
                        con.Teilnehmer.Add(t);

                        // insert everything needed in the db
                        con.SaveChanges();
                    }
                }
            }

            fillTreeView();
        }

        // gets the file
        private String getFile()
        {
            // create the windows explorer window
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "(*.csv)";
            ofd.Title = "Select a file";

            // open it and check if the "open" button was clicked
            if (ofd.ShowDialog() == true)
            {
                return ofd.FileName;
            }

            // if the button isn't pressed, just return nothing
            return "";
        }

        // reads the csv file
        private string[][] readCSV(string file)
        {
            // if something has been returned
            if (!file.Equals(""))
            {
                // for each line of the csv file
                String[] lines = File.ReadAllLines(file);
                // for each part in each line [line][part]
                String[][] parts = new string[lines.Length -1][];

                // as long as there are more lines
                for (int i = 1; i < lines.Length; i++)
                {
                    // get the seperated parts
                    parts[i - 1] = lines[i].Split(';');
                }

                // return all the parts
                return parts;

            }
            else
            {
                // if it has failed, return nothing
                getFile();
                return null;
            }
        }

        // to fill the TreeView
        private void fillTreeView()
        {
            // delete evry entry in the treeview
            treeView.Items.Clear();

            List<Klasse> listKlasse = null;
            List<Teilnehmer> listTeilnehmer = null;

            // connect to the db
            using (var con = new DBKlassenliste())
            {
                // get a list of the Klassen and Teilnehmer
                listKlasse = con.Klasse.ToList();
                listTeilnehmer = con.Teilnehmer.ToList();
            }

            // a list for the "parents" / header
            List<TreeViewItem> parentKlasse = new List<TreeViewItem>();

            // for each class
            for(int i = 0; i < listKlasse.Count();i++)
            {
                /* TreeViewItem tr = new TreeViewItem();
                  tr.Header = listKlasse.ElementAt(i).Kuerzel;
                  parentKlasse.Add(tr);*/

                // add the Kuerzel as the header / parent
                parentKlasse.Add(new TreeViewItem());
                parentKlasse.ElementAt(i).Header = listKlasse.ElementAt(i).Kuerzel;
                treeView.Items.Add(parentKlasse.ElementAt(i));
            }

            // for every Klasse
            for(int i = 0; i < parentKlasse.Count(); i++)
            {
                // for every Teilnehmer
               for(int x = 0; x < listTeilnehmer.Count(); x++)
                {
                    // if the Kuerzel of the Klasse from the Teilnehmer is the same as the header / parent 
                    if(listTeilnehmer.ElementAt(x).Klasse.Kuerzel.Equals(parentKlasse.ElementAt(i).Header))
                    {
                        // insert the Teilnehmer as a child
                        TreeViewItem tr = new TreeViewItem();
                        tr.Header = listTeilnehmer.ElementAt(x).Name + " " + listTeilnehmer.ElementAt(x).Vorname;
                        parentKlasse.ElementAt(i).Items.Add(tr);
                    }
                }
            }
        }
    }
}
