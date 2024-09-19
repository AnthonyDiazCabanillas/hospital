using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using LogicaNegocio.HospitalLN;
using Entidades.HospitalE;

public partial class CS : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        //ReportDocument crystalReport = new ReportDocument();
        //crystalReport.Load(Server.MapPath("~/CustomerReport.rpt"));
        ////Customers dsCustomers = GetData("select * from customers");
        //DataTable dt = new DataTable();
        //dt.Columns.Add("");
        //dt.Columns.Add("");
        //dt.Columns.Add("");
        //dt.Columns.Add("");


        //crystalReport.SetDataSource(dt);
        //CrystalReportViewer1.ReportSource = crystalReport;

        string FechaInicio = "";
        string FechaFin = "";

            FechaInicio = Request.Params["FI"];
            FechaFin = Request.Params["FF"];

        HospitalE oHospitalE = new HospitalE();
        HospitalLN oHospitalLN = new HospitalLN();


        //CONSULTANDO DATOS DEL REPORTE PRINCIPAL
        oHospitalE.FecInicio = (Convert.ToDateTime(FechaInicio)).ToString("MM/dd/yyyy"); //Format(CDate(FechaInicio), "MM/dd/yyyy");
        oHospitalE.FecFin = (Convert.ToDateTime(FechaFin)).ToString("MM/dd/yyyy");
        oHospitalE.Orden = 1;
        DataTable dt = new DataTable();
        dt = oHospitalLN.Rp_GestionClinica(oHospitalE);
        ReportDocument rpt = new ReportDocument();


        rpt.Load(Server.MapPath("~/Intranet/Reporte/RpGestionClinica.rpt"));
        rpt.SetDataSource(dt);
        CrystalReportViewer1.ReportSource = rpt;


        //'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
        CrystalDecisions.CrystalReports.Engine.TextObject txtFechaInicio;
        txtFechaInicio = rpt.ReportDefinition.Sections[1].ReportObjects["Text7"] as TextObject;
        txtFechaInicio.Text = FechaInicio;

        CrystalDecisions.CrystalReports.Engine.TextObject txtFechaFin;
        txtFechaFin = rpt.ReportDefinition.Sections[1].ReportObjects["Text5"] as TextObject;
        txtFechaFin.Text = FechaInicio;



        //Dim txtFechaInicio As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text7")
        //txtFechaInicio.Text = FechaInicio

        //Dim txtFechaFin As CrystalDecisions.CrystalReports.Engine.TextObject = rpt.ReportDefinition.Sections(1).ReportObjects("Text5")
        //        txtFechaFin.Text = FechaFin
    }

    //private Customers GetData(string query)
    //{
    //    string conString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    //    SqlCommand cmd = new SqlCommand(query);
    //    using (SqlConnection con = new SqlConnection(conString))
    //    {
    //        using (SqlDataAdapter sda = new SqlDataAdapter())
    //        {
    //            cmd.Connection = con;

    //            sda.SelectCommand = cmd;
    //            using (Customers dsCustomers = new Customers())
    //            {
    //                sda.Fill(dsCustomers, "DataTable1");
    //                return dsCustomers;
    //            }
    //        }
    //    }
    //}
}