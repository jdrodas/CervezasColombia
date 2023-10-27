using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervezasColombia_NoSQL_WindowsForms.Modelos
{
    public class CervezasDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ColeccionCervecerias { get; set; } = null!;
    }
}


