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
    [Route("api/Mensagem")]
    [ApiController]
    public class MensagemApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        /*public MensagemApiController(ApplicationDbContext context)
        {
            _context = context;
        }*/

        // GET: api/mensagem/busca/5
        [HttpGet("busca/{id}")]
        public async Task<IActionResult> BuscaMensagem([FromRoute] int id)
        {
            var listaMensagem = _context.Mensagens.Where(m => m.Id == id).Include(m => m.Autor);

            if (listaMensagem == null)
                return NotFound();

            var mensagem = await listaMensagem.FirstOrDefaultAsync();

            return Ok(mensagem);
        }

        // GET: api/mensagem/timeline/5
        [HttpGet("timeline/{id}")]
        public IEnumerable<Mensagem> Timeline([FromRoute] int id)
        {
            List<Convite> convites = _context.Convites.Where(c => (c.Convidado.Id == id || c.Convidante.Id == id) && c.Status.Contains("Aceito")).Include(c => c.Convidado).Include(c => c.Convidante).ToList();
            List<Perfil> perfis = new List<Perfil>();
            List<Mensagem> mensagens = new List<Mensagem>();
            List<Mensagem> mensagensDoPerfil;

            perfis = _context.Perfis.Where(p => p.Id == id).ToList();

            foreach (Convite convite in convites)
            {
                if (!perfis.Contains(convite.Convidado))
                    perfis.Add(convite.Convidado);

                if (!perfis.Contains(convite.Convidante))
                    perfis.Add(convite.Convidante);
            }

            foreach (Perfil perfil in perfis)
            {
                mensagensDoPerfil = _context.Mensagens.Where(m => m.Autor.Id == perfil.Id).Include(m => m.Autor).ToList();
                mensagens = mensagens.Concat(mensagensDoPerfil).ToList();
            }

            mensagens = mensagens.OrderBy(m => m.Id).ToList();

            return mensagens;
        }
         
        // POST: api/Mensagem/5
        [HttpPost("{idPerfil}")]
        public async Task<IActionResult> CriarMensagem([FromRoute] int idPerfil,[FromBody] Mensagem mensagem)
        {
            Perfil autor = await _context.Perfis.FindAsync(idPerfil);

            mensagem.Autor = autor;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Mensagens.Add(mensagem);
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        // DELETE: api/Mensagem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarMensagem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mensagem = await _context.Mensagens.FindAsync(id);
            if (mensagem == null)
            {
                return NotFound();
            }

            _context.Mensagens.Remove(mensagem);
            await _context.SaveChangesAsync();

            return Ok(mensagem);
        }
        
    }
}