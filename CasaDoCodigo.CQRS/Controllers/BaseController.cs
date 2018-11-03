﻿using CasaDoCodigo.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IHttpContextAccessor contextAccessor;
        protected readonly ILogger logger;

        protected BaseController(IHttpContextAccessor contextAccessor, ILogger logger)
        {
            this.contextAccessor = contextAccessor;
            this.logger = logger;
        }

        protected void HandleBrokenCircuitException(IService service)
        {
            ViewBag.MsgServicoIndisponivel = $"O serviço '{service.Scope}' não está ativo, por favor tente novamente mais tarde.";
        }

        protected void HandleException()
        {
            ViewBag.MsgServicoIndisponivel = $"O serviço está indisponível no momento, por favor tente novamente mais tarde.";
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        protected int? GetPedidoId()
        {
            return contextAccessor.HttpContext.Session.GetInt32("pedidoId");
        }

        protected void SetPedidoId(int pedidoId)
        {
            contextAccessor.HttpContext.Session.SetInt32("pedidoId", pedidoId);
        }

        protected string GetUserId()
        {
            var claims = User.Claims;
            var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            if (userIdClaim == null)
            {
                userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            }
            if (userIdClaim == null)
            {
                throw new Exception("Usuário desconhecido");
            }

            var idUsuario = userIdClaim.Value;
            return idUsuario;
        }

    }
}