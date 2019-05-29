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
        public Grafo(int[,] m)
        {
            matriz = m;
        }

        public List<Caminho> Caminhos(int origem, int fim)
        {
            List<Caminho> ret = new List<Caminho>();
            return ret;
        }
    }
}
