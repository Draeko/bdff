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
        private DataTable DataTVA = null;
        private DataTable DataCategorie = null;
        private DataTable DataArticle = null;

        public AjoutDonnees()
        {
            this.InitializeComponent();
            Database data = new Database();

            //Récupération des valeurs de TVA
            DataTVA = data.getTVA();
            CBB_TauxTVA.ItemsSource = setComponent(DataTVA, "taux");

            //Récupérations des valeurs de Catégorie
            DataCategorie = data.getCategorie();
            CBB_Categorie.ItemsSource = setComponent(DataCategorie, "nom");

            //Récupération des valeurs de Article
            DataArticle = data.getArticle();
            CBB_NomArticle.ItemsSource = setComponent(DataArticle, "nom");

        }

        private List<string> setComponent(DataTable dataIn, string nomIndice)
        {
            List<string> listString = new List<string>();
            foreach(DataRow row in dataIn.Rows)
            {
                listString.Add(row[nomIndice].ToString());
            }
            return listString;
        }

        private void Validation_Click(object sender, RoutedEventArgs e)
        {
            //get TVA ID
            var id = DataTVA.Select("taux = " + CBB_TauxTVA.Text)[0][0];

            //data pour insert
            /*
             * date
             * id_article
             * prix_unitaire
             * quantité
             * id_tva
             * 
             */
        }

        private void CBB_NomArticle_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            var texte = DataCategorie.Select("nom = " + CBB_NomArticle.Text)[0][0];
            if (texte != null)
            {
                CBB_Categorie.Text = texte.ToString();
            }
        }
    }
}
