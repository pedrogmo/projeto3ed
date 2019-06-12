using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    interface IStack<T>
    {
        void Empilhar(T info);
        T Desempilhar();
        T Topo();
        int Tamanho { get; }
        bool EstaVazia();
    }
}
