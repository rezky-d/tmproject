using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java.util;
using jsastrawi.morphology;

namespace TextMining
{
    public class PreProcessing
    {
       
        DefaultLemmatizer stemmer;
        private String[] originalDocument;
        private int totalDocument;
        private List<String>[] documentTerm;
        private String[] term;
        private int[][] termFrequency;
        private int[] documentFrequency;

        public PreProcessing(String[] documents)
        {
            this.originalDocument = documents;
            this.totalDocument = documents.Length;
            this.documentTerm = new List<String>[this.totalDocument];
            this.term = new String[0];
        }

        public void tokenizing()
        {
            for (int i = 0; i < totalDocument; i++)
            {
                String text = originalDocument[i].ToLower();
                String strippedText = new String
                    ((from c in text where char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' select c).ToArray());
                documentTerm[i] = new List<String>();
                documentTerm[i].AddRange(strippedText.Split(' '));
            }
        }
        public void filtering()
        {
            String[] stopword = Properties.Resources.Stopword.Replace(System.Environment.NewLine, " ").Split(' ');
            for (int i = 0; i < documentTerm.Length; i++)
            {
                foreach (String sw in stopword)
                {
                    documentTerm[i].RemoveAll(item => item.Equals(sw.ToLower()));
                }
            }
        }
        public void stemming()
        {
            String[] baseword = Properties.Resources.Baseword.Replace(System.Environment.NewLine, " ").Split(' ');
            Set dict = new HashSet();
            for (int i = 0; i < baseword.Length; i++)
            {
                dict.add(baseword[i]);
            }
            stemmer = new DefaultLemmatizer(dict);

            for (int i = 0; i < documentTerm.Length; i++)
            {
                foreach(String word in documentTerm[i].ToList())
                {
                    String s = stemmer.lemmatize(word);
                    documentTerm[i].Remove(word);
                    documentTerm[i].Add(s);
                }
                
            }

        }
        public void combine()
        {
            foreach (List<String> doc in documentTerm)
            {
                term = term.Union(doc.ToArray()).ToArray();
            }
        }
     
        public void counting()
        {
            termFrequency = new int[term.Length][];
            for (int i = 0; i < term.Length; i++)
            {
                termFrequency[i] = new int[totalDocument];
                for (int j = 0; j < totalDocument; j++)
                {
                    termFrequency[i][j] = documentTerm[j].FindAll(s => { return s.Equals(term[i]); }).Count;
                }
            }
            documentFrequency = new int[term.Length];
            for (int i = 0; i < term.Length; i++)
            {
                foreach (int tf in termFrequency[i])
                {
                    documentFrequency[i] += (tf > 0 ? 1 : 0);
                }
            }
        }
        public void calculate()
        {
            tokenizing();
            stemming();
            filtering();
            combine();
            counting();
        }

        public List<String>[] getDocumentTerm()
        {
            return documentTerm;
        }
        public String[] getTerm()
        {
            return term;
        }
        public int[][] getTermFrequency()
        {
            return termFrequency;
        }
        public int[] getDocumentFrequency()
        {
            return documentFrequency;
        }
    }
}
