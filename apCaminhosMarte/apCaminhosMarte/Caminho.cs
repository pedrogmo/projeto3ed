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
        protected List<int> rota;
        protected int distanciaTotal;

        public List<int> Rota { get => rota; }

        public int DistanciaTotal { get => distanciaTotal; }

        public Caminho(int origem)
        {
            distanciaTotal = 0;            
            rota = new List<int>();
            rota.Add(origem);
        }

        public Caminho(List<int> rota, int distanciaTotal)
        {
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
