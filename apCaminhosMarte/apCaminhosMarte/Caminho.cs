using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apCaminhosMarte;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    class Caminho
    {
        protected int origem, destino;
        protected List<int> rota;
        protected int distanciaTotal;

        public int Origem { get => origem; }
        public int Destino { get => destino; }
        public List<int> Rota { get => rota; }

        public Caminho(int origem, int destino)
        {
            this.origem = origem;
            this.destino = destino;
            distanciaTotal = 0;            
            rota = new List<int>();
        }

        public Caminho(int origem, int destino, List<int> rota, int distanciaTotal)
        {
            this.origem = origem;
            this.destino = destino;
            this.rota = rota;
            this.distanciaTotal = distanciaTotal;
        }

        public void AdicionarARota(int cidadeNova, int dist)
        {
            distanciaTotal += dist;
            rota.Add(cidadeNova);
        }
    }
}
