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
using Entidades.EvolucionE;
using LogicaNegocio.EvolucionLN;
using Entidades.InterconsultaE;
using LogicaNegocio.InterconsultaLN;

public partial class VisorReporteHM2 : System.Web.UI.Page
{

    RceEvolucionE oRceEvolucionE = new RceEvolucionE();
    RceEvolucionLN oRceEvolucionLN = new RceEvolucionLN();
    HospitalE oHospitalE = new HospitalE();
    HospitalLN oHospitalLN = new HospitalLN();
    InterconsultaE oInterconsultaE = new InterconsultaE();
    InterconsultaLN oInterconsultaLN = new InterconsultaLN();

    ReportDocument rpt = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        
        string FechaInicio = "";
        string FechaFin = "";

        FechaInicio = Request.Params["FI"];
        FechaFin = Request.Params["FF"];


        if (Request.Params["OP"] == "EC")
        {
            DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dt11, dt12, dt13, dt14, dt15, dt16 = new DataTable();
            DataTable dtx = new DataTable();            
            
            dtx.Columns.Add("ide_evolucion");
            dtx.Columns.Add("codmedico");
            dtx.Columns.Add("nommedico");
            dtx.Columns.Add("especialidad");
            dtx.Columns.Add("servicio");
            dtx.Columns.Add("cod_atencion");
            dtx.Columns.Add("fec_registra");
            dtx.Columns.Add("hora_registro");
            dtx.Columns.Add("fecha_registro");

            oRceEvolucionE.Orden = 1;
            dt = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Load(Server.MapPath("~/Intranet/Reporte/RpEvolucionClinicaHM.rpt"));
            rpt.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rpt;            


            //'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
            CrystalDecisions.CrystalReports.Engine.TextObject txtFechaInicio;
            txtFechaInicio = rpt.ReportDefinition.Sections[1].ReportObjects["Text9"] as TextObject;
            txtFechaInicio.Text = FechaInicio;

            CrystalDecisions.CrystalReports.Engine.TextObject txtFechaFin;
            txtFechaFin = rpt.ReportDefinition.Sections[1].ReportObjects["Text12"] as TextObject;
            txtFechaFin.Text = FechaFin;


            oRceEvolucionE.Orden = 2;
            //'01
            oRceEvolucionE.CodServicio = "01";
            dt1 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_01"].SetDataSource(dt1);
            if (dt1.Rows.Count > 0)
            {
            }
            else {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion1 = rpt.ReportDefinition.Sections["ReportFooterSection1"];
                DetalleSeccion1.SectionFormat.EnableSuppress = true;
            }

            //'02
            oRceEvolucionE.CodServicio = "02";
            dt2 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_02"].SetDataSource(dt2);
            if (dt2.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion2 = rpt.ReportDefinition.Sections["ReportFooterSection2"];
                DetalleSeccion2.SectionFormat.EnableSuppress = true;
            }

            
            //'03
            oRceEvolucionE.CodServicio = "03";
            dt3 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_03"].SetDataSource(dt3);
            if (dt3.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion3 = rpt.ReportDefinition.Sections["ReportFooterSection3"];
                DetalleSeccion3.SectionFormat.EnableSuppress = true;
            }

            

            //'04
            oRceEvolucionE.CodServicio = "04";
            dt4 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_04"].SetDataSource(dt4);
            if (dt4.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion4 = rpt.ReportDefinition.Sections["ReportFooterSection4"];
                DetalleSeccion4.SectionFormat.EnableSuppress = true;
            }

            

            //'05
            oRceEvolucionE.CodServicio = "05";
            dt5 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_05"].SetDataSource(dt5);
            if (dt5.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion5 = rpt.ReportDefinition.Sections["ReportFooterSection5"];
                DetalleSeccion5.SectionFormat.EnableSuppress = true;
            }

            
            //'06
            oRceEvolucionE.CodServicio = "06";
            dt6 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_06"].SetDataSource(dt6);
            if (dt6.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion6 = rpt.ReportDefinition.Sections["ReportFooterSection6"];
                DetalleSeccion6.SectionFormat.EnableSuppress = true;
            }
            

            //'07
            oRceEvolucionE.CodServicio = "07";
            dt7 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_07"].SetDataSource(dt7);
            if (dt7.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion7 = rpt.ReportDefinition.Sections["ReportFooterSection7"];
                DetalleSeccion7.SectionFormat.EnableSuppress = true;
            }
           

            //'08
            oRceEvolucionE.CodServicio = "08";
            dt8 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_08"].SetDataSource(dt8);
            if (dt8.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion8 = rpt.ReportDefinition.Sections["ReportFooterSection8"];
                DetalleSeccion8.SectionFormat.EnableSuppress = true;
            }

            

            //'09
            oRceEvolucionE.CodServicio = "09";
            dt9 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_09"].SetDataSource(dt9);
            if (dt9.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion9 = rpt.ReportDefinition.Sections["ReportFooterSection9"];
                DetalleSeccion9.SectionFormat.EnableSuppress = true;
            }
            

            //'10
            oRceEvolucionE.CodServicio = "10";
            dt10 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_10"].SetDataSource(dt10);
            if (dt10.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion10 = rpt.ReportDefinition.Sections["ReportFooterSection10"];
                DetalleSeccion10.SectionFormat.EnableSuppress = true;
            }
            

            //'11
            oRceEvolucionE.CodServicio = "11";
            dt11 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_11"].SetDataSource(dt11);
            if (dt11.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion11 = rpt.ReportDefinition.Sections["ReportFooterSection11"];
                DetalleSeccion11.SectionFormat.EnableSuppress = true;
            }
            

            //'12
            oRceEvolucionE.CodServicio = "12";
            dt12 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_12"].SetDataSource(dt12);
            if (dt12.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion12 = rpt.ReportDefinition.Sections["ReportFooterSection12"];
                DetalleSeccion12.SectionFormat.EnableSuppress = true;
            }
            

            //'13
            oRceEvolucionE.CodServicio = "13";
            dt13 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_13"].SetDataSource(dt13);
            if (dt13.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion13 = rpt.ReportDefinition.Sections["ReportFooterSection13"];
                DetalleSeccion13.SectionFormat.EnableSuppress = true;
            }

            
            //'14
            oRceEvolucionE.CodServicio = "14";
            dt14 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_14"].SetDataSource(dt14);
            if (dt14.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion14 = rpt.ReportDefinition.Sections["ReportFooterSection14"];
                DetalleSeccion14.SectionFormat.EnableSuppress = true;
            }
                        

            //'15
            oRceEvolucionE.CodServicio = "15";
            dt15 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_15"].SetDataSource(dt15);
            if (dt15.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion15 = rpt.ReportDefinition.Sections["ReportFooterSection15"];
                DetalleSeccion15.SectionFormat.EnableSuppress = true;
            }
            

            //'16
            oRceEvolucionE.CodServicio = "AC";
            dt16 = oRceEvolucionLN.Rp_EvolucionClinicaHM(oRceEvolucionE);
            rpt.Subreports["RpEvolucionClinicaHMSub.rpt_16"].SetDataSource(dt16);
            if (dt16.Rows.Count > 0)
            {
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.Section DetalleSeccion16 = rpt.ReportDefinition.Sections["ReportFooterSection16"];
                DetalleSeccion16.SectionFormat.EnableSuppress = true;
            }
        }
        if (Request.Params["OP"] == "PR")
        {
            oHospitalE.FecInicio = (Convert.ToDateTime(FechaInicio)).ToString("MM/dd/yyyy");
            oHospitalE.FecFin = (Convert.ToDateTime(FechaFin)).ToString("MM/dd/yyyy");
            oHospitalE.Orden = 1;                
            DataTable dt, dt1 = new DataTable();
            dt = oHospitalLN.Rp_ProcedimientosHM(oHospitalE);

            rpt.Load(Server.MapPath("~/Intranet/Reporte/RpProcedimientosHM.rpt"));
            rpt.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rpt;


            //'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
            CrystalDecisions.CrystalReports.Engine.TextObject txtFechaInicio;
            txtFechaInicio = rpt.ReportDefinition.Sections[1].ReportObjects["Text8"] as TextObject;
            txtFechaInicio.Text = FechaInicio;

            CrystalDecisions.CrystalReports.Engine.TextObject txtFechaFin;
            txtFechaFin = rpt.ReportDefinition.Sections[1].ReportObjects["Text11"] as TextObject;
            txtFechaFin.Text = FechaFin;                


            oHospitalE.Orden = 2;
            dt1 = oHospitalLN.Rp_ProcedimientosHM(oHospitalE);
            rpt.Subreports["RpProcedimientosHMSub.rpt"].SetDataSource(dt1);
        }

        if (Request.Params["OP"] == "IN")
        {
            //'CONSULTANDO DATOS DEL REPORTE PRINCIPAL
            oInterconsultaE.FecInicio = (Convert.ToDateTime(FechaInicio)).ToString("MM/dd/yyyy");
            oInterconsultaE.FecFin = (Convert.ToDateTime(FechaFin)).ToString("MM/dd/yyyy");
            oInterconsultaE.Orden = 1;
            DataTable dt, dt1 = new DataTable();
            dt = oInterconsultaLN.Rp_InterconsultaHM(oInterconsultaE);

            rpt.Load(Server.MapPath("~/Intranet/Reporte/RpInterconsultaHM.rpt"));
            rpt.SetDataSource(dt);

            //'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
            CrystalDecisions.CrystalReports.Engine.TextObject txtFechaInicio;
            txtFechaInicio = rpt.ReportDefinition.Sections[1].ReportObjects["Text2"] as TextObject;
            txtFechaInicio.Text = FechaInicio;

            CrystalDecisions.CrystalReports.Engine.TextObject txtFechaFin;
            txtFechaFin = rpt.ReportDefinition.Sections[1].ReportObjects["Text5"] as TextObject;
            txtFechaFin.Text = FechaFin;  

            
            //'CONSULTANDO DATOS DEL SUBREPORTE (RESUMEN)
            oInterconsultaE.Orden = 2;
            dt1 = oInterconsultaLN.Rp_InterconsultaHM(oInterconsultaE);
            rpt.Subreports["RpInterconsultaHMSub.rpt"].SetDataSource(dt1);
            CrystalReportViewer1.ReportSource = rpt;
        }
        if (Request.Params["OP"] == "GC")
        {
            //CONSULTANDO DATOS DEL REPORTE PRINCIPAL
            oHospitalE.FecInicio = (Convert.ToDateTime(FechaInicio)).ToString("MM/dd/yyyy"); //Format(CDate(FechaInicio), "MM/dd/yyyy");
            oHospitalE.FecFin = (Convert.ToDateTime(FechaFin)).ToString("MM/dd/yyyy");
            oHospitalE.Orden = 1;
            DataTable dt = new DataTable();
            dt = oHospitalLN.Rp_GestionClinica(oHospitalE);



            rpt.Load(Server.MapPath("~/Intranet/Reporte/RpGestionClinica.rpt"));
            rpt.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rpt;


            //'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
            CrystalDecisions.CrystalReports.Engine.TextObject txtFechaInicio;
            txtFechaInicio = rpt.ReportDefinition.Sections[1].ReportObjects["Text7"] as TextObject;
            txtFechaInicio.Text = FechaInicio;

            CrystalDecisions.CrystalReports.Engine.TextObject txtFechaFin;
            txtFechaFin = rpt.ReportDefinition.Sections[1].ReportObjects["Text5"] as TextObject;
            txtFechaFin.Text = FechaFin;
        }
        if (Request.Params["OP"] == "LH")
        {

            oHospitalE.FecInicio = (Convert.ToDateTime(FechaInicio)).ToString("MM/dd/yyyy");
            oHospitalE.FecFin = (Convert.ToDateTime(FechaFin)).ToString("MM/dd/yyyy");
            oHospitalE.Orden = 0;
            DataTable dt, dt1 = new DataTable();
            dt = oHospitalLN.Rp_LibrodeHospitalizacion(oHospitalE);

            rpt.Load(Server.MapPath("~/Intranet/Reporte/RpLibroHospitalizacion.rpt"));
            rpt.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rpt;


            //'COLOCANDO EL RANGO DE FECHAS EN LA PARTE SUPERIOR
            CrystalDecisions.CrystalReports.Engine.TextObject txtFechaInicio;
            txtFechaInicio = rpt.ReportDefinition.Sections[1].ReportObjects["Text8"] as TextObject;
            txtFechaInicio.Text = FechaInicio;

            CrystalDecisions.CrystalReports.Engine.TextObject txtFechaFin;
            txtFechaFin = rpt.ReportDefinition.Sections[1].ReportObjects["Text11"] as TextObject;
            txtFechaFin.Text = FechaFin;

        }
    }


    protected void Page_Unload(object sender, EventArgs e)
    {
        rpt.Close();
        rpt.Dispose();
    }

    //[System.Web.Services.WebMethod]   
    //public static void CerrarReporteHM()
    //{
    //    rpt.Close();
    //    rpt.Dispose();
    //}
}