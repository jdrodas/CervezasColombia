namespace CervezasColombia_CS_API_PostgreSQL_Dapper.Models
{
    public class Ingrediente
    {
        public int Id { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public int Tipo_Ingrediente_Id { get; set; } = 0;
        public string Tipo_Ingrediente { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroIngrediente = (Ingrediente)obj;

            return Id == otroIngrediente.Id
                   && Nombre.Equals(otroIngrediente.Nombre)
                   && Tipo_Ingrediente_Id == otroIngrediente.Tipo_Ingrediente_Id
                   && Tipo_Ingrediente.Equals(otroIngrediente.Tipo_Ingrediente);
        }
    }
}
