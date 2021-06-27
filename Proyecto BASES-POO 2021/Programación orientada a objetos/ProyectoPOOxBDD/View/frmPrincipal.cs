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

            //Cargar datos al combobox de efectos secundarios
            List<SideEffect> sideEffects = db.SideEffects.ToList();
            cmbSideEffect.DataSource = sideEffects;
            cmbSideEffect.DisplayMember = "SideEffectName";
            cmbSideEffect.ValueMember = "Id";
            cmbSideEffectTime.Text = "1";
            cmbSideEffect.DropDownWidth = 450;

            //Habilitar radio button de institución
            radNo.Checked = true;

            //Habilitar radio button de proceso de vacunación
            radNoProcess.Checked = true;

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

            if(appointmentType.Id == 1)
                ExitFirstAppointment();
            else
                ExitSecondAppointment();

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

            //Activar el menú y el botón de cerrar formulario
            mspPrincipal.Enabled = true;
            this.ControlBox = true;
        }

        private void ExitSecondAppointment()
        {
            //Revirtiendo variables gloabales a nulos
            anAppointment = null;
            ClearTracking();

            //Vaciando labels anteriores
            lblCitizenNameSecondAppointment.Text = String.Empty;
            lblVaccinationPlaceSecondAppointment.Text = String.Empty;
            lblDateSecondAppointment.Text = String.Empty;
            lblHourSecondAppointment.Text = String.Empty;
            lblMinutesSecondAppointment.Text = String.Empty;

            radNoProcess.Checked = true;

            //Regresando a pestaña de inicio
            tabPrincipal.SelectedIndex = 0;

            //Restablecer valores iniciales al combobox de minutos de aparición
            cmbSideEffectTime.Text = "1";

            //Activar el menú y el botón de cerrar formulario
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
            if (anAppointment.IdAppointmentType == 1)
                ExitFirstAppointment();
            else
                ExitSecondAppointment();
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
                aCitizen = citizens[0];

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
                
                if(citizens[0].Diseases.ToList().Count != 0 )
                {
                    citizens[0].Diseases.ToList().ForEach(d => diseasesNames.Add(d.DiseaseName));
                    txtDiseaseTraking.Lines = diseasesNames.ToArray();

                }
                else
                {
                    txtDiseaseTraking.Text = "N/A";
                }

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
                //Cuando se ingresa un DUI que no ha sido registrado
                MessageBox.Show("No se ha encontrado el DUI ingresado", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnExportPdfTraking_Click(object sender, EventArgs e)
        {
            if (aCitizen == null)
                MessageBox.Show("No hay datos que exportar", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                CreatePDFTracking(aCitizen.Dui, aCitizen.FullName, aCitizen.HomeAddress, aCitizen.PhoneNumber, aCitizen.EmailAddress,
                    aCitizen.IdPriorityGroupNavigation.PriorityGroupName, aCitizen.InstitutionIdentification,
                    aCitizen.IdInstitutionNavigation.InstitutionName, aCitizen.Diseases.ToList());
        }

        private void CreatePDFTracking(string dui, string fullName, string address, string phone, string email, string priorityGroup,
            string institutionIdentification, string institution, List<Disease> citizenDiseases)
        {
            // Asignar la ruta en la que se guardará el PDF
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //Crear instancias para la creación del PDF
            PdfWriter pdfWriter = new PdfWriter(path + $"\\{dui} informacion.pdf");
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

            //Validando campos vacios
            if (email == null)
                email = "N/A";
            if (institutionIdentification == null)
                institutionIdentification = "N/A";
            if (institution == null)
                institutionIdentification = "N/A";

            //Validando la lista de enfermedades
            string diseases = String.Empty;

            if (citizenDiseases.Count == 0)
                diseases = "N/A";
            else
            {
                //Obteniendo nombres de enfermedades de la lista
                List<String> diseasesNames = new List<string>();
                citizenDiseases.ForEach(d => diseasesNames.Add(d.DiseaseName));

                foreach (var item in diseasesNames)
                {
                    diseases = $"{diseases} {item}, ";
                }

                diseases = diseases.Remove(diseases.Length - 2);
            }

            //Estructura del documento
            document.Add(new Paragraph("").Add(logo).SetTextAlignment(TextAlignment.CENTER));

            document.Add(new LineSeparator(new SolidLine())); //Línea de separación

            document.Add(new Paragraph(new Text("Información del registro").SetFont(boldFont).SetFontSize(20)).SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph(new Text("Número de DUI: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(dui).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Nombre: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(fullName).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Dirección de domicilio: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(address).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Número de teléfono: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(phone).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Correo electrónico: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(email).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Grupo prioritario: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(priorityGroup).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Número identificador de la institución: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(institutionIdentification).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Institución a la que pertenece: ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(institution).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Enfermedad(es) crónica(s): ").SetFont(boldFont).SetFontSize(16))
                    .Add(new Text(diseases).SetFont(font).SetFontSize(16)).SetTextAlignment(TextAlignment.JUSTIFIED));

            document.Add(new Paragraph(new Text("Cita(s) agendada(s)").SetFont(boldFont).SetFontSize(20)).SetTextAlignment(TextAlignment.CENTER));

            //Exportando DataGridView
            Table table = PDFTableFromDGV(dgvAppointment, font, boldFont);

            document.Add(table);

            document.Close();

            //Mensaje de terminado
            MessageBox.Show($"Pdf exportado correctamente en {path}", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Table PDFTableFromDGV(DataGridView dgv, PdfFont font, PdfFont boldFont)
        {
            // Obteniedo el valor de columnas y filas
            int dgvrowcount = dgv.Rows.Count;
            int dgvcolumncount = dgv.Columns.Count;

            // Estableciendo el ancho de celdas
            float[] size = { 50, 190, 180, 370, 120 };

            Table table = new Table(UnitValue.CreatePercentArray(size));
            table.SetWidth(iText.Layout.Properties.UnitValue.CreatePercentValue(100));

            // Exportando los header del dgv
            for (int i = 0; i < dgvcolumncount; i++)
            {
                Cell headerCells = new Cell()
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(boldFont)
                    .SetFontSize(16);

                var gteCell = headerCells.Add(new Paragraph(dgv.Columns[i].HeaderText));
                table.AddHeaderCell(gteCell);
            }

            // Exportando las celdas del dgv
            for (int i = 0; i < dgvrowcount; i++)
            {
                for (int c = 0; c < dgvcolumncount; c++)
                {
                    Cell cells = new Cell()
                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFont(font)
                        .SetFontSize(16);

                    if (c == 4)
                    {
                        if (dgv[c, i].Value.Equals(true))
                        {
                            var gteCell = cells.Add(new Paragraph("Sí"));
                            table.AddCell(gteCell);
                        }
                        else
                        {
                            var gteCell = cells.Add(new Paragraph("No"));
                            table.AddCell(gteCell);
                        }
                    }
                    else
                    {
                        var gteCell = cells.Add(new Paragraph(dgv[c, i].Value.ToString()));
                        table.AddCell(gteCell);
                    }
                }
            }

            return table;
        }

        private void ClearTracking()
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
            aCitizen = null;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTracking();
        }

        private void dgvAppointment_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                AppointmentVm aVm = dgvAppointment.SelectedRows[0].DataBoundItem as AppointmentVm;

                if (!aVm.Attendance)
                {
                   DialogResult result = MessageBox.Show("¿Acepta seguir con el proceso de vacunación?", "Vacunación Covid-19", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                   if (result == DialogResult.Yes)
                   {
                        var db = new VaccinationDBContext();

                        //Obteniendo la cita de la base de datos
                        List<Appointment> appointment = db.Appointments
                            .Include(a => a.IdVaccinationPlaceNavigation)
                            .Include(a => a.IdAppointmentTypeNavigation)
                            .Where(a => a.Id.Equals(aVm.Id))
                            .ToList();

                        anAppointment = appointment[0];

                        //Bloquear el menu y evitar que se cierre el programa
                        mspPrincipal.Enabled = false;
                        this.ControlBox = false;

                        tabPrincipal.SelectedIndex = 6;
                   }
                   else
                   {
                       //Cuando el ciudadano decide no seguir con el proceso de vacunación
                        MessageBox.Show("El proceso se ha cancelado", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                   }
                }
                else
                {
                    //Cuando se intenta atender a una cita ya atendida
                    MessageBox.Show("La cita ya ha sido atendida", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void radNoProcess_CheckedChanged(object sender, EventArgs e)
        {
            //Habilitar o deshabilitar los datos de efecto secundario
            cmbSideEffect.Enabled = !cmbSideEffect.Enabled;
            cmbSideEffectTime.Enabled = !cmbSideEffectTime.Enabled;
            btnAddSideEffect.Enabled = !btnAddSideEffect.Enabled;
        }

        private void btnArrivalDateTime_Click(object sender, EventArgs e)
        {
            
            var db = new VaccinationDBContext();

            //Actualizando hora de llegada a la cita
            
            Appointment a = (from x in db.Appointments
                              where x.Id == anAppointment.Id
                              select x).First();
            if (a.ArrivalDateTime == null)
            {
                a.ArrivalDateTime = DateTime.Now;

                MessageBox.Show("Se registro su hora de llegada", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Information);
                db.SaveChanges();

                anAppointment.ArrivalDateTime = a.ArrivalDateTime;
            }
            else
                MessageBox.Show("Su hora de llegada ya ha sido registrada", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void btnVaccinationDateTime_Click(object sender, EventArgs e)
        {
            var db = new VaccinationDBContext();

            //Actualizando hora de vacunación

            Appointment a = (from x in db.Appointments
                             where x.Id == anAppointment.Id
                             select x).First();
            if (a.VaccinationDateTime == null)
            {
                a.VaccinationDateTime = DateTime.Now;

                MessageBox.Show("Se registro su hora de vacunación", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Information);
                db.SaveChanges();

                anAppointment.VaccinationDateTime = a.VaccinationDateTime;
            }
            else
                MessageBox.Show("Su hora de vacunación ya ha sido registrada", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void btnAddSideEffect_Click(object sender, EventArgs e)
        {
            var db = new VaccinationDBContext();

            SideEffect s = (SideEffect) cmbSideEffect.SelectedItem;

            List<VaccineReaction> existingReaction = db.VaccineReactions
                        .Where(r => r.IdSideEffect == s.Id && r.IdAppointment == anAppointment.Id)
                        .ToList();
            if(existingReaction.Count == 0)
            {
                db.Add(new VaccineReaction()
                {
                    IdAppointment = anAppointment.Id,
                    IdSideEffect = s.Id,
                    AppearenceTime = Int32.Parse(cmbSideEffectTime.Text)
                });
                MessageBox.Show("Efecto secundario registrado con exito", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Information);
                db.SaveChanges();
            } 
            else
                MessageBox.Show("El efecto secundario seleccionado ya ha sido registrado", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnFinishProcess_Click(object sender, EventArgs e)
        {
            var db = new VaccinationDBContext();

            //Consiguiendo cita actual
            
            Appointment a = (from x in db.Appointments
                             where x.Id == anAppointment.Id
                             select x).First();
            //Validando que se completen los pasos
            if (a.VaccinationDateTime != null && a.ArrivalDateTime != null)
            {
                if (anAppointment.IdAppointmentType == 2)
                {
                    //Regresando a pestaña de inicio
                    tabPrincipal.SelectedIndex = 0;
                    radNoProcess.Checked = true;

                    anAppointment = null;
                    this.ControlBox = true;
                    mspPrincipal.Enabled = true;
                    ClearTracking();
                }
                else
                {

                    //Definiendo labels de segunda cita
                    lblCitizenNameSecondAppointment.Text = aCitizen.FullName;
                    lblVaccinationPlaceSecondAppointment.Text = anAppointment.IdVaccinationPlaceNavigation.Place;

                    //Definiendo fecha para la cita de segunda dósis 
                    DateTime dateTime = anAppointment.VaccinationDateTime.Value.Date;

                    //Obtener datos para el DateTime
                    dateTime = dateTime.AddDays(56);
                    dateTime = dateTime.AddHours(anAppointment.AppointmentDateTime.Hour);
                    dateTime = dateTime.AddMinutes(anAppointment.AppointmentDateTime.Minute);

                    //Definiendo labels de fecha y hora de segunda cita
                    lblDateSecondAppointment.Text = dateTime.ToShortDateString();
                    lblHourSecondAppointment.Text = dateTime.Hour.ToString();
                    lblMinutesSecondAppointment.Text = dateTime.Minute.ToString();

                    //Avanzando a pestaña de segunda cita
                    tabPrincipal.SelectedIndex = 7;
                }
            }
            else
            {
                //Si no se ha registrado la hora de llegada y vacunación
                MessageBox.Show("La fecha de llegada y vacunacion no han sido registradas", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnAddSecondAppointment_Click(object sender, EventArgs e)
        {
            var db = new VaccinationDBContext();

            //Obtener DateTime
            DateTime dateTime = DateTime.Parse(lblDateSecondAppointment.Text);
            dateTime = dateTime.AddHours(Int32.Parse(lblHourSecondAppointment.Text));
            dateTime = dateTime.AddMinutes(Int32.Parse(lblMinutesSecondAppointment.Text));

            //Creamos y llenamos la segunda cita
            Appointment aSecondAppointment = new Appointment()
            {
                IdCitizen = aCitizen.Id,
                IdManager = manager.Id,
                AppointmentDateTime = dateTime,
                IdVaccinationPlace = anAppointment.IdVaccinationPlace,
                IdAppointmentType = 2
            };
            MessageBox.Show("Cita registrada con exito", "Vacunación Covid-19", MessageBoxButtons.OK, MessageBoxIcon.Information);

            db.Add(aSecondAppointment);
            db.SaveChanges();
            
            anAppointment = aSecondAppointment;

            //Preparando datos de la siguiente pestaña
            lblDuiResume.Text = "Número de DUI: " + aCitizen.Dui;
            lblNameResume.Text = "Nombre: " + aCitizen.FullName;
            lblDateResume.Text = "Fecha: " + anAppointment.AppointmentDateTime.ToShortDateString();
            lblTimeResume.Text = "Hora: " + anAppointment.AppointmentDateTime.ToShortTimeString();
            lblVaccinationPlaceResume.Text = "Lugar de vacunación: " + lblVaccinationPlaceSecondAppointment.Text;
            lblAppointmentTypeResume.Text = "Tipo de cita: Segunda dosis";

            //Nullificar las variables
            aSecondAppointment = null;

            //Dirigiendo al resumen de cita
            tabPrincipal.SelectedIndex = 4;
        }
    }
}