using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    //Felipe Scherer Vicentin (18178)
    //Pedro Gomes Moreira (18174)

    //Classe que será usada para guardar cada cidade do mapa
    public class Cidade : IComparable<Cidade>
    {
        //Código da cidade
        protected int codigo;
        //Coordenada X e Y da cidade
        protected int x, y;      
        //Nome da cidade
        protected string nome;

        //Constantes de começo e tamanho para cada atributo para separação de uma string
        public const int COMECO_CODIGO = 0;
        public const int TAMANHO_CODIGO = 3;
        public const int COMECO_NOME = COMECO_CODIGO + TAMANHO_CODIGO;
        public const int TAMANHO_NOME = 15;
        public const int COMECO_X = COMECO_NOME + TAMANHO_NOME;
        public const int TAMANHO_X = 5;
        public const int COMECO_Y = COMECO_X + TAMANHO_X;
        public const int TAMANHO_Y = 5;

        //Construtor completo com parâmetros para todos os atributos atributos
        public Cidade(int c, string n, int x, int y)
        {
            Codigo = c;
            Nome = n;
            X = x;
            Y = y;
        }

        //Construtor que recebe uma linha por parâmetro e a divide, armazenando os valores correspondentes a cada atributo
        //Joga exceção caso haja erro de separação e conversão da string
        public Cidade(string linha)
        {
            try
            {
                Codigo = int.Parse(linha.Substring(COMECO_CODIGO, TAMANHO_CODIGO).Trim());
                Nome = linha.Substring(COMECO_NOME, TAMANHO_NOME).Trim();
                X = int.Parse(linha.Substring(COMECO_X, TAMANHO_X).Trim());
                Y = int.Parse(linha.Substring(COMECO_Y, TAMANHO_Y).Trim());
            }
            catch
            {
                throw new Exception("String da cidade inválida");
            }
        }

        //Construtor apenas com código da cidade, usado para pesquisa
        public Cidade(int codC)
        {
            Codigo = codC;
        }

        //Propriedade inteira Codigo, com getter público e setter privado da classe
        //Lança exceção se o valor for menor do que 0
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

        //Propriedade string Nome, com getter público e setter privado
        //Joga exceção se string for nula ou uma cadeia vazia
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

        //Propriedade inteira X, com getter e setter
        //Joga exceção de valor for menor do que 0
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

        //Propriedade inteira Y, com getter e setter
        //Joga exceção de valor for menor do que 0
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
        
        //Método que retorna número inteiro que representa a diferença de código de this menos código de c
        public int CompareTo(Cidade c)
        {
            return codigo - c.codigo;
        }

        //Método que retorna string da cidade com seu código e seu nome
        public override string ToString()
        {
            return $"{codigo} - {nome}";
        }
    }
}
