# Projeto .NET Core 8 + Vue.js com Microserviços e CQRS

Este repositório contém um projeto baseado em **.NET Core 8** no backend e **Vue.js** no frontend. O projeto utiliza uma arquitetura de micro-serviços implementando o padrão **CQRS** (Command Query Responsibility Segregation) e inclui testes de unidade para garantir a qualidade do código.

## Visão Geral

Este projeto tem como objetivo exemplificar uma aplicação web moderna usando **.NET Core 8** como backend e **Vue.js** como frontend. No backend, estamos aplicando uma arquitetura de micro-serviços, onde cada serviço é responsável por uma funcionalidade específica e implementa o padrão CQRS para separar claramente as responsabilidades de leitura e escrita de dados.

## Tecnologias Utilizadas

### Backend (.NET Core 8)
- **.NET Core 8**: Plataforma principal do backend.
- **CQRS**: Segregação de comandos e consultas para melhor escalabilidade e performance.
- **MediatR**: Biblioteca para facilitar a implementação de CQRS.
- **Entity Framework Core**: ORM para interação com o banco de dados.
- **xUnit**: Framework de testes para testes de unidade no backend.

### Frontend (Vue.js)
- **Vue.js**: Framework JavaScript para construção de interfaces de usuário.
- **Vue Router**: Para gerenciamento de rotas na aplicação.
- **Axios**: Para chamadas HTTP e comunicação com o backend.

## Arquitetura

A arquitetura do projeto segue o padrão de **Microserviços**, onde cada serviço tem uma responsabilidade específica. Para a comunicação entre os serviços e o controle de requisições, utilizamos o padrão **CQRS**, separando as operações de leitura e escrita:

- **Comandos**: Responsáveis por operações que modificam o estado do sistema (e.g., criação de registros, atualizações).
- **Consultas**: Responsáveis por operações que apenas retornam dados, sem modificar o estado do sistema.

Cada micro-serviço implementa suas próprias regras de negócio e tem seu próprio banco de dados.
