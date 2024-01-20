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
    public partial class detay : System.Web.UI.Page
    {
        SqlConnection baglan = new SqlConnection(WebConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);


        static int mevcutStok = 0;
        static int sepetAdet = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //ürün id'si detay.aspx sayfasının URL adresindeki Urun id
            //değerine sahip olan urunü urun tablosundan oku ve listview kontrolüne aktar
            baglan.Open();
            SqlCommand oku = new SqlCommand("select * from urun where urun_id=@id", baglan);
            oku.Parameters.AddWithValue("@id", Request.QueryString["urunID"]);
            SqlDataReader dr = oku.ExecuteReader();
            ListView1.DataSource = dr;
            ListView1.DataBind();
            baglan.Close();
        }




        /*seçilen ürün ve seçilen adeti parametre olarak alıp, ürün stoğu ile karşılaştıran stok yeterli ise True, değil ise False değerini döndüren StokYeterliMi metodunu tanımlayalım
         */

        Boolean StokyeterliMi(string urun, int adet)
        {
            SqlCommand oku = new SqlCommand("select * from urun where urun_id=@id", baglan);
            oku.Parameters.AddWithValue("@id", urun);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(oku);
            da.Fill(dt);
            mevcutStok = Convert.ToInt32(dt.Rows[0][9]);
            if (adet < mevcutStok)
                return true;
            else
                return false;
        }

        /*
         oturum açmış kullanıcının id'sini ve seçilen ürünü parmetre olarak alıp, seept tablosunda kayıt var ise true, yoksa false değerini döndüren SepetKontrol metodu tanımlayım

         */
        Boolean SepetKontrol(string urun, string kullanici)
        {
            SqlCommand oku = new SqlCommand("select * from Sepet where urun_id=@u_id and uye_id=@k_id", baglan);
            oku.Parameters.AddWithValue("@u_id", urun);
            oku.Parameters.AddWithValue("@k_id", kullanici);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(oku);
            da.Fill(dt);
            //geriye değer döndü ise
            if (dt.Rows.Count > 0)
            {
                sepetAdet = Convert.ToInt32(dt.Rows[0][3]);
                return true;
            }
            else
                return false;

        }
        /*seçilen ürün, oturum açmış kullanıcının sepetinde varsa, sepetteki adeti seçilen adet kadar arttıran SepetAdetArttır metodunu tanımlayalım */
        void SepetAdetArttir(string urun, int secilenadet)
        {
            SqlCommand updateCount = new SqlCommand("update Sepet Set adet=@adet where urun_id=@id ", baglan);
            updateCount.Parameters.AddWithValue("@adet", mevcutStok + secilenadet);
            updateCount.Parameters.AddWithValue("@id", urun);
            updateCount.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('sepete eklendi'); window.location.href='Sepet.aspx';", true);

        }
        /*secilen urun ve secilen adet değerlerini alıp, urun tablosundaki stok alanını güncelleyen StokGüncelle metodunu tanımlayalım*/
        void StokGuncelle(string urun, int secilenadet)
        {
            SqlCommand guncelle = new SqlCommand("update Urun set stok=@stok where urun_id=@urun", baglan);
            guncelle.Parameters.AddWithValue("@stok", mevcutStok - secilenadet);
            guncelle.Parameters.AddWithValue("@urun", urun);
            guncelle.ExecuteNonQuery();

        }

        /*secilen urun, oturum açmış kullanıcının id'si, secilen adet ve fiyat değerlerini parametre olarak alan ve sepet tablosuna kayıt ekleyen metodu tanımlayalım*/
        /*void Yeni_Kayit_Ekle(string urun, string kullanici, int adet)
        {
            SqlCommand ekle = new SqlCommand("insert into Sepet Values(@urun,@adet,@kullanici)", baglan);
            ekle.Parameters.AddWithValue("@adet", adet);
            ekle.Parameters.AddWithValue("@kullanici", kullanici);
            ekle.Parameters.AddWithValue("@urun", urun);
            ekle.ExecuteNonQuery();
            Response.Write("<script>alert('Sepete Eklendi')</script>");
            Response.Redirect("anasayfa.aspx");

        }*/

        void Yeni_Kayit_Ekle(string urun, string kullanici, int adet)
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["baglanti"].ConnectionString))
            {
                connection.Open();

                // Aynı değerin zaten tabloda olup olmadığını kontrol et
                SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Sepet WHERE urun_id = @urun", connection);
                checkCommand.Parameters.AddWithValue("@urun", urun);
                int existingCount = (int)checkCommand.ExecuteScalar();

                if (existingCount == 0)
                {
                    // Aynı değer tabloda yoksa ekle
                    SqlCommand ekle = new SqlCommand("INSERT INTO Sepet (urun_id, adet, uye_id) VALUES (@urun, @adet, @kullanici)", connection);
                    ekle.Parameters.AddWithValue("@urun", urun);
                    ekle.Parameters.AddWithValue("@adet", adet);
                    ekle.Parameters.AddWithValue("@kullanici", kullanici);
                    ekle.ExecuteNonQuery();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('sepete eklendi'); window.location.href='Sepet.aspx';", true);
                    Response.Redirect("anasayfa.aspx");
                }
                else
                {
                    // Aynı değer tabloda varsa hata mesajı ver
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('bu ürün zaten sepetinizde bulunmaktadır'); window.location.href='Sepet.aspx';", true);
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {//sepete ekle butonuna tıklandığında 

            
            string secilen_urun = Request.QueryString["urunID"].ToString();
            int secilenadet = 0;
            Button btn = (Button)sender; //button1 kontrolüne eriştik
            DropDownList adetliste = (DropDownList)btn.Parent.FindControl("DropDownList1");

            //adet seçilmişmi
            if (adetliste.SelectedIndex > 0)
            {
                secilenadet = Convert.ToInt32(adetliste.SelectedValue);
                //üye girişi yapılmış mı ?
                if (Session["uye_id"] != null)
                {
                    string kullanici = Session["uye_id"].ToString();
                    //stok yeterli mi
                    baglan.Open();
                    if (StokyeterliMi(secilen_urun, secilenadet) == true)
                    {
                        if (SepetKontrol(secilen_urun, kullanici) == true)
                        {
                            SepetAdetArttir(secilen_urun, secilenadet);
                            StokGuncelle(secilen_urun, secilenadet);

                        }
                        else
                        {
                            Yeni_Kayit_Ekle(secilen_urun, kullanici, secilenadet);
                            StokGuncelle(secilen_urun, secilenadet);

                        }


                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Stok Yetersiz'); window.location.href='Sepet.aspx';", true);



                }
                else
                    Response.Redirect("Login.aspx?returnURL=" + Request.RawUrl);

            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Lütfen ürün adeti seçin!');", true);
        }

        protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ListView1_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }
    }
}