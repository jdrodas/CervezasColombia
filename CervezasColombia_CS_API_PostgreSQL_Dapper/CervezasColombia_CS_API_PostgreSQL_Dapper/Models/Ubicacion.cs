﻿namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class Ubicacion
    {
        public int Id { get; set; } = 0;
        public string Municipio { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraUbicacion = (Ubicacion)obj;

            return Id == otraUbicacion.Id
                   && Municipio.Equals(otraUbicacion.Municipio)
                   && Departamento.Equals(otraUbicacion.Departamento);
        }
    }
}
