using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedeSocial.Data;
using RedeSocial.Domain;

namespace RedeSocial.Controllers
{
    [Route("api/Perfil")]
    [ApiController]
    public class PerfilApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        /*public PerfilApiController(ApplicationDbContext context)
        {
            _context = context;
        }*/
        
        // GET: api/perfil/5
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscaPerfil([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var perfil = await _context.Perfis.FindAsync(id);

            if (perfil == null)
            {
                return NotFound();
            }

            return Ok(perfil);
        }

        // GET: api/perfil/5
        [HttpGet("logado/{id}")]
        public Perfil BuscaLogado([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            var perfil = _context.Perfis.FirstOrDefault(p => p.IdentityUser.Id == id);

            if (perfil == null)
            {
                return null;
            }

            return perfil;
        }

        [HttpGet("sugestoes/{id}")]
        public List<Perfil> SugestoesAmizade([FromRoute] int id)
        {           

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/
            
            List<Convite> convites = _context.Convites.Where(c => c.Convidante.Id != id && c.Convidado.Id != id).Include(c => c.Convidado).Include(c => c.Convidante).Take(5).ToList();
            List<Perfil> sugestoes = new List<Perfil>();

            if (convites.Count == 0)
            {
                List<Perfil> candidatos = new List<Perfil>();
                candidatos = _context.Perfis.Where(p => p.Id != id).Take(10).ToList();

                foreach(Perfil perfil in candidatos)
                {
                    bool existeConvite = _context.Convites.Any(c => (c.Convidado.Id == id && c.Convidante.Id == perfil.Id) || (c.Convidado.Id == perfil.Id && c.Convidante.Id == id));
                    if (existeConvite == false)
                        sugestoes.Add(perfil);
                }
            }
            else
            {
                foreach (Convite convite in convites)
                {
                    if (!sugestoes.Contains(convite.Convidado))
                    {
                        bool existente = _context.Convites.Any(c => (c.Convidado.Id == id && c.Convidante.Id == convite.Convidado.Id) || (c.Convidado.Id == convite.Convidado.Id && c.Convidante.Id == id));
                        if (existente == false)
                            sugestoes.Add(convite.Convidado);
                    }

                    if (!sugestoes.Contains(convite.Convidante))
                    {
                        bool existente = _context.Convites.Any(c => (c.Convidado.Id == id && c.Convidante.Id == convite.Convidante.Id) || (c.Convidado.Id == convite.Convidante.Id && c.Convidante.Id == id));
                        if (existente == false)
                            sugestoes.Add(convite.Convidante);
                    }
                }
            }           

            return sugestoes;
        }

        [HttpGet("amizades/{id}")]
        public IEnumerable<Perfil> MeusAmigos([FromRoute] int id)
        {
            List<Convite> convites = _context.Convites.Where(c => (c.Convidado.Id == id || c.Convidante.Id == id) && c.Status.Contains("Aceito")).Include(c => c.Convidado).Include(c => c.Convidante).ToList();
            List<Perfil> amigos = new List<Perfil>();

            foreach(Convite convite in convites)
            {
                if (!amigos.Contains(convite.Convidado) && convite.Convidado.Id != id)
                    amigos.Add(convite.Convidado);

                if (!amigos.Contains(convite.Convidante) && convite.Convidante.Id != id)
                    amigos.Add(convite.Convidante);
            }

            return amigos;
        }

        [HttpGet("{id}/pessoas/{nome}")]
        public IEnumerable<Perfil> PesquisaUsuarios([FromRoute] int id, [FromRoute] string nome)
        {
            List<Perfil> pessoas = _context.Perfis.Where(p => p.Id != id && p.Nome.ToLower().Contains(nome.ToLower())).ToList();

            if (pessoas == null)
                return new List<Perfil>();

            return pessoas;
        }

        // PUT: api/perfil/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaPerfil([FromRoute] int id, [FromBody] Perfil perfil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var perfilAtual = await _context.Perfis.FindAsync(id);

            if(perfilAtual != null)
            {
                perfilAtual.Nome = perfil.Nome;
                perfilAtual.Email = perfil.Email;
                perfilAtual.DataNascimento = perfil.DataNascimento;
                perfilAtual.Sobre = perfil.Sobre;
                perfilAtual.Avatar = perfil.Avatar;

                _context.SaveChanges();

                return Ok(perfil);
            }

            return NotFound();
           
        }        

        // DELETE: api/perfil/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletaPerfil([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var perfil = await _context.Perfis.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }

            List<Mensagem> mensagens = _context.Mensagens.Where(m => m.Autor.Id == id).ToList();
            List<Convite> convites = _context.Convites.Where(c => c.Convidado.Id == id || c.Convidante.Id == id).ToList();

            foreach(Mensagem m in mensagens)
            {
                _context.Mensagens.Remove(m);
            }

            foreach(Convite c in convites)
            {
                _context.Convites.Remove(c);
            }

            _context.Perfis.Remove(perfil);
            await _context.SaveChangesAsync();

            return Ok(perfil);
        }
        
    }
}