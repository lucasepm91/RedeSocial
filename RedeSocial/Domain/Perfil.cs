using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedeSocial.Domain
{
    public class Perfil
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Sobre { get; set; }
        public string Avatar { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
