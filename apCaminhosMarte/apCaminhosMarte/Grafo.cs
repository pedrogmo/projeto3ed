using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    class Grafo
    {
        protected int[,] matriz;
        protected int quantidade;

        public Grafo(int[,] m)
        {
            matriz = m;
            quantidade = (int) Math.Sqrt(matriz.Length);
        }

        public int Quantidade { get => quantidade; }

        public int DistanciaEntre(int l, int c)
        {
            if (l < 0 || l >= quantidade)
                throw new Exception("Linha inválida");
            if (c < 0 || c >= quantidade)
                throw new Exception("Coluna inválida");
            return matriz[l, c];
        }

        public List<Caminho> Caminhos(int origem, int fim)
        {
            List<Caminho> ret = new List<Caminho>();
            return ret;
        }
    }
}
