using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Classe Fila, disciplina FIFO
    class Fila<T> : IQueue<T> //Implementa interface IQueue
    {       
        //Ponteiros para inicio e fim da fila
        protected No<T> inicio, fim;

        //Quantidade de elementos armazenados
        protected int qtd;

        //Construtor vazio, inicializa atributos para null, null e 0, respectivamente
        public Fila()
        {
            inicio = fim = null;
            qtd = 0;
        }  
        
        //Propriedade que retorna atributo qtd
        public int Tamanho { get => qtd; }

        //Método boolean que retorna true caso o inicio seja nulo, false caso contrário
        public bool EstaVazia()
        {
            return inicio == null;
            //ou
            //return fim==null;
            //ou
            //return qtd==0;
        }

        //Método void que armazena dado passado por parâmetro na fila, inserindo-o no fim da fila e aumentando a quantidade
        public void Enfileirar(T dado)
        {
            No<T> no = new No<T>(dado, null);
            if (EstaVazia())
                inicio = no; //Se está vazia, o fim também é o inicio
            else
                fim.Prox = no; //Se não está vazia, fim aponta para novo elemento
            fim = no; //Fim passa a ser novo nó
            ++qtd;
        }

        //Método que retorna elemento no início da fila e o remove, diminuindo o tamanho
        //Lança FilaVaziaException caso fila esteja vazia
        public T Retirar()
        {
            if (EstaVazia())
                throw new FilaVaziaException("Fila vazia");
            T ret = inicio.Info;
            inicio = inicio.Prox; //Ponteiro inicio vai para seu próximo
            if (inicio == null)
                fim = null; //Se início ficar nulo, fila está vazia então fim deve ser nulo também
            --qtd;
            return ret;
        }

        //Método que retorna valor do início da fila
        //Lança FilaVaziaException caso fila esteja vazia
        public T Inicio()
        {
            if (EstaVazia())
                throw new FilaVaziaException("Fila vazia");
            return inicio.Info;
        }

        //Método que retorna valor do fim da fila
        //Lança FilaVaziaException caso fila esteja vazia
        public T Fim()
        {
            if (EstaVazia())
                throw new FilaVaziaException("Fila vazia");
            return fim.Info;
        }
    }
}
