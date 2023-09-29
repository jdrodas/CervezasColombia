namespace CervezasColombia_CS_PoC_Consola
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("PoC - Cervezas Artesanales de Colombia");

            //Console.WriteLine("Ejecutando PoC en SQLite...");
            //PoC_SQLite.Ejecuta_PoC();

            //Console.WriteLine("Ejecutando PoC en PostgreSQL...");
            //PoC_Pgsql.Ejecuta_PoC();

            Console.WriteLine("Ejecutando PoC en MongoDB...");
            PoC_Mongo.Ejecuta_PoC();
        }
    }
}