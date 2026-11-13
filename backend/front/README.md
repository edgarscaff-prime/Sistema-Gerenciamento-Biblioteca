# Front-end da Biblioteca
## Integrantes

- Samuel Luiz - RGM 114.057
- Yuri Mamada - RGM 697.679
- ...

Trabalho final das disciplinas de Programação Orientada a Objetos e Desenvolvimento Web e Mobile I.
- **`front/`**: página em HTML, CSS, Bootstrap e JavaScript que consome essa API com `fetch`.

## Como rodar

1. Entrar na pasta `backend` e rodar:
   ```bash
   dotnet run
   ```
   A API sobe em `http://localhost:5152` (Swagger em `http://localhost:5152/swagger`).
2. Abrir `front/index.html` com o Live Server do VS Code.


Página que consome a API da biblioteca e permite ver e gerenciar os livros e revistas.

## O que foi desenvolvido

Uma página única onde dá para:

- ver um resumo com o total de materiais, livros e revistas;
- listar todos os materiais em cards (título, ano, tipo, autor/páginas ou edição/mês);
- consultar um material pelo ID;
- ver a descrição e o prazo de empréstimo;
- cadastrar livros e revistas;
- editar um material (o formulário é preenchido com os dados atuais);
- excluir um material (com confirmação);
- tentar emprestar um material.

## Como executar

1. Rodar a API (na pasta `backend`): `dotnet run`
2. Abrir o `index.html` com o Live Server do VS Code.

Se a API estiver em outra porta, é só mudar a constante `API_URL` no começo do `script.js`.

## URL da API

```
http://localhost:5152/api/Materiais
```

## Endpoints consumidos

| Método | Endpoint | Onde é usado |
|---|---|---|
| GET | `/api/Materiais` | carregar a lista |
| GET | `/api/Materiais/{id}` | completar os dados de cada card e o botão "Consultar" / busca por ID |
| GET | `/api/Materiais/{id}/descricao` | botão "Descrição" |
| POST | `/api/Materiais/livro` | formulário de livro |
| POST | `/api/Materiais/revista` | formulário de revista |
| PUT | `/api/Materiais/livro/{id}` | salvar edição de livro |
| PUT | `/api/Materiais/revista/{id}` | salvar edição de revista |
| DELETE | `/api/Materiais/{id}` | botão "Excluir" |
| POST | `/api/Materiais/{id}/emprestar` | botão "Emprestar" |

Observação: o `GET /api/Materiais` só devolve `id`, `titulo` e `anoPublicacao`. Para mostrar o tipo e os outros campos, o front busca cada material pelo ID depois de carregar a lista.

## Organização

```
front/
├── index.html   estrutura da página
├── script.js    toda a lógica e as chamadas à API
└── README.md
```

No `script.js` ficaram poucas funções: `requisicao` (faz o `fetch`), `mostrarMensagem`, `descrever` (monta o texto de um material), `carregarMateriais` (GET e cards) e `editarMaterial` (preenche o formulário). O resto são eventos:

- `submit` dos dois formulários (POST ou PUT, o mesmo código serve para livro e revista);
- `click` na lista de cards (Editar, Emprestar e Excluir);
- `show.bs.modal`, evento do próprio Bootstrap, que preenche o modal de Consultar / Descrição.

Os campos dos formulários têm `name` igual às propriedades da API (`titulo`, `anoPublicacao`, `autor`...), então os dados são lidos com `FormData` e enviados direto.

O card de cada material vem de um `<template>` no HTML, que o JavaScript copia e preenche com `textContent`.

## Componentes do Bootstrap

Navbar, grid (`row` / `col`), cards, formulários com validação, input group, botões, alert e modal. O modal abre sozinho pelos atributos `data-bs-toggle="modal"` e `data-bs-target`, sem código nosso para abrir.

## Validações

Usamos a validação de formulários do Bootstrap: os campos têm `required` e `min`, e no `submit` o JavaScript chama `form.checkValidity()` e adiciona a classe `was-validated`, que mostra as mensagens `invalid-feedback` embaixo de cada campo.

- título e autor preenchidos;
- ano entre 1 e 2100;
- páginas e edição maiores que zero;
- mês selecionado;
- na busca, ID maior que zero.

Se algo estiver errado aparece um alerta amarelo. Os erros que a própria API devolve (por exemplo "Material não encontrado." ou "Este material não permite empréstimo.") aparecem em um alerta vermelho.

## Testes realizados

_(preencher pela equipe com o que vocês testaram e o resultado, por exemplo:)_

- [ ] Listar os materiais iniciais
- [ ] Cadastrar livro e revista
- [ ] Tentar cadastrar com campos vazios
- [ ] Editar livro e revista
- [ ] Excluir material
- [ ] Emprestar livro (sucesso) e revista (erro)
- [ ] Consultar ID que não existe
- [ ] Abrir a página com a API desligada

## Dificuldades e como resolvemos

_(preencher pela equipe)_

## O que cada integrante aprendeu

_(preencher pela equipe)_
