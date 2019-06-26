using System;
using System.Collections.Generic;

namespace CoreBot.Models
{
    public partial class Acciones
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? ModifedDate { get; set; }
    }
}
