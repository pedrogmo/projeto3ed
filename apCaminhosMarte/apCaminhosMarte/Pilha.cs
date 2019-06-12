using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Classe Pilha, armazenamento de dados em disciplina LIFO
    class Pilha<T> : IStack<T> //Implementação da interface IStack
    {
        //Nó que guarda ponteiro para topo da pilha
        protected No<T> topo;

        //Quantidade de elementos armazenados
        protected int qtd;

        //Construtor default, inicializa atributos para null e 0, respectivamente
        public Pilha()
        {
            topo = null;
            qtd = 0;
        }

        //Propriedade Tamanho, retorna atributo qtd no get
        public int Tamanho { get => qtd; }

        //Método boolean que retorna true caso o topo seja nulo, false caso contrário
        public bool EstaVazia()
        {
            return topo == null;
            //ou 
            //return qtd==0;
        }

        //Método void que guarda informação passada no topo da pilha e aumenta quantidade
        public void Empilhar(T info)
        {
            var novoNo = new No<T>(info, topo); //Prox do novoNo é o topo anterior
            topo = novoNo;
            ++qtd;
        }

        //Método que retorna o elemento do topo da pilha
        //Joga PilhaVaziaException caso a pilha esteja vazia
        public T Topo()
        {
            if (EstaVazia())
                throw new PilhaVaziaException("Pilha vazia");
            return topo.Info;
        }

        //Método que retorna topo da pilha e o retira, diminuindo a quantidade
        //Joga PilhaVaziaException caso a pilha esteja vazia
        public T Desempilhar()
        {
            if (EstaVazia())
                throw new PilhaVaziaException("Pilha vazia");
            T ret = topo.Info;
            topo = topo.Prox; //move o topo para o próximo elemento
            --qtd;
            return ret;
        }

        //Método que retorna string com todos os elementos da pilha, do topo até a base
        //Feito para visualização na depuração
        public override string ToString()
        {
            string ret = EstaVazia() ? "Pilha vazia" : "";
            for (No<T> p = topo; p != null; p = p.Prox) //Ponteiro p percorre lista de valores armazenados
            {
                ret += "(" + p.Info.ToString() + ")";
                if (p.Prox != null)
                    ret += " , ";
            }
            return ret;
        }
    }
}
