# Passo 4 — Corrigir e comprovar

Neste passo, você passará da revisão para a implementação por meio de um handoff. O agente
implementador receberá o contexto anterior e iniciará um diálogo para escolher uma única
conclusão, confirmar a escolha e definir se a alteração pode começar imediatamente.

Você escolherá revisar a proposta antes da implementação. Depois, conferirá o diff e executará
o teste focado para demonstrar que a conclusão escolhida foi tratada sem alterar a
especificação ou o código de produção.

Use o relatório para tratar somente a conclusão do cenário vazio:

1. Selecione o handoff **Preparar correção**.
2. Confirme que o Chat mudou para o agente implementador e iniciou o diálogo automaticamente.
3. Quando solicitado, use a opção da UI equivalente à conclusão do critério 8 em
   **Não foi possível comprovar**.
4. Confirme novamente essa escolha.
5. Escolha revisar e confirmar as alterações antes da implementação.
6. Aceite somente uma proposta coerente com a conclusão escolhida.
7. Autorize a inclusão de um único teste funcional para treinamento existente sem inscritos.
8. Revise o diff no **Source Control**.
9. Confirme que especificação e código de produção não mudaram.
10. Execute novamente somente `AttendeeListingTests`.
11. Relacione o teste novo ao critério 8 e registre a decisão final.

Não corrija outros itens, não altere o contrato e não transforme a falta de evidência em uma
mudança desnecessária de produção.

> [!TIP]
> Para alternativas quando a UI não apresentar uma opção equivalente, além do teste esperado,
> comandos e recuperação, consulte as
> [instruções completas](https://github.com/{{ repository }}/blob/main/.github/help/04-corrigir-comprovar.md).

Quando o diff estiver restrito e o teste focado passar, comente `comprovado`.
