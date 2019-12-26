using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedeSocial.Domain
{
    public class Mensagem
    {
        public int Id { get; set; }
        public string Conteudo { get; set; }
        public Perfil Autor { get; set; }
    }
}
