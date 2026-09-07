# Passo 3 — Revisar os critérios 8 e 9

Neste passo, você usará **Revisor da entrega** como um usuário real usaria: informará apenas o
objeto da revisão e observará se o agente segue sua configuração persistente. O prompt não
mandará delegar nem antecipará onde procurar as evidências.

O revisor deverá invocar o pesquisador, avaliar o resultado recebido, executar a validação
focada e produzir as conclusões finais. Você inspecionará a chamada do subagente e conferirá
manualmente uma evidência, porque uma resposta plausível do agente não substitui a verificação
humana.

Use a configuração criada para revisar somente a listagem de inscritos:

1. Abra uma conversa nova e selecione **Revisor da entrega**.
2. Solicite a revisão exclusiva dos critérios 8 e 9 da especificação de inscritos.
3. Não diga como o agente deve distribuir o trabalho: observe se ele segue a própria
   configuração e delega o levantamento inicial.
4. Autorize somente o teste focado de `AttendeeListingTests`.
5. Expanda a chamada do subagente e confira:
   - o agente escolhido;
   - a tarefa autossuficiente recebida;
   - as tools disponíveis;
   - o resumo devolvido.
6. Confira manualmente no teste ao menos uma evidência citada.
7. Verifique se o revisor diferencia:
   - critério comprovado;
   - comportamento contrário ao contrato;
   - ausência de evidência suficiente.
8. Confirme que nenhum arquivo foi alterado durante a revisão.

O relatório deve citar critério, implementação, teste e justificativa. Testes passando não
comprovam cenários que eles não exercitam.

> [!TIP]
> Para o prompt copiável, o comando permitido e a interpretação esperada, consulte as
> [instruções completas](https://github.com/{{ repository }}/blob/main/.github/help/03-revisar-criterios.md).

Quando o relatório e uma evidência tiverem sido conferidos, comente `revisado`.
