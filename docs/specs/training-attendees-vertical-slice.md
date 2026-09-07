# Especificação — Fatia vertical de inscritos em treinamento

## Estado

- Status: proposto
- Responsáveis: turma e instrutor
- Última revisão: preencher ao versionar

## Objetivo

Permitir que uma pessoa responsável cadastre inscritos em um treinamento existente, e confirme pela interface que o inscrito foi aceito e aparece na lista de inscritos do treinamento.

## Escopo

- receber os dados de inscrição pela API para um treinamento existente;
- validar nome, sobrenome e e-mail obrigatórios;
- normalizar e-mail com remoção de espaços externos e comparação sem diferença de caixa;
- impedir inscrição duplicada do mesmo e-mail no mesmo treinamento;
- armazenar inscrição válida com identificador gerado pelo sistema;
- permitir consultar os inscritos de um treinamento;
- oferecer interface para cadastrar e listar inscritos;
- produzir evidências automatizadas do comportamento principal.

## Fora do escopo desta fatia

- turmas;
- cadastro global de alunos;
- autenticação e autorização;
- paginação, busca e ordenação avançada;
- edição e exclusão de inscritos;
- notificações, confirmação por e-mail ou integrações externas;
- requisitos de produção, observabilidade e alta disponibilidade.

Operações adicionais podem ser implementadas depois com contratos explícitos, desde que não alterem silenciosamente os comportamentos aprovados aqui.

## Dados do inscrito

| Campo | Tipo | Regra |
| --- | --- | --- |
| `id` | identificador | gerado pelo sistema |
| `firstName` | texto | obrigatório e não vazio |
| `lastName` | texto | obrigatório e não vazio |
| `email` | texto | obrigatório, não vazio, com espaços externos removidos para validação e comparação sem diferença de caixa |
| `trainingId` | identificador | informado na rota da requisição, não no corpo |

## Contrato de cadastro

### Requisição

- Método e rota: `POST /api/trainings/{trainingId}/attendees`
- Corpo: firstName, lastName e email

### Sucesso

- Status: `201 Created`
- Inclui o identificador gerado e a representação do inscrito criado
- Informa a localização do recurso criado

### Falha de validação

- Status: `400 Bad Request`
- Ocorre quando firstName, lastName ou email estiver ausente ou inválido
- Corpo no formato:

  ```json
  {
    "errors": {
      "fieldName": ["Mensagem útil para correção."]
    }
  }
  ```

### Treinamento ausente

- Status: `404 Not Found`
- Ocorre quando o trainingId da rota não corresponde a um treinamento existente

### Conflito de duplicidade

- Status: `409 Conflict`
- Ocorre quando já existe inscrição no mesmo treinamento para o mesmo e-mail após normalização
- Corpo identifica o campo email no formato de erros

## Contrato de listagem

### Requisição

- Método e rota: `GET /api/trainings/{trainingId}/attendees`

### Sucesso

- Status: `200 OK`
- Retorna a coleção de inscritos do treinamento informado

### Treinamento ausente

- Status: `404 Not Found`
- Ocorre quando o trainingId da rota não corresponde a um treinamento existente

## Comportamento da interface

- permitir informar firstName, lastName e email para um treinamento;
- proteger novo envio enquanto a requisição estiver em andamento;
- informar sucesso após confirmação da API;
- atualizar a lista de inscritos do treinamento após cadastro bem-sucedido;
- em caso de erro, apresentar mensagem útil sem apagar os dados preenchidos;
- exibir conflito de duplicidade de forma clara para o campo email.

## Critérios de aceitação

1. Dado firstName ausente, quando o cadastro for enviado, então a API retorna `400` e identifica firstName.
2. Dado lastName ausente, quando o cadastro for enviado, então a API retorna `400` e identifica lastName.
3. Dado email ausente, quando o cadastro for enviado, então a API retorna `400` e identifica email.
4. Dado um trainingId inexistente, quando o cadastro for enviado, então a API retorna `404`.
5. Dados válidos produzem `201`, identificador gerado e representação do inscrito.
6. Dado um e-mail já inscrito no mesmo treinamento, quando novo cadastro equivalente for enviado com diferença apenas de caixa ou espaços externos, então a API retorna `409` e identifica email.
7. Dado o mesmo e-mail em treinamento diferente, quando o cadastro for enviado, então a API aceita a inscrição.
8. Dado um treinamento existente sem inscritos, quando a listagem for consultada, então a API retorna `200` com coleção vazia.
9. Dado um treinamento existente com inscritos, quando a listagem for consultada, então a API retorna `200` com os inscritos daquele treinamento.
10. Pela interface, dados válidos produzem confirmação e o novo inscrito aparece na lista do treinamento.
11. Pela interface, uma falha preserva os dados preenchidos e apresenta mensagem útil.

## Evidências esperadas

| Critério | Evidência mínima |
| --- | --- |
| validação de entrada | resposta HTTP e teste automatizado |
| cadastro válido | resposta `201` e teste automatizado |
| treinamento ausente | resposta `404` e teste automatizado |
| unicidade por treinamento | resposta `409` e teste automatizado confirmando ausência de duplicata |
| normalização de e-mail | teste automatizado cobrindo espaços externos e diferença de caixa |
| listagem por treinamento | resposta `200` e teste automatizado para coleção vazia e não vazia |
| sucesso na interface | fluxo executado no navegador |
| erro na interface | fluxo de falha executado no navegador |
| integração contínua | workflow executando build e testes |

## Decisões ainda abertas

- regra observável para ordenação da lista de inscritos: manter sem garantia de ordem ou definir ordem estável;
- formato mínimo de validação sintática de e-mail além de obrigatório e não vazio;
- conteúdo exato das mensagens de erro por campo, preservando utilidade sem acoplamento excessivo a texto literal.
