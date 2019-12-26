using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedeSocial.Models
{
    public class MensagemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.MultilineText)]
        [StringLength(140, ErrorMessage = "Mínimo de 2 e máximo de 140 caracteres ", MinimumLength = 2)]
        public string Conteudo { get; set; }
         
        public int PerfilId { get; set; }
    }
   
}
