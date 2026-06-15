using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        ExecutarExemplo(new[] { 2, 7, 11, 15 }, 9);
        ExecutarExemplo(new[] { 3, 2, 4 }, 6);
        ExecutarExemplo(new[] { 3, 3 }, 6);
    }

    static void ExecutarExemplo(int[] nums, int target)
    {
        int[] simples = SolucaoSimples(nums, target);
        int[] melhor = SolucaoMelhor(nums, target);

        Console.WriteLine($"nums = [{string.Join(", ", nums)}], target = {target}");
        Console.WriteLine($"  Simples: [{string.Join(", ", simples)}]");
        Console.WriteLine($"  Melhor:  [{string.Join(", ", melhor)}]");
        Console.WriteLine();
    }

    // -------------------------------------------------------------------------
    // SOLUÇÃO 1 — Simples
    // -------------------------------------------------------------------------
    static int[] SolucaoSimples(int[] nums, int target)
    {
        for (int i = 0; i < nums.Length; i++)
        {
            for (int j = i + 1; j < nums.Length; j++)
            {
                if (nums[i] + nums[j] == target)
                    return new[] { i, j };
            }
        }

        throw new InvalidOperationException("Nenhum par encontrado.");
    }

    /*
     * Abordagem:
     * Testa todas as combinações de dois índices distintos (i, j) com j > i.
     * Quando a soma dos dois elementos é igual ao target, retorna os índices.
     *
     * Diferença em relação à solução melhor:
     * - Não usa estrutura auxiliar (memória extra mínima).
     * - Código direto e fácil de entender, mas lento: O(n²) no pior caso.
     * - A solução melhor percorre o array uma única vez com dicionário (O(n)).
     */

    // -------------------------------------------------------------------------
    // SOLUÇÃO 2 — Melhor
    // -------------------------------------------------------------------------
    static int[] SolucaoMelhor(int[] nums, int target)
    {
        var indicePorValor = new Dictionary<int, int>();

        for (int i = 0; i < nums.Length; i++)
        {
            int complemento = target - nums[i];

            if (indicePorValor.TryGetValue(complemento, out int indice))
                return new[] { indice, i };

            indicePorValor[nums[i]] = i;
        }

        throw new InvalidOperationException("Nenhum par encontrado.");
    }

    /*
     * Abordagem:
     * Percorre o array uma vez. Para cada elemento, calcula o complemento
     * (target - nums[i]) e verifica se ele já foi visto. Se sim, retorna os
     * dois índices; senão, guarda o valor atual e seu índice no dicionário.
     *
     * Diferença em relação à solução simples:
     * - Complexidade de tempo O(n) em média (busca/inserção no dicionário é O(1)).
     * - Usa O(n) de memória extra para mapear valores já visitados.
     * - Troca simplicidade pura por eficiência, o que importa em arrays grandes.
     */
}
