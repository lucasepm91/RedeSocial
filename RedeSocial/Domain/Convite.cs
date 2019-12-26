using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedeSocial.Domain
{
    public class Convite
    {
        public int Id { get; set; }
        public Perfil Convidante { get; set; }
        public Perfil Convidado { get; set; }
        public string Status { get; set; }
    }
}
