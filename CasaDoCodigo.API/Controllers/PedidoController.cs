﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasaDoCodigo.API.Model;
using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CasaDoCodigo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : BaseApiController
    {
        private readonly IPedidoRepository pedidoRepository;

        public PedidoController(ILogger<CarrinhoController> logger,
            IPedidoRepository pedidoRepository) : base(logger)
        {
            this.pedidoRepository = pedidoRepository;
        }

        /// <summary>
        /// Obtém um pedido.
        /// </summary>
        /// <param name="id">O id do pedido</param>
        /// <returns>Um pedido com o id solicitado</returns>
        /// <response code="404">Pedido não encontrado</response>
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(404)]
        public async Task<PedidoViewModel> Get(int id)
        {
            try
            {
                Pedido pedido = await pedidoRepository.GetPedido();
                PedidoViewModel viewModel = new PedidoViewModel(pedido);
                return viewModel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, "GetPedido");
                throw;
            }
        }
    }
}