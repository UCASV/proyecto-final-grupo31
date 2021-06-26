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
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.EntityFrameworkCore;
using ProyectoPOOxBDD.Properties;
using ProyectoPOOxBDD.VaccinationContext;
using ProyectoPOOxBDD.ViewModels;

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

            //Configurar DGV de cabinas
            List<Booth> boothList = db.Booths.ToList();
            List<BoothVm> boothVmList = new List<BoothVm>();

            boothList.ForEach(e => boothVmList.Add(VaccinationMapper.MapBoothToBoothVm(e)));

            dgvBooth.DataSource = boothVmList;

            dgvBooth.Columns[0].Width = 50;
            dgvBooth.Columns[0].HeaderText = "ID";
            dgvBooth.Columns[1].Width = 700;
            dgvBooth.Columns[1].HeaderText = "Dirección";
            dgvBooth.Columns[2].Width = 180;
            dgvBooth.Columns[2].HeaderText = "Teléfono";
            dgvBooth.Columns[3].Width = 320;
            dgvBooth.Columns[3].HeaderText = "Correo Electrónico";

            dgvBooth.RowTemplate.Height = 40;

            //Quitar titulos del tab control
            tabPrincipal.ItemSize = new Size(0, 1);

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
                    //Numero de identificador de la institución
                    aCitizen.InstitutionIdentification = txtInstitutionIdentification.Text;

                    //Institución de referencia
                    Institution institutionRef = (Institution)cmbInstitution.SelectedItem;

                    //Obtener la institución que está en la BDD a partir de la referencia
                    Institution institutionBdd = db.Set<Institution>()
                        .SingleOrDefault(s => s.Id == institutionRef.Id);

                    //Agregando la institución al usuario
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

            //Bloquear el menu y evitar que se cierre el programa
            mspPrincipal.Enabled = false;
            this.ControlBox = false;

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

            //Obtener el lugar de vacunación que está en la BDD a partir de la referencia
            VaccinationPlace placeBdd = db.Set<VaccinationPlace>()
                .SingleOrDefault(s => s.Id == placeRef.Id);


            List<Appointment> appointment = db.Appointments
            .Where(a => a.IdVaccinationPlace == placeBdd.Id && a.AppointmentDateTime == dateTime)
                .ToList();

            if (dateTime >= DateTime.Now)
            {
                if (appointment.Count < 5)
                {
                    //Almacenando datos en cita auxiliar 
                    anAppointment = new Appointment();

                    //Llenando datos de cita auxiliar
                    anAppointment.AppointmentDateTime = dateTime;
                    anAppointment.IdVaccinationPlace = placeBdd.Id;
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
                    lblVaccinationPlaceResume.Text = "Lugar de vacunación: " + placeBdd.Place;
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

        private void btnExportPdfResume_Click(object sender, EventArgs e)
        {
            var db = new VaccinationDBContext();

            //Obtener el lugar de vacunación que está en la BDD
            VaccinationPlace place = db.Set<VaccinationPlace>()
                .SingleOrDefault(s => s.Id == anAppointment.IdVaccinationPlace);

            //Obtener el tipo de cita que está en la BDD
            AppointmentType appointmentType = db.Set<AppointmentType>()
                .SingleOrDefault(a => a.Id == anAppointment.IdAppointmentType);

            CreatePDF(aCitizen.Dui, aCitizen.FullName, anAppointment.AppointmentDateTime.ToLongDateString(),
                anAppointment.AppointmentDateTime.ToShortTimeString(), place.Place, appointmentType.TypeName);

            ExitFirstAppointment();
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

        private void CreatePDF(string dui, string fullName, string date, string time, string place, string appointmentType)
        {
            // Asignar la ruta en la que se guardará el PDF
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //Crear instancias para la creación del PDF
            PdfWriter pdfWriter = new PdfWriter(path + $"\\{dui} {appointmentType}.pdf");
            PdfDocument pdf = new PdfDocument(pdfWriter);
            Document document = new Document(pdf, PageSize.LETTER);

            //Margenes del documento
            document.SetMargins(70, 70, 70, 70);

            //Creación de fuentes para el documento
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            //Convertir logo de formato bitmap a byte[]
            byte[] data = default(byte[]);

            using (System.IO.MemoryStream sampleStream = new System.IO.MemoryStream())

            {
                //save to stream.
                Resources.VaccinationLogo.Save(sampleStream, System.Drawing.Imaging.ImageFormat.Bmp);
                //the byte array
                data = sampleStream.ToArray();
            }

            //Logo del documento
            iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(data)).SetWidth(200);

            //Estructura del documento
            document.Add(new Paragraph("").Add(logo).SetTextAlignment(TextAlignment.CENTER));

            document.Add(new LineSeparator(new SolidLine())); //Línea de separación

            document.Add(new Paragraph(new Text("Información de cita").SetFont(boldFont).SetFontSize(20)).SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph(new Text("Número de DUI: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(dui).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Nombre: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(fullName).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Fecha: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(date).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Hora: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(time).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Lugar de vacunación: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(place).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Tipo de cita: ").SetFont(boldFont).SetFontSize(16))
                .Add(new Text(appointmentType).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Close();

            MessageBox.Show($"Pdf exportado correctamente en {path}", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExitResume_Click(object sender, EventArgs e)
        {
            ExitFirstAppointment();
        }

        private void welcomeMenuItem_Click(object sender, EventArgs e)
        {
            tabPrincipal.SelectedIndex = 0;
        }

        private void registerMenuItem_Click(object sender, EventArgs e)
        {
            tabPrincipal.SelectedIndex = 1;
        }

        private void trackingMenuItem_Click(object sender, EventArgs e)
        {
            tabPrincipal.SelectedIndex = 5;
        }

        private void boothMenuItem_Click(object sender, EventArgs e)
        {
            tabPrincipal.SelectedIndex = 8;          
        }

        private void logOutMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var db = new VaccinationDBContext();

            //Verifcando que exista el ciudadano que está en la BDD
            List<Citizen> citizens = db.Citizens
                                    .Include(c => c.IdInstitutionNavigation)
                                    .Include(c => c.Diseases)
                                    .Include(c => c.IdPriorityGroupNavigation)
                                    .Where(c => c.Dui.Equals(txtSearch.Text))
                                    .ToList();

            if (citizens.Count > 0)
            {

                lblDuiNumberTraking.Text = "Número de DUI: " + citizens[0].Dui;
                lblNameTraking.Text = "Nombre completo: " + citizens[0].FullName;
                lblAddressTraking.Text = "Dirreccion de domicilio: " + citizens[0].HomeAddress;
                lblPhoneTraking.Text = "Número de teléfono: " + citizens[0].PhoneNumber;
                lblPriorityGroupTraking.Text = "Grupo prioritario: " + citizens[0].IdPriorityGroupNavigation.PriorityGroupName;

                if (citizens[0].EmailAddress != null)
                {
                    lblEmailTraking.Text = "Correo Electrónico: " + citizens[0].EmailAddress;
                }
                else
                {
                    //Cuando no se ingreso un correo electronico
                    lblEmailTraking.Text = "Correo Electrónico: N/A";
                }

                if (citizens[0].IdInstitutionNavigation != null)
                {
                    lblInstitutionIdentificationTraking.Text = "Número identificador de la institución: " + citizens[0].InstitutionIdentification;
                    lblInstitutionTraking.Text = "Institución a la que pertenece: " + citizens[0].IdInstitutionNavigation.InstitutionName;
                }
                else
                {
                    //Cuando no pertenece a una institucion
                    lblInstitutionIdentificationTraking.Text = "Número identificador de la institución: N/A ";
                    lblInstitutionTraking.Text = "Institución a la que pertenece: N/A";
                }

                //Llenar textbox multiline de enfermedades
                List<String> diseasesNames = new List<string>();
                citizens[0].Diseases.ToList().ForEach(d => diseasesNames.Add(d.DiseaseName));
                txtDiseaseTraking.Lines = diseasesNames.ToArray();

                //Creando lista de citas
                List<Appointment> appointmentList = db.Appointments
                    .Include(a =>a.IdVaccinationPlaceNavigation)
                    .Include(a => a.IdAppointmentTypeNavigation)
                    .Where(a => a.IdCitizen.Equals(citizens[0].Id))
                    .ToList();

                List<AppointmentVm> appointmentVmList = new List<AppointmentVm>();

                appointmentList.ForEach(a => appointmentVmList.Add(VaccinationMapper.MapAppointmentToAppointmentVm(a)));

                //Agregando datos de citas al Datagrid View
                dgvAppointment.RowTemplate.Height = 40;
                dgvAppointment.DataSource = null;
                dgvAppointment.DataSource = appointmentVmList;

                dgvAppointment.Columns[0].Width = 50;
                dgvAppointment.Columns[0].HeaderText = "ID";
                dgvAppointment.Columns[1].Width = 190;
                dgvAppointment.Columns[1].HeaderText = "Tipo de cita";
                dgvAppointment.Columns[2].Width = 180;
                dgvAppointment.Columns[2].HeaderText = "Fecha y hora";
                dgvAppointment.Columns[3].Width = 370;
                dgvAppointment.Columns[3].HeaderText = "Lugar de vacunación";
                dgvAppointment.Columns[4].Width = 120;
                dgvAppointment.Columns[4].HeaderText = "Asistencia";

            }
            else
            {
                MessageBox.Show("No se ha encontrado el DUI ingresado", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

           

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Limpiando labels,textbox y datagrid view
            txtSearch.Text = String.Empty;
            lblDuiNumberTraking.Text = "Número de DUI: ";
            lblNameTraking.Text = "Nombre completo: ";
            lblAddressTraking.Text = "Dirreccion de domicilio: ";
            lblPhoneTraking.Text = "Número de teléfono: ";
            lblPriorityGroupTraking.Text = "Grupo prioritario: ";
            lblEmailTraking.Text = "Correo Electrónico: ";
            lblInstitutionIdentificationTraking.Text = "Número identificador de la institución: ";
            lblInstitutionTraking.Text = "Institución a la que pertenece: ";
            txtDiseaseTraking.Text = String.Empty;
            dgvAppointment.DataSource = null;
        }
    }
}