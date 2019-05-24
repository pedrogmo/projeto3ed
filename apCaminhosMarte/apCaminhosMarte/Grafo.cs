using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Grafo
    {
        protected int[,] arestas;
        public Grafo()
        {
            arestas = new int[25, 25];
        }
        public void AdicionarAMatriz(int codCidade, int distancia)
        {
            arestas[0, codCidade] = arestas[codCidade, 0] = distancia;
        }
    }
}
