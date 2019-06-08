using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    class Fila<T> : IQueue<T>
    {       
        protected No<T> inicio, fim;
        protected int qtd;

        public Fila()
        {
            inicio = fim = null;
            qtd = 0;
        }  
        
        public int Tamanho { get => qtd; }

        public bool EstaVazia()
        {
            return inicio == null;
        }

        public void Enfileirar(T dado)
        {
            No<T> no = new No<T>(dado, null);
            if (EstaVazia())
                inicio = no;
            else
                fim.Prox = no;
            fim = no;
            ++qtd;
        }

        public T Retirar()
        {
            if (EstaVazia())
                throw new FilaVaziaException("Fila vazia");
            T ret = inicio.Info;
            inicio = inicio.Prox;
            if (inicio == null)
                fim = null;
            --qtd;
            return ret;
        }

        public T Inicio()
        {
            if (EstaVazia())
                throw new FilaVaziaException("Fila vazia");
            return inicio.Info;
        }

        public T Fim()
        {
            if (EstaVazia())
                throw new FilaVaziaException("Fila vazia");
            return fim.Info;
        }
    }
}
