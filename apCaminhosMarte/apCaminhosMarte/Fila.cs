﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Fila<T> : IQueue<T>
    {
        protected class No<X>
        {
            protected X info;
            protected No<X> prox;

            public No(X i, No<X> p)
            {
                Info = i;
                Prox = p;
            }

            public No(X i) : this(i, null)
            { }

            public X Info
            {
                get => info;
                set
                {
                    if (value == null)
                        throw new Exception("Informação ausente");
                    info = value;
                }
            }

            public No<X> Prox
            {
                get => prox;
                set => prox = value;
            }
        }

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
            qtd++;
        }

        public T Retirar()
        {
            if (EstaVazia())
                throw new FilaVaziaException("Fila vazia");
            T ret = inicio.Info;
            inicio = inicio.Prox;
            if (inicio == null)
                fim = null;
            qtd--;
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
