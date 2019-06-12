using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Classe usada para armazenar dados da Pilha e da Fila
    class No<T> //Classe T genérica
    {
        //Conteúdo T interno
        protected T info;
        //Ponteiro para próximo nó
        protected No<T> prox;

        //Construtor com valor interno inf e ponteiro prx
        public No(T inf, No<T> prx)
        {
            Info = inf;
            Prox = prx;
        }

        //Construtor com valor inf, chama o contrutor acima com null para o prox
        public No(T inf) : this(inf, null)
        { }

        //Propriedade Info com getter e setter
        //Joga exceção se valor for nulo
        public T Info
        {
            get => info;
            set
            {
                if (value == null)
                    throw new Exception("Informação ausente");
                info = value;
            }
        }

        //Propriedade Prox com getter e setter
        public No<T> Prox
        {
            get => prox;
            set => prox = value;
        }
    }
}
