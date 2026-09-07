## Problema

Participantes relataram que treinamentos muito longos prejudicam a retenção e dificultam a participação.

## Decisão

Foi aprovada a limitação da duração dos treinamentos a quatro horas.

## Resultado esperado

- Rejeitar a criação ou alteração de treinamentos com `durationHours` maior que 4.
- Preservar a validação existente para valores iguais ou inferiores a zero.
- Retornar uma mensagem útil associada ao campo `durationHours`.
- Atualizar a interface para refletir o novo limite.
- Atualizar a especificação e os testes afetados.

## Evidências

- Build concluída.
- Testes existentes passando.
- Testes automatizados cobrindo o novo limite.
