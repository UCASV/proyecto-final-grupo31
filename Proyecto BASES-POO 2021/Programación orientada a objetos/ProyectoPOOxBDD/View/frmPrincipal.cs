using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ProyectoPOOxBDD.VaccinationContext;

namespace ProyectoPOOxBDD
{
    public partial class frmPrincipal : Form
    {
        //Usuario que inicia sesión
        private Manager manager;

        //Ciudadano auxiliar
        private Citizen aCitizen = null;

        //Cita auxiliar
        private Appointment anAppointment = null;

        //Lista enfermedades
        private List<String> diseases = new List<string>();

        public frmPrincipal(Manager manager)
        {
            InitializeComponent();
            this.manager = manager;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            //Declaracion del context
            var db = new VaccinationDBContext();

            //Quitar titulos del tab control
            //tabPrincipal.ItemSize = new Size(0, 1);

            //Usuario activo en el sistema
            lblUser.Text = "Usuario: " + manager.Username;

            //Cargar datos al combobox de grupos prioritarios
            List<PriorityGroup> prioritygroups = db.PriorityGroups.ToList();
            cmbPriorityGroup.DataSource = prioritygroups;
            cmbPriorityGroup.DisplayMember = "PriorityGroupName";
            cmbPriorityGroup.ValueMember = "Id";

            //Cargar datos al combobox lugar de vacunación
            List<VaccinationPlace> vaccionationPlaces = db.VaccinationPlaces.ToList();
            cmbVaccinationPlaceFirstAppo.DataSource = vaccionationPlaces;
            cmbVaccinationPlaceFirstAppo.DisplayMember = "Place";
            cmbVaccinationPlaceFirstAppo.ValueMember = "Id";

            //Cargar datos al combobox  de institucion
            List<Institution> institutions = db.Institutions.ToList();
            cmbInstitution.DataSource = institutions;
            cmbInstitution.DisplayMember = "InstitutionName";
            cmbInstitution.ValueMember = "Id";

            //Habilitar radio button de institución
            radNo.Checked = true;

            //Asignar valores iniciales a los combobox de fecha y hora
            cmbHourFirstAppointment.Text = "07";
            cmbMinutesFirstAppointment.Text = "00";
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //Verificando que todas las cajas de datos esten rellenadas
            if (txtDuiNumber.Text != String.Empty && txtName.Text != String.Empty && txtAddress.Text != String.Empty && txtPhoneNumber.Text != String.Empty)
            {
                string expressionDUI = "^[0-9]{8}-[0-9]{1}$";

                if (Regex.IsMatch(txtDuiNumber.Text, expressionDUI))
                {
                    var db = new VaccinationDBContext();

                    //Obteniendo la lista de ciudadanos existentes
                    List<Citizen> existingCitizens = db.Citizens
                        .Where(c => c.Dui == txtDuiNumber.Text)
                        .ToList();

                    if (existingCitizens.Count == 0)
                    {
                        string expressionPhone = "^[2||6||7]{1}[0-9]{7}$";

                        if (Regex.IsMatch(txtPhoneNumber.Text, expressionPhone))
                        {
                            //Almacenando datos en Ciudadano auxiliar 
                            aCitizen = new Citizen();

                            aCitizen.Dui = txtDuiNumber.Text;
                            aCitizen.FullName = txtName.Text;
                            aCitizen.HomeAddress = txtAddress.Text;
                            aCitizen.PhoneNumber = txtPhoneNumber.Text;
                            if (txtEmail.Text != String.Empty)
                                aCitizen.EmailAddress = txtEmail.Text;

                            //Bloquear el menu y evitar que se cierre el programa
                            mspPrincipal.Enabled = false;
                            this.ControlBox = false;

                            //Cambiando de pestaña
                            tabPrincipal.SelectedIndex = 2;
                        }
                        else
                        {
                            //Si se introduce un numero de telefono invalido
                            MessageBox.Show("Introduzca correctamente su número de telefono", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        //Si se introduce un DUI ya registrado en la base de datos
                        MessageBox.Show("Este DUI ya ha sido registrado", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    //Si los digitos del DUI no son correctos
                    MessageBox.Show("DUI invalido, asegurese de escrbirlo correctamente", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                //Si hay un textbox vacio
                MessageBox.Show("Debe llenar todos los campos obligatorios", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void radNo_CheckedChanged(object sender, EventArgs e)
        {
            //Habilitar o deshabilitar los datos de institución
            txtInstitutionIdentification.Enabled = !txtInstitutionIdentification.Enabled;
            cmbInstitution.Enabled = !cmbInstitution.Enabled;
        }

        private void btnAddDisease_Click(object sender, EventArgs e)
        {
            if (txtDisease.Text != String.Empty)
            {
                //Agregando enfermedades a la lista auxiliar
                diseases.Add(txtDisease.Text);
                MessageBox.Show("Elemento agregado a la lista", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Vaciando Textbox
                txtDisease.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Llene el campo antes de agregar", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //Regresando a pestaña anterior
            tabPrincipal.SelectedIndex = 1;
        }

        private void btnFinishRegister_Click(object sender, EventArgs e)
        {
            var db = new VaccinationDBContext();

            //Grupo prioritario de referencia
            PriorityGroup priorityRef = (PriorityGroup)cmbPriorityGroup.SelectedItem;

            //Obtener el grupo prioritario que está en la BDD a partir de la referencia
            PriorityGroup priorityBdd = db.Set<PriorityGroup>()
                .SingleOrDefault(s => s.Id == priorityRef.Id);

            //Agregando el grupo prioritario al usuario
            aCitizen.IdPriorityGroupNavigation = priorityBdd;

            if (!radNo.Checked)
            {
                if (txtInstitutionIdentification.Text != String.Empty)
                {
                    //Numero de identificador de la institucion
                    aCitizen.InstitutionIdentification = txtInstitutionIdentification.Text;

                    //Institucion de referencia
                    Institution institutionRef = (Institution)cmbInstitution.SelectedItem;

                    //Obtener el grupo prioritario que está en la BDD a partir de la referencia
                    Institution institutionBdd = db.Set<Institution>()
                        .SingleOrDefault(s => s.Id == institutionRef.Id);

                    //Agregando la institucion al usuario
                    aCitizen.IdInstitutionNavigation = institutionBdd;

                    //Agregamos el ciudadano a la bdd
                    db.Add(aCitizen);
                    db.SaveChanges();

                    //Finalizamos el registro
                    FinishRegister();
                }
                else
                {
                    //Si hay un textbox vacio
                    MessageBox.Show("Debe ingresar su identificador de institución", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                //Agregamos el ciudadano a la bdd
                db.Add(aCitizen);
                db.SaveChanges();

                //Finalizamos el registro
                FinishRegister();
            }
        }

        private void FinishRegister()
        {
            var db = new VaccinationDBContext();

            //Obteniendo al ciudadano recién registrado
            List<Citizen> citizen = db.Citizens
                .Where(c => c.Dui == aCitizen.Dui)
                .ToList();

            diseases.ForEach(d => db.Add(new Disease() { DiseaseName = d, IdCitizen = citizen[0].Id }));

            db.SaveChanges();

            lblCitizenNameFirstAppointment.Text = aCitizen.FullName;
            tabPrincipal.SelectedIndex = 3;
        }

        private void btnAddFirstAppointment_Click(object sender, EventArgs e)
        {
            var db = new VaccinationDBContext();

            //Obtener datos para el DateTime
            DateTime dateTime = dtpDateFirstAppointment.Value.Date;

            dateTime = dateTime.AddHours(Int32.Parse(cmbHourFirstAppointment.Text));
            dateTime = dateTime.AddMinutes(Int32.Parse(cmbMinutesFirstAppointment.Text));

            //Obtener reservas existentes en el lugar y fecha seleccionado
            VaccinationPlace placeRef = (VaccinationPlace)cmbVaccinationPlaceFirstAppo.SelectedItem;

            //Obtener el grupo prioritario que está en la BDD a partir de la referencia
            VaccinationPlace PlaceBdd = db.Set<VaccinationPlace>()
                .SingleOrDefault(s => s.Id == placeRef.Id);


            List<Appointment> appointment = db.Appointments
            .Where(a => a.IdVaccinationPlace == PlaceBdd.Id && a.AppointmentDateTime == dateTime)
                .ToList();

            if (dateTime >= DateTime.Now)
            {
                if (appointment.Count < 5)
                {
                    //Almacenando datos en cita auxiliar 
                    anAppointment = new Appointment();

                    //Llenando datos de cita auxiliar
                    anAppointment.AppointmentDateTime = dateTime;
                    anAppointment.IdVaccinationPlace = PlaceBdd.Id;
                    anAppointment.IdManager = manager.Id;
                    anAppointment.IdCitizen = aCitizen.Id;
                    anAppointment.IdAppointmentType = 1;

                    db.Add(anAppointment);
                    db.SaveChanges();

                    //Preparando datos de la siguiente pestaña

                    lblDuiResume.Text = "Número de DUI: " + aCitizen.Dui;
                    lblNameResume.Text = "Nombre: " + aCitizen.FullName;
                    lblDateResume.Text = "Fecha: " + anAppointment.AppointmentDateTime.ToShortDateString();
                    lblTimeResume.Text = "Hora: " + anAppointment.AppointmentDateTime.ToShortTimeString();
                    lblVaccinationPlaceResume.Text = "Lugar de vacunación: " + PlaceBdd.Place;
                    lblAppointmentTypeResume.Text = "Tipo de cita: Primera dosis";

                    //Dirigiendo al resumen de cita
                    tabPrincipal.SelectedIndex = 4;
                }
                else
                {
                    //Si hay mas de un cierto numero de citas en un espacio de tiempo
                    MessageBox.Show("Los cupos en la fecha y hora seleccionada se han agotado", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                //Si se ingresa una fecha anterior a la actual
                MessageBox.Show("La fecha que ha ingresado no es valida", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ExitFirstAppointment()
        {
            //Revirtiendo variables gloabales a nulos
            aCitizen = null;
            anAppointment = null;
            diseases.Clear();

            //Vaciando Textbox anteriores
            txtDuiNumber.Text = String.Empty;
            txtName.Text = String.Empty;
            txtAddress.Text = String.Empty;
            txtPhoneNumber.Text = String.Empty;
            txtEmail.Text = String.Empty;

            radNo.Checked = true;
            txtInstitutionIdentification.Text = String.Empty;
            txtDisease.Text = String.Empty;

            dtpDateFirstAppointment.Value = DateTime.Now;

            //Regresando a pestaña de inicio
            tabPrincipal.SelectedIndex = 0;

            //Restablecer valores iniciales a los combobox de fecha y hora
            cmbHourFirstAppointment.Text = "07";
            cmbMinutesFirstAppointment.Text = "00";

            //Activar el menú y el botóon de cerrar formulario
            mspPrincipal.Enabled = true;
            this.ControlBox = true;
        }

        private void btnExitResume_Click(object sender, EventArgs e)
        {
            ExitFirstAppointment();
        }
    }
}
