# Catálogo de treinamentos — instruções do repositório

## Propósito

Este repositório contém a aplicação de catálogo de treinamentos usada no walkthrough do curso GitHub Copilot para Engenharia de Software Moderna.

## Plataforma

Use .NET 10 e C# como plataforma principal. O ambiente de desenvolvimento esperado é o VS Code em GitHub Codespaces.

Considere o ambiente de desenvolvimento como sendo o VS Code em um dev container Ubuntu 24.04.3 LTS, usando as ferramentas já disponíveis no ambiente, como Node.js, npm, ESLint e .NET SDK. Os testes serão executados no próprio container.

Use somente ferramentas e dependências disponíveis ou documentadas no repositório. Não presuma frameworks, pacotes ou serviços externos que ainda não tenham sido aprovados.

## Validação

Antes de concluir uma alteração, identifique e execute os comandos de build, testes e demais validações documentados no repositório. Se os comandos necessários não estiverem documentados, sinalize essa lacuna em vez de inventá-los.

## Especificação do catálogo

Antes de planejar ou alterar qualquer comportamento do catálogo de treinamentos, leia a(s) especificação(ões) relevante(s) em [docs/specs](../docs/specs) para o escopo da solicitação atual. Se a solicitação afetar comportamentos de treinamentos, inscritos ou outros fluxos independentes, consulte a especificação correspondente. Se a solicitação conflitar com qualquer especificação aplicável, sinalize o conflito antes de editar. Novos comportamentos exigem contrato explícito e não podem alterar silenciosamente os critérios aprovados.