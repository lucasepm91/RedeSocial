using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedeSocial.Data;
using RedeSocial.Domain;

namespace RedeSocial.Controllers
{
    [Route("api/Amigo")]
    [ApiController]
    public class AmigoApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        /*public AmigoApiController(ApplicationDbContext context)
        {
            _context = context;
        }*/

        // GET: api/Amigo/5  ---> Todos os convites relacionados ao perfil (feitos e recebidos)
        [HttpGet("{id}")]
        public IEnumerable<Convite> MeusConvites([FromRoute] int id)
        {
            List<Convite> convites = _context.Convites.Where(c => (c.Convidado.Id == id || c.Convidante.Id == id) && c.Status.Contains("Aguardando")).Include(c => c.Convidado).Include(c => c.Convidante).ToList();
            
            return convites;
        }        
         
        // GET: api/amigo/5/aceitar/8
        [HttpGet("{perfilId}/aceitar/{idConvidante}")]
        public async Task<IActionResult> Aceitar([FromRoute] int perfilId,[FromRoute] int idConvidante)
        {
            Convite conviteOriginal = await _context.Convites.SingleOrDefaultAsync(c => c.Convidado.Id == perfilId && c.Convidante.Id == idConvidante);

            if (conviteOriginal != null)
            {
                conviteOriginal.Status = "Aceito";

                await _context.SaveChangesAsync();

                return Ok();
            }
            
            return NotFound();
        }

        // GET: api/amigo/5/bloquear/8
        [HttpGet("{perfilId}/bloquear/{idUsuario}")]
        public async Task<IActionResult> Bloquear([FromRoute] int perfilId, [FromRoute] int idUsuario)
        {           
            Convite conviteOriginal = await _context.Convites.SingleOrDefaultAsync(c => (c.Convidado.Id == perfilId && c.Convidante.Id == idUsuario) ||(c.Convidado.Id == idUsuario && c.Convidante.Id == perfilId));

            if (conviteOriginal != null)
            {
                conviteOriginal.Status = "Bloqueado";

                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        // GET: api/Amigo/5/convidar/8
        [HttpGet("{perfilId}/convidar/{idConvidado}")]
        public async Task<IActionResult> Convidar([FromRoute] int perfilId, [FromRoute] int idConvidado)
        {
            bool existente = _context.Convites.Any(c => (c.Convidado.Id == perfilId && c.Convidante.Id == idConvidado) || (c.Convidante.Id == perfilId && c.Convidado.Id == idConvidado));

            if (existente == false)
            {
                Convite convite = new Convite();
                convite.Status = "Aguardando";
                convite.Convidante = await _context.Perfis.FindAsync(perfilId);
                convite.Convidado = await _context.Perfis.FindAsync(idConvidado);

                _context.Convites.Add(convite);
                await _context.SaveChangesAsync();
            }

            return Ok();
        } 
        
    }
}