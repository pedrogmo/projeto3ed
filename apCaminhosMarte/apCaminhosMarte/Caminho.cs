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
        protected Cidade origem, destino;
        protected List<Cidade> rota;
        protected int distanciaTotal;

        public Cidade Origem { get => origem; }
        public Cidade Destino { get => destino; }
        public List<Cidade> Rota { get => rota; }

        public Caminho(Cidade origem, Cidade destino)
        {
            this.origem = origem;
            this.destino = destino;
            distanciaTotal = 0;            
            rota = new List<Cidade>();
        }

        public Caminho(Cidade origem, Cidade destino, List<Cidade> rota, int distanciaTotal)
        {
            this.origem = origem;
            this.destino = destino;
            this.rota = rota;
            this.distanciaTotal = distanciaTotal;
        }

        public void AdicionarARota(Cidade um, int dist, Cidade dois)
        {
            distanciaTotal += dist;
            rota.Add(um);
            rota.Add(dois);
        }
    }
}
