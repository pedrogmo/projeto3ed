using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    class No<T>
    {
        protected T info;
        protected No<T> prox;

        public No(T i, No<T> p)
        {
            Info = i;
            Prox = p;
        }
        public No(T i) : this(i, null)
        { }
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
        public No<T> Prox
        {
            get => prox;
            set => prox = value;
        }
    }
}
