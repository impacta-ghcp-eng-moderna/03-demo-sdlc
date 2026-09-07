# Demo do Módulo 03 — GitHub Copilot no SDLC

Este repositório é o template neutro usado para demonstrar o GitHub Copilot ao longo do ciclo de desenvolvimento de software. Ele contém uma aplicação de catálogo de treinamentos em .NET 10, banco de dados de exemplo, especificações, testes e customizações do Copilot preparadas nos módulos anteriores.

## Criar uma instância para a turma

Crie duas cópias limpas deste template para cada turma, uma para cada parte da demonstração:

1. Na página do template, selecione **Use this template** e **Create a new repository**.
2. Escolha a organização e use um nome que identifique a turma e a parte da demonstração.
3. Mantenha `main` como branch padrão e não selecione a cópia de branches adicionais.
4. Repita o processo para criar a segunda instância a partir do template original.

Cada cópia deve começar no mesmo estado. Não reutilize a instância da primeira parte para a segunda.

## Configurações externas

O conteúdo versionado é copiado pelo template, mas configurações do repositório não são. Revise em cada nova instância, conforme a política da organização:

- permissões do GitHub Actions e criação de issues pelo `GITHUB_TOKEN`;
- rulesets, proteção de branches e checks obrigatórios;
- secrets, variables, environments e configurações de deployment;
- permissões de colaboradores e equipes;
- configurações de GitHub Pages, GitHub Apps e secrets de Codespaces.

Os workflows deste template não dependem de secrets externos nem realizam deploy.

## Preparar o cenário da aula

Depois de criar a cópia:

1. Abra a aba **Actions**.
2. Se necessário, habilite a execução de workflows para o repositório.
3. Selecione **Preparar cenário da aula** e execute **Run workflow** na branch `main`.
4. Confirme que a issue **Limitar a duração dos treinamentos** foi criada.

O workflow é idempotente: execuções posteriores reconhecem a issue existente, aberta ou fechada, e não criam duplicatas.

## Desenvolvimento local

Com o .NET 10 SDK instalado, execute na raiz do repositório:

```bash
dotnet restore src/TrainingCatalog.slnx
dotnet build src/TrainingCatalog.slnx --no-restore
dotnet test src/TrainingCatalog.slnx --no-build --no-restore
```

As especificações funcionais estão em [`docs/specs`](docs/specs). A API e a interface estão nos projetos `src/Api` e `src/Client`, respectivamente.
