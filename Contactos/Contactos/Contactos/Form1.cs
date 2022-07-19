using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contactos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                App.ContactList.AddContactListRow(App.ContactList.NewContactListRow()); //Crea una nueva linea donde estarán los datos en el DataGridView
                contactListBindingSource.MoveNext(); //Este creará la nueva linea debajo de la ultima linea
                txtNumero.Focus(); //Empieza en la linea de Numero Telefónico
                txtCorreo.Enabled = true;
                txtDirección.Enabled = true;
                txtNombre.Enabled = true;
                txtNumero.Enabled = true;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error, se cancelará el proceso.", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                App.ContactList.RejectChanges(); // En caso de algun error, saldrá este mensaje y quitará cualquier información que haya sido agregada
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Hará focus a la primera linea y habilitará las text box para poder editar la información
            txtNumero.Focus(); 
            txtCorreo.Enabled = true;
            txtDirección.Enabled = true;
            txtNombre.Enabled = true;
            txtNumero.Enabled = true;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Cancelará cualquier cambio hecho ya sea en un contacto nuevo o en edición
            contactListBindingSource.ResetBindings(false); 
            txtCorreo.Enabled = false;
            txtDirección.Enabled = false;
            txtNombre.Enabled = false;
            txtNumero.Enabled = false;

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                contactListBindingSource.EndEdit(); // Esto no permitirá más cambios en la información
                App.ContactList.AcceptChanges(); // Guardará los cambios y los mandará al DataSet
                App.ContactList.WriteXml(string.Format("{0}//data.dat", Application.StartupPath)); // Esto hace se escriban en el XML que se crea cuando abrimos el codigo y los escribe en el DataGridView
                txtCorreo.Enabled = false;
                txtDirección.Enabled = false;
                txtNombre.Enabled = false;
                txtNumero.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error, se cancelará el proceso.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                App.ContactList.RejectChanges(); 
            }
        }

        static DataSet1 db;
        protected static DataSet1 App
        {
            get
            {
                if (db == null)
                    db = new DataSet1();
                return db;
            }       
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fileName = string.Format("{0}//data.dat", Application.StartupPath); // Crea el XML que se utiliza en el proyecto
            if (File.Exists(fileName))
                App.ContactList.ReadXml(fileName); //Lee el XML en el gridview
            contactListBindingSource.DataSource = App.ContactList; // Enlaza el DataSet con la aplicación
            txtCorreo.Enabled = false;
            txtDirección.Enabled = false;
            txtNombre.Enabled = false;
            txtNumero.Enabled = false; 
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            // Hace que al presionar la tecla Delete, se borre el contacto seleccionado.
            if (e.KeyCode == Keys.Delete) 
            {
                if (contactListBindingSource == null)
                {
                    if (MessageBox.Show("Está seguro que quiere eliminar este contacto?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        contactListBindingSource.RemoveCurrent();
                    }
                    else
                    {
                        contactListBindingSource.CancelEdit();
                    }
                }
               
            }
               
        }

     

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permite que se pongan numeros en el textbox de Numero Telefónico
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && 
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
