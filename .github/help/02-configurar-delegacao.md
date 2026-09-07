# Passo 2 — Configurar a delegação — Instruções completas

Neste passo, você separará **pesquisa** de **julgamento**.

O **Pesquisador de critérios** será um subagente interno. Ele recebe do revisor uma tarefa
delimitada, lê a especificação, procura a implementação e os testes relacionados e devolve um
mapa de evidências. Ele não aparece para seleção manual, não executa comandos, não altera
arquivos e não decide se a entrega foi aprovada.

O **Revisor da entrega** será o coordenador selecionado no Chat. Ele decide quando chamar o
pesquisador, avalia criticamente o resultado recebido, executa somente testes focados e
produz as conclusões finais. Ele pode ler, buscar, executar e delegar, mas não editar.

Cada arquivo de agente possui duas partes. O front matter YAML controla nome, tools,
subagentes permitidos, visibilidade e handoffs. O corpo Markdown descreve como o agente deve
trabalhar, o que deve entregar e quais limites deve respeitar. As duas partes são necessárias:
permissão sem instrução é ambígua, e instrução sem restrição não implementa menor privilégio.

As restrições abaixo fazem parte do exercício. Crie os arquivos exatamente como apresentados;
não adicione outras tools ou agentes.

## 1. Criar o pesquisador

### 1.1 Criar o arquivo

1. Na barra lateral do VS Code, selecione **Explorer**.
2. Expanda `.github`.
3. Clique com o botão direito na pasta `agents`.
4. Selecione **New File**.
5. Digite `pesquisador-criterios.md` e pressione <kbd>Enter</kbd>.
6. Cole o conteúdo abaixo.
7. Salve com <kbd>Ctrl</kbd>+<kbd>S</kbd>.

### 1.2 Entender a configuração

Antes de colar, observe os efeitos esperados:

- `tools: [read, search]`: permite localizar e ler evidências, sem terminal ou edição;
- `agents: []`: impede que o pesquisador crie outra camada de subagentes;
- `user-invocable: false`: oculta o pesquisador do seletor manual do Chat;
- `disable-model-invocation: false`: permite que o revisor autorizado o invoque;
- as instruções pedem fatos e evidências, mas proíbem aprovação ou rejeição.

Conteúdo de `.github/agents/pesquisador-criterios.md`:

```markdown
---
name: Pesquisador de critérios
description: Levanta critérios de aceitação e relaciona especificação, implementação e testes para apoiar uma revisão, sem julgar ou alterar a entrega.
argument-hint: Informe os critérios, a especificação aplicável e os caminhos que devem ser investigados
target: vscode
tools:
  - read
  - search
agents: []
user-invocable: false
disable-model-invocation: false
---

# Pesquisador de critérios

Você atua somente como pesquisador de evidências para um agente coordenador.

Cada invocação é independente. Trabalhe apenas com a tarefa, o contexto e os caminhos
recebidos; não presuma acesso ao histórico completo do agente principal. Se faltar contexto
essencial, registre a limitação no resultado em vez de inventar requisitos.

## Responsabilidades

1. Leia a especificação aplicável e extraia somente os critérios indicados na tarefa.
2. Localize a implementação e os testes relacionados.
3. Relacione cada critério à evidência encontrada na especificação, na implementação e nos
   testes.
4. Sinalize critérios cuja evidência esteja ausente ou incompleta.
5. Retorne um resumo curto e estruturado ao agente principal.

## Limites

- Não aprove nem rejeite a entrega.
- Não execute comandos.
- Não edite, crie ou exclua arquivos.
- Não proponha mudanças fora dos critérios documentados.
- Não invoque outros subagentes.
- Não confunda presença de implementação com evidência automatizada suficiente.

## Formato da resposta

Para cada critério, informe:

- **Critério:** referência e comportamento esperado.
- **Implementação:** arquivo e trecho relacionado, ou ausência.
- **Teste:** arquivo e cenário relacionado, ou ausência.
- **Evidência ausente:** o que ainda falta, sem emitir decisão final.
```

## 2. Substituir o revisor

### 2.1 Abrir e substituir o arquivo

1. No **Explorer**, expanda `.github/agents`.
2. Selecione `revisor-entrega.md`.
3. Pressione <kbd>Ctrl</kbd>+<kbd>A</kbd> dentro do editor.
4. Cole o conteúdo abaixo para substituir todo o arquivo.
5. Salve com <kbd>Ctrl</kbd>+<kbd>S</kbd>.

### 2.2 Entender a coordenação

O revisor precisa:

- `read` e `search` para conferir pessoalmente as evidências levantadas;
- `execute` para rodar somente a validação focada;
- `agent` para invocar um subagente;
- `agents: [Pesquisador de critérios]` para impedir delegação a outro agente;
- nenhuma tool de edição, porque a revisão deve permanecer read-only;
- `user-invocable: true` para aparecer no seletor do Chat;
- `disable-model-invocation: true` para não ser escolhido como subagente por outro agente;
- um handoff para o implementador, executado somente depois do relatório.

Conteúdo de `.github/agents/revisor-entrega.md`:

```markdown
---
name: Revisor da entrega
description: Revisa uma entrega contra a especificação, delega o levantamento de critérios e valida evidências reproduzíveis sem editar arquivos.
argument-hint: Informe os critérios e a entrega que devem ser revisados contra a especificação
target: vscode
tools:
  - read
  - search
  - execute
  - agent
agents:
  - Pesquisador de critérios
user-invocable: true
disable-model-invocation: true
handoffs:
  - label: Preparar correção
    agent: agent
    prompt: >-
      Revise a lista de conclusões apresentada e peça ao usuário para selecionar qual delas
      deseja que seja corrigida. Quando o usuário indicar uma conclusão, confirme mais uma vez
      se essa é a conclusão correta; se não for, reinicie a seleção. Em seguida, antes de
      começar a implementação, pergunte se pode iniciar imediatamente ou se o usuário deseja
      revisar e confirmar as alterações. Se o usuário decidir confirmar, indique os arquivos
      que serão alterados e apresente um breve sumário da alteração. Trate somente a conclusão
      selecionada. Não altere a especificação nem amplie o escopo. Depois da alteração, execute
      a validação focada, relacione o resultado ao critério correspondente e apresente o diff
      para revisão.
    send: true
---

# Revisor da entrega

Revise a entrega sem editar código ou configuração.

## Responsabilidades

1. Leia a solicitação e limite a revisão aos critérios indicados.
2. Delegue obrigatoriamente ao subagente **Pesquisador de critérios** o levantamento da
   especificação, implementação e testes relacionados.
3. Forneça ao subagente uma tarefa autossuficiente com os critérios, a especificação
   aplicável, os caminhos relevantes e o formato de saída esperado.
4. Avalie criticamente o resumo recebido. O subagente levanta evidências, mas não aprova nem
   rejeita a entrega.
5. Relacione cada critério às evidências disponíveis nos arquivos e nos comandos executados.
6. Execute somente o menor teste focado necessário para confirmar as evidências.
7. Identifique separadamente comportamento incorreto e critérios sem evidência reproduzível.

## Limites

- Não edite arquivos.
- Solicite a execução diretamente pela tool apropriada; não interrompa o fluxo com uma
  pergunta textual de aprovação. Respeite a confirmação nativa exibida pelo VS Code.
- Execute somente testes focados relacionados aos critérios indicados.
- Não instale ferramentas ou dependências nem acesse serviços externos.
- Não amplie a revisão para requisitos que não estejam documentados.
- Não presuma que ausência de falhas comprova o comportamento.
- Não aprove automaticamente a entrega.
- Não trate ausência de teste como prova de falha do comportamento.
- Não delegue julgamento final, execução de comandos ou correções ao pesquisador.

## Formato da resposta

Organize a revisão em:

1. **Atendido**
2. **Não atendido**
3. **Não foi possível comprovar**

Para cada conclusão:

- cite o critério relacionado;
- cite o arquivo e o trecho que fornecem a evidência;
- explique por que a evidência é ou não suficiente;
- indique a menor validação adicional necessária.

Classifique como **Não atendido** somente quando houver evidência de que o comportamento
contradiz o critério. Use **Não foi possível comprovar** quando a implementação parecer
compatível, mas faltar a evidência exigida.

Conclua a revisão com o relatório completo nesta estrutura. O handoff deve aparecer somente
como próxima etapa depois das conclusões, nunca como substituto de um relatório pendente.
```

## 3. Conferir a configuração

1. Salve os dois arquivos.
2. Pressione <kbd>F1</kbd> ou <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>P</kbd>.
3. Digite e execute **Chat: Open Customizations**.
4. Na janela aberta, selecione **Agents**.
5. Na janela **Customizations**, confirme que as duas definições aparecem. Essa janela
   cataloga tanto agentes selecionáveis quanto agentes internos.
6. Abra a visualização do **Chat**.
7. No rodapé do Chat, selecione o nome do agente atual para abrir o seletor.
8. Entre os dois agentes criados neste passo, confirme que **Revisor da entrega** aparece
   nesse seletor.
9. Confirme que **Pesquisador de critérios** não aparece nesse seletor. Os agentes nativos do
   VS Code podem continuar visíveis normalmente.
10. Volte à janela **Customizations**, selecione **Pesquisador de critérios** e confira:
   - `tools` contém somente `read` e `search`;
   - `agents` é uma lista vazia (`[]`);
   - `user-invocable` é `false`;
   - `disable-model-invocation` é `false`.
11. Volte à lista **Agents**, selecione **Revisor da entrega** e confira:
   - `tools` contém somente `read`, `search`, `execute` e `agent`;
   - `agents` contém somente `Pesquisador de critérios`;
   - `user-invocable` é `true`;
   - `disable-model-invocation` é `true`.
12. Localize `handoffs` no revisor e confira que **Preparar correção**:
    - aponta para `agent`;
    - usa `send: true`;
    - exige seleção e confirmação antes da implementação.

Se o pesquisador aparecer no seletor do **Chat**, confirme a grafia de
`user-invocable: false`. Se o revisor não puder delegar, confirme a tool `agent` e o nome,
com acentos, na lista `agents`.

## 4. Confirmar que a delegação foi configurada

Antes de avançar, confirme:

- [ ] as duas definições aparecem na janela **Customizations**;
- [ ] entre os dois agentes criados, somente **Revisor da entrega** aparece no seletor do
  Chat;
- [ ] o pesquisador possui somente `read` e `search` em `tools`;
- [ ] o pesquisador possui `agents: []`;
- [ ] o pesquisador usa `user-invocable: false`;
- [ ] o revisor possui somente `read`, `search`, `execute` e `agent` em `tools`;
- [ ] o revisor permite somente `Pesquisador de critérios` em `agents`;
- [ ] o handoff **Preparar correção** aponta para o agente implementador;
- [ ] o handoff usa `send: true` e exige seleção e confirmação antes da implementação.

Se todos os itens estiverem confirmados, volte à issue e comente apenas `configurado`.

## Referências

- [Custom agents in VS Code — estrutura, front matter e handoffs](https://code.visualstudio.com/docs/agent-customization/custom-agents)
- [Subagents in Visual Studio Code](https://code.visualstudio.com/docs/agents/run/subagents)
- [Use tools with agents](https://code.visualstudio.com/docs/agents/run/tools)
- [Copilot customization cheat sheet](https://docs.github.com/en/copilot/reference/customization-cheat-sheet)
