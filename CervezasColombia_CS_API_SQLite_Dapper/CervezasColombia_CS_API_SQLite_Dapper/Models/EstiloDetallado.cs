﻿namespace CervezasColombia_CS_API_SQLite_Dapper.Models
{
    public class EstiloDetallado:Estilo
    {
        public List<Cerveza> Cervezas { get; set; } = new List<Cerveza>();
    }
}