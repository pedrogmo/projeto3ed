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
        int x, y;

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
                if (value == null || value == "")
                    throw new Exception("Nome inválido");
                nome = value;
            }
        }

        public int X
        {
            get => x;
            set
            {
                if (value < 0)
                    throw new Exception("Coordenada x inválida");
                x = value;
            }
        }

        public int Y
        {
            get => y;
            set
            {
                if (value < 0)
                    throw new Exception("Coordenada y inválida");
                y = value;
            }
        }

        public Cidade(int c, string n, int x, int y)
        {
            Codigo = c;
            Nome = n;
            X = x;
            Y = y;
        }
    }
}
