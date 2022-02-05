# Web API de Livros Com Testes e Asp.NET Core MVC#

Este projeto consiste em uma **Web API** que desenvolvi utilizando .NET 5, EntityFramework Core e banco de dados SQL Server e uma aplicação **Asp.NET Core MVC** que irá consumi-lá. O script para a criação dos bancos de dados DB_LIVROS E DB_LIVROS_TESTING e da tabela Livros está na pasta Database).

Esta API é capaz de adicionar novos livros, atualizar livros existentes e realizar consultas por id, título, autor ou todos os livros cadastrados.

Também **desenvolvi para a API**, utilizando xUnit, **testes de integração e testes de unidade** da classe LivrosController, LivroService e da entidade Livro. Neste último utilizei FluentValidation para auxiliar na validação.

Na pasta WebLivros.Core.UnitTests estão presentes os testes de unidade da entidade Livro.

Na pasta WebLivros.Service.UnitTests estão presentes os testes de unidade da classe LivroService.

Na pasta WebLivros.Api.UnitTests estão presentes os testes de unidade da classe LivrosController.

Na pasta WebLivros.Api.IntegrationTests estão presentes os testes de integração da API e as classes de configuração do TestServer.

A **Uri base da API**, utilizada na criação do HttpClient e nas requisições Http, foi definida no **arquivo appsettings.json do projeto WebLivros.Mvc**. Você terá que alterar esta Uri de acordo com seu localhost para utilizar a aplicação MVC em seu computador.

Os **ícones utilizados** nas views podem ser encontrados no seguinte link: https://www.w3schools.com/icons/fontawesome_icons_intro.asp