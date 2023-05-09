public class Operadores
{
    //metodo portal al que le voy a pasar todos los parametros para hacer las funciones de los operadores en el query
    public static float[] verqueryoriginal(string query, float[] Score, List<List<string>> documentos)
    {
        for (int i = 0; i < query.Length; i++)
        {
            int contador = 0;
            string palabra = "";
            // para el operador del  '!'
            if (query[i] == '!')
            {
                for (int j = i + 1; j < query.Length; j++)
                {
                    if (query[j] == ' ')
                    {
                        break;
                    }
                    else if (Char.IsLetterOrDigit(query[j]))
                    {
                        palabra += query[j];
                    }
                    else
                    {
                        break;
                    }

                }
                palabra = palabra.ToLower();
                NoAparece(palabra, Score, documentos);
            }

            // para el operador del  '*'
            if (query[i] == '*')
            {
                contador++;

                for (int j = i + 1; j < query.Length; j++)
                {
                    if (query[j] == '*')
                    {
                        contador++;
                        continue;
                    }
                    else if (query[j] == ' ')
                    {
                        break;
                    }
                    else if (Char.IsLetterOrDigit(query[j]))
                    {
                        palabra += query[j];
                    }
                    else
                    {
                        break;
                    }
                }
                palabra = palabra.ToLower();
                Asterisco(palabra, Score, documentos, contador);
            }
        }
        return Score;
    }

    //operador ('!') su funcion es hacer 0 el score de los documentos donde esta la palabra con el operador
    public static void NoAparece(string palabra, float[] score, List<List<string>> documentos)
    {
        for (int i = 0; i < documentos.Count; i++)
        {
            if (documentos[i].Contains(palabra))
            {
                score[i] = 0;
            }
        }
    }

    //operador ('*') mi forma de hacerlo es multiplicar el score por la cantidad de ('*') mas uno para ir dandole mas relevancia
    public static void Asterisco(string palabra, float[] score, List<List<string>> documentos, int contador)
    {
        for (int i = 0; i < documentos.Count; i++)
        {
            if (documentos[i].Contains(palabra))
            {
                score[i] = score[i] * (contador + 1);
            }
        }
    }
}

