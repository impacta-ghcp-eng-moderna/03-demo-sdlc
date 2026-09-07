# Passo 2 — Configurar a delegação

Neste passo, você criará um subagente especializado e transformará o revisor existente em um
coordenador. A pesquisa de critérios e evidências será executada em contexto isolado, enquanto
o julgamento final permanecerá no agente escolhido pelo aluno.

Essa separação permite aplicar menor privilégio: o pesquisador poderá somente ler e buscar; o
revisor poderá também executar testes focados e delegar, mas nenhum dos dois poderá editar.
As capacidades e a disponibilidade de cada agente serão definidas no front matter; suas
responsabilidades e limites serão descritos no corpo Markdown.

Você criará dois papéis diferentes:

- **Pesquisador de critérios:** subagente interno que lê a especificação, encontra
  implementação e testes e devolve fatos, sem executar comandos, editar ou aprovar a entrega.
- **Revisor da entrega:** agente selecionado pelo aluno que delega a pesquisa, executa a
  validação focada e decide quais critérios estão ou não comprovados.

Para configurar esses papéis:

1. Em `.github/agents/`, crie `pesquisador-criterios.md`.
2. Dê ao pesquisador somente `read` e `search`, impeça nova delegação e oculte-o do seletor
   do Chat.
3. Instrua o pesquisador a relacionar cada critério à implementação e aos testes, reportando
   evidências ausentes sem fazer o julgamento final.
4. Abra `.github/agents/revisor-entrega.md`.
5. Dê ao revisor leitura, busca, execução e delegação, sem edição.
6. Permita que ele invoque somente **Pesquisador de critérios**.
7. Instrua o revisor a julgar as evidências nas categorias **Atendido**, **Não atendido** e
   **Não foi possível comprovar**.
8. Adicione o handoff **Preparar correção**, com `send: true`, para iniciar um diálogo que
   selecione e confirme uma conclusão antes de qualquer implementação.
9. Confira na interface que as duas definições existem, mas que, entre elas, somente o revisor
   aparece no seletor de agentes do Chat.

> [!TIP]
> Para o passo a passo clique a clique, os dois arquivos completos e a verificação de cada
> permissão, consulte as
> [instruções completas](https://github.com/{{ repository }}/blob/main/.github/help/02-configurar-delegacao.md).

Quando as restrições e o handoff estiverem conferidos, comente `configurado`.
