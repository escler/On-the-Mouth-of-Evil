using System;
using System.Collections.Generic;
using UnityEngine;

public static class Util {
    // Transformaciones

    //Clamp básico
    public static int Clamp(int v, int min, int max)
    {
        return v < min ? min : v > max ? max : v;
    }

    //Generator genérico, lo vamos a ver más adelante.
    public static IEnumerable<Src> GenerateGrid<Src>(Src seed, Func<Src, Src> generator) {
        while (true) {
            yield return seed;
            seed = generator(seed);
        }
    }
    
    public static IEnumerable<T> Generate<T>(T seed, Func<T, T> generator) {
        var acum = seed;
        while (true) {
            yield return acum;
            acum = generator(acum);
        }
    }
}