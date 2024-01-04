using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using Npgsql;
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.ObjectModel;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BDFF
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        static readonly NpgsqlConnection connection = new NpgsqlConnection("Server=localhost;Port=5432;CommandTimeout=5000;User Id=postgres;" +
                                                     "Password=pcKC5sty;Database=postgres;");

        public static void RunQuery(string query)
        {
            connection.Open();

            // Define a query
            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);

            // Execute a query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            connection.Close();
        }

        public static DataTable SelectData(string query)
        {
            connection.Open();
            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Prepare();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);

                DataSet _ds = new DataSet();
                DataTable _dt = new DataTable();

                da.Fill(_ds);

                try
                {
                    _dt = _ds.Tables[0];
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Erro: ---> " + ex.Message);
                }

                connection.Close();
                return _dt;
            }

        }


        public MainPage()
        {            
                       

            this.InitializeComponent();

            var table = SelectData(
                "select "
                + "a.nom as \"Nom\", "
                + "ac.nom as \"Categorie\", "
                + "ha.prix_unitaire as \"Prix Unitaire\", "
                + "ha.quantite as \"Quantité\", "
                + "t.taux as \"Taux TVA\", "
                + "ha.date as \"Date\" "
                + "from dev.historique_achat ha, dev.article a, dev.article_categorie ac, dev.tva t "
                + "where ha.id_article = a.id and "
                + "ha.id_tva = t.id"
            );

            for (int i = 0; i < table.Columns.Count; i++)
            {
                HistoriqueAchat.Columns.Add(new DataGridTextColumn()
                {
                    Header = table.Columns[i].ColumnName,
                    Binding = new Binding { Path = new PropertyPath("[" + i.ToString() + "]") }
                });
            }

            var collection = new ObservableCollection<object>();
            foreach (DataRow row in table.Rows)
            {
                collection.Add(row.ItemArray);
            }

            HistoriqueAchat.ItemsSource = collection;
        }
    }
}
