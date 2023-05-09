class Levenshtein
{
    public static string[] Distacncia(string[] palabrasquery, List<string> palabrassinrepetir)
    {
        double porcentaje = 0;
        double[] l = new double[palabrassinrepetir.Count];
        string[] sugerencias = new string[palabrasquery.Length];
        for (int i = 0; i < palabrasquery.Length; i++)
        {
            for (int j = 0; j < palabrassinrepetir.Count; j++)
            {
                // Verifica que exista algo que comparar
                if (palabrassinrepetir[j].Length == 0) break;
                if (palabrasquery[i].Length == 0) break;
                porcentaje = Calculo(palabrasquery[i], palabrassinrepetir[j]);
                l[j] = porcentaje;
            }
            sugerencias[i] = palabrassinrepetir[Menor(l)];
        }

        return sugerencias;
    }
    private static int Menor(double[] l)
    {
        double min = double.MaxValue;
        int indice = 0;
        for (int i = 0; i < l.Length; i++)
        {
            if (l[i] < min)
            {
                min = l[i];
                indice = i;
            }
        }
        return indice;
    }
    private static double Calculo(string palabra1, string palabra2)
    {
        int costo = 0;
        int m = palabra1.Length;
        int n = palabra2.Length;
        // d es una tabla con m+1 renglones y n+1 columnas
        int[,] d = new int[m + 1, n + 1];
        // Llena la primera columna y la primera fila.
        for (int i = 0; i <= m; d[i, 0] = i++) ;
        for (int h = 0; h <= n; d[0, h] = h++) ;
        /// recorre la matriz llenando cada unos de los pesos.
        /// i columnas, j renglones
        for (int i = 1; i <= m; i++)
        {
            // recorre para j
            for (int j = 1; j <= n; j++)
            {
                /// si son iguales en posiciones equidistantes el peso es 0
                /// de lo contrario el peso suma a uno.
                costo = (palabra1[i - 1] == palabra2[j - 1]) ? 0 : 1;
                d[i, j] = System.Math.Min(System.Math.Min(d[i - 1, j] + 1,  //Eliminacion
                          d[i, j - 1] + 1),                             //Insercion 
                          d[i - 1, j - 1] + costo);                     //Sustitucion
            }
        }
        /// Calculamos el porcentaje de cambios en la palabra.
        if (palabra1.Length > palabra2.Length)
            return ((double)d[m, n] / (double)palabra1.Length);
        else
            return ((double)d[m, n] / (double)palabra2.Length);
    }

}