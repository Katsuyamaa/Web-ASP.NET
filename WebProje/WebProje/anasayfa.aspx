<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="anasayfa.aspx.cs" Inherits="WebProje.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ListView ID="ListView1" runat="server">
            <ItemTemplate>

                <div class="urun">
                    <h1><%# Eval("urun_ad") %></h1>
                    <a href="#"><figure>
                        <img src="images/<%# Eval("urun_resim") %>" alt='Yazı Başlığı' width="166" height="166"/></figure>
                    </a>
                <div class="fiyatKismi">
                <div class="fiyat">
                <span>Eski Fiyat  :</span>
             <br /><p><strike><%# Eval("urun_eski_fiyat") %>TL</strike></p>
                <h4><%# Eval("urun_fiyat") %> TL</h4>
                </div>

                <div class="detay">
          	    <span>Detay</span>
                    <a href="detay.aspx?urunID=<%# Eval("urun_id") %>" ><div id='uincele'>İncele</div></a>
                </div>
                </div>
                </div>
            </ItemTemplate>
        </asp:ListView>

    

</asp:Content>
