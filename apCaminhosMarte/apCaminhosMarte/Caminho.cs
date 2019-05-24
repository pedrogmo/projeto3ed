using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apCaminhosMarte;

namespace apCaminhosMarte
{
    class Caminho
    {
        protected Cidade origem, destino;
        protected Fila<Aresta> rota;
        protected int distanciaTotal/*, precoTotal*/;

        //public int Preco { get => preco; set => preco = value; }
        internal Cidade Origem { get => origem; set => origem = value; }
        internal Cidade Destino { get => destino; set => destino = value; }
        internal Fila<Aresta> Rota { get => rota; private set => rota = value; }

        public Caminho()
        {
            distanciaTotal /*= preco */= 0;
            origem = destino = null;
            rota = null;
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
            rota.Enfileirar(aresta);
        }
    }
}
