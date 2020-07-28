using Franquicia.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PagaLaEscuela.Views
{
    public partial class Configuracion : System.Web.UI.Page
    {
        ProcesosEnsenianzasServices procesosEnsenianzasServices = new ProcesosEnsenianzasServices();
        TiposEnsenianzasServices tiposEnsenianzasServices = new TiposEnsenianzasServices();
        NivelesEnsenianzasServices nivelesEnsenianzasServices = new NivelesEnsenianzasServices();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                procesosEnsenianzasServices.CargarProcesosEnsenianzas();
                ddlProceso.DataSource = procesosEnsenianzasServices.lsProcesosEnsenianzas;
                ddlProceso.DataTextField = "VchDescripcion";
                ddlProceso.DataValueField = "UidProcesoEnsenianza";
                ddlProceso.DataBind();

                ddlProceso_SelectedIndexChanged(null, null);
                ddlTipo_SelectedIndexChanged(null, null);

                gvNivelEnsenianza.DataSource = null;
                gvNivelEnsenianza.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Mult", String.Format(@"multi();"), true);
            }
        }

        protected void ddlProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlNivel.Visible = true;
            pnlNombreCarrera.Visible = false;
            pnlPeriodo.Visible = false;

            if (ddlProceso.SelectedItem.Text == "UNIVERSITARIO")
            {
                lblTitleNombreCarrera.Text = "Nombre de carrera";
                lblTitlePeriodo.Text = "Periodo";

                txtNombreCarrera.Attributes.Add("placeholder", "LIC. EN TECNOLOGÍAS DE LA INFORMACIÓN");
                txtPeriodo.Attributes.Add("placeholder", "PRIMER CUATRIMESTRE");

                pnlNivel.Visible = false;
                pnlNombreCarrera.Visible = true;
                pnlPeriodo.Visible = true;
            }
            else if (ddlProceso.SelectedItem.Text == "OTRO")
            {
                lblTitleNombreCarrera.Text = "Curso";
                lblTitlePeriodo.Text = "Nivel";

                txtNombreCarrera.Attributes.Add("placeholder", "COMPUTO 1");
                txtPeriodo.Attributes.Add("placeholder", "BASICO");

                pnlNivel.Visible = false;
                pnlNombreCarrera.Visible = true;
                pnlPeriodo.Visible = true;
            }

            tiposEnsenianzasServices.CargarTiposEnsenianzas(Guid.Parse(ddlProceso.SelectedValue));
            ddlTipo.DataSource = tiposEnsenianzasServices.lsTiposEnsenianzas;
            ddlTipo.DataTextField = "VchDescripcion";
            ddlTipo.DataValueField = "UidTipoEnsenianza";
            ddlTipo.DataBind();

            ddlTipo_SelectedIndexChanged(null, null);
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            nivelesEnsenianzasServices.CargarNivelesEnsenianzas(Guid.Parse(ddlTipo.SelectedValue));
            ListBoxNivel.DataSource = nivelesEnsenianzasServices.lsNivelesEnsenianzas;
            ListBoxNivel.DataTextField = "VchDescripcion";
            ListBoxNivel.DataValueField = "UidNivelEnsenianza";
            ListBoxNivel.DataBind();
        }
    }
}