namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class Cerveceria
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public string Sitio_Web { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public int Ubicacion_Id { get; set; } = 0;
        public string Ubicacion { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraCerveceria = (Cerveceria)obj;

            return Id == otraCerveceria.Id
                   && Nombre.Equals(otraCerveceria.Nombre)
                   && Sitio_Web.Equals(otraCerveceria.Sitio_Web)
                   && Instagram.Equals(otraCerveceria.Instagram)
                   && Ubicacion_Id == otraCerveceria.Ubicacion_Id
                   && Ubicacion.Equals(otraCerveceria.Ubicacion);
        }
    }
}