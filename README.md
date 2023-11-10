# ALWABP-Solver


<img src="https://github.com/caio-de-oliveira-lopes/ALWABP-Solver/blob/main/ALWABP/Assignment-example-of-workers-and-tasks-for-ALWABP.png" alt="ALWABP">

> Este projeto implementa as heurísticas construtivas apresentadas em:</br></br>
Moreira, Mayron & Ritt, Marcus & Costa, Alysson & Chaves, Antonio. (2010).</br>
Simple heuristics for the assembly line worker assignment and balancing problem.</br>
Computing Research Repository - CORR. 18. 10.1007/s10732-012-9195-5. </br>

## 💻 Pré-requisitos

Antes de começar, verifique se você atendeu aos seguintes requisitos:

* Você instalou a versão mais recente de .NET 6.0.
* Recomenda-se uma breve leitura do artigo de referência para melhor entendimento das heurísticas construtivas.

## ☕ Usando ALWABP-Solver

Para usar ALWABP-Solver, siga estas etapas:

```
Parâmetros necessários são:
<Caminho dos arquivos de input> <Caminho onde serão escritos os outputs>

Exemplo:
C:\Projetos\ALWABP-Solver\ALWABP\Instances\alwabp C:\Projetos\ALWABP-Solver\ALWABP\Outputs

Podem ser usadas aspas caso existam espaços e afins:
"C:\Projetos\ALWABP-Solver\ALWABP\Instances\alwabp" "C:\Projetos\ALWABP-Solver\ALWABP\Outputs"
```

O projeto pode ser publicado e chamado através do executável, ou pela IDE.</br>
O código irá gerar todas as combinações de heurísticas e, para cada input, um arquivo de output com todas as soluções será escrito, detalhando qual combinação levou a cada solução (a melhor solução é escrita primeiro).</br>
Exemplo de escrita de uma solução no arquivo de output:</br>

```
{
  "Id": 27,
  "MaxCycleTime": 99,
  "ExecutionTimeMs": 4,
  "Feasible": true,
  "WorkstationsCicleTimes": {
    "0": 99,
    "1": 93,
    "2": 97,
    "3": 80
  },
  "WorkerTasks": {
    "0": "[4, 26, 20, 22, 25, 3, 17, 14, 15, 21, 6, 23]",
    "1": "[24, 10, 13, 18, 11, 2, 16]",
    "2": "[0, 7, 19, 1]",
    "3": "[9, 12, 8, 5, 27]"
  },
  "AssignedWorkers": {
    "0": 0,
    "1": 1,
    "2": 2,
    "3": 3
  },
  "GraphDirection": "Forward",
  "WorkerRuleCriteria": "MinBWA",
  "TaskRuleCriteria": "MinD",
  "TaskRuleSecondaryCriteria": "None"
}
```

## 🤝 Colaboradores

Agradeço às seguintes pessoas que contribuíram para este projeto:

<table>
  <tr>
    <td align="center">
      <a href="#">
        <img src="https://avatars.githubusercontent.com/u/27699897?v=4" width="100px;" alt="Mayron César de Oliveira Moreira no GitHub"/><br>
        <sub>
          <b>Mayron César de Oliveira Moreira</b>
        </sub>
      </a>
    </td>
  </tr>
</table>
