namespace CervezasColombia_CS_API_Mongo.Models
{
    public class CervezasDatabaseSettings
    {
        public string DatabaseName { get; set; } = null!;
        public string ColeccionCervecerias { get; set; } = null!;
        public string ColeccionCervezas { get; set; } = null!;
        public string ColeccionEstilos { get; set; } = null!;
        public string ColeccionEnvasados { get; set; } = null!;
        public string ColeccionEnvasadosCervezas { get; set; } = null!;
        public string ColeccionIngredientes { get; set; } = null!;
        public string ColeccionIngredientesCervezas { get; set; } = null!;
        public string ColeccionTiposIngredientes { get; set; } = null!;
        public string ColeccionRangosIbu { get; set; } = null!;
        public string ColeccionRangosAbv { get; set; } = null!;
        public string ColeccionUbicaciones { get; set; } = null!;
        public string ColeccionUnidadesVolumen { get; set; } = null!;

        public CervezasDatabaseSettings(IConfiguration unaConfiguracion)
        {
            var configuracion = unaConfiguracion.GetSection("CervezasDatabaseSettings");

            DatabaseName = configuracion.GetSection("DatabaseName").Value!;
            ColeccionCervecerias = configuracion.GetSection("ColeccionCervecerias").Value!;
            ColeccionCervezas = configuracion.GetSection("ColeccionCervezas").Value!;
            ColeccionEstilos = configuracion.GetSection("ColeccionEstilos").Value!;
            ColeccionEnvasados = configuracion.GetSection("ColeccionEnvasados").Value!;
            ColeccionEnvasadosCervezas = configuracion.GetSection("ColeccionEnvasadosCervezas").Value!;
            ColeccionIngredientes = configuracion.GetSection("ColeccionIngredientes").Value!;
            ColeccionIngredientesCervezas = configuracion.GetSection("ColeccionIngredientesCervezas").Value!;
            ColeccionTiposIngredientes = configuracion.GetSection("ColeccionTiposIngredientes").Value!;
            ColeccionRangosIbu = configuracion.GetSection("ColeccionRangosIbu").Value!;
            ColeccionRangosAbv = configuracion.GetSection("ColeccionRangosAbv").Value!;
            ColeccionUbicaciones = configuracion.GetSection("ColeccionUbicaciones").Value!;
            ColeccionUnidadesVolumen = configuracion.GetSection("ColeccionUnidadesVolumen").Value!;
        }
    }
}