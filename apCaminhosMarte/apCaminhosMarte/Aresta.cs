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
    class Aresta
    {
        protected int distancia, preco;
        protected Cidade um, dois;

        public Aresta()
        {
            distancia = preco = 0;
            um = dois = null;
        }
        public Aresta(Cidade um, Cidade dois, int distancia, int preco)
        {
            this.um = um;
            this.dois = dois;
            this.distancia = distancia;
            this.preco = preco;
        }
        
        public Cidade Um { get => um; }
        public Cidade Dois { get => dois; }

        public int Distancia { get => distancia; }

        public int Preco { get => preco; }
    }
}