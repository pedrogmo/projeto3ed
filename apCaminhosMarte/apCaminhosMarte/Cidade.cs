using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)
    class Cidade
    {
        int codigo;
        string nome;

        public int Codigo
        {
            get => codigo;
            private set
            {
                if (value < 0)
                    throw new Exception("Código inválido");
                codigo = value;
            }
        }

        public string Nome
        {
            get => nome;
            private set
            {

            }
        }

        public Cidade(int c, string n)
        {
            Codigo = c;
            Nome = n;
        }
    }
}
