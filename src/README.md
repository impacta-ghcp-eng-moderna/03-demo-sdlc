## Estrutura inicial da solução

A base está organizada em projetos separados para manter as responsabilidades isoladas
durante os próximos incrementos:

- `Api`: host ASP.NET Core da API.
- `Client`: host da interface Blazor WebAssembly.
- `Application`: contratos e casos de uso, a serem adicionados em incremento posterior.
- `Infrastructure`: ponto de extensão para persistência, a ser adicionado em incremento posterior.
- `Tests/Api.Tests`: testes da API.

Nesta etapa os projetos são somente a base da solução. Não há endpoints, persistência,
modelos ou componentes de negócio.

## Validação

Com o .NET 10 SDK instalado, execute na raiz do repositório:

```bash
dotnet restore src/TrainingCatalog.slnx
dotnet build src/TrainingCatalog.slnx --no-restore
dotnet test src/TrainingCatalog.slnx --no-restore
```
