# Passo 3 — Revisar os critérios 8 e 9 — Instruções completas

Neste passo, você verificará o comportamento dos agentes configurados no passo anterior. A
solicitação será curta e natural para testar se o revisor delega por causa de suas próprias
instruções, e não porque o prompt explicou como ele deveria funcionar.

Você inspecionará a execução do subagente, a tarefa autossuficiente que ele recebeu e o resumo
que devolveu. Em seguida, comparará o julgamento do revisor com uma evidência aberta
manualmente no repositório e confirmará que a revisão não alterou arquivos.

## 1. Iniciar a revisão

1. Abra uma conversa nova no Chat.
2. Selecione **Revisor da entrega**.
3. Envie exatamente:

```text
Revise a implementação da listagem de inscritos contra os critérios 8 e 9 de
`docs/specs/training-attendees-vertical-slice.md`.
```

O prompt informa somente o objeto e o limite da revisão. A decisão de delegar, o formato do
relatório, a busca por evidências, a validação focada e os limites de atuação devem vir das
instruções do próprio revisor.

Quando o VS Code mostrar a confirmação nativa do terminal, autorize somente se o comando
estiver restrito a `AttendeeListingTests`. Um comando adequado é:

```bash
dotnet test src/Tests/Api.Tests/TrainingCatalog.Api.Tests.csproj \
  --filter FullyQualifiedName~AttendeeListingTests
```

## 2. Inspecionar a delegação

Expanda a chamada do subagente no Chat e confirme:

- nome **Pesquisador de critérios**;
- critérios 8 e 9 e a especificação aplicável no prompt recebido;
- contexto suficiente para localizar implementação e testes sem depender do histórico do
  Chat principal;
- formato de evidências esperado;
- somente tools de leitura e busca;
- ausência de comandos e edições.

Se não houver delegação, não torne o prompt mais prescritivo. Interrompa a revisão, confira a
responsabilidade de delegação e o campo `agents` do revisor e repita o mesmo prompt em uma
conversa nova.

## 3. Conferir manualmente uma evidência

Abra `src/Tests/Api.Tests/AttendeeListingTests.cs`.

Confirme que existe um teste que:

1. cria um treinamento;
2. cadastra um inscrito;
3. consulta a listagem;
4. verifica o inscrito retornado.

Esse cenário é evidência para o critério 9. Não use a presença do endpoint ou o sucesso geral
da classe como substituto para um cenário não exercitado.

## 4. Interpretar o relatório

O relatório esperado deve concluir:

| Critério | Situação esperada antes da correção | Motivo |
| --- | --- | --- |
| 8 — treinamento existente sem inscritos | **Não foi possível comprovar** | a implementação parece compatível, mas falta o teste funcional exigido para a coleção vazia |
| 9 — treinamento existente com inscritos | **Atendido** | implementação e teste funcional exercitam a coleção com inscrito |

Se o critério 8 aparecer como **Não atendido**, peça ao revisor para identificar a evidência de
comportamento contrário. Ausência de teste, sozinha, não comprova defeito funcional.

## 5. Confirmar ausência de edição

Execute:

```bash
git status --short
```

Antes do próximo passo, somente os dois arquivos de agentes criados no passo 2 podem aparecer.
A revisão não deve adicionar outras mudanças.

## 6. Confirmar que a revisão foi concluída

Antes de avançar, confirme:

- [ ] o revisor invocou **Pesquisador de critérios** sem que o prompt mandasse delegar;
- [ ] o pesquisador usou somente leitura e busca;
- [ ] a tarefa recebida pelo pesquisador tinha contexto suficiente;
- [ ] o revisor fez o julgamento final nas três categorias previstas;
- [ ] o critério 9 foi classificado como **Atendido**;
- [ ] o critério 8 foi classificado como **Não foi possível comprovar**;
- [ ] conferi manualmente no teste a evidência do critério 9;
- [ ] a revisão não alterou arquivos.

Se todos os itens estiverem confirmados, volte à issue e comente apenas `revisado`.

## Referências

- [Subagents in Visual Studio Code — invocação e inspeção](https://code.visualstudio.com/docs/agents/run/subagents)
- [Use tools with agents — chamadas e aprovações](https://code.visualstudio.com/docs/agents/run/tools)
- [`dotnet test` — filtros e execução focada](https://learn.microsoft.com/dotnet/core/tools/dotnet-test)
