using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebProje
{
    public partial class Sepet : System.Web.UI.Page
    {
        SqlConnection baglan = new SqlConnection(WebConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            string kullanici_id = Session["uye_id"].ToString();
            baglan.Open();
            SqlCommand oku = new SqlCommand("select Urun.urun_ad,Urun.urun_fiyat,Urun.urun_resim,Urun.urun_aciklama,Sepet.adet from Sepet Inner Join Urun on Sepet.urun_id=Urun.urun_id where uye_id=@kullanici", baglan);
            oku.Parameters.AddWithValue("@kullanici", kullanici_id);
            SqlDataReader dr23432 = oku.ExecuteReader();




            double tutar = 0.0;
            double toplamTutar = 0.0;
            while (dr23432.Read())
            {

                int adet = Convert.ToInt32(dr23432["adet"]);
                double fiyat = Convert.ToDouble(dr23432["urun_fiyat"]);
                tutar = adet * fiyat;
                toplamTutar += tutar;

            }
            dr23432.Close();
            SqlDataAdapter adap32432432 = new SqlDataAdapter(oku);
            DataTable dt32432 = new DataTable();
            adap32432432.Fill(dt32432);
            sptlbl.Text= toplamTutar.ToString();
            ListView1.DataSource = dt32432;
            ListView1.DataSourceID = "";
            ListView1.DataBind();
            baglan.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (baglan.State != ConnectionState.Open)
                baglan.Open();

            SqlCommand komut32849327432 = new SqlCommand("insert into siparis values ('" + Session["uye_id"].ToString() + "','" + sptlbl.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')", baglan);
            komut32849327432.ExecuteNonQuery();
            SqlCommand komut328493274323 = new SqlCommand("delete from sepet where uye_id = '" + Session["uye_id"].ToString() + "'", baglan);
            komut328493274323.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('sipariş tamamlandı'); window.location.href='Sepet.aspx';", true);
            baglan.Close();

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (baglan.State != ConnectionState.Open)
                baglan.Open();

            SqlCommand komut32849327432 = new SqlCommand("delete from sepet where uye_id = '" + Session["uye_id"].ToString() + "'", baglan);
            komut32849327432.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Sepet Temizlendi'); window.location.href='Sepet.aspx';", true);
            baglan.Close();
            Response.Redirect("sepet.aspx");
        }
    }
}