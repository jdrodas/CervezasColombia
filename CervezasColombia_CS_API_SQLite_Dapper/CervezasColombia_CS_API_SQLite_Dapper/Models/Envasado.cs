﻿namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class Envasado
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public string Unidad_Volumen { get; set; } = string.Empty;
        public int Volumen { get; set; } = 0;
    }
}