public class Snippet
{
    //Este metodo me va a retornar un fragmento del txt donde aparecen las palabras del query
    public static string BuscarSnippet(List<string> documentos, string palabra)
    {
        string parte = "";
        string parte1 = "";
        string parte2 = "";
        for (int j = 0; j < documentos.Count; j++)
        {
            if (documentos[j] == palabra && parte == "")
            {
                int minpalabras = 15;
                for (int k = j; k >= 0; k--)
                {
                    parte1 = documentos[k] + " " + parte1;
                    minpalabras--;
                    if (minpalabras == 0)break;
                }
                int maxpalabras = 14;
                for (int k = j + 1; k < documentos.Count; k++)
                {
                    parte2 += " " + documentos[k];
                    maxpalabras--;
                    if (maxpalabras == 0)break;
                }
                parte = parte1 + parte2;
            }
        }
        return parte;
    }
}