---
description: Criar ou ajustar um único endpoint da API de treinamentos a partir de um contrato explícito
argument-hint: Informe operação, rota, contrato e validação esperada
agent: agent
---

# Criar Endpoint da API de Treinamentos

Implemente somente um endpoint por invocação, usando as entradas fornecidas abaixo.

## Entradas

- **Operação:** ${input:operation:qual operação HTTP deve ser implementada?}
- **Rota:** ${input:route:qual é a rota do endpoint?}
- **Contrato:** ${input:contract:qual é o contrato de entrada, sucesso, erros e regras de negócio?}
- **Validação:** ${input:validation:quais comandos e cenários devem ser executados para validar a alteração?}

## Regras obrigatórias

- Leia `docs/specs/training-catalog-vertical-slice.md` antes de alterar qualquer comportamento do catálogo.
- Preserve `.github/copilot-instructions.md` e todas as repository instructions aplicáveis.
- Inspecione primeiro o endpoint de criação de treinamentos existente e as abstrações diretamente relacionadas. Use-o como referência de organização, validação, armazenamento, respostas HTTP e testes.
- Verifique as convenções já validadas no projeto, incluindo:
  - .NET 10 e C#;
  - organização atual dos projetos;
  - abstrações existentes para armazenamento;
  - dependências já utilizadas;
  - formato das respostas HTTP;
  - convenções dos testes;
  - comandos de validação documentados.
- Não invente regras de negócio, status HTTP, mensagens, formatos, dependências ou critérios de validação que não estejam no contrato informado ou na especificação aplicável.
- Se o contrato conflitar com a especificação, pare e explique o conflito antes de editar.
- Se uma decisão aberta ou uma informação necessária estiver ausente, peça esclarecimento antes de editar.
- Implemente somente o endpoint indicado nesta invocação.
- Não implemente endpoints relacionados, CRUD adicional, refatorações não necessárias, persistência definitiva, interface ou componentes fora do escopo da entrada.
- Preserve os endpoints, contratos e comportamentos existentes.
- Não fixe uma tecnologia ou biblioteca nova sem justificar sua necessidade e confirmar compatibilidade com as dependências atuais.
- Use a menor abstração necessária para manter o código testável e permitir substituição posterior quando isso já for uma convenção do projeto.

## Fluxo de execução

1. Inspecione a especificação, o endpoint de criação existente, os projetos relacionados, os testes próximos e a configuração de dependências.
2. Confirme se a operação, a rota e o contrato são compatíveis com a especificação.
3. Apresente:
   - a hipótese sobre o ponto de extensão correto;
   - os arquivos que pretende alterar ou criar;
   - o motivo de cada alteração;
   - as dependências que pretende adicionar, se houver;
   - os cenários de validação que serão executados.
4. Aguarde aprovação caso exista uma decisão aberta, conflito ou ambiguidade que altere a estrutura ou o comportamento.
5. Implemente somente o endpoint solicitado, preservando as convenções locais.
6. Adicione ou ajuste testes focados no contrato informado.
7. Execute os comandos e cenários fornecidos em **Validação**. Se não houver comandos documentados para uma validação necessária, sinalize a lacuna em vez de inventar comandos silenciosamente.
8. Relacione cada resultado observado ao contrato:
   - operação e rota;
   - status HTTP;
   - corpo e headers relevantes;
   - regras de validação exercitadas;
   - comportamento de armazenamento, quando aplicável;
   - resultado dos testes.
9. Informe arquivos alterados, dependências adicionadas, validações executadas, resultados e desvios em relação ao contrato ou ao plano.

## Limites

Não edite até ter apresentado os arquivos envolvidos e a justificativa das alterações.

Não continue para outro endpoint na mesma invocação.

Se a validação falhar, corrija somente problemas relacionados a este endpoint. Se a falha revelar um conflito de contrato ou uma decisão não especificada, pare e peça esclarecimento.