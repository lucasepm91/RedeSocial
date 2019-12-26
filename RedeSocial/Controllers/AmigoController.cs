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
    public class AmigoController : Controller
    {
        BemTeViRepository repository = new BemTeViRepository();

        // GET: Amigo
        public ActionResult Index()
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);
            List<PerfilViewModel> amigos = repository.BuscarAmigos(perfil.Id);

            ViewBag.Id = perfil.Id;
            ViewBag.NomePerfil = perfil.Nome;
            ViewBag.AvatarPerfil = perfil.Avatar;

            ViewBag.amigos = amigos;

            return View();
        }

        public ActionResult Convites()
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            List<ConviteViewModel> convitesParaMim;
            List<ConviteViewModel> meusConvites = repository.BuscarConvitesPerfil(perfil.Id, out convitesParaMim);
            
            ViewBag.Id = perfil.Id;
            ViewBag.NomePerfil = perfil.Nome;
            ViewBag.AvatarPerfil = perfil.Avatar;

            ViewBag.meusConvites = meusConvites;
            ViewBag.convitesParaMim = convitesParaMim;

            return View();
        }

        // GET: Amigo/Convidar/8
        public ActionResult Convidar(int id)
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            ViewBag.Id = perfil.Id;
            ViewBag.NomePerfil = perfil.Nome;
            ViewBag.AvatarPerfil = perfil.Avatar;
           
            PerfilViewModel usuario = repository.BuscarUsuario(id);

            return View(usuario);
        }

        // POST: Amigo/Convidar/8
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Convidar(int id , PerfilViewModel amigo)
        {   
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            try
            {
                repository.ConvidarPessoa(perfil.Id, id);
                
                return RedirectToAction("Convites", "Amigo", null);
            }
            catch
            {
                return RedirectToAction("Index", "Perfil", null);
            }
        }

        // GET: Amigo/Aceitar/8
        public ActionResult Aceitar(int id)
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            ViewBag.Id = perfil.Id;
            ViewBag.NomePerfil = perfil.Nome;
            ViewBag.AvatarPerfil = perfil.Avatar;

            PerfilViewModel usuario = repository.BuscarUsuario(id);

            return View(usuario);
        }

        // POST: Amigo/Aceitar/8
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aceitar(int id, PerfilViewModel amigo)
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            try
            {
                repository.AceitarConvite(perfil.Id, id);
                
                return RedirectToAction("Index", "Amigo", null);
            }
            catch
            {
                return RedirectToAction("Index", "Perfil", null);
            }
        }

        // GET: Amigo/Delete/5
        public ActionResult Bloquear(int id)
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            ViewBag.Id = perfil.Id;
            ViewBag.NomePerfil = perfil.Nome;
            ViewBag.AvatarPerfil = perfil.Avatar;

            PerfilViewModel usuario = repository.BuscarUsuario(perfil.Id);

            return View(usuario);            
        }

        // POST: Amigo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bloquear(int id, PerfilViewModel amigo)
        {
            string idLogado = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            PerfilViewModel perfil = repository.BuscarPerfilLogado(idLogado);

            try
            {
                repository.BloquearPessoa(perfil.Id, id);

                return RedirectToAction("Index", "Amigo", null);
            }
            catch
            {
                return RedirectToAction("Index", "Perfil", null);
            }
        }
    }
}