﻿namespace CervezasColombia_CS_API_SQLite_Dapper.Cervecerias
{
    public class CerveceriaQueryParameters
    {
        private static readonly List<string> criteriosValidos =
            ["nombre", "instagram"];

        private static readonly List<string> ordenesValidos =
            ["asc", "desc"];

        private string orden = string.Empty;
        private string criterio = string.Empty;

        public int Id { get; set; } = 0;

        public string? Nombre { get; set; }

        public string? Instagram { get; set; }

        public int Pagina { get; set; } = 1;

        public int ElementosPorPagina { get; set; } = 10;

        public string Orden
        {
            get
            {
                if (string.IsNullOrEmpty(orden))
                    orden = "asc";

                return orden;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !ordenesValidos.Contains(value.ToLower()))
                    orden = "asc";
                else
                    orden = value;
            }
        }

        public string Criterio
        {
            get
            {
                if (string.IsNullOrEmpty(criterio))
                    criterio = "nombre";

                return criterio;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !criteriosValidos.Contains(value.ToLower()))
                    criterio = "nombre";
                else
                    criterio = value;
            }
        }
    }
}
