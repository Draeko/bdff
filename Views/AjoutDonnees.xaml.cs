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
        private Database data = null;
        private DataTable DataTVA = null;
        private DataTable DataCategorie = null;
        private DataTable DataArticle = null;

        public AjoutDonnees()
        {
            this.InitializeComponent();
            data = new Database();

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
            var id_categorie = 0;
            var categorie = DataCategorie.Select("nom = '" +CBB_Categorie.Text + "'");
            if(categorie.Length > 0)
            {
                id_categorie = Convert.ToInt32(categorie[0]["id"]);
            }
            else
            {
                id_categorie = data.setCategorie(CBB_Categorie.Text);
            }
            if (id_categorie == 0) throw new Exception("id_categorie == 0, erreur dans la récupération ou la création d'un nouvel id");


            var id_article = 0;
            var article = DataArticle.Select("nom = '" + CBB_NomArticle.Text + "'");
            if (article.Length > 0)
            {
                id_article = Convert.ToInt32(article[0]["id"]);
            }
            else
            {
                id_article = data.setArticle(CBB_NomArticle.Text, id_categorie);
            }
            if (id_article == 0) throw new Exception("id_article == 0, erreur dans la récupération ou la création d'un nouvel id");

            var id_tva = Convert.ToInt32(DataTVA.Select("taux = '" + CBB_TauxTVA.Text + "'")[0]["id"]);

            data.setHistoriqueAchat(DP_DateAchat.Date.Date, id_article, Convert.ToDecimal(TB_PrixUnitaire.Text), Convert.ToInt32(TB_Quantite.Text), id_tva);
        }
        

        private void CBB_NomArticle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Categorie = DataArticle.Select("nom = '" + CBB_NomArticle.Text + "'");
            
            if(Categorie.Length > 0)
            {
                CBB_Categorie.Text = DataCategorie.Select("id =" + Categorie[0]["id_categorie"])[0]["nom"].ToString();
            }
        }
    }
}
