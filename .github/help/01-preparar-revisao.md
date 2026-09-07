# Passo 1 — Preparar a revisão — Instruções completas

Neste passo, você preparará o ambiente usado durante todo o lab. A criação do Codespace, a
troca para a branch `inicio` e a execução do teste focado estabelecem uma linha de base antes
de qualquer configuração.

Você também reconhecerá onde ficam contrato, implementação, evidências automatizadas e
customizações do Copilot. Essa exploração é intencionalmente read-only: não corrija nem
configure nada ainda.

Siga este roteiro sem alterar arquivos.

## 1. Abrir o ambiente correto

1. Volte ao comentário do passo 1.
2. Clique em **Abrir no GitHub Codespaces**.
3. Confirme o repositório copiado por você e crie o Codespace.
4. Aguarde o terminal terminar a preparação.
5. Execute:

```bash
git switch inicio
git branch --show-current
git status --short
dotnet --version
```

O nome da branch deve ser `inicio`, o status não deve listar alterações e a versão do SDK
deve começar com `10.`.

Ao trocar de branch, o README também muda. Leia o README de `inicio`: ele confirma que você
está no ponto de partida correto e orienta a voltar à issue. Não use novamente o botão de
criação do template.

## 2. Executar a linha de base focada

Na raiz do repositório, execute:

```bash
dotnet test src/Tests/Api.Tests/TrainingCatalog.Api.Tests.csproj \
  --filter FullyQualifiedName~AttendeeListingTests
```

Os testes existentes devem passar. Não conclua apenas por isso que todos os cenários da
especificação possuem evidência.

Se o comando falhar antes de executar testes:

1. confirme que está na raiz do repositório;
2. execute `dotnet restore src/TrainingCatalog.slnx`;
3. repita exatamente o comando focado;
4. se continuar falhando, registre a primeira mensagem de erro e peça ajuda ao instrutor;
5. não altere o código para corrigir uma falha ambiental.

## 3. Localizar o cenário

Abra estes arquivos:

| Caminho | O que localizar |
| --- | --- |
| `docs/specs/training-attendees-vertical-slice.md` | critérios 8 e 9 e evidência esperada para listagem |
| `src/Api/Program.cs` | rota `GET /api/trainings/{trainingId}/attendees` |
| `src/Tests/Api.Tests/AttendeeListingTests.cs` | cenários automatizados existentes |
| `.github/agents/revisor-entrega.md` | responsabilidades, tools e limites atuais |

Não tente resolver ainda qualquer diferença entre critérios e testes.

## 4. Abrir as customizações

1. Pressione <kbd>F1</kbd> ou <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>P</kbd>.
2. Execute **Chat: Open Customizations**.
3. Abra **Agents**.
4. Confirme que **Revisor da entrega** aparece como customização do workspace.

Se o comando não aparecer, atualize o VS Code e as extensões GitHub Copilot e GitHub Copilot
Chat antes de continuar.

## 5. Verificação final

- [ ] estou na branch `inicio`;
- [ ] a árvore de trabalho está limpa;
- [ ] o .NET 10 está disponível;
- [ ] `AttendeeListingTests` passa;
- [ ] localizei critérios, implementação, testes e revisor;
- [ ] abri a área de customizações.

Volte à issue e comente `preparado`.

## Referências

- [Quickstart for GitHub Codespaces](https://docs.github.com/en/codespaces/quickstart)
- [`dotnet test` — .NET CLI](https://learn.microsoft.com/dotnet/core/tools/dotnet-test)
- [Custom agents in VS Code](https://code.visualstudio.com/docs/agent-customization/custom-agents)
