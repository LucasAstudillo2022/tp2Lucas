using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace prueba.Models
{
    public class Cooperativa
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string biografia { get; set; }
        public DateTime fechacreacion { get; set; }



        public string fotocoop { get; set; }

        //instancia de las clases con las relaciones de Id

        public int mutualid { get; set; }

        public Mutual mutualpers { get; set; }

        public int personaid { get; set; }
        public Datopersona nombre { get; set; }
    }
}
