Dado um array de inteiros `nums` e um inteiro `target`, retorne os índices dos dois números cuja soma seja igual a `target`.

Você pode assumir que cada entrada terá exatamente uma solução, e não pode usar o mesmo elemento duas vezes.

Você pode retornar a resposta em qualquer ordem.

 

Exemplo 1:

Entrada: nums = [2,7,11,15], target = 9
Saída: [0,1]
Explicação: Como nums[0] + nums[1] == 9, retornamos [0, 1].

Exemplo 2:

Entrada: nums = [3,2,4], target = 6
Saída: [1,2]

Exemplo 3:

Entrada: nums = [3,3], target = 6
Saída: [0,1]

 

Restrições:

2 <= nums.length <= 10^4
-10^9 <= nums[i] <= 10^9
-10^9 <= target <= 10^9
Existe apenas uma resposta válida.
