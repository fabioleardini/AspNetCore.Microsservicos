﻿using CasaDoCodigo.Client.Generated;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasaDoCodigo.Client.Playground
{
    class Program
    {

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Iniciando...");
            var relatorio = new Relatorio();
            await relatorio.Executar();
            Console.WriteLine("Tecle algo para sair...");
            Console.ReadKey();
        }

    }

    class Relatorio
    {
        private const string API_BASE_URL = "https://localhost:44339";
        private const int DELAY_SEGUNDOS = 5;
        private Generated.Client cliente;

        public async Task Executar()
        {
            await ListarProdutos();
        }

        private async Task ListarProdutos()
        {
            ImprimirLogo();
            cliente = new Generated.Client(API_BASE_URL);
            ImprimirListagem(await ObterProdutos());
        }

        private void ImprimirLogo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
@"
 .d8888b.                                 888            .d8888b.              888d8b                
d88P  Y88b                                888           d88P  Y88b             888Y8P                
888    888                                888           888    888             888                   
888        8888b. .d8888b  8888b.     .d88888 .d88b.    888        .d88b.  .d88888888 .d88b.  .d88b. 
888           '88b88K         '88b   d88' 888d88''88b   888       d88''88bd88' 888888d88P'88bd88''88b
888    888.d888888'Y8888b..d888888   888  888888  888   888    888888  888888  888888888  888888  888
Y88b  d88P888  888     X88888  888   Y88b 888Y88..88P   Y88b  d88PY88..88PY88b 888888Y88b 888Y88..88P
 'Y8888P' 'Y888888 88888P''Y888888    'Y88888 'Y88P'     'Y8888P'  'Y88P'  'Y88888888 'Y88888 'Y88P' 
                                                                                      'Y88P' 
");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private async Task<IList<Produto>> ObterProdutos()
        {
            IList<Produto> produtos = null;
            var sucesso = false;
            while (!sucesso)
            {
                try
                {
                    Console.WriteLine("Obtendo produtos...");
                    produtos = await cliente.ApiProdutoGetAsync();
                    sucesso = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro ao acessar {API_BASE_URL}:\r\n" +
                        $"{ex.Message}\r\n" +
                        $"tentando novamente em 5s");
                    await Task.Delay(TimeSpan.FromSeconds(DELAY_SEGUNDOS));
                }
            }

            return produtos;
        }
        private void ImprimirListagem(IList<Produto> produtos)
        {
            Console.WriteLine(new string('=', 50));
            foreach (var produto in produtos)
            {
                Console.WriteLine(
                    $"Id: {produto.Id}\r\n" +
                    $"Codigo:{produto.Codigo}\r\n" +
                    $"Nome:{produto.Nome}\r\n" +
                    $"Preco:{produto.Preco:C}");
                Console.WriteLine(new string('=', 50));
            }
        }


    }
}