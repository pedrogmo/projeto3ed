using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class NoArvore<T> where T : IComparable<T>
    {
        //Felipe Scherer Vicentin (18178)
        //Pedro Gomes Moreira (18174)
        protected T info;
        protected NoArvore<T> esquerdo, direito;
        public NoArvore(T i, NoArvore<T> e, NoArvore<T> d)
        {
            Info = i;
            Esquerdo = e;
            Direito = d;
        }
        public NoArvore()
        {
            esquerdo = direito = null;
            info = default(T);
        }
        public NoArvore(T i)
        {
            esquerdo = direito = null;
            info = i;
        }
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
        public NoArvore<T> Esquerdo
        {
            get => esquerdo;
            set => esquerdo = value;
        }
        public NoArvore<T> Direito
        {
            get => direito;
            set => direito = value;
        }
        public bool EhFolha()
        {
            return esquerdo == null && direito == null;
        }
        public override string ToString()
        {
            return $"({esquerdo.info.ToString()}){info.ToString()}({direito.info.ToString()})";
        }
        public static bool operator !=(NoArvore<T> um, NoArvore<T> outro)
        {
            return !(um == outro);
        }
        public static bool operator ==(NoArvore<T> um, NoArvore<T> outro)
        {
            if (um is null && outro is null)
                return true;
            if (um is null || outro is null)
                return false;
            return um.info.CompareTo(outro.info) == 0;
        }
    }
}
