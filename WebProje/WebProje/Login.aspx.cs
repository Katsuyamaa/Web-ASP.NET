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
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection baglanti = new SqlConnection(WebConfigurationManager.ConnectionStrings["baglanti"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["kadi"] != null)
            {
                Panel1.Visible = false;
                Panel2.Visible = false;
            }
            Panel2.Visible = false;
        }

        protected void Password_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (baglanti.State != System.Data.ConnectionState.Open)
                baglanti.Open();
            SqlCommand komut59328743245 = new SqlCommand("select * from Uye where kadi = '" + TextBox1.Text + "' and parola = '" + TextBox2.Text + "'", baglanti);
            SqlDataReader dr32948732 = komut59328743245.ExecuteReader();

            if (dr32948732.Read())
            {
                Session["kadi"] = dr32948732["kadi"].ToString();
                Session["yetki"] = dr32948732["yetki"].ToString();
                Session["uye_id"] = dr32948732["uye_id"].ToString();
            }
            dr32948732.Close();
            baglanti.Close();
            Response.Redirect("anasayfa.aspx");

            if (CheckBox1.Checked)
            {
                HttpCookie eklemecerez = new HttpCookie("beniHatirla");
                eklemecerez.Values.Add("yetki", Session["yetki"].ToString());
                eklemecerez.Values.Add("kadi", Session["kadi"].ToString());
                eklemecerez.Values.Add("uye_id", Session["uye_id"].ToString());
                eklemecerez.Expires = DateTime.Now.AddDays(20);
                Response.Cookies.Add(eklemecerez);
            }
            FormsAuthentication.SetAuthCookie(Session["kadi"].ToString(), CheckBox1.Checked);

        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            /*if (baglanti.State != System.Data.ConnectionState.Open)
                baglanti.Open();
            SqlCommand komut593287432 = new SqlCommand("insert into Uye(adsoyad,kadi,parola,mail,soru,cevap,yetki) values ('" + TextBox3.Text + "','" + TextBox4.Text + "','" + TextBox5.Text + "','" + TextBox6.Text + "','" + TextBox7.Text + "','" + TextBox8.Text + "','"+ uye "')", baglanti);
            komut593287432.ExecuteNonQuery();
            baglanti.Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Kayıt İşlemi tamamlandı'); window.location.href='anasayfa.aspx';", true);*/

            SqlCommand komut599 = new SqlCommand("INSERT INTO Uye (adsoyad, kadi, parola, mail, soru, yetki, cevap) VALUES (@adsoyad, @kadi, @parola, @mail,@soru, @yetki, @cevap)", baglanti);

            komut599.Parameters.AddWithValue("@adsoyad", TextBox3.Text);
            komut599.Parameters.AddWithValue("@kadi", TextBox4.Text);
            komut599.Parameters.AddWithValue("@parola", TextBox5.Text);
            komut599.Parameters.AddWithValue("@mail", TextBox6.Text);
            komut599.Parameters.AddWithValue("@soru", TextBox7.Text);
            komut599.Parameters.AddWithValue("@yetki", "user");
            komut599.Parameters.AddWithValue("@cevap", TextBox8.Text);

            baglanti.Open();
            komut599.ExecuteNonQuery();
            baglanti.Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Kayıt İşlemi Tamamlanmıştır.');", true);
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Panel1.Visible = false;
            Panel2.Visible = true;
        }
    }
}