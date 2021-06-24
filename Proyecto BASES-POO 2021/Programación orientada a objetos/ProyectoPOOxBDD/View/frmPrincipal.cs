using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoPOOxBDD.VaccinationContext;

namespace ProyectoPOOxBDD
{
    public partial class frmPrincipal : Form
    {
        private Manager manager;
        public frmPrincipal(Manager manager)
        {
            InitializeComponent();
            this.manager = manager;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            //Quitar titulos del tab control
            tabPrincipal.ItemSize = new Size(0, 1);
        }
    }
}
