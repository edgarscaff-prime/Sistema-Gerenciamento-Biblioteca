# Sistema de Gerenciamento de Biblioteca OO

Projeto desenvolvido em C# com ASP.NET Core Web API para gerenciamento de materiais de uma biblioteca.

## Tecnologias utilizadas
- C#
- ASP.NET Core Web API
- Swagger
- Programação Orientada a Objetos

## Estrutura do projeto
- **Models:** classes Livro, Revista, MaterialBiblioteca e interface IEmprestavel.
- **Repositories:** interface IMaterialRepository e implementação MaterialRepository.
- **Controllers:** MateriaisController, responsável pelos endpoints da API.

## Funcionalidades
- Listar materiais cadastrados.
- Buscar material por ID.
- Cadastrar livros.
- Cadastrar revistas.
- Atualizar livros e revistas.
- Remover materiais.
- Consultar descrição de materiais.
- Realizar empréstimos de livros.

## Execução
1. Abra a pasta do projeto no VS Code.
2. Execute `dotnet run` no terminal.
3. Acesse o Swagger pelo endereço apresentado no terminal, por exemplo: `http://localhost:5152/swagger`.

## Armazenamento
Os materiais são armazenados em memória utilizando Dictionary, por meio de um repositório registrado como Singleton.

## Autor
Edgar Scaff