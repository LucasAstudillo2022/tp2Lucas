using System.Collections.Generic;

namespace prueba.Models
{
    public class Mutual
    {
        public int Id { get; set; }
        public string mutualpers { get; set; }

        public List<Cooperativa> mutuals { get; set; }

        //a estos datos mandarlos a datos personales
    }
}
