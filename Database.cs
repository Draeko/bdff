using Npgsql;
using System.Data;
using System.Diagnostics;
using System;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;

namespace BDFF
{
    public sealed partial class Database
    {


        private static NpgsqlConnection connection = null; 

        public Database()
        {
            var resource = new ResourceLoader();
            connection = new NpgsqlConnection(
                    "Server=" + resource.GetString("Server") + ";" +
                    "Port=" + resource.GetString("Port") + ";" +
                    "CommandTimeout=" + resource.GetString("CommandTimeout") + ";" +
                    "User Id=" + resource.GetString("User Id") + ";" +
                    "Password=" + resource.GetString("Password") + ";" +
                    "Database=" + resource.GetString("Database") +";");
        }


        public static void RunQuery(string query)
        {
            connection.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            NpgsqlDataReader dr = cmd.ExecuteReader();

            connection.Close();
        }

        public DataTable getTVA()
        {
            return requestData("SELECT id, taux FROM dev.tva;");
        }

        public DataTable getCategorie()
        {
            return requestData("SELECT id, nom FROM dev.article_categorie ORDER BY nom;");
        }

        public DataTable getArticle()
        {
            return requestData("SELECT id, nom, id_categorie FROM dev.article ORDER BY nom;");
        }

        private DataTable requestData(string query)
        {
            DataTable dt = SelectData(query);
            dt.PrimaryKey = new DataColumn[] { dt.Columns[0] };
            return dt;
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
                    Debug.WriteLine("Error: ---> " + ex.Message);
                }

                connection.Close();
                return _dt;
            }
        }

        public DataTable getHistoriqueAchat()
        {
            return SelectData(
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
        }

        public void setHistoriqueAchat()
        {
            //"INSERT INTO historique_achat ha VALUES();"
        }
    }
}