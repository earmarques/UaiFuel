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

Neste _namespace_ ficam as classes dos objetos que representam as entidades do domínio, a versão orientada a objetos das entidades do modelo relacional do banco de dados. A sua maioria são POCO, _Plain Old Class Object_, o mesmo conceito que POJO (_Plain Old Java Object_), porém a Microsoft teve por política limpar qualquer vestígio Java e cunhou um nome mais neutro. A diferença entre POCO e DTO (_Data Transfer Object_) é que DTO só tem estado, enquanto POCO tem estado e comportamento.

As classes que não são POCO são a interface `IDomainObject` usada para tipagem genérica no namespace DAO e os _singletons_ criados para os status: `Status`, `StatusVeiculo`, `StatusCombustivel` e `StatusAbastecimento`. Na  CLR (_Common Language Runtime_) do .NET _enumerations_ são apenas constantes nomeadas, cujo tipo subjacente deve ser inteiro. Optamos por criar ___instâncias de tipo___ nomeadas, assim podemos criar _enums_ mais complexos, com vários tipos associados e ainda com comportamento. Nossa classe _enum_ foi feita com o padrão de projeto _Singleton_ - selamos a classe para impedir que seja estendida (_sealed_) por mecanismo de herança e fizemos o construtor privado para que ninguém, além da própria classe, possa instanciar objetos. 

A classe possui duas propriedades públicas de instância, `Codigo` e `Descricao`, mas apenas com acesso para leitura (`get`), o que impede eventual corrupção do seu valor. As instâncias nomeadas são atributos de classe (_static_) e são guardadas em uma lista _static_. Essa lista pode ser usada para carregar uma caixa de seleção suspensa(_ComboBox_), por exemplo. Podemos acessar os singletons diretamente pelo nome da classes (´StatusVeiculo.GARAGEM´, ´StatusVeiculo.MANUTENCAO´), ou pedindo ao singleton para buscar na lista, fornecendo o código ao método `StatusVeiculo.getInstance(codigo)`.

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

### DAO

Não usamos nenhum _framework_ de mapeamento objeto-relacional, tinhamos menos de 3 meses para codificar e não daria tempo de estudar os detalhes do framework, então usamos o padrão DAO - _Data Access Object_. Seguindo o padrão, criamos uma classe abstrata para fazer a conexão com o banco e que todos os DAO's que manipulam os objetos de domínio devem estender. 

Os objetos DAO tem a responsabilidade de acessar e manipular os objetos de domínio. O acrônimo CRUD (`Create`, `Read`, `Update` , Delete`) enumera as ações essencias que deve executar sobre dos dados das entidades persistidas no banco de dados: criar, ler e listar, atualizar e apagar. Apesar de vermos alguns projetos com regras de negócio espalhadas em parte na camada _Service_ e parte na camada DAO, dentro do que estudamos nós entendemos como sendo um erro. A programação orientada a objetos orienta o isolamento de responsabilidade, e a camada DAO não deve ter nenhuma regra de negócio, ela é responsável exclusivamente por fazer a comunicação entre o banco de dados e a aplicação. Qualquer exceção lançada na execução dos métodos do DAO não devem ser tratadas por ele, quem deve ter a sapiência de avaliar a gravidade da exception e dar o devido tratamento é a camada de serviço.  

O que queremos chamar a atenção do leitor é na interface `IDAO`. Esta interface faz uso de `Generics` para fazer acoplamento forte na tipagem entre o DAO e o objeto de domínio que manipula.

```C#
namespace UaiFuel.Models.DAO
{
    public interface IDAO<T> where T : IDomainObject, new()
    {
        public T Create(T domainObject);

        public void Update(T domainObject);

        public void Delete(T domainObject);

        public IList<T> Read();

        public T Read(object pk);
    }
}
```

No namespace `Domain` 









