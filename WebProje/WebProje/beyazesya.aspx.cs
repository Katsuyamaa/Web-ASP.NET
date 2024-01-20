using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebProje
{
    public partial class beyazesya : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["baglanti"].ConnectionString;

            using (SqlConnection baglan = new SqlConnection(connectionString))
            {
                baglan.Open();

                using (SqlCommand oku = new SqlCommand("SELECT * FROM Urun WHERE urun_kat_id='4'", baglan))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(oku))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        DataView dataView = new DataView(dataTable);

                        ListView1.DataSource = dataView;
                        ListView1.DataBind();
                    }
                }
            }
        }
    }
}