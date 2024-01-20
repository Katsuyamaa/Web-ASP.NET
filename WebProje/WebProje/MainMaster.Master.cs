using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebProje
{

    public partial class MainMaster : System.Web.UI.MasterPage
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            Panel2.Visible= false;
            HttpCookie beniHatirla = Request.Cookies["beniHatirla"];

            if (beniHatirla != null)
            {
                Session["yetki"] = beniHatirla.Values["yetki"];
                Session["uye_id"] = beniHatirla.Values["uye_id"];
                Session["kadi"] = beniHatirla.Values["kadi"];
            }

            if (Session["kadi"] != null)
            {
                string deger = "";
                if (Session["yetki"].ToString() == "admin")
                {
                    deger = "Yönetici";
                    Panel2.Visible = true;
                }
                else if (Session["yetki"].ToString() == "musteri")
                {
                    deger = "Müşteri";

                    // Kullanıcı girişi yapıldıktan sonra sepet adetlerini topla ve Label2.Text'e yaz
                    string uye_id = Session["uye_id"].ToString();
                    int toplamAdet = 0;

                    // TODO: Sepet veritabanından ilgili üye_id'ye ait adet sütununu al
                    // Bu veriyi veritabanınıza uygun bir sorgu kullanarak almanız gerekiyor
                    using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["baglanti"].ConnectionString))
                    {
                        connection.Open();

                        string query = "SELECT ISNULL(SUM(CAST(adet AS int)), 0) AS ToplamAdet FROM Sepet WHERE uye_id = @uye_id";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@uye_id", uye_id);

                            object result = command.ExecuteScalar();

                            if (result != null)
                            {
                                toplamAdet = Convert.ToInt32(result);
                            }
                        }
                    }

                    // Eğer adet değeri null ise, Label2.Text'e 0 yaz
                    if (toplamAdet == null)
                    {

                        Label2.Text = "0";
                    }
                    else
                    {

                        // Eğer adet değeri null değilse, Label2.Text'e toplamAdet'i yaz
                        Label2.Text = toplamAdet.ToString();
                    }
                }

                Label1.Text = "Hoşgeldin " + Session["kadi"].ToString() + "\n" + deger;
                iletisim.Text = "Çıkış Yap";
            }
            else
            {
                Session["yetki"] = "";
                Label1.Text = "Hoşgeldin Misafir";
                iletisim.Text = "Giriş Yap";
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
          

        }

        protected void benihatirla_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void iletisim_Click(object sender, EventArgs e)
        {
            if (Session["kadi"] != null || Session["yetki"] != null || Session["uye_id"] != null)
            {
                Session["kadi"] = null;
                Session["yetki"] = null;
                Session["uye_id"] = null;
                Label1.Text = "Hoşgeldin Misafir";
                iletisim.Text = "Giriş Yap";
                Response.Redirect("Login.aspx");

            }
            else
            {

            }
        }

        protected void tümü_Click(object sender, EventArgs e)
        {
            if (Session["yetki"].ToString() != "")
            {
                Response.Redirect("sepet.aspx");

            }
        }
    }
}