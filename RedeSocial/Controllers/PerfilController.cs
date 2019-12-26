using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedeSocial.Models;
using RedeSocial.Repository;

namespace RedeSocial.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        BemTeViRepository repository = new BemTeViRepository();

        // GET: Perfil
        public ActionResult Index()
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel logado = repository.BuscarPerfilLogado(idLogado);
            List<PerfilViewModel> sugestoes;

            ViewBag.Id = logado.Id;
            ViewBag.NomePerfil = logado.Nome;
            ViewBag.AvatarPerfil = logado.Avatar;

            sugestoes = repository.SugestoesAmizade(logado.Id);

            ViewBag.sugestoes = sugestoes;

            return View();
        }

        // GET: Perfil/Details/5
        public ActionResult Details(int id)
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            return View(perfil);
        }

        // GET: Perfil/Edit/5
        public ActionResult Edit(int id)
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            return View(perfil);
        }

        // POST: Perfil/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind] PerfilViewModel perfilNovo)
        {
            try
            {
                string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
                repository.AtualizarPerfil(id, idLogado, perfilNovo);

                return RedirectToAction("Details", "Perfil", id); ;
            }
            catch
            {
                return RedirectToAction("Index", "Perfil", null);
            }
        }

        // GET: Perfil/Delete/5
        public ActionResult Delete(int id)
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            return View(perfil);
        }

        // POST: Perfil/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PerfilViewModel perfil)
        {
            try
            {

                HttpContext.SignOutAsync();
                foreach (var cookieKey in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookieKey);
                }

                repository.DeletarPerfil(id);
                return RedirectToAction("Index", "Home", null); ;
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Timeline()
        {
            // Pegar as mensagens do usuário e amigos em ordem decrescente
            // Pegar na mesma consulta o nome e o id dos amigos

            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            ViewBag.Id = perfil.Id;
            ViewBag.NomePerfil = perfil.Nome;
            ViewBag.AvatarPerfil = perfil.Avatar;

            Dictionary<int, string> pessoas;
            Dictionary<int, string> fotos;
            List<MensagemViewModel> mensagens = repository.CarregarTimeline(perfil.Id, out pessoas, out fotos);

            ViewBag.mensagens = mensagens;
            ViewBag.usuarios = pessoas;
            ViewBag.fotos = fotos;

            return View();
        }

        public ActionResult Pesquisa()
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            ViewBag.Id = perfil.Id;
            ViewBag.NomePerfil = perfil.Nome;
            ViewBag.AvatarPerfil = perfil.Avatar;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pesquisa(string nomePessoa)
        {
            try
            {
                string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
                PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

                return RedirectToAction("Pessoas", "Perfil", new { nome = nomePessoa });
            }
            catch
            {
                return RedirectToAction("Index", "Perfil", null);
            }

        }

        [Route("/perfil/pessoas/{nome}")]
        public ActionResult Pessoas(string nome)
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            ViewBag.Id = perfil.Id;
            ViewBag.NomePerfil = perfil.Nome;
            ViewBag.AvatarPerfil = perfil.Avatar;

            List<PerfilViewModel> pessoas = repository.PesquisaDeUsuarios(perfil.Id, nome);

            if (pessoas.Count > 0)
                ViewBag.encontradas = pessoas;
            else
                ViewBag.encontradas = null;

            return View();
        }
    }
}