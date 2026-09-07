# Especificação — Primeira fatia vertical do catálogo de treinamentos

## Estado

- Status: aprovado
- Responsáveis: turma e instrutor
- Última revisão: preencher ao versionar

## Objetivo

Permitir que uma pessoa responsável cadastre um treinamento interno e confirme, pela interface, que o novo item foi aceito e incluído no catálogo.

## Escopo

- receber os dados de um treinamento pela API;
- rejeitar dados obrigatórios ausentes ou inválidos;
- armazenar um treinamento válido;
- permitir consultar os itens cadastrados;
- oferecer uma interface para cadastrar e visualizar o novo item;
- produzir evidências automatizadas do comportamento principal.

## Fora do escopo desta fatia

- autenticação e autorização;
- paginação, busca e ordenação;
- regras de capacidade ou inscrição;
- edição e exclusão na interface;
- escolha definitiva do provedor de banco de dados;
- requisitos de produção, observabilidade e alta disponibilidade.

Operações adicionais de API podem ser implementadas depois com contratos explícitos, desde que não alterem silenciosamente os comportamentos aprovados aqui.

## Dados do treinamento

| Campo | Tipo | Regra |
| --- | --- | --- |
| `id` | identificador | gerado pelo sistema |
| `title` | texto | obrigatório e não vazio |
| `description` | texto | obrigatório e não vazio |
| `startDate` | data no formato `YYYY-MM-DD` | obrigatória e exclusiva no catálogo |
| `durationHours` | inteiro | obrigatório e maior que zero |

## Contrato da API para criação

### Requisição

- Método e rota: `POST /api/trainings`
- Corpo: título, descrição, data de início e carga horária

### Sucesso

- Status: `201 Created`
- Inclui o identificador gerado e a representação do treinamento
- Informa a localização do recurso criado

### Falha de validação

- Status: `400 Bad Request`
- Corpo no formato:

  ```json
  {
    "errors": {
      "fieldName": ["Mensagem útil para correção."]
    }
  }
  ```

### Conflito

- Status: `409 Conflict`
- Ocorre quando já existe um treinamento com a mesma `startDate`
- Corpo no formato:

  ```json
  {
    "errors": {
      "startDate": ["Já existe um treinamento com esta data de início."]
    }
  }
  ```

## Comportamento da interface

- desabilitar ou proteger novo envio enquanto a requisição estiver em andamento;
- informar sucesso depois da confirmação da API;
- atualizar a lista com o item criado;
- em caso de erro, apresentar mensagem útil sem apagar os dados preenchidos.

## Critérios de aceitação

1. Dado um título ausente, quando o cadastro for enviado, então a API retorna `400` e identifica o campo `title`.
2. Dada uma descrição ausente, quando o cadastro for enviado, então a API retorna `400` e identifica o campo `description`.
3. Dada uma data de início ausente, quando o cadastro for enviado, então a API retorna `400` e identifica o campo `startDate`.
4. Dada uma carga horária igual ou inferior a zero, quando o cadastro for enviado, então a API retorna `400` e identifica o campo `durationHours`.
5. Dados válidos produzem `201`, um identificador e um recurso consultável depois da criação.
6. Pela interface, dados válidos produzem confirmação e o novo item aparece na lista.
7. Pela interface, uma falha preserva os dados preenchidos e apresenta mensagem útil.
8. Dado um treinamento já cadastrado para uma data de início, quando outro treinamento for enviado com a mesma `startDate`, então a API retorna `409` e identifica o campo `startDate`, sem armazenar o segundo treinamento.

## Evidências esperadas

| Critério | Evidência mínima |
| --- | --- |
| validação de entrada | resposta HTTP e teste automatizado |
| criação válida | resposta `201` e teste automatizado |
| exclusividade da data | resposta `409` e teste automatizado confirmando que o segundo item não foi armazenado |
| armazenamento | consulta bem-sucedida após reiniciar a API |
| sucesso na interface | fluxo executado no navegador |
| erro na interface | fluxo de falha executado no navegador |
| integração contínua | workflow executando build e testes |

## Decisões ainda abertas

- provedor e configuração do banco de dados;
- organização interna dos projetos, desde que preserve os contratos;
- detalhes visuais da interface;
- estratégia adicional de testes além da evidência mínima.

Decisões abertas devem ser resolvidas antes da etapa que depende delas e registradas neste documento quando alterarem o comportamento esperado.