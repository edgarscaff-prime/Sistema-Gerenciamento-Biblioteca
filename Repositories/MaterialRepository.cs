using BibliotecaApi.Models;

namespace BibliotecaApi.Repositories;

public class MaterialRepository : IMaterialRepository
{
    private readonly Dictionary<int, MaterialBiblioteca> _materiais = new();

    public MaterialRepository()
    {
        Adicionar(new Livro
        {
            Titulo = "Clean Code",
            AnoPublicacao = 2008,
            Autor = "Robert C. Martin",
            Paginas = 464
        });

        Adicionar(new Revista
        {
            Titulo = "Tecnologia e Inovação",
            AnoPublicacao = 2026,
            Edicao = 15,
            MesPublicacao = 11
        });
    }

    private int GerarNovoId()
    {
        if (_materiais.Count == 0)
            return 1;

        return _materiais.Keys.Max() + 1;
    }

    public IEnumerable<MaterialBiblioteca> ObterTodos()
    {
        return _materiais.Values;
    }

    public MaterialBiblioteca? ObterPorId(int id)
    {
        if (_materiais.ContainsKey(id))
            return _materiais[id];

        return null;
    }

    public MaterialBiblioteca Adicionar(MaterialBiblioteca material)
    {
        material.Id = GerarNovoId();

        _materiais.Add(material.Id, material);

        return material;
    }

    public bool Atualizar(int id, MaterialBiblioteca material)
    {
        if (!_materiais.ContainsKey(id))
            return false;

        material.Id = id;

        _materiais[id] = material;

        return true;
    }

    public bool Remover(int id)
    {
        return _materiais.Remove(id);
    }
}