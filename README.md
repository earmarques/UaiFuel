# UaiFuel
Gerenciador de abastecimento para postos de gasolina.

### Contexto

Projeto Inter.


## Arquitetura

O Microsoft Visual Studio tem um _template_ de projeto MVC (_Model-View-Controller_) para C# que utilizamos como base para nosso projeto. Os arquivos de estilização e javascript ficam na pasta `wwwroot`. Na pasta `Views` ficam os arquivos _Razor_ (cshtml) com seus _scriptlets_ de servidor embutido, usado pelo VB.NET para gerar as páginas html para a interação com o usuário; basicamente teremos um arquivo cshtml para cada ação do controlador. Na pasta `Controller` temos os controladores da aplicação, onde são definidas as ações (`ActionResult`) que consomem os serviços da camada `Model` e direcionam o fluxo da aplicação. A camada do MVC mais interessante e que iremos nos deter é a `Model`, porque representa o negócio e é a camada responsável pelo acesso e manipulação dos dados.

![Figura: Estrutura do Projeto](images/arquitetura.png)

