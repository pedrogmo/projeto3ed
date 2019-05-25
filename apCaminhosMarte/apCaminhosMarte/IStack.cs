using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    interface IStack<Dado>
    {
        void Empilhar(Dado info);
        Dado Desempilhar();
        Dado Topo();
        int Tamanho { get; }
        bool EstaVazia();
    }
}
