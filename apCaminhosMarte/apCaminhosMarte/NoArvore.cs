using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Classe usada para armazenar dados de uma árvore de busca binária
    public class NoArvore<T> where T : IComparable<T> //Classe deve ser comparável para aplicação em uma árvore de busca binária
    {
        //Informação interna
        protected T info;

        //Ponteiros para nós à esquerda e à direita
        protected NoArvore<T> esquerdo, direito;

        //Construtor com todos os atributos da classe
        public NoArvore(T inf, NoArvore<T> esq, NoArvore<T> dir)
        {
            Info = inf;
            Esquerdo = esq;
            Direito = dir;
        }

        //Construtor somente com valor interno, que chama o de cima com ponteiros nulos
        public NoArvore(T inf) : this(inf, null, null)
        { }     

        //Propriedade Info com getter e setter
        //Joga exceção se valor for nulo
        public T Info
        {
            get => info;
            set
            {
                if (value == null)
                    throw new Exception("Informação inválida");
                info = value;
            }
        }

        //Propriedade Esquerdo com getter e setter
        public NoArvore<T> Esquerdo
        {
            get => esquerdo;
            set => esquerdo = value;
        }

        //Propriedade Direito com getter e setter
        public NoArvore<T> Direito
        {
            get => direito;
            set => direito = value;
        }

        //Método boolean que retorna true se nó for folha
        public bool EhFolha()
        {
            return esquerdo == null && direito == null;
        }

        //Método que retorna string no formato (esquerdo)<-(this)->(direito)
        public override string ToString()
        {
            string stringEsq = esquerdo != null ? esquerdo.Info.ToString() : "null";
            string stringDir = esquerdo != null ? direito.Info.ToString() : "null";
            return $"({stringEsq})<-({Info.ToString()})->({stringDir})";
        }     
    }
}
