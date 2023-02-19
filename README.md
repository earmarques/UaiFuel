UaiFuel é um gerenciador de abastecimentos para postos de gasolina.

### Contexto

Projeto Inter.

---

# Arquitetura

O Microsoft Visual Studio tem um _template_ de projeto MVC (_Model-View-Controller_) para C# que utilizamos como base para o nosso projeto. 

## View
Os arquivos de estilização e javascript ficam na pasta `wwwroot`. Na pasta `Views` ficam os arquivos _Razor_ (cshtml) com seus _scriptlets_ de servidor embutido, usado pelo VB.NET para gerar as páginas html e fazer a interação com o usuário; basicamente teremos um arquivo cshtml para cada ação do controlador. 

## Controller
Na pasta `Controller` temos os controladores da aplicação, onde são definidas as ações (`ActionResult`) que consomem os serviços da camada `Model` e direcionam o fluxo da aplicação. 

![Figura: Estrutura do Projeto](images/arquitetura.png)<br>
_Figura: Estrutura do Projeto_

## Model

Esta é camada do MVC mais interessante e a que iremos aprofundar, porque representa o negócio e é a camada responsável pelo acesso e manipulação dos dados. A camada `Model` está subdivida em vários `namespaces` ou pastas para agrupar funcionalidades em comum. 

![Figura: namespace Domain](images/domain.png)<br>
_Figura: namespace Domain_

### Domain

Neste _namespace_ ficam as classes dos objetos que representam as entidades do domínio, a versão orientada a objetos das entidades do modelo relacional do banco de dados. A sua maioria são POCO, _Plain Old Class Object_, o mesmo conceito que POJO (Plain Old Java Object), porém a Microsoft teve por política limpar qualquer vestígio Java e cunhou um nome mais neutro. A diferença entre POCO e DTO (Data Transfer Object) é que DTO só tem estado, enquanto POCO tem estado e comportamento.

As classes que não são POCO são os singletons criados para os status: `Status`, `StatusVeiculo`, `StatusCombustivel` e `StatusAbastecimento`. No  CLR (_Common Language Runtime_) do .NET _enumerations_ são apenas constantes nomeadas, cujo tipo subjacente deve ser inteiro. Optamos por criar instâncias de tipo nomeadas, assim podemos criar enumerations mais complexos, com vários tipos associados e ainda com comportamento. Nossa classe enum foi feita com o padrão de projeto Singleton - selamos a classe para impedir que seja estendida (_sealed_) e e fizemos o construtor privado para que ninguém, além da própria classe, possa instanciar objetos. A classe possui duas propriedades públicas de instância, `Codigo` e `Descricao`, mas apenas com acesso para leitura (`get`), o que impede eventual corrupção do seu valor. As instâncias nomeadas são atributos de classe (static) e são guardadas em uma lista _static_. Essa lista pode ser usada para carregar uma caixa de seleção suspensa(_ComboBox_), por exemplo. Podemos acessar os singletons diretamente pelo nome da classes (´StatusVeiculo.GARAGEM´, ´StatusVeiculo.MANUTENCAO´) ou pedindo ao singleton para buscar na lista fornecendo o código ao método `getInstance`.

```c#
namespace UaiFuel.Models.Domain
{
    /** 
     * Singleton Pattern 
     */
   sealed public class StatusVeiculo
    {
        public int Codigo { get; }
        public string Descricao { get; }

        static public IList<StatusVeiculo> lista = new List<StatusVeiculo>();

        static public StatusVeiculo ATIVO       = new StatusVeiculo(1, "Ativo");
        static public StatusVeiculo MANUTENCAO  = new StatusVeiculo(2, "Em Manutenção");
        static public StatusVeiculo GARAGEM     = new StatusVeiculo(3, "Garagem");
        static public StatusVeiculo VENDIDO     = new StatusVeiculo(4, "Vendido");

        private StatusVeiculo(int codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
            lista.Add(this);
        }

        static public StatusVeiculo getInstance(int codigo)
        {
            foreach (StatusVeiculo s in lista)
            {
                if (s.Codigo == codigo)
                {
                    return s;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return $"{Codigo} - {Descricao}";
        }
    }
    ```


