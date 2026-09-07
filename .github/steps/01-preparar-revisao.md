# Passo 1 — Preparar a revisão

Você partirá da aplicação concluída no Lab 01. Antes de configurar novos agentes, confirme a
linha de base e localize os contratos e evidências que serão revisados.

Neste passo, você abrirá o Codespace, mudará para a branch `inicio` e executará somente os
testes relacionados à listagem de inscritos. Essa linha de base permitirá distinguir um
problema introduzido durante o lab de um comportamento que já existia.

Você também localizará a especificação, a implementação, os testes e o agente revisor. Ainda
não haverá configuração nem correção: o objetivo é reconhecer o material que será usado nos
próximos passos e confirmar que o ambiente está pronto.

[![Abrir no GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/{{ repository }}?quickstart=1)

1. Abra o Codespace pelo botão acima.
2. Aguarde a criação e a preparação do ambiente terminarem.
3. No terminal, selecione a branch `inicio`.
4. Leia o README exibido nessa branch e volte à issue para continuar o roteiro.
5. Confirme que o .NET 10 está disponível.
6. Execute somente os testes funcionais de listagem de inscritos.
7. Localize:
   - os critérios 8 e 9 da especificação de inscritos;
   - a implementação da rota de listagem;
   - os testes de listagem;
   - o agente `Revisor da entrega`.
8. Abra **Chat: Open Customizations** e encontre a seção **Agents**.

Não altere arquivos neste passo. O teste focado deve passar, mas isso ainda não comprova que
todos os cenários exigidos pela especificação foram exercitados.

> [!TIP]
> Para os comandos exatos, caminhos e resultados esperados, consulte as
> [instruções completas](https://github.com/{{ repository }}/blob/main/.github/help/01-preparar-revisao.md).

Quando a linha de base estiver confirmada, comente `preparado`.
