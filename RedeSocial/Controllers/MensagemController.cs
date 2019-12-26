using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedeSocial.Models;
using RedeSocial.Repository;

namespace RedeSocial.Controllers
{
    [Authorize]
    public class MensagemController : Controller
    {
        BemTeViRepository repository = new BemTeViRepository();               
        
        // GET: Mensagem/Create
        public ActionResult Create()
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel logado = repository.BuscarPerfilLogado(idLogado);

            ViewBag.Id = logado.Id;
            ViewBag.AvatarPerfil = logado.Avatar;
            ViewBag.NomePerfil = logado.Nome;

            return View();
        }
        
        // POST: Mensagem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] MensagemViewModel mensagem)
        {            
            try
            {                
                if (ModelState.IsValid)
                {
                    string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
                    PerfilViewModel logado = repository.BuscarPerfilLogado(idLogado);

                    mensagem.PerfilId = logado.Id;

                    bool resposta = repository.CriarMensagem(mensagem,logado);
                    if(resposta == true)
                        return RedirectToAction("Timeline", "Perfil", null);
                }

                return RedirectToAction("Index", "Perfil", null);
            }
            catch
            {
                return RedirectToAction("Index", "Perfil", null);
            }
        }       
        
        // GET: Mensagem/Delete/5
        public ActionResult Delete([FromRoute]int id)
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel logado = repository.BuscarPerfilLogado(idLogado);
            MensagemViewModel mensagem;            

            ViewBag.Id = logado.Id;
            ViewBag.AvatarPerfil = logado.Avatar;
            ViewBag.NomePerfil = logado.Nome;
            
            mensagem = repository.BuscarMensagem(id);

            return View(mensagem);
        }

        // POST: Mensagem/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([FromRoute]int id, MensagemViewModel mensagem)
        {            
            try
            {
                repository.DeletarMensagem(id);

                return RedirectToAction("Timeline", "Perfil", null);
            }
            catch
            {
                return RedirectToAction("Index", "Perfil", null);
            }
        }
    }
}