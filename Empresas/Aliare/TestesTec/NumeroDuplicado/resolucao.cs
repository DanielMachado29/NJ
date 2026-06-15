using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine(SolucaoSimples(new[] { 1, 3, 4, 2, 2 })); // 2
        Console.WriteLine(SolucaoSimples(new[] { 3, 1, 3, 4, 2 })); // 3

        Console.WriteLine(SolucaoMelhor(new[] { 1, 3, 4, 2, 2 })); // 2
        Console.WriteLine(SolucaoMelhor(new[] { 3, 1, 3, 4, 2 })); // 3
    }

    // -------------------------------------------------------------------------
    // SOLUÇÃO 1 — Simples
    // -------------------------------------------------------------------------
    static int SolucaoSimples(int[] numeros)
    {
        for (int i = 0; i < numeros.Length; i++)
        {
            for (int j = i + 1; j < numeros.Length; j++)
            {
                if (numeros[i] == numeros[j])
                    return numeros[i];
            }
        }

        throw new InvalidOperationException("Nenhum número duplicado encontrado.");
    }

    /*
     * Abordagem:
     * Compara cada par de posições do array. Para cada elemento na posição i,
     * percorre todos os elementos à direita (j > i) e verifica se algum é igual.
     *
     * Diferença em relação à solução melhor:
     * - Não usa estrutura auxiliar (memória extra mínima).
     * - Código direto e fácil de entender, mas lento: O(n²) no pior caso.
     * - A solução melhor percorre o array uma única vez com HashSet (O(n)).
     */

    // -------------------------------------------------------------------------
    // SOLUÇÃO 2 — Melhor
    // -------------------------------------------------------------------------
    static int SolucaoMelhor(int[] numeros)
    {
        var vistos = new HashSet<int>();

        foreach (int n in numeros)
        {
            if (vistos.Contains(n))
                return n;

            vistos.Add(n);
        }

        throw new InvalidOperationException("Nenhum número duplicado encontrado.");
    }

    /*
     * Abordagem:
     * Percorre o array uma vez, guardando em um HashSet cada número já visto.
     * Se o número já está no conjunto (Contains), é o duplicado; senão, adiciona.
     *
     * Diferença em relação à solução simples:
     * - Complexidade de tempo O(n) em média (busca/inserção no HashSet é O(1)).
     * - Usa O(n) de memória extra para o conjunto de números vistos.
     * - Troca simplicidade pura por eficiência, o que importa em arrays grandes.
     */
}
