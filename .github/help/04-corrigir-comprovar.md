# Passo 4 — Corrigir e comprovar — Instruções completas

Neste passo, você usará um handoff para continuar o trabalho no agente implementador sem
perder o contexto da revisão. Como `send: true` está configurado, o handoff inicia o diálogo
automaticamente; isso não representa autorização automática para editar.

O agente deverá oferecer uma escolha de conclusão, pedir confirmação e perguntar se pode
implementar imediatamente ou se você deseja revisar a proposta. Dê preferência às opções
apresentadas pela UI. Os textos deste help existem somente como contingência quando não houver
uma opção equivalente.

Depois de autorizar uma alteração delimitada, você revisará o diff e executará o teste focado.
O objetivo é relacionar a mudança à conclusão escolhida, não corrigir outros itens encontrados
durante a conversa.

## 1. Iniciar o handoff

1. No final do relatório, selecione **Preparar correção**.
2. Confirme que o Chat mudou para o agente implementador.
3. Como o handoff usa `send: true`, confirme que o diálogo começou automaticamente.
4. Quando o agente pedir a conclusão que deve ser corrigida, selecione na UI a opção
   correspondente ao critério 8 em **Não foi possível comprovar**.
5. Use texto somente se a UI não apresentar uma opção equivalente:

```text
Quero tratar a conclusão referente ao critério 8 em "Não foi possível comprovar".
```

6. Quando o agente repetir a conclusão, confira se ela corresponde à opção escolhida e use a
   confirmação oferecida pela UI.
7. Somente se a UI não oferecer uma opção equivalente, responda:

```text
Sim, essa é a conclusão correta.
```

Se o agente repetir uma conclusão diferente, use a opção negativa da UI e reinicie a seleção.
Se essa opção não existir, responda `Não`.

## 2. Confirmar a alteração antes da implementação

Quando o agente perguntar se pode começar imediatamente ou se você deseja revisar e confirmar
as alterações, selecione na UI a opção equivalente a **revisar e confirmar antes da
implementação**.

Somente se a UI não oferecer essa opção, responda:

```text
Quero revisar e confirmar as alterações antes da implementação.
```

Confira se os arquivos e o sumário apresentados são compatíveis com a conclusão realmente
selecionada. Para o cenário esperado deste lab, a proposta deve se limitar ao teste funcional
de listagem, sem alterar especificação ou produção.

## 3. Aprovar somente a alteração necessária

Se a proposta estiver correta, use a opção de confirmação oferecida pela UI.

Somente se a UI não oferecer uma opção equivalente, responda:

```text
Confirmo. Pode implementar somente essa alteração.
```

Se ele propuser alterar `Program.cs`, a especificação, contratos, persistência ou interface,
use a opção da UI para rejeitar ou solicitar ajustes. Somente se a UI não oferecer essa opção,
responda:

```text
Não aprovei esses arquivos. A conclusão informa somente ausência de evidência automatizada.
Restrinja a mudança a AttendeeListingTests.cs e adicione um único teste funcional para o
critério 8.
```

## 4. Conferir a alteração produzida

Para a conclusão esperada neste lab, o teste deve:

1. criar um treinamento existente;
2. não cadastrar inscritos;
3. executar `GET /api/trainings/{trainingId}/attendees`;
4. verificar `200 OK`;
5. desserializar a coleção;
6. verificar que a coleção está vazia.

Se a alteração não corresponder à conclusão selecionada, peça correção pelo diálogo. Se o
agente ainda não conseguir produzir o teste correto e o grupo precisar prosseguir, use este
fallback:

```csharp
[Fact]
public async Task ReturnsEmptyCollectionWhenTrainingHasNoAttendees()
{
	using var factory = new TrainingCatalogApiFactory();
	using var client = factory.CreateClient();
	var training = await CreateTraining(client, "2026-09-14");

	var response = await client.GetAsync($"/api/trainings/{training.Id}/attendees");

	Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	var attendees = await response.Content.ReadFromJsonAsync<Attendee[]>();
	Assert.Empty(attendees!);
}
```

## 5. Revisar o diff

Abra **Source Control** e selecione o arquivo alterado. Como alternativa:

```bash
git diff -- src/Tests/Api.Tests/AttendeeListingTests.cs
git diff -- docs/specs src/Api src/Application src/Infrastructure src/Client
```

O segundo comando não deve produzir saída.

## 6. Executar a validação focada

```bash
dotnet test src/Tests/Api.Tests/TrainingCatalog.Api.Tests.csproj \
  --filter FullyQualifiedName~AttendeeListingTests
```

Todos os testes da classe devem passar, incluindo o cenário novo.

Se houver falha:

1. leia a primeira mensagem de erro;
2. não altere o contrato;
3. confirme que o treinamento foi criado antes do `GET`;
4. confirme que nenhum inscrito foi cadastrado;
5. corrija somente o teste e repita o mesmo comando.

## 7. Confirmar que o critério foi comprovado

Antes de avançar, confirme:

- [ ] `ReturnsEmptyCollectionWhenTrainingHasNoAttendees` executa a API pública;
- [ ] o teste usa um treinamento existente sem inscritos;
- [ ] o resultado esperado é `200 OK` com coleção vazia;
- [ ] `AttendeeListingTests` passou;
- [ ] especificação e código de produção permaneceram inalterados.

Se todos os itens estiverem confirmados, volte à issue e comente apenas `comprovado`.

## Referências

- [Custom agents in VS Code — handoffs e `send`](https://code.visualstudio.com/docs/agent-customization/custom-agents#_handoffs)
- [Use tools with agents — revisão de chamadas e aprovações](https://code.visualstudio.com/docs/agents/run/tools)
- [Source Control in VS Code](https://code.visualstudio.com/docs/sourcecontrol/overview)
- [`dotnet test` — .NET CLI](https://learn.microsoft.com/dotnet/core/tools/dotnet-test)
