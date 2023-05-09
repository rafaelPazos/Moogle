using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class Data_Processing
    {
        static List<Document> documents;
        static double[,] TFIDF;
        static List<string> needWords;
        static List<string> notNeedWords;
        static List<string> valueWords;
        static List<string> queryWords;
        static int[] valueAmount;
        public Data_Processing(string query)
        {
            documents = new List<Document>();
            needWords = new List<string>();
            notNeedWords = new List<string>();
            valueWords = new List<string>();
            queryWords = new List<string>();
            LoadFilesContent();
            SeparateContent();
            AnalizeChirimbolos(query);
            CalculateTFIDF();
            Score();
        }
        private void LoadFilesContent()
        {
            string[] fileNames = Directory.GetFiles("../Content");

            for (int i = 0; i < fileNames.Length; i++)
            {
                if (fileNames[i] == "../Content\\.gitignore")
                    continue;

                StreamReader reader = new StreamReader(fileNames[i]);
                string fileContent = reader.ReadToEnd();
                Document aux = new Document();
                aux.Content = fileContent;
                documents.Add(aux);
            }
        }
        private void SeparateContent()
        {
            foreach(Document document in documents)
            {
                document.PureContent = document.Content.Split(" @$/#.-:&*+=[]?!(){},''\">_<;%\\".ToCharArray());
            }
        }
        public void CalculateTFIDF()
        {
            List<int> index = new List<int>();
            TFIDF = new double[queryWords.Count,documents.Count];
            List<string> aux = new List<string>();
            aux = needWords;
            double[] totalDocumentAppears = new double[queryWords.Count];
            for (int i = 0;i < documents.Count;i++)
            {
                foreach(string word in documents[i].PureContent)
                {
                    if(notNeedWords.Contains(word.ToLower()))
                    {
                        index.Add(i);
                    }
                    if(aux.Contains(word.ToLower()))
                    {
                        aux.Remove(word.ToLower());
                    }
                }
                if(aux.Count != 0)
                {
                    index.Add(i);
                }
                aux = needWords;
            }

            for (int i = 0; i < queryWords.Count;i++)
            {
                double[] tf = new double[documents.Count];
                for (int j = 0; j < documents.Count; j++)
                {
                    if (index.Contains(j)) continue;
                    int documentAppears = 0;
                    foreach(string words in documents[j].PureContent)
                    {
                        if(queryWords[i] == words.ToLower())
                        {
                            documentAppears++;
                        }
                    }
                    if(documentAppears > 0)
                    {
                        totalDocumentAppears[i]++;
                    }
                    tf[j] = (double)documentAppears / (double)documents[j].PureContent.Length;
                }
                if (totalDocumentAppears[i] == 0) totalDocumentAppears[i] = 1;
                double idf = System.Math.Log10(((double)documents.Count) / ((double)totalDocumentAppears[i]));
                for(int k = 0; k < documents.Count; k++)
                {
                    if(valueAmount[i] > 0) TFIDF[i, k] = tf[k] * idf * Math.Pow(2,valueAmount[i]);
                    else TFIDF[i, k] = tf[k] * idf;
                }
            }
        }
        private void Score()
        {
            for (int i = 0; i < TFIDF.GetLength(1); i++)
            {
                float aux = 0;
                for (int j = 0; j < TFIDF.GetLength(0); j++)
                {
                    aux += (float)TFIDF[j, i];
                }
                documents[i].Score = aux;
            }
        }
        private void AnalizeChirimbolos(string query)
        {
            string[] splittedQuery = query.Split(' ');
            valueAmount = new int[splittedQuery.Length];
            for(int i = 0;i < splittedQuery.Length;i++)
            {
                if(splittedQuery[i].Contains('!'))
                {
                    string aux = splittedQuery[i].Substring(1);
                    notNeedWords.Add(aux.ToLower());
                    queryWords.Add(aux.ToLower());
                    continue;
                }
                if (splittedQuery[i].Contains('*'))
                {
                    string aux = "";
                    int temp = 0;
                    for (int j = 0; j < splittedQuery[i].Length; j++)
                    {
                        if(splittedQuery[i][j] != '*')
                        {
                            aux += splittedQuery[i][j];
                        }
                        if(splittedQuery[i][j] == '*')
                        {
                            temp++;
                        }
                    }
                    valueAmount[i] = temp;
                    if(aux.Contains('^'))
                    {
                        string aux2 = "";
                        for (int k = 0; k < aux.Length; k++)
                        {
                            if (aux[k] != '^')
                            {
                                aux2 += aux[k];
                            }
                        }
                        valueWords.Add(aux2.ToLower());
                        needWords.Add(aux2.ToLower());
                        queryWords.Add(aux2.ToLower());
                        continue;
                    }
                    queryWords.Add(aux.ToLower());
                    valueWords.Add(aux.ToLower());
                    continue;
                }
                if (splittedQuery[i].Contains('^'))
                {
                    string aux = splittedQuery[i].Substring(1);
                    needWords.Add(aux.ToLower());
                    queryWords.Add(aux.ToLower());
                    continue;
                }
                else
                {
                    queryWords.Add(splittedQuery[i].ToLower());
                }
            }
        }
    }
}
