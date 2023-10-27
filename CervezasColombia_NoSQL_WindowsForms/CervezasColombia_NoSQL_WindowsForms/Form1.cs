using CervezasColombia_NoSQL_WindowsForms.Modelos;

namespace CervezasColombia_NoSQL_WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ActualizaListaCervecerias();
        }

        private void ActualizaListaCervecerias()
        {
            cbxNombreCervecerias.DataSource = null;
            cbxNombreCervecerias.DataSource = AccesoDatos.ObtenerListaNombresCervecerias();

            cbxNombreCervecerias.SelectedIndex = 0;
        }

        private void cbxNombreCervecerias_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                string? nombreCerveceria = cbxNombreCervecerias.SelectedItem!.ToString();

                Cerveceria unaCerveceria = AccesoDatos.ObtenerCerveceriaPorNombre(nombreCerveceria!);

                txtNombreCerveceria.Text = unaCerveceria.Nombre;
                txtInstagramCerveceria.Text = unaCerveceria.Instagram;
                txtSitioWebCerveceria.Text = unaCerveceria.Sitio_Web;
                txtUbicacionCerveceria.Text = unaCerveceria.Ubicacion;

            }
        }
    }
}