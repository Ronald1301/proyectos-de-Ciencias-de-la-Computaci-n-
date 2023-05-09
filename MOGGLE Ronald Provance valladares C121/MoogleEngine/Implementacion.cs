using System.IO;
public class Implementation
{
    //variables generales para poder usarlas desde cualquier clase 
    public static string[] docGenerales = Worktxt().Item1;
    public static (Dictionary<string, int>[],List<List<string>>,List<string>)diccionario_global = WorkWords();
    public static Dictionary<string, double>[] tf_global = TFxIDF();

    //obtener tanto la direccion como los nombres de los txt 
    public static (string[], string[]) Worktxt()
    {
        List<string> nombres = new();

        //Modifico la direccion(por la del padre(de donde estoy corriendo))
        string currentadrress = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
        string[] adrresstxt = Directory.GetFiles(currentadrress + @"/Content");

        //lo convierto a lista para remover el .gitignore
        var convertirenlista = adrresstxt.ToList();
        convertirenlista.Remove(currentadrress + @"/Content/" + ".gitignore");
        adrresstxt = convertirenlista.ToArray();

        //sacar los nombres de los txt de las rutas
        string[] titletxt = new string[adrresstxt.Length];
        for (int i = 0; i < titletxt.Length; i++)
        {
            //gracias a Path puedo obtener directamente el nombre del txt
            titletxt[i] = Path.GetFileNameWithoutExtension(adrresstxt[i]);

        }
        return (adrresstxt, titletxt);
    }


    //voy a almacenar todas las palabras de la coleccion de txt exceptuando lo que no sea letra o digito para posteriormente trabajar con ellas 
    public static (Dictionary<string, int>[], List<List<string>>, List<string>) WorkWords()
    {
        string[] addressdoc = docGenerales;

        int canttxt = addressdoc.Length;

        Dictionary<string, int>[] Dictionarydoc = new Dictionary<string, int>[canttxt];

        List<List<string>> listtxtAllWords = new();

        List<string> wordsdontrepeat = new();

        for (int i = 0; i < canttxt; i++)
        {
            string words = "";

            listtxtAllWords.Add(new());

            Dictionarydoc[i] = new();
            //esta linea es para trabajar con documentos ANSI, los codifico a UTF8
            StreamReader lector = new StreamReader(addressdoc[i], System.Text.Encoding.UTF8);

            string textototal = lector.ReadToEnd().ToLower();


            for (int j = 0; j < textototal.Length; j++)
            {


                if (textototal[j] == ' ' && words == "")
                {
                    continue;
                }

                if (Char.IsLetterOrDigit(textototal[j]))
                {
                    words = words + textototal[j];
                }

                else if (textototal[j] == ' ' || words != "")
                {

                    listtxtAllWords[i].Add(words);

                    if (!wordsdontrepeat.Contains(words))
                    {
                        wordsdontrepeat.Add(words);

                    }
                    if (Dictionarydoc[i].ContainsKey(words))
                    {
                        Dictionarydoc[i][words]++;

                    }
                    else
                    {
                        Dictionarydoc[i].Add(words, 1);
                    }
                    words = "";
                }
            }
            if (words != "")
            {

                listtxtAllWords[i].Add(words);
                if (!wordsdontrepeat.Contains(words))
                {
                    wordsdontrepeat.Add(words);

                }
                if (Dictionarydoc[i].ContainsKey(words))
                {

                }
                else
                {
                    Dictionarydoc[i].Add(words, 1);
                }
            }
            lector.Close();

        }

        return (Dictionarydoc, listtxtAllWords, wordsdontrepeat);
    }


    //calcular el TF_IDF de todas las palabras (de las repetidas solo se calcula una vez)
    public static Dictionary<string, double>[] TFxIDF()
    {
        //TF= FECUENCIA DE LA PALABRA (REPETICION)/ NUMERO TOTAL DE PALABRAS
        //IDF= LOG(NUMERO TOTAL DE TXT/ NUMERO DE DOCUMENTOS QUE CONTIENEN LA PALABRA)

        Dictionary<string, int>[] dictionarydoc = diccionario_global.Item1;

        Dictionary<string, double>[] tfxidf = new Dictionary<string, double>[diccionario_global.Item2.Count];

        int canttxt = dictionarydoc.Length;

        for (int i = 0; i < canttxt; i++)
        {
            //convierto el diccionario en una lista
            tfxidf[i] = new();
            var listadiccdoc = dictionarydoc[i].ToList();
            int temporal = dictionarydoc[i].Count;
            for (int j = 0; j < temporal; j++)
            {
                int contador = 0;
                var palabra = listadiccdoc[j];


                for (int h = 0; h < canttxt; h++)
                {
                    if (dictionarydoc[i].ContainsKey(palabra.Key))
                    {

                        contador++;
                    }
                }

                double resultado = (double)palabra.Value * (double)((double)Math.Log10(canttxt / (double)(contador + 0.0000001)));
                resultado = Math.Abs(resultado);

                tfxidf[i].Add(palabra.Key, resultado);
            }

        }
        return tfxidf;
    }


    //query pero con solo palabras y numeros (normalizado)
    public static string[] QueryNormalizar(string query)
    {
        List<string> palabrasquery = new();
        string palabra = "";
        for (int i = 0; i < query.Length; i++)
        {
            if (query[i] == ' ' && palabra == "")
            {
                continue;
            }
            if (Char.IsLetterOrDigit(query[i]))
            {
                palabra += query[i];
            }
            else if (query[i] == ' ' || palabra != "")
            {
                palabra = palabra.ToLower();
                palabrasquery.Add(palabra);

                palabra = "";
            }

        }

        if (palabra != "")
        {
            palabra = palabra.ToLower();
            palabrasquery.Add(palabra);
        }
        string[] arrayquerynormalizado = palabrasquery.ToArray();
        return arrayquerynormalizado;
    }


    // TFxIDF del query en un diccionario donde va a estar cada palabra y su TFxIDF asocioado pasandole como parametro el query normalizado 
    public static Dictionary<string, double> TFxIDFQuery(string[] querynormalizado)
    {
        Dictionary<string, int>[] Dicpalabrayfrecuencia = diccionario_global.Item1;
        Dictionary<string, double> TFxidfquery = new();

        int[] Repeticiones = new int[querynormalizado.Length];
        int canttxt = Dicpalabrayfrecuencia.Length;


        for (int i = 0; i < querynormalizado.Length; i++)
        {
            int t = 0;
            for (int j = i; j < querynormalizado.Length; j++)
            {
                if (querynormalizado[i] == querynormalizado[j])
                {
                    t++;
                }
            }
            Repeticiones[i] = t;


        }
        for (int i = 0; i < querynormalizado.Length; i++)
        {
            int contador = 0;
            var palabra1 = querynormalizado[i];
            for (int h = 0; h < canttxt; h++)
            {
                if (Dicpalabrayfrecuencia[h].ContainsKey(palabra1))
                {
                    contador++;
                }
            }
            double resultado = Repeticiones[i] * (double)(Math.Log10((canttxt + 1) / (double)(contador + 0.0000001)));
            if (!TFxidfquery.ContainsKey(palabra1))
            {
                TFxidfquery.Add(palabra1, resultado);

            }
        }


        return TFxidfquery;
    }


    //valor del TFxIDF de las palabras del query  
    public static double[] Queryvectorizado(Dictionary<string, double> tfxidfquery, string[] query)
    {
        double[] vectorquery = new double[query.Length];

        for (int i = 0; i < vectorquery.Length; i++)
        {
            vectorquery[i] = tfxidfquery[query[i]];
        }
        return vectorquery;
    }


    //matriz que almacena los valores de TFxIDF de las palabras del query en cada txt
    public static double[,] Matriztfxidf(string[] query)
    {   //esta va a ser mi cant de filas en la matriz
        Dictionary<string, double>[] tfxidfdoc = tf_global;
        double[,] matriztfxidf = new double[tfxidfdoc.Length, query.Length];

        for (int i = 0; i < tfxidfdoc.Length; i++)
        {
            for (int j = 0; j < query.Length; j++)
            {
                if (tfxidfdoc[i].ContainsKey(query[j]))
                {
                    matriztfxidf[i, j] = tfxidfdoc[i][query[j]];
                }
                else
                {
                    matriztfxidf[i, j] = 0;
                }
            }
        }
        return matriztfxidf;
    }

    //multiplicar el vector del query por la matriz tfxidf
    public static float[] SimilitudCos(double[] vectorquery, double[,] matriztfxidf)
    {
        float[] Score = new float[matriztfxidf.GetLength(0)];

        double sumatoriaquery = 0;
        for (int i = 0; i < vectorquery.Length; i++)
        {
            sumatoriaquery += Math.Pow(vectorquery[i], 2);
        }
        sumatoriaquery = Math.Sqrt(sumatoriaquery);

        for (int j = 0; j < matriztfxidf.GetLength(0); j++)
        {
            double sumatoriamatriz = 0;
            double subscore = 0;

            for (int i = 0; i < vectorquery.Length; i++)
            {
                sumatoriamatriz += Math.Pow(matriztfxidf[j, i], 2);
                subscore += vectorquery[i] * matriztfxidf[j, i];
            }
            sumatoriamatriz = Math.Sqrt(sumatoriamatriz);
            Score[j] = (float)((double)subscore / ((double)(sumatoriamatriz * sumatoriaquery) + 0.0000001));

        }
        return Score;
    }
}