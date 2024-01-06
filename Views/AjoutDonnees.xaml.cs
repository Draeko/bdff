using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace BDFF.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class AjoutDonnees : Page
    {
        DataTable TVA = null;

        public AjoutDonnees()
        {
            this.InitializeComponent();
            Database data = new Database();
            TVA = data.getTVA();
            List<string> tauxTVA = new List<string>();
            foreach(DataRow row in TVA.Rows)
            {
                tauxTVA.Add(row["taux"].ToString());
            }
            Combobox.ItemsSource = tauxTVA;
        }

        private void Validation_Click(object sender, RoutedEventArgs e)
        {
            //get TVA ID
            var id = TVA.Select("taux = " + Combobox.Text)[0][0];
        }
    }
}
