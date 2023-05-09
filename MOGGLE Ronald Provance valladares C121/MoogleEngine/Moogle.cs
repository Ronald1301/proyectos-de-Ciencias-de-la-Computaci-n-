namespace MoogleEngine;
public static class Moogle
{
    public static SearchResult Query(string query)
    {

        string[] titulo_txt = Implementation.Worktxt().Item2;

        (Dictionary<string, int>[], List<List<string>>, List<string>) trabajo_con_palabras = Implementation.diccionario_global;

        string[] query_normalizado = Implementation.QueryNormalizar(query);

        Dictionary<string, double> tf_idf_query = Implementation.TFxIDFQuery(query_normalizado);

        double[] vector_query = Implementation.Queryvectorizado(tf_idf_query, query_normalizado);

        double[,] matriz_tf_idf = Implementation.Matriztfxidf(query_normalizado);

        float[] similitud_cos = Implementation.SimilitudCos(vector_query, matriz_tf_idf);

        string[] sugerencias = Levenshtein.Distacncia(query_normalizado, trabajo_con_palabras.Item3);
        string sugerencia = "";
        for (int i = 0; i < sugerencias.Length; i++)
        {
            sugerencia += sugerencias[i] + " ";
        }

        float[] score = Operadores.verqueryoriginal(query, similitud_cos, trabajo_con_palabras.Item2);

        //con esto vamos a devolver el snippet en los resultados de la busqueda 
        string[] aux_snippet = new string[trabajo_con_palabras.Item2.Count];
        for (int i = 0; i < query_normalizado.Length; i++)
        {
            for (int j = 0; j < trabajo_con_palabras.Item2.Count; j++)
            {
                if (trabajo_con_palabras.Item2[j].Contains(query_normalizado[i]))
                {
                    aux_snippet[j] = Snippet.BuscarSnippet(trabajo_con_palabras.Item2[j], query_normalizado[i]);
                }
            }
        }
        //este es el algoritmo de ordenacion de los titulos de los txt y los valores de score(similitud del coseno)
        for (int i = 0; i < score.Length; i++)
        {

            for (int j = i; j < score.Length; j++)
            {
                if (score[j] > score[i])
                {
                    string temp1 = aux_snippet[i];
                    string temporal = titulo_txt[i];
                    float temp = score[i];
                    aux_snippet[i] = aux_snippet[j];
                    score[i] = score[j];
                    titulo_txt[i] = titulo_txt[j];
                    aux_snippet[j] = temp1;
                    titulo_txt[j] = temporal;
                    score[j] = temp;

                }
            }

        }
        if (score[0] == 0) {
            SearchItem[] item = new SearchItem[1];
            item[0] = new SearchItem("No hay resultado","",0F);
            return new SearchResult(item, sugerencia);
        }
        int contador = 0;
        for (int i = 0; i < score.Length; i++)
        {
            if (score[i] != 0) contador++;
        }
        float[] score_new1 = new float[contador];
    
        for (int i = 0; i < score_new1.Length; i++)
        {
            score_new1[i] = score[i];
        }
        SearchItem[] items = new SearchItem[score_new1.Length];
        for (int i = 0; i < score_new1.Length; i++)
        {
            items[i] = new SearchItem(titulo_txt[i], aux_snippet[i], score_new1[i]);
        }
        return new SearchResult(items, sugerencia);

    }

}
