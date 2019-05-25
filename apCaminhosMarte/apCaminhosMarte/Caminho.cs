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
        protected Fila<Aresta> rota;
        protected int distanciaTotal, precoTotal;

        public int PrecoTotal { get => precoTotal; }
        public Cidade Origem { get => origem; }
        public Cidade Destino { get => destino; }
        public Fila<Aresta> Rota { get => rota; }

        public Caminho(Cidade origem, Cidade destino)
        {
            this.origem = origem;
            this.destino = destino;
            distanciaTotal = precoTotal = 0;            
            rota = new Fila<Aresta>();
        }

        public Caminho(Cidade origem, Cidade destino, Fila<Aresta> rota, int distanciaTotal)
        {
            this.origem = origem;
            this.destino = destino;
            this.rota = rota;
            this.distanciaTotal = distanciaTotal;
        }

        public void AdicionarARota(Aresta aresta)
        {
            distanciaTotal += aresta.Distancia;
            precoTotal += aresta.Preco;
            rota.Enfileirar(aresta);
        }
    }
}
