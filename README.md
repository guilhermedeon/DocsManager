## Sobre o projeto:

### Tecnologias utilizadas:

* .NET 8.0 / Asp.NET Core
* EF Core -> Code-First com Migrations
* HTML/CSS/JS + Bootstrap (deploy com NGINX)
* Docker
* xUnit
* JWT

### Decisões técnicas:

* *Deploy completo com docker-compose* -> Facilidade no deploy e possibilidade de ter um ambiente rodando em qualquer lugar com 1 comando.
* *JWT simplificado* -> Contém as funcionalidades do JWT, mas sem usuário e senha por questão de simplicidade e facilidade no uso.
* *Clean Architecture* -> Não necessária para o projeto devido ao tamanho, mas mesmo assim decidi seguir este padrão para demonstração.
* *Testes unitários* -> Utilizei o xUnit e o Mok para criar os testes, acabei incluindo somente aonde pareceu fazer mais sentido por fins de simplicidade.
* *Muitos commits* -> Para auxiliar a demonstrar a história de projeto e o passo a passo.

### Instruções de uso

1 - Clonar o repositório

2 - Aplicar o comando `docker-compose up --build` para subir os containers

3 - Acessar a aplicação front em `http://localhost:8080` ou então o backend (que possui interface swagger) em `http://localhost:5000/swagger/index.html`

### Feedback

Projeto "simples" mas que possibilita o emprego de diversas tecnologias e boas práticas.

Caso tenha alguma dica ou feedback, por favor, entre em contato. Um bom feedback vale mais que 10 video-aulas.