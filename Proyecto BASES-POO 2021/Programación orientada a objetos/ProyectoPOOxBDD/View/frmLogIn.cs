using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using ProyectoPOOxBDD.VaccinationContext;

namespace ProyectoPOOxBDD
{
    public partial class frmLogIn : Form
    {
        public frmLogIn()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                //Obteniendo todos los gestores
                var db = new VaccinationDBContext();

                List<Manager> managers = db.Managers
                                        .OrderBy(m => m.Id)
                                        .ToList();

                //Filtrar datos
                string userName = txtUser.Text;

                string userPass = txtPassword.Text;

                List<Manager> result = managers
                                        .Where(m => m.Username == userName)
                                        .ToList();

                //Enlazar datos al siguiente formulario

                if (userName.Length > 0 && userPass.Length > 0)
                {
                    if (result.Count() > 0)
                    {
                        if (result[0].KeyCode == userPass)
                        {
                            //Agregar el inicio de sesión al historial
                            LogInHistory login = new LogInHistory()
                            {
                                IdManager = result[0].Id,
                                LogInDateTime = DateTime.Now
                            };

                            db.Add(login);
                            db.SaveChanges();

                            //Instanciar formulario que aun no hemos creado
                            frmPrincipal principal = new frmPrincipal(result[0]);

                            //Ocultar formulario actual
                            this.Hide();

                            //Mostrar formulario que aun no tenemos
                            principal.ShowDialog();

                            //Reestablecer textbox
                            txtUser.Text = String.Empty;
                            txtPassword.Text = String.Empty;
                            this.Show();
                        }
                        else
                        {
                            MessageBox.Show("Contraseña incorrecta", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Usuario no existente", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Debe llenar todos los campos", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error al conectar con la base de datos", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}