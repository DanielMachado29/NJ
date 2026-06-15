using System;
using System.Linq;

class Program
{
    static void Main()
    {
        int[] exemplos = { 121, -121, 10, 0, 12321, 12345 };

        foreach (int x in exemplos)
        {
            bool simples = SolucaoSimples(x);
            bool melhor = SolucaoMelhor(x);

            Console.WriteLine($"x = {x}");
            Console.WriteLine($"  Simples: {simples}");
            Console.WriteLine($"  Melhor:  {melhor}");
            Console.WriteLine();
        }
    }

    // -------------------------------------------------------------------------
    // SOLUÇÃO 1 — Simples
    // -------------------------------------------------------------------------
    static bool SolucaoSimples(int x)
    {
        if (x < 0)
            return false;

        string texto = x.ToString();
        string invertido = new string(texto.Reverse().ToArray());

        return texto == invertido;
    }

    /*
     * Abordagem:
     * Converte o número em string e compara com a versão invertida.
     * Números negativos não são palíndromos (o sinal "-" quebra a simetria).
     *
     * Diferença em relação à solução melhor:
     * - Código curto e fácil de entender.
     * - Usa memória extra para as strings (O(log n) caracteres).
     * - A solução melhor evita conversão para string e trabalha só com inteiros.
     */

    // -------------------------------------------------------------------------
    // SOLUÇÃO 2 — Melhor
    // -------------------------------------------------------------------------
    static bool SolucaoMelhor(int x)
    {
        // Negativos e números terminados em 0 (exceto o próprio 0) não são palíndromos.
        if (x < 0 || (x % 10 == 0 && x != 0))
            return false;

        int metadeInvertida = 0;
        int original = x;

        // Inverte apenas a metade direita do número.
        // Quando original <= metadeInvertida, já processamos metade (ou mais) dos dígitos.
        while (original > metadeInvertida)
        {
            metadeInvertida = metadeInvertida * 10 + original % 10;
            original /= 10;
        }

        // Número com quantidade par de dígitos: original == metadeInvertida (ex.: 1221).
        // Número com quantidade ímpar de dígitos: descarta o dígito central (ex.: 121 -> 12 e 1).
        return original == metadeInvertida || original == metadeInvertida / 10;
    }

    /*
     * Abordagem:
     * Reverte matematicamente só a metade direita do número e compara com a metade esquerda.
     * Não aloca strings nem arrays — apenas operações aritméticas.
     *
     * Diferença em relação à solução simples:
     * - Complexidade de tempo O(log n) — proporcional à quantidade de dígitos.
     * - Complexidade de espaço O(1) — não usa estruturas auxiliares.
     * - Evita overflow em cenários típicos ao reverter só metade do número.
     */
}
