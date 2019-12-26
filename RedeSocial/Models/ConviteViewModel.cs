using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedeSocial.Models
{
    public class ConviteViewModel
    {
        public int Id { get; set; }
        public PerfilViewModel Convidante { get; set; }
        public PerfilViewModel Convidado { get; set; }
        public string Status { get; set; }
    }
}
