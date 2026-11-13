using Microsoft.AspNetCore.Mvc;
using BibliotecaApi.Models;
using BibliotecaApi.Repositories;

namespace BibliotecaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MateriaisController : ControllerBase
{
    private readonly IMaterialRepository _repository;

    public MateriaisController(IMaterialRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult ObterTodos()
    {
        return Ok(_repository.ObterTodos());
    }

    [HttpGet("{id}")]
    public IActionResult ObterPorId(int id)
    {
        var material = _repository.ObterPorId(id);

        if (material == null)
            return NotFound("Material não encontrado.");

        return Ok(material);
    }

    [HttpPost("livro")]
    public IActionResult CadastrarLivro([FromBody] Livro livro)
    {
        if (string.IsNullOrWhiteSpace(livro.Titulo))
            return BadRequest("O título é obrigatório.");

        if (livro.AnoPublicacao < 0)
            return BadRequest("O ano de publicação não pode ser negativo.");

        if (string.IsNullOrWhiteSpace(livro.Autor))
            return BadRequest("O autor é obrigatório.");

        if (livro.Paginas <= 0)
            return BadRequest("O número de páginas deve ser maior que zero.");

        var novoLivro = _repository.Adicionar(livro);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = novoLivro.Id },
            novoLivro
        );
    }

    [HttpPost("revista")]
    public IActionResult CadastrarRevista([FromBody] Revista revista)
    {
        if (string.IsNullOrWhiteSpace(revista.Titulo))
            return BadRequest("O título é obrigatório.");

        if (revista.AnoPublicacao < 0)
            return BadRequest("O ano de publicação não pode ser negativo.");

        if (revista.Edicao <= 0)
            return BadRequest("A edição deve ser maior que zero.");

        if (revista.MesPublicacao < 1 || revista.MesPublicacao > 12)
            return BadRequest("O mês deve estar entre 1 e 12.");

        var novaRevista = _repository.Adicionar(revista);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = novaRevista.Id },
            novaRevista
        );
    }

    [HttpPut("livro/{id}")]
    public IActionResult AtualizarLivro(int id, [FromBody] Livro livro)
    {
        var existente = _repository.ObterPorId(id);

        if (existente == null)
            return NotFound("Material não encontrado.");

        if (existente is not Livro)
            return BadRequest("O material informado não é um livro.");

        if (string.IsNullOrWhiteSpace(livro.Titulo))
            return BadRequest("O título é obrigatório.");

        if (livro.AnoPublicacao < 0)
            return BadRequest("O ano de publicação não pode ser negativo.");

        if (string.IsNullOrWhiteSpace(livro.Autor))
            return BadRequest("O autor é obrigatório.");

        if (livro.Paginas <= 0)
            return BadRequest("O número de páginas deve ser maior que zero.");

        _repository.Atualizar(id, livro);

        return Ok(livro);
    }

    [HttpPut("revista/{id}")]
    public IActionResult AtualizarRevista(int id, [FromBody] Revista revista)
    {
        var existente = _repository.ObterPorId(id);

        if (existente == null)
            return NotFound("Material não encontrado.");

        if (existente is not Revista)
            return BadRequest("O material informado não é uma revista.");

        if (string.IsNullOrWhiteSpace(revista.Titulo))
            return BadRequest("O título é obrigatório.");

        if (revista.AnoPublicacao < 0)
            return BadRequest("O ano de publicação não pode ser negativo.");

        if (revista.Edicao <= 0)
            return BadRequest("A edição deve ser maior que zero.");

        if (revista.MesPublicacao < 1 || revista.MesPublicacao > 12)
            return BadRequest("O mês deve estar entre 1 e 12.");

        _repository.Atualizar(id, revista);

        return Ok(revista);
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        if (!_repository.Remover(id))
            return NotFound("Material não encontrado.");

        return NoContent();
    }

    [HttpGet("{id}/descricao")]
    public IActionResult ObterDescricao(int id)
    {
        var material = _repository.ObterPorId(id);

        if (material == null)
            return NotFound("Material não encontrado.");

        return Ok(new
        {
            material.Id,
            Tipo = material.GetType().Name,
            Descricao = material.Descricao(),
            PrazoEmprestimo = material.CalcularPrazo()
        });
    }

    [HttpPost("{id}/emprestar")]
    public IActionResult Emprestar(int id)
    {
        var material = _repository.ObterPorId(id);

        if (material == null)
            return NotFound("Material não encontrado.");

        if (material is not IEmprestavel emprestimo)
            return BadRequest("Este material não permite empréstimo.");

        return Ok(emprestimo.Emprestar());
    }
}