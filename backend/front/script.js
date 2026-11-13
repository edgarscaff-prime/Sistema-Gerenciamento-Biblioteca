const API_URL = "http://localhost:5152/api/Materiais";

const lista = document.querySelector("#listaMateriais");
const mensagem = document.querySelector("#mensagem");
const formLivro = document.querySelector("#formLivro");
const formRevista = document.querySelector("#formRevista");

async function requisicao(caminho, dados) {
    const resposta = await fetch(API_URL + caminho, {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        body: dados ? JSON.stringify(dados) : undefined
    }).catch(function () {
        throw new Error("Não foi possível conectar à API.");
    });

    const texto = await resposta.text();
    if (!resposta.ok) throw new Error(texto || "Erro " + resposta.status);

    try {
        return JSON.parse(texto);
    } catch {
        return texto;
    }
}

function mostrarMensagem(texto, tipo = "success") {
    mensagem.className = "alert alert-" + tipo;
    mensagem.textContent = texto;
}

function descrever(m) {
    const extra = m.autor !== undefined
        ? `Autor: ${m.autor}\nPáginas: ${m.paginas}`
        : `Edição: ${m.edicao}\nMês: ${m.mesPublicacao}`;
    return `ID: ${m.id}\nAno: ${m.anoPublicacao}\n${extra}`;
}

async function carregarMateriais() {
    try {
        const resumo = await requisicao("");
        const materiais = await Promise.all(resumo.map(m => requisicao("/" + m.id)));
        const livros = materiais.filter(m => m.autor !== undefined).length;

        document.querySelector("#totalMateriais").textContent = materiais.length;
        document.querySelector("#totalLivros").textContent = livros;
        document.querySelector("#totalRevistas").textContent = materiais.length - livros;

        lista.replaceChildren();
        if (materiais.length === 0) lista.textContent = "Nenhum material cadastrado.";

        materiais.forEach(function (m) {
            const card = document.querySelector("#modeloCard").content.cloneNode(true);
            card.firstElementChild.dataset.id = m.id;
            card.querySelector(".card-title").textContent = m.titulo;
            card.querySelector(".card-subtitle").textContent = m.autor !== undefined ? "Livro" : "Revista";
            card.querySelector(".card-text").textContent = descrever(m);
            lista.append(card);
        });
    } catch (erro) {
        lista.replaceChildren();
        mostrarMensagem(erro.message, "danger");
    }
}

async function editarMaterial(id) {
    const material = await requisicao("/" + id);
    const form = material.autor !== undefined ? formLivro : formRevista;

    form.reset();
    for (const campo of form.elements) {
        if (campo.name) campo.value = material[campo.name];
    }

    form.dataset.id = id;
    form.querySelector("[type=submit]").textContent = "Salvar alterações";
    form.querySelector("[type=reset]").classList.remove("d-none");
    form.scrollIntoView({ behavior: "smooth" });
}

[formLivro, formRevista].forEach(function (form) {
    const tipo = form === formLivro ? "livro" : "revista";

    form.addEventListener("submit", async function (evento) {
        evento.preventDefault();

        form.classList.add("was-validated");
        if (!form.checkValidity()) {
            mostrarMensagem("Preencha corretamente os campos destacados.", "warning");
            return;
        }

        const dados = Object.fromEntries(new FormData(form));
        const id = form.dataset.id;

        try {
            if (id) {
                await requisicao(`/${tipo}/${id}`, "PUT", dados);
                mostrarMensagem("Material atualizado com sucesso!");
            } else {
                await requisicao("/" + tipo, "POST", dados);
                mostrarMensagem("Material cadastrado com sucesso!");
            }
            form.reset();
            carregarMateriais();
        } catch (erro) {
            mostrarMensagem(erro.message, "danger");
        }
    });

    form.addEventListener("reset", function () {
        delete form.dataset.id;
        form.classList.remove("was-validated");
        form.querySelector("[type=submit]").textContent = "Cadastrar";
        form.querySelector("[type=reset]").classList.add("d-none");
    });
});

lista.addEventListener("click", async function (evento) {
    const acao = evento.target.dataset.acao;
    const card = evento.target.closest("[data-id]");
    if (!card) return;
    const id = card.dataset.id;

    try {
        if (acao === "editar") {
            await editarMaterial(id);
        }
        if (acao === "emprestar") {
            mostrarMensagem(await requisicao(`/${id}/emprestar`, "POST"));
        }
        if (acao === "excluir" && confirm("Deseja excluir este material?")) {
            await requisicao("/" + id, "DELETE");
            mostrarMensagem("Material excluído com sucesso!");
            carregarMateriais();
        }
    } catch (erro) {
        mostrarMensagem(erro.message, "danger");
    }
});

document.querySelector("#modalDetalhes").addEventListener("show.bs.modal", async function (evento) {
    const botao = evento.relatedTarget;
    const card = botao.closest("[data-id]");
    const id = card ? card.dataset.id : document.querySelector("#buscaId").value;
    const corpo = this.querySelector(".modal-body");

    if (!(id > 0)) {
        corpo.textContent = "Informe um ID válido.";
        return;
    }

    corpo.textContent = "Carregando...";
    try {
        if (botao.dataset.acao === "descricao") {
            const info = await requisicao(`/${id}/descricao`);
            corpo.textContent = `Tipo: ${info.tipo}\n${info.descricao}\nPrazo de empréstimo: ${info.prazoEmprestimo} dias`;
        } else {
            const material = await requisicao("/" + id);
            corpo.textContent = `${material.titulo}\n${descrever(material)}`;
        }
    } catch (erro) {
        corpo.textContent = erro.message;
    }
});

carregarMateriais();