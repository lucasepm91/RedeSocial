using RedeSocial.Domain;
using RedeSocial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedeSocial.Repository
{
    public class BemTeViRepository
    {
        private ServiceRepository repositorio = new ServiceRepository();

        public PerfilViewModel BuscarPerfilLogado(string idLogado)
        {
            PerfilViewModel logado = new PerfilViewModel();

            HttpResponseMessage response = repositorio.GetResponse("api/perfil/logado/" + idLogado);
            if (response.IsSuccessStatusCode == true)
            {
                Perfil cadastrado = response.Content.ReadAsAsync<Perfil>().Result;

                logado.Id = cadastrado.Id;
                logado.Nome = cadastrado.Nome;
                logado.DataNascimento = cadastrado.DataNascimento;
                logado.Email = cadastrado.Email;
                logado.Sobre = cadastrado.Sobre;
                logado.Avatar = cadastrado.Avatar;
            }

            return logado;
        }

        public void AtualizarPerfil(int id, string idIdentity ,PerfilViewModel perfilAtualizado)
        {
            Perfil novoPerfil = new Perfil();

            novoPerfil.Id = id;
            novoPerfil.Nome = perfilAtualizado.Nome;
            novoPerfil.DataNascimento = perfilAtualizado.DataNascimento;
            novoPerfil.Email = perfilAtualizado.Email;
            novoPerfil.Sobre = perfilAtualizado.Sobre;
            novoPerfil.Avatar = perfilAtualizado.Avatar;
            novoPerfil.IdentityUser = new Microsoft.AspNetCore.Identity.IdentityUser();
            novoPerfil.IdentityUser.Id = idIdentity;

            HttpResponseMessage response = repositorio.PutResponse("api/perfil/" + id,novoPerfil);
        }

        public void DeletarPerfil(int id)
        {
            // DELETAR TAMBÉM REGISTRO NA TABELA GERADA PELO IDENTITY
            HttpResponseMessage response = repositorio.DeleteResponse("api/perfil/" + id);
        }

        public List<PerfilViewModel> SugestoesAmizade(int perfilId)
        {            
            List<Perfil> sugestoesPerfil;

            HttpResponseMessage response = repositorio.GetResponse("api/perfil/sugestoes/" + perfilId);
            if (response.IsSuccessStatusCode == true)
            {
                List<PerfilViewModel> sugestoesViewModel = new List<PerfilViewModel>();

                sugestoesPerfil = response.Content.ReadAsAsync<List<Perfil>>().Result;

                foreach (var sugestao in sugestoesPerfil)
                {
                    PerfilViewModel perfil = new PerfilViewModel
                    {
                        Id = sugestao.Id,
                        Nome = sugestao.Nome,
                        Avatar = sugestao.Avatar,
                        Email = sugestao.Email,
                        DataNascimento = sugestao.DataNascimento,
                        Sobre = sugestao.Sobre
                    };

                    sugestoesViewModel.Add(perfil);
                }
                if (sugestoesViewModel.Count == 0)
                    return null;

                return sugestoesViewModel;
            }


            return null;
        }

        public List<MensagemViewModel> CarregarTimeline(int idPerfil, out Dictionary<int, string> pessoas, out Dictionary<int, string> fotos)
        {
            pessoas = new Dictionary<int, string>();
            fotos = new Dictionary<int, string>();
            List<MensagemViewModel> mensagens = new List<MensagemViewModel>();
                       
            HttpResponseMessage response = repositorio.GetResponse("api/mensagem/timeline/" + idPerfil);

            if (response.IsSuccessStatusCode == true)
            {
                List<Mensagem> mensagensBanco = response.Content.ReadAsAsync<List<Mensagem>>().Result;

                foreach (Mensagem m in mensagensBanco)
                {
                    MensagemViewModel m1 = new MensagemViewModel();
                    m1.Id = m.Id;
                    m1.Conteudo = m.Conteudo;
                    m1.PerfilId = m.Autor.Id;
                    mensagens.Add(m1);
                    pessoas.Add(m.Autor.Id, m.Autor.Nome);
                    fotos.Add(m.Autor.Id, m.Autor.Avatar);
                }
                if (mensagens.Count == 0)
                    mensagens = null;
            }
            else
            {
                mensagens = null;
            }
            return mensagens;
        }

        public MensagemViewModel BuscarMensagem(int idMensagem)
        {
            MensagemViewModel mensagem = null;

            HttpResponseMessage response = repositorio.GetResponse("api/mensagem/busca/" + idMensagem);
            if (response.IsSuccessStatusCode)
            {
                Mensagem res = response.Content.ReadAsAsync<Mensagem>().Result;
                mensagem = new MensagemViewModel();

                mensagem.Conteudo = res.Conteudo;
                mensagem.Id = res.Id;
                mensagem.PerfilId = res.Autor.Id;
            }

            return mensagem;
        }

        public bool CriarMensagem(MensagemViewModel mensagem,PerfilViewModel perfil)
        {
            Mensagem completa = new Mensagem();

            completa.Conteudo = mensagem.Conteudo;
            completa.Autor = null;            

            HttpResponseMessage response = repositorio.PostResponse("api/mensagem/" + perfil.Id, completa);
            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

        public void DeletarMensagem(int idMensagem)
        {
            HttpResponseMessage response = repositorio.DeleteResponse("api/mensagem/" + idMensagem);
        }

        public List<PerfilViewModel> BuscarAmigos(int idPerfil)
        {
            List<PerfilViewModel> amigos = null;
            HttpResponseMessage response = repositorio.GetResponse("api/perfil/amizades/" + idPerfil);

            if (response.IsSuccessStatusCode == true)
            {                
                List<Perfil> friends = response.Content.ReadAsAsync<List<Perfil>>().Result;

                if(friends.Count > 0)
                {
                    amigos = new List<PerfilViewModel>();

                    foreach(Perfil perfil in friends)
                    {
                        PerfilViewModel amigo = new PerfilViewModel();

                        amigo.Id = perfil.Id;
                        amigo.Nome = perfil.Nome;
                        amigo.DataNascimento = perfil.DataNascimento;
                        amigo.Email = perfil.Email;
                        amigo.Sobre = perfil.Sobre;
                        amigo.Avatar = perfil.Avatar;

                        amigos.Add(amigo);
                    }
                }
            }

            return amigos;
        }

        public PerfilViewModel BuscarUsuario(int id)
        {
            PerfilViewModel usuario = new PerfilViewModel(); ;
            HttpResponseMessage response = repositorio.GetResponse("api/perfil/" + id);

            if (response.IsSuccessStatusCode == true)
            {
                Perfil perfil = response.Content.ReadAsAsync<Perfil>().Result;

                usuario.Id = perfil.Id;
                usuario.Nome = perfil.Nome;
                usuario.DataNascimento = perfil.DataNascimento;
                usuario.Email = perfil.Email;
                usuario.Sobre = perfil.Sobre;
                usuario.Avatar = perfil.Avatar;
            }

            return usuario;
        }

        public void ConvidarPessoa(int idPerfil, int idPessoa)
        {
            HttpResponseMessage response = repositorio.GetResponse("api/amigo/" + idPerfil + "/convidar/" + idPessoa);
        }

        public void BloquearPessoa(int idPerfil, int idPessoa)
        {
            HttpResponseMessage response = repositorio.GetResponse("api/amigo/" + idPerfil + "/bloquear/" + idPessoa);
        }

        public void AceitarConvite(int idPerfil, int idPessoa)
        {
            HttpResponseMessage response = repositorio.GetResponse("api/amigo/" + idPerfil + "/aceitar/" + idPessoa);
        }

        public List<ConviteViewModel> BuscarConvitesPerfil(int perfilId, out List<ConviteViewModel> convitesParaPerfil)
        {
            List<ConviteViewModel> meusConvites = new List<ConviteViewModel>();

            convitesParaPerfil = new List<ConviteViewModel>();

            HttpResponseMessage response = repositorio.GetResponse("api/amigo/" + perfilId);

            if (response.IsSuccessStatusCode == true)
            {
                List<Convite> convites = response.Content.ReadAsAsync<List<Convite>>().Result;

                foreach(Convite convite in convites)
                {
                    ConviteViewModel con = ConverteConvites(convite);

                    if (con.Convidante.Id == perfilId)
                        meusConvites.Add(con);
                    else
                        convitesParaPerfil.Add(con);
                    
                }
            }

            return meusConvites;
        }

        public List<PerfilViewModel> PesquisaDeUsuarios(int perfilId,string nome)
        {
            List<PerfilViewModel> pessoas = new List<PerfilViewModel>();

            HttpResponseMessage response = repositorio.GetResponse("api/perfil/"+ perfilId +"/pessoas/" + nome);
            if(response.IsSuccessStatusCode)
            {
                List<Perfil> perfis = response.Content.ReadAsAsync<List<Perfil>>().Result;

                foreach(Perfil p in perfis)
                {
                    PerfilViewModel pessoa = new PerfilViewModel();
                    pessoa.Id = p.Id;
                    pessoa.Nome = p.Nome;
                    pessoa.Email = p.Email;
                    pessoa.Sobre = p.Sobre;
                    pessoa.Avatar = p.Avatar;
                    pessoa.DataNascimento = p.DataNascimento;

                    pessoas.Add(pessoa);
                }
            }

            return pessoas;
        }

        private ConviteViewModel ConverteConvites(Convite convite)
        {
            ConviteViewModel con = new ConviteViewModel();
            PerfilViewModel convidado = new PerfilViewModel(), convidante = new PerfilViewModel();

            convidado.Id = convite.Convidado.Id;
            convidado.Nome = convite.Convidado.Nome;
            convidado.Email = convite.Convidado.Email;
            convidado.DataNascimento = convite.Convidado.DataNascimento;
            convidado.Avatar = convite.Convidado.Avatar;
            convidado.Sobre = convite.Convidado.Sobre;

            convidante.Id = convite.Convidante.Id;
            convidante.Nome = convite.Convidante.Nome;
            convidante.Email = convite.Convidante.Email;
            convidante.DataNascimento = convite.Convidante.DataNascimento;
            convidante.Avatar = convite.Convidante.Avatar;
            convidante.Sobre = convite.Convidante.Sobre;

            con.Convidado = convidado;
            con.Convidante = convidante;
            con.Id = convite.Id;
            con.Status = convite.Status;

            return con;
        }
    }
}
