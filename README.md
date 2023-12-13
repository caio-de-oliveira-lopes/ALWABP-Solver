# ALWABP-Solver


<img src="https://github.com/caio-de-oliveira-lopes/ALWABP-Solver/blob/main/ALWABP/Assignment-example-of-workers-and-tasks-for-ALWABP.png" alt="ALWABP">

> Este projeto implementa as heurísticas construtivas apresentadas em:</br></br>
Moreira, M.C.O., Ritt, M., Costa, A.M. et al. Simple heuristics for the assembly line worker assignment and balancing problem.</br>
J Heuristics 18, 505–524 (2012). https://doi.org/10.1007/s10732-012-9195-5</br>

### Motivação
A razão para a implementação destes métodos deu-se pelo desenvolvimento do meu projeto de mestrado, que pretende extender este trabalho de Moreira et al. (2012) e outros.

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

### Input
Os arquivos de input podem ser obtidos neste mesmo projeto, porém foram extraídos de [mrpritt/ALWABP](https://github.com/mrpritt/ALWABP).
Cada input segue o seguinte formato:
* A primeira linha contém a quantidade de tarefas (n) da instância.
* As próximas n linhas contém o tempo de processamento da tarefa para cada trabalhador. Por exemplo: na linha 2, temos os tempos de processamento da tarefa 1 pelo trabalhadores 1, 2, 3 e 4. E assim por diante. **(Matriz de tempo de execução das tarefas)**
* Se em uma linha i, coluna j, existe o valor "Inf", então o trabalhador j não é capaz de executar a tarefa i.
* Após as n linhas, tem-se outras linhas que contém, cada uma, o par **u,v** que indica que a tarefa **u** deve ser executada antes da tarefa **v**. **(Grafo de Precedência)**
* O arquivo termina com dois valores -1.

Exemplo:
```
28
70 21 7 64
59 1 15 22
33 2 10 30
6 4 5 2
1 1 Inf 1
27 26 20 18
17 12 15 6
62 Inf 16 41
31 17 Inf 20
53 52 53 10
21 18 2 18
19 12 Inf Inf
108 91 83 85
52 Inf 52 22
5 1 5 5
8 3 Inf 2
97 32 4 50
8 4 2 1
47 23 45 1
67 21 56 54
17 1 11 Inf
8 6 2 4
3 1 3 3
21 1 20 4
107 Inf Inf 104
3 3 1 Inf
2 1 1 1
72 Inf Inf 49
1 3
1 4
1 5
1 8
1 19
1 21
1 22
1 23
1 24
1 26
2 6
2 17
3 28
4 28
5 28
6 7
7 18
8 9
9 10
10 11
10 12
11 15
12 13
13 14
13 16
14 28
15 28
16 28
17 28
18 28
19 20
20 28
21 28
22 28
23 28
24 25
25 28
26 27
27 28
-1 -1
```

### Output
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
