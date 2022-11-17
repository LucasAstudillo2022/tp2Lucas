using Microsoft.AspNetCore.Mvc.Rendering;
using prueba.Models;
using System;
using System.Collections.Generic;

namespace prueba.ViewModels
{
    public class PersonasViewModels
    {
        public List<Cooperativa> personas { get; set; }
        public string nombre { get; set; }
        public SelectList Mutual { get; set; }      
        public string busqNombre { get; set; }
        public paginador paginador { get; set; }
    }
    public class paginador
    {
        public int cantReg { get; set; }
        public int regXpag { get; set; }
        public int pagActual { get; set; }
        public int totalPag { get; set; }
        
        public Dictionary<string, string> ValoresQueryString { get; set; } = new Dictionary<string, string>();
    }
}
