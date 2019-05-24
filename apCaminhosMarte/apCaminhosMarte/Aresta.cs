using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apCaminhosMarte;

namespace apCaminhosMarte
{
    internal class Aresta
    {
        protected int distancia/*, preco*/;
        protected Cidade um, dois;

        public Aresta()
        {
            distancia =/* preco =*/ 0;
            um = dois = null;
        }
        public Aresta(int distancia, Cidade dois, Cidade um)
        {
            this.distancia = distancia;
            this.dois = dois;
            this.um = um;
        }

        public int Distancia { get => distancia; set => distancia = value; }
        public Cidade Um { get => um; set => um = value; }
        public Cidade Dois { get => dois; set => dois = value; }
    }
}