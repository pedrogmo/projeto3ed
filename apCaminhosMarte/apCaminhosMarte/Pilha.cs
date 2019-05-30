using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    class Pilha<T> : IStack<T>
    {
        protected No<T> topo;
        protected int qtd;

        public Pilha()
        {
            topo = null;
            qtd = 0;
        }

        public int Tamanho { get => qtd; }

        public bool EstaVazia()
        {
            return topo == null;
        }

        public void Empilhar(T info)
        {
            var novoNo = new No<T>(info, topo);
            topo = novoNo;
            ++qtd;
        }

        public T Topo()
        {
            if (EstaVazia())
                throw new PilhaVaziaException("Pilha vazia");
            return topo.Info;
        }

        public T Desempilhar()
        {
            if (EstaVazia())
                throw new PilhaVaziaException("Pilha vazia");
            T ret = topo.Info;
            topo = topo.Prox;
            --qtd;
            return ret;
        }

        public override string ToString()
        {
            string ret = EstaVazia() ? "Pilha vazia" : "";
            for (No<T> p = topo; p != null; p = p.Prox)
            {
                ret += "(" + p.Info.ToString() + ")";
                if (p.Prox != null)
                    ret += " , ";
            }
            return ret;
        }
    }
}
