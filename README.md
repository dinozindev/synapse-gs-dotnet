# Synapse - API - Global Solution

## Integrantes

- Giovanna Revito Roz - RM558981
- Kaian Gustavo de Oliveira Nascimento - RM558986
- Lucas Kenji Kikuchi - RM554424

## Descrição do Projeto

O Synapse é uma plataforma inteligente voltada para orientação profissional e bem-estar. O usuário — esteja trabalhando, estudando ou em transição — informa sua área atual, sua área de interesse dentro do universo de desenvolvimento (como Front-end, Back-end, DevOps, IA, entre outras) e suas competências. Com esses dados, o sistema utiliza uma API de Inteligência Artificial para recomendar vagas, cursos e oportunidades de capacitação alinhados ao perfil e aos objetivos do usuário.

Além disso, o Synapse oferece um módulo de bem-estar, onde o usuário pode registrar diariamente informações como horas de sono, horas de trabalho, nível de estresse, nível de energia e humor. A plataforma envia esse histórico para a IA, que analisa padrões e fornece insights e sugestões personalizadas para melhorar a saúde e o equilíbrio do usuário.

Em resumo, o Synapse combina orientação profissional e gestão de bem-estar em um único ambiente, usando IA para oferecer recomendações realmente úteis, tanto para a carreira quanto para a qualidade de vida.

## Justificativa da Arquitetura

Optamos por utilizar **ASP.NET Core com Minimal APIs** pela simplicidade na definição de rotas e menor boilerplate em comparação com Controllers tradicionais.  

A separação em **camadas (Models, DTOs, Services, Examples e Endpoints)** garante melhor manutenção e testabilidade do código.  

A escolha do **Entity Framework Core** com banco Oracle se deu por facilitar o mapeamento objeto-relacional, reduzindo código de SQL manual. 

## Diagrama da Arquitetura

![AppScreenshot](https://imgur.com/aLa4y1J.png)

## Instalação

### Instalação e Execução da API (.NET 9)
#### 📋 Pré-requisitos
Antes de instalar, verifique se os seguintes itens estão instalados:

- .NET 9 SDK

- Oracle Database ou acesso a um banco Oracle

- Oracle Entity Framework Core Provider

- Visual Studio 2022+ ou Rider (opcional)

- Git (opcional)

### Clone o repositório e acesse o diretório:

```bash
git clone https://github.com/dinozindev/synapse-gs-dotnet.git
cd synapse-gs-dotnet
```

### Instale as dependências:
```bash
dotnet restore
```

### Para acessar a pasta da API:
```
cd GlobalSolution2
```

### Se deseja utilizar o banco de dados Oracle já desenvolvido (com todos os inserts), insira a linha abaixo em um arquivo .env dentro de GlobalSolution2:
```code
ConnectionStrings__OracleConnection=User Id=RM558986;Password=fiap25;Data Source=oracle.fiap.com.br:1521/orcl;
```

### Se deseja utilizar o próprio banco de dados Oracle, substitua o id e senha com suas credenciais:
```code
ConnectionStrings__OracleConnection=User Id=<id>;Password=<senha>;Data Source=oracle.fiap.com.br:1521/orcl;
```

### No mesmo arquivo .env, adicione as seguintes linhas para utilizar a autenticação JWT:
```code
JwtSettings__Secret=m4XzF02r5UtGBqDsuSHsV1b1a+y8U8hD7AGx4a5Bv0E=
JwtSettings__Issuer=SynapseAPI
JwtSettings__Audience=SynapseUsers
JwtSettings__ExpirationMinutes=60
```

### E execute para criar as tabelas (caso esteja usando seu próprio banco): 
```bash
dotnet ef database update
```

### Inicie a aplicação: 
```bash
dotnet run
```

### Para acessar a documentação da aplicação (Swagger): 
```bash
http://localhost:5141/swagger
```


## Versionamento

#### A API conta com duas versões diferentes:

| Versão | Caminho Base | Autenticação | Descrição |
|:-------|:--------------|:--------------|:------------|
| **v1** | `/api/v1` | ❌ Não requer JWT | Primeira versão com endpoints acessíveis sem autenticação. |
| **v2** | `/api/v2` | ✅ Requer JWT | Segunda versão protegida por autenticação JWT, com maior segurança. |

#### 🌐 Exemplos de uso

```bash
v1 - acesso público
GET /api/v1/usuarios/1

v2 - acesso autenticado
GET /api/v2/usuarios/1
Authorization: Bearer <seu_token_jwt>
```

## Autenticação JWT

#### Para conseguir acessar os endpoints da API v2, é necessário realizar o Login para obter o Token de autenticação JWT:

```http
POST /api/v2/auth/login
```

#### Utilize as seguintes credenciais no Request Body para autenticação:

```json
{
  "nomeUsuario": "maria.silva",
  "senhaUsuario": "senha123"
}
```

#### Você obterá um Response Body contendo o Token de autenticação, que poderá ser utilizado para acessar os endpoints da API v2:

Exemplo de retorno:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InJvZHJpZ28ubmV2ZXMiLCJyb2xlIjoiZ2VyZW50ZSIsIm5iZiI6MTc2MTE2NzMzOSwiZXhwIjoxNzYxMTcwOTM5LCJpYXQiOjE3NjExNjczMzksImlzcyI6Ik1vdHR1TW90dGlvbkFQSSIsImF1ZCI6Ik1vdHR1TW90dGlvbkNsaWVudHMifQ.RUsg9P7MHebgXfe3NdhBTqL94Ce-rdnBo15mfDVUPhg",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```

Exemplo de requisição com CURL:
```
curl -X 'GET' \
  'http://localhost:5147/api/v2/usuarios/1' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InJvZHJpZ28ubmV2ZXMiLCJyb2xlIjoiZ2VyZW50ZSIsIm5iZiI6MTc2MTE2NzMzOSwiZXhwIjoxNzYxMTcwOTM5LCJpYXQiOjE3NjExNjczMzksImlzcyI6Ik1vdHR1TW90dGlvbkFQSSIsImF1ZCI6Ik1vdHR1TW90dGlvbkNsaWVudHMifQ.RUsg9P7MHebgXfe3NdhBTqL94Ce-rdnBo15mfDVUPhg'
```

## Testes Unitários

#### Para realizar todos os testes, certifique-se de estar na raiz do projeto, e execute o seguinte comando:
```
dotnet test
```

## Rotas da API

### Parâmetros de Rotas Paginadas (aplicável a todas)

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `pageNumber`      | `int` | **Obrigatório**. O número da página atual |
| `pageSize`      | `int` | **Obrigatório**. A quantidade de registros por página |

### Health Checks

- #### Retorna o Health Check do Banco de dados
```
GET /api/health-checks/database
```

Response Body:
```
{
  "status": "Healthy",
  "totalDuration": "00:00:00.5931808",
  "entries": {
    "oracle-database": {
      "data": {

      },
      "duration": "00:00:00.5899727",
      "status": "Healthy",
      "tags": [
        "db",
        "oracle",
        "sql"
      ]
    }
  }
}
```

### Auth / Login

- #### Faz login e retorna um token JWT

```
POST /api/v2/auth/login
```

Request Body:
```
{
  "nomeUsuario": "maria.silva",
  "senhaUsuario": "senha123"
}
```

Response Body:
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InJvZHJpZ28ubmV2ZXMiLCJyb2xlIjoiZ2VyZW50ZSIsIm5iZiI6MTc2MTE2NzMzOSwiZXhwIjoxNzYxMTcwOTM5LCJpYXQiOjE3NjExNjczMzksImlzcyI6Ik1vdHR1TW90dGlvbkFQSSIsImF1ZCI6Ik1vdHR1TW90dGlvbkNsaWVudHMifQ.RUsg9P7MHebgXfe3NdhBTqL94Ce-rdnBo15mfDVUPhg",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```

- #### Renova um token JWT expirado
```
POST /api/v2/auth/refresh-token
```

Response Body:
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InJvZHJpZ28ubmV2ZXMiLCJyb2xlIjoiZ2VyZW50ZSIsIm5iZiI6MTc2MTE3ODk4MywiZXhwIjoxNzYxMTgyNTgzLCJpYXQiOjE3NjExNzg5ODMsImlzcyI6Ik1vdHR1TW90dGlvbkFQSSIsImF1ZCI6Ik1vdHR1TW90dGlvbkNsaWVudHMifQ.2qzWCIELDHVAK_U94G3u2iNnpYE8AKcRm5nGlN6Ex7I",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```

### Usuários

- #### Retorna todos os usuários

```http
  GET /api/v2/usuarios?pageNumber=&pageSize=
```

Response Body:

```json
{
  "totalCount": 3,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "data": [
    {
      "usuarioId": 1,
      "nomeUsuario": "maria.silva",
      "senhaUsuario": "senha123",
      "areaAtual": "Suporte Técnico",
      "areaInteresse": "DevOps",
      "objetivoCarreira": "Migrar para área de infraestrutura e automação",
      "nivelExperiencia": "Júnior",
      "competencias": []
    },
    {
      "usuarioId": 2,
      "nomeUsuario": "joao.santos",
      "senhaUsuario": "pass456",
      "areaAtual": "Analista de Sistemas",
      "areaInteresse": "Data Science",
      "objetivoCarreira": "Tornar-me cientista de dados especializado em IA",
      "nivelExperiencia": "Pleno",
      "competencias": []
    },
    {
      "usuarioId": 3,
      "nomeUsuario": "ana.costa",
      "senhaUsuario": "secure789",
      "areaAtual": "Designer Gráfico",
      "areaInteresse": "UX/UI",
      "objetivoCarreira": "Transição para design de experiência do usuário",
      "nivelExperiencia": "Júnior",
      "competencias": []
    }
  ],
  "links": [
    {
      "rel": "self",
      "href": "/usuarios?pageNumber=1&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "/usuarios?pageNumber=2&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "prev",
      "href": "",
      "method": "GET"
    }
  ]
}
```

Códigos de Resposta

| Código HTTP | Significado                     | Quando ocorre                                             |
|-------------|----------------------------------|-----------------------------------------------------------|
| 200 OK      | Requisição bem-sucedida         | Quando há usuários cadastrados                            |
| 204 No Content | Sem conteúdo a retornar      | Quando não há usuários cadastrados                        |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 500 Internal Server Error | Erro interno     | Quando ocorre uma falha inesperada no servidor            |

- #### Retorna um usuário pelo ID

```http
  GET /api/v2/usuarios/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do usuário que você deseja consultar |

Response Body:

```json
{
  "data": {
    "usuarioId": 1,
    "nomeUsuario": "maria.silva",
    "senhaUsuario": "senha123",
    "areaAtual": "Suporte Técnico",
    "areaInteresse": "DevOps",
    "objetivoCarreira": "Migrar para área de infraestrutura e automação",
    "nivelExperiencia": "Júnior",
    "competencias": []
  },
  "links": [
    {
      "rel": "self",
      "href": "/usuarios/1",
      "method": "GET"
    },
    {
      "rel": "update",
      "href": "/usuarios/1",
      "method": "PUT"
    },
    {
      "rel": "delete",
      "href": "/usuarios/1",
      "method": "DELETE"
    },
    {
      "rel": "list",
      "href": "/usuarios",
      "method": "GET"
    }
  ]
}
```

Códigos de Resposta

| Código HTTP | Significado                     | Quando ocorre                                             |
|-------------|----------------------------------|-----------------------------------------------------------|
| 200 OK      | Requisição bem-sucedida         | Quando o usuário foi encontrado                            |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        | Quando o usuário especificado não existe       |
| 500 Internal Server Error | Erro interno     | Quando ocorre uma falha inesperada no servidor            |

- #### Cria um usuário

```http
  POST /api/v2/usuarios
```

Request Body:

```json
{{
  "nomeUsuario": "",
  "senhaUsuario": "",
  "areaAtual": "",
  "areaInteresse": "",
  "objetivoCarreira": "",
  "nivelExperiencia": ""
}
```

Exemplo:
```json
{
  "nomeUsuario": "jorge.roberto",
  "senhaUsuario": "jorge12345",
  "areaAtual": "Frentista",
  "areaInteresse": "Back-end Java",
  "objetivoCarreira": "Transição para Aplicações Back-end com Java e Spring Boot",
  "nivelExperiencia": "Nenhuma"
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando um usuário é criado com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 409 Conflict      | Conflito de estado              | Quando há conflito, como dados duplicados (Nome de usuário)                     |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Atualiza um usuário

```http
  PUT /api/v2/usuarios/{id}
```

Request Body:

```json
{{
  "nomeUsuario": "",
  "senhaUsuario": "",
  "areaAtual": "",
  "areaInteresse": "",
  "objetivoCarreira": "",
  "nivelExperiencia": ""
}
```

Exemplo:
```json
{
  "nomeUsuario": "jorge.dias.freitas",
  "senhaUsuario": "jorge123456",
  "areaAtual": "Frentista",
  "areaInteresse": "Back-end Java",
  "objetivoCarreira": "Transição para Aplicações Back-end com Java e Spring Boot",
  "nivelExperiencia": "Nenhuma"
}
```


| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do usuário que você atualizar |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando um usuário é criado com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 409 Conflict      | Conflito de estado              | Quando há conflito, como dados duplicados (Nome do usuário)                     |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Deleta um usuário

```http
  DELETE /api/v2/usuarios/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do usuário que você deseja deletar |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 204 No Content    | Sem conteúdo a retornar         | Quando a remoção do usuário é válida, mas não há dados para retornar   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando o usuário especificado não é encontrado                |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Adiciona uma competência ao Usuário

```http
  POST /api/v2/usuarios/{usuarioId}/adicionar-competencia/{competenciaId}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `usuarioId`      | `int` | **Obrigatório**. O ID do usuário que você deseja ter uma competência associada |
| `competenciaId`      | `int` | **Obrigatório**. O ID da competência você deseja associar a um usuário |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 204 No Content    | Sem conteúdo a retornar         | Quando a adição da associação é válida, mas não há dados para retornar   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando o usuário e competência especificados não são encontrados               |
| 409 Conflict     | Conflito de estado          | Quando o usuário e competência especificados já estão associados um ao outro             |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Remove uma competência do Usuário

```http
  DELETE /api/v2/usuarios/{usuarioId}/remover-competencia/{competenciaId}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `usuarioId`      | `int` | **Obrigatório**. O ID do usuário que você deseja ter uma competência removida |
| `competenciaId`      | `int` | **Obrigatório**. O ID da competência você deseja remover do usuário |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 204 No Content    | Sem conteúdo a retornar         | Quando a remoção da associação é válida, mas não há dados para retornar   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando o usuário e competência especificados não são encontrados               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Competências

- #### Retorna todas as competências

```http
  GET /api/v2/competencias?pageNumber=&pageSize=
```

Response Body:

```json
{
  "totalCount": 3,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "data": [
    {
      "competenciaId": 1,
      "nomeCompetencia": "Python",
      "categoriaCompetencia": "Back-end",
      "descricaoCompetencia": "Linguagem versátil para desenvolvimento e ciência de dados"
    },
    {
      "competenciaId": 2,
      "nomeCompetencia": "JavaScript",
      "categoriaCompetencia": "Front-end",
      "descricaoCompetencia": "Linguagem essencial para desenvolvimento web"
    },
    {
      "competenciaId": 3,
      "nomeCompetencia": "React",
      "categoriaCompetencia": "Front-end",
      "descricaoCompetencia": "Biblioteca moderna para interfaces de usuário"
    }
  ],
  "links": [
    {
      "rel": "self",
      "href": "/competencias?pageNumber=1&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "/competencias?pageNumber=2&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "prev",
      "href": "",
      "method": "GET"
    }
  ]
}
```

Códigos de Resposta

| Código HTTP | Significado                     | Quando ocorre                                             |
|-------------|----------------------------------|-----------------------------------------------------------|
| 200 OK      | Requisição bem-sucedida         | Quando há competências cadastradas                            |
| 204 No Content | Sem conteúdo a retornar      | Quando não há competências cadastradas                        |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 500 Internal Server Error | Erro interno     | Quando ocorre uma falha inesperada no servidor            |

- #### Retorna uma competência pelo ID

```http
  GET /api/v2/competencias/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da competência que você deseja consultar |

```json
{
  "data": {
    "competenciaId": 1,
    "nomeCompetencia": "Python",
    "categoriaCompetencia": "Back-end",
    "descricaoCompetencia": "Linguagem versátil para desenvolvimento e ciência de dados"
  },
  "links": [
    {
      "rel": "self",
      "href": "/competencias/1",
      "method": "GET"
    },
    {
      "rel": "update",
      "href": "/competencias/1",
      "method": "PUT"
    },
    {
      "rel": "delete",
      "href": "/competencias/1",
      "method": "DELETE"
    },
    {
      "rel": "list",
      "href": "/competencias",
      "method": "GET"
    }
  ]
}
```
Códigos de Resposta

| Código HTTP | Significado                     | Quando ocorre                                             |
|-------------|----------------------------------|-----------------------------------------------------------|
| 200 OK      | Requisição bem-sucedida         | Quando a competência foi encontrada                            |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        | Quando a competência especificada não existe       |
| 500 Internal Server Error | Erro interno     | Quando ocorre uma falha inesperada no servidor            |


- #### Cria uma competência para um usuário 

```http
  POST /api/v2/competencias/{usuarioId}
```

Request Body:

```json
{
  "nomeCompetencia": "",
  "categoriaCompetencia": "",
  "descricaoCompetencia": ""
}
```

Exemplo: 

```json
{
  "nomeCompetencia": "Flutter",
  "categoriaCompetencia": "Front-end",
  "descricaoCompetencia": "Kit de desenvolvimento de software de interface de usuário"
}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `usuarioId`      | `int` | **Obrigatório**. O ID do usuário que você deseja adicionar uma competência nova |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando uma competência é criada com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Atualiza uma competência 

```http
  PUT /api/v2/competencias/{id}
```

Request Body:

```json
{
  "nomeCompetencia": "",
  "categoriaCompetencia": "",
  "descricaoCompetencia": ""
}
```

Exemplo: 

```json
{
  "nomeCompetencia": "Swift",
  "categoriaCompetencia": "Front-end",
  "descricaoCompetencia": "Kit de desenvolvimento de software de interface de usuário"
}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da competência que você deseja atualizar |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK       | Requisição bem-sucedida      | Quando uma competência é atualizada com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhuma competência foi encontrada com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Remove uma competência

```http
  DELETE /api/v2/competencias/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da competência que você deseja remover |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 204 No Content    | Sem conteúdo a retornar         | Quando a remoção da competência é válida, mas não há dados para retornar   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando a competência não é encontrada                 |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |


### Registros de Bem Estar

- #### Retorna a lista de registros de bem estar

```http
  GET /api/v2/registros-bem-estar?pageNumber=&pageSize=
```

Response Body: 

```json
{
  "totalCount": 3,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "data": [
    {
      "registroId": 1,
      "dataRegistro": "2025-11-13T19:51:41.9082713Z",
      "humorRegistro": "Estressado",
      "horasSono": 6,
      "horasTrabalho": 10,
      "nivelEnergia": 5,
      "nivelEstresse": 8,
      "observacaoRegistro": "Muita demanda no trabalho",
      "usuario": {
        "usuarioId": 1,
        "nomeUsuario": "maria.silva",
        "areaAtual": "Suporte Técnico",
        "areaInteresse": "DevOps",
        "objetivoCarreira": "Migrar para área de infraestrutura e automação",
        "nivelExperiencia": "Júnior"
      }
    },
    {
      "registroId": 2,
      "dataRegistro": "2025-11-13T19:51:41.9082716Z",
      "humorRegistro": "Calmo",
      "horasSono": 7,
      "horasTrabalho": 8,
      "nivelEnergia": 7,
      "nivelEstresse": 5,
      "observacaoRegistro": "Dia mais tranquilo",
      "usuario": {
        "usuarioId": 1,
        "nomeUsuario": "maria.silva",
        "areaAtual": "Suporte Técnico",
        "areaInteresse": "DevOps",
        "objetivoCarreira": "Migrar para área de infraestrutura e automação",
        "nivelExperiencia": "Júnior"
      }
    },
    {
      "registroId": 3,
      "dataRegistro": "2025-11-13T19:51:41.9082717Z",
      "humorRegistro": "Feliz",
      "horasSono": 8,
      "horasTrabalho": 7,
      "nivelEnergia": 8,
      "nivelEstresse": 4,
      "observacaoRegistro": "Finalizei projeto importante",
      "usuario": {
        "usuarioId": 1,
        "nomeUsuario": "maria.silva",
        "areaAtual": "Suporte Técnico",
        "areaInteresse": "DevOps",
        "objetivoCarreira": "Migrar para área de infraestrutura e automação",
        "nivelExperiencia": "Júnior"
      }
    }
  ],
  "links": [
    {
      "rel": "self",
      "href": "/registros-bem-estar?pageNumber=1&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "/registros-bem-estar?pageNumber=2&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "prev",
      "href": "",
      "method": "GET"
    }
  ]
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando os registros são encontrados                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhum registro existe  |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna um registro de bem estar a partir de um ID

```http
  GET /api/v2/registros-bem-estar/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do registro que deseja consultar |

Response Body: 

```json
{
  "data": {
    "registroId": 1,
    "dataRegistro": "2025-11-13T19:51:41.9107999Z",
    "humorRegistro": "Estressado",
    "horasSono": 6,
    "horasTrabalho": 10,
    "nivelEnergia": 5,
    "nivelEstresse": 8,
    "observacaoRegistro": "Muita demanda no trabalho",
    "usuario": {
      "usuarioId": 1,
      "nomeUsuario": "maria.silva",
      "areaAtual": "Suporte Técnico",
      "areaInteresse": "DevOps",
      "objetivoCarreira": "Migrar para área de infraestrutura e automação",
      "nivelExperiencia": "Júnior"
    }
  },
  "links": [
    {
      "rel": "self",
      "href": "/registros-bem-estar/1",
      "method": "GET"
    },
    {
      "rel": "update",
      "href": "/registros-bem-estar/1",
      "method": "PUT"
    },
    {
      "rel": "delete",
      "href": "/registros-bem-estar/1",
      "method": "DELETE"
    },
    {
      "rel": "list",
      "href": "/registros-bem-estar",
      "method": "GET"
    }
  ]
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando o registro é encontrado                     |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando o registro especificado não é encontrado               |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna a lista de registros de bem estar de um usuário específico

```http
  GET /api/v2/registros-bem-estar/registros-usuario/{usuarioId}?pageNumber=&pageSize=
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `usuarioId`      | `int` | **Obrigatório**. O ID do usuário que deseja consultar os registros |

Response Body: 

```json
{
  "totalCount": 3,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "data": [
    {
      "registroId": 1,
      "dataRegistro": "2025-11-13T20:29:18.9486313Z",
      "humorRegistro": "Estressado",
      "horasSono": 6,
      "horasTrabalho": 10,
      "nivelEnergia": 5,
      "nivelEstresse": 8,
      "observacaoRegistro": "Muita demanda no trabalho"
    },
    {
      "registroId": 2,
      "dataRegistro": "2025-11-13T20:29:18.9486315Z",
      "humorRegistro": "Calmo",
      "horasSono": 7,
      "horasTrabalho": 8,
      "nivelEnergia": 7,
      "nivelEstresse": 5,
      "observacaoRegistro": "Dia mais tranquilo"
    },
    {
      "registroId": 3,
      "dataRegistro": "2025-11-13T20:29:18.9486316Z",
      "humorRegistro": "Feliz",
      "horasSono": 8,
      "horasTrabalho": 7,
      "nivelEnergia": 8,
      "nivelEstresse": 4,
      "observacaoRegistro": "Finalizei projeto importante"
    }
  ],
  "links": [
    {
      "rel": "self",
      "href": "/registros-bem-estar?pageNumber=1&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "/registros-bem-estar?pageNumber=2&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "prev",
      "href": "",
      "method": "GET"
    }
  ]
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando os registros são encontrados                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhum registro existe  |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Cria um registro de bem estar 

```http
  POST /api/v2/registros-bem-estar
```

Request Body:

```json
{
  "dataRegistro": "",
  "humorRegistro": "",
  "horasSono": 0,
  "horasTrabalho": 0,
  "nivelEnergia": 0,
  "nivelEstresse": 0,
  "observacaoRegistro": "",
  "usuarioId": 0
}
```

Exemplo: 

```json
{
  "dataRegistro": "2025-11-13T20:29:18.9479977Z",
  "humorRegistro": "Feliz",
  "horasSono": 9,
  "horasTrabalho": 6,
  "nivelEnergia": 8,
  "nivelEstresse": 4,
  "observacaoRegistro": "Finalizei as demandas no trabalho",
  "usuarioId": 1
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando um registro é criado com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Atualiza um registro de bem estar 

```http
  PUT /api/v2/registros-bem-estar/{id}
```

Request Body:

```json
{
  "humorRegistro": "",
  "horasSono": 0,
  "horasTrabalho": 0,
  "nivelEnergia": 0,
  "nivelEstresse": 0,
  "observacaoRegistro": ""
}
```

Exemplo: 

```json
{
  "humorRegistro": "Bravo",
  "horasSono": 5,
  "horasTrabalho": 9,
  "nivelEnergia": 5,
  "nivelEstresse": 9,
  "observacaoRegistro": "Extremamente incomodado com certos comportamentos no trabalho"
}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do registro de bem estar que você deseja atualizar |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK       | Requisição bem-sucedida      | Quando um registro é atualizado com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum registro foi encontrado com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Remove um registro de bem estar

```http
  DELETE /api/v2/registros-bem-estar/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID do registro que você deseja remover |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 204 No Content    | Sem conteúdo a retornar         | Quando a remoção do registro é válido, mas não há dados para retornar   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando o registro não é encontrado                |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Recomendações Profissionais

- #### Retorna a lista de recomendações profissionais

```http
  GET /api/v2/recomendacoes/profissional?pageNumber=&pageSize=
```

Response Body: 

```json
{
  "totalCount": 2,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "data": [
    {
      "recomendacaoId": 1,
      "dataRecomendacao": "2025-11-13T20:43:37.1949089Z",
      "tituloRecomendacao": "Vaga Front-end Júnior",
      "descricaoRecomendacao": "Oportunidade para desenvolvedor front-end iniciante",
      "categoriaRecomendacao": "Vaga",
      "areaRecomendacao": "Front-end",
      "fonteRecomendacao": "LinkedIn",
      "usuario": {
        "usuarioId": 1,
        "nomeUsuario": "maria.silva",
        "areaAtual": "Suporte Técnico",
        "areaInteresse": "DevOps",
        "objetivoCarreira": "Migrar para área de infraestrutura e automação",
        "nivelExperiencia": "Júnior"
      }
    },
    {
      "recomendacaoId": 2,
      "dataRecomendacao": "2025-11-13T20:43:37.1949977Z",
      "tituloRecomendacao": "Curso de Back-end com Spring Boot",
      "descricaoRecomendacao": "Aprofunde seus conhecimentos em APIs Java",
      "categoriaRecomendacao": "Curso",
      "areaRecomendacao": "Back-end",
      "fonteRecomendacao": "Alura",
      "usuario": {
        "usuarioId": 1,
        "nomeUsuario": "maria.silva",
        "areaAtual": "Suporte Técnico",
        "areaInteresse": "DevOps",
        "objetivoCarreira": "Migrar para área de infraestrutura e automação",
        "nivelExperiencia": "Júnior"
      }
    }
  ],
  "links": [
    {
      "rel": "self",
      "href": "/recomendacoes/profissional?pageNumber=1&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "/recomendacoes/profissional?pageNumber=2&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "prev",
      "href": "",
      "method": "GET"
    }
  ]
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando as recomendações são encontradas                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhuma recomendação existe  |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna uma recomendação profissional a partir de um ID

```http
  GET /api/v2/recomendacoes/profissional/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da recomendação profissional que deseja consultar |

Response Body: 

```json
{
  "data": {
    "recomendacaoId": 1,
    "dataRecomendacao": "2025-11-13T20:45:57.6697486Z",
    "tituloRecomendacao": "Vaga Front-end Júnior",
    "descricaoRecomendacao": "Oportunidade para desenvolvedor front-end iniciante",
    "categoriaRecomendacao": "Vaga",
    "areaRecomendacao": "Front-end",
    "fonteRecomendacao": "LinkedIn",
    "usuario": {
      "usuarioId": 1,
      "nomeUsuario": "maria.silva",
      "areaAtual": "Suporte Técnico",
      "areaInteresse": "DevOps",
      "objetivoCarreira": "Migrar para área de infraestrutura e automação",
      "nivelExperiencia": "Júnior"
    }
  },
  "links": [
    {
      "rel": "self",
      "href": "/recomendacoes/profissional/1",
      "method": "GET"
    },
    {
      "rel": "delete",
      "href": "/recomendacoes/profissional/1",
      "method": "DELETE"
    },
    {
      "rel": "list",
      "href": "/recomendacoes/profissional",
      "method": "GET"
    }
  ]
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando a recomendação é encontrada                     |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando a recomendação especificada não é encontrada             |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna a lista de recomendações profissionais de um usuário específico

```http
  GET /api/v2/recomendacoes/profissional/usuario/{usuarioId}?pageNumber=&pageSize=
```

Response Body: 

```json
{
  "totalCount": 2,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "data": [
    {
      "recomendacaoId": 1,
      "dataRecomendacao": "2025-11-13T20:45:57.6713796Z",
      "tituloRecomendacao": "Vaga Front-end Júnior",
      "descricaoRecomendacao": "Oportunidade para desenvolvedor front-end iniciante",
      "categoriaRecomendacao": "Vaga",
      "areaRecomendacao": "Front-end",
      "fonteRecomendacao": "LinkedIn"
    },
    {
      "recomendacaoId": 2,
      "dataRecomendacao": "2025-11-13T20:45:57.6714313Z",
      "tituloRecomendacao": "Curso de Back-end com Spring Boot",
      "descricaoRecomendacao": "Aprofunde seus conhecimentos em APIs Java",
      "categoriaRecomendacao": "Curso",
      "areaRecomendacao": "Back-end",
      "fonteRecomendacao": "Alura"
    }
  ],
  "links": [
    {
      "rel": "self",
      "href": "/recomendacoes/profissional?pageNumber=1&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "/recomendacoes/profissional?pageNumber=2&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "prev",
      "href": "",
      "method": "GET"
    }
  ]
}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `usuarioId`      | `int` | **Obrigatório**. O ID do usuário que deseja consultar as recomendações profissionais |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando as recomendações são encontradas                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhuma recomendação existe  |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |


- #### Cria uma recomendação profissional

```http
  POST /api/v2/recomendacoes/profissional
```

Request Body:

```json
{
  "tituloRecomendacao": "",
  "descricaoRecomendacao": "",
  "promptUsado": "",
  "categoriaRecomendacao": "",
  "areaRecomendacao": "",
  "fonteRecomendacao": "",
  "usuarioId": 0
}
```

Exemplo: 

```json
{
  "tituloRecomendacao": "Vaga Front-end Pleno",
  "descricaoRecomendacao": "Oportunidade para desenvolvedor front-end com anos de experiência",
  "promptUsado": "IA me de uma vaga para um desenvolvedor com conhecimentos avançados em React, Tailwind e Mobile",
  "categoriaRecomendacao": "Vaga",
  "areaRecomendacao": "Front-end",
  "fonteRecomendacao": "LinkedIn",
  "usuarioId": 1
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando uma recomendação é criada com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |


- #### Remove uma recomendação profissional

```http
  DELETE /api/v2/recomendacoes/profissional/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da recomendação que você deseja remover |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 204 No Content    | Sem conteúdo a retornar         | Quando a remoção da recomendação é válida, mas não há dados para retornar   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando a recomendação não é encontrada                |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

### Recomendações de Saúde

- #### Retorna a lista de recomendações de saúde

```http
  GET /api/v2/recomendacoes/saude?pageNumber=&pageSize=
```

Response Body: 

```json
{
  "totalCount": 2,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "data": [
    {
      "recomendacaoId": 1,
      "dataRecomendacao": "2025-11-13T20:45:57.6507802Z",
      "tituloRecomendacao": "Melhorar qualidade do sono",
      "descricaoRecomendacao": "Evite cafeína e telas antes de dormir",
      "tipoSaude": "Sono",
      "nivelAlerta": "Moderado",
      "mensagemSaude": "Estabeleça rotina de sono consistente",
      "usuario": {
        "usuarioId": 1,
        "nomeUsuario": "maria.silva",
        "areaAtual": "Suporte Técnico",
        "areaInteresse": "DevOps",
        "objetivoCarreira": "Migrar para área de infraestrutura e automação",
        "nivelExperiencia": "Júnior"
      }
    },
    {
      "recomendacaoId": 2,
      "dataRecomendacao": "2025-11-13T20:45:57.6508802Z",
      "tituloRecomendacao": "Aumentar produtividade",
      "descricaoRecomendacao": "Organize tarefas com pausas regulares",
      "tipoSaude": "Produtividade",
      "nivelAlerta": "Baixo",
      "mensagemSaude": "Utilize a técnica Pomodoro para melhor desempenho",
      "usuario": {
        "usuarioId": 1,
        "nomeUsuario": "maria.silva",
        "areaAtual": "Suporte Técnico",
        "areaInteresse": "DevOps",
        "objetivoCarreira": "Migrar para área de infraestrutura e automação",
        "nivelExperiencia": "Júnior"
      }
    }
  ],
  "links": [
    {
      "rel": "self",
      "href": "/recomendacoes/saude?pageNumber=1&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "/recomendacoes/saude?pageNumber=2&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "prev",
      "href": "",
      "method": "GET"
    }
  ]
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando as recomendações são encontradas                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhuma recomendação existe  |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna uma recomendação de saúde a partir de um ID

```http
  GET /api/v2/recomendacoes/saude/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da recomendação de saúde que deseja consultar |

Response Body: 

```json
{
  "data": {
    "recomendacaoId": 1,
    "dataRecomendacao": "2025-11-13T20:45:57.6599894Z",
    "tituloRecomendacao": "Melhorar qualidade do sono",
    "descricaoRecomendacao": "Evite cafeína e telas antes de dormir",
    "tipoSaude": "Sono",
    "nivelAlerta": "Moderado",
    "mensagemSaude": "Estabeleça rotina de sono consistente",
    "usuario": {
      "usuarioId": 1,
      "nomeUsuario": "maria.silva",
      "areaAtual": "Suporte Técnico",
      "areaInteresse": "DevOps",
      "objetivoCarreira": "Migrar para área de infraestrutura e automação",
      "nivelExperiencia": "Júnior"
    }
  },
  "links": [
    {
      "rel": "self",
      "href": "/recomendacoes/saude/1",
      "method": "GET"
    },
    {
      "rel": "delete",
      "href": "/recomendacoes/saude/1",
      "method": "DELETE"
    },
    {
      "rel": "list",
      "href": "/recomendacoes/saude",
      "method": "GET"
    }
  ]
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando a recomendação é encontrada                     |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando a recomendação especificada não é encontrada             |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna a lista de recomendações de saúde de um usuário específico

```http
  GET /api/v2/recomendacoes/saude/usuario/{usuarioId}?pageNumber=&pageSize=
```

Response Body: 

```json
{
  "totalCount": 2,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1,
  "data": [
    {
      "recomendacaoId": 1,
      "dataRecomendacao": "2025-11-13T20:45:57.6617494Z",
      "tituloRecomendacao": "Melhorar qualidade do sono",
      "descricaoRecomendacao": "Evite cafeína e telas antes de dormir",
      "tipoSaude": "Sono",
      "nivelAlerta": "Moderado",
      "mensagemSaude": "Estabeleça rotina de sono consistente"
    },
    {
      "recomendacaoId": 2,
      "dataRecomendacao": "2025-11-13T20:45:57.6618003Z",
      "tituloRecomendacao": "Aumentar produtividade",
      "descricaoRecomendacao": "Organize tarefas com pausas regulares",
      "tipoSaude": "Produtividade",
      "nivelAlerta": "Baixo",
      "mensagemSaude": "Utilize a técnica Pomodoro para melhor desempenho"
    }
  ],
  "links": [
    {
      "rel": "self",
      "href": "/recomendacoes/saude?pageNumber=1&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "/recomendacoes/saude?pageNumber=2&pageSize=10",
      "method": "GET"
    },
    {
      "rel": "prev",
      "href": "",
      "method": "GET"
    }
  ]
}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `usuarioId`      | `int` | **Obrigatório**. O ID do usuário que deseja consultar as recomendações de saúde |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK            | Requisição bem-sucedida         | Quando as recomendações são encontradas                     |
| 204 No Content    | Sem conteúdo a retornar         | Quando nenhuma recomendação existe  |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |


- #### Cria uma recomendação de saúde

```http
  POST /api/v2/recomendacoes/saude
```

Request Body:

```json
{
  "tituloRecomendacao": "",
  "descricaoRecomendacao": "",
  "promptUsado": "",
  "tipoSaude": "",
  "nivelAlerta": "",
  "mensagemSaude": "",
  "usuarioId": 0
}
```

Exemplo: 

```json
{
  "tituloRecomendacao": "Melhorar sono",
  "descricaoRecomendacao": "Optar por dormir em um horário antes da Meia-noite para uma melhor noite de sono.",
  "promptUsado": "IA me de uma sugestão de como ajustar meu horário de sono para melhorar minha energia e estresse durante o dia.",
  "tipoSaude": "Sono",
  "nivelAlerta": "Moderado",
  "mensagemSaude": "Estabeleça rotina de sono consistente",
  "usuarioId": 1
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando uma recomendação é criada com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |


- #### Remove uma recomendação de saúde

```http
  DELETE /api/v2/recomendacoes/saude/{id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `int` | **Obrigatório**. O ID da recomendação de saúde que você deseja remover |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 204 No Content    | Sem conteúdo a retornar         | Quando a remoção da recomendação é válida, mas não há dados para retornar   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando a recomendação não é encontrada                |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |


### Procedures (apenas para a disciplina de Database)

- #### Cria um usuário a partir da procedure sp_inserir_usuario

```http
  POST /api/v2/procedures/usuarios
```

Request Body:

```json
{
  "nomeUsuario": "",
  "senhaUsuario": "",
  "areaAtual": "",
  "areaInteresse": "",
  "objetivoCarreira": "",
  "nivelExperiencia": ""
}
```

Exemplo: 

```json
{
  "nomeUsuario": "jorge.roberto",
  "senhaUsuario": "jorge12345",
  "areaAtual": "Frentista",
  "areaInteresse": "Back-end Java",
  "objetivoCarreira": "Transição para Aplicações Back-end com Java e Spring Boot",
  "nivelExperiencia": "Nenhuma"
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando um usuário é criado com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Adiciona uma competência ao Usuário através da procedure sp_inserir_usuario_competencia

```http
  POST /api/v2/procedures/usuarios/{usuarioId}/competencias/{competenciaId}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `usuarioId`      | `int` | **Obrigatório**. O ID do usuário que você deseja ter uma competência associada |
| `competenciaId`      | `int` | **Obrigatório**. O ID da competência você deseja associar a um usuário |

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK    | Requisição bem-sucedida        | Quando a associação é criada com sucesso   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado          | Quando o usuário e competência especificados não são encontrados               |
| 409 Conflict     | Conflito de estado          | Quando o usuário e competência especificados já estão associados um ao outro             |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Cria uma competência através da procedure sp_inserir_competencia

```http
  POST /api/v2/procedures/competencias
```

Request Body:

```json
{
  "nomeCompetencia": "",
  "categoriaCompetencia": "",
  "descricaoCompetencia": ""
}
```

Exemplo: 

```json
{
  "nomeCompetencia": "Flutter",
  "categoriaCompetencia": "Front-end",
  "descricaoCompetencia": "Kit de desenvolvimento de software de interface de usuário"
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando uma competência é criada com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Cria um registro de bem estar através da procedure sp_inserir_registro_bem_estar

```http
  POST /api/v2/procedures/bem-estar
```

Request Body:

```json
{
  "dataRegistro": "",
  "humorRegistro": "",
  "horasSono": 0,
  "horasTrabalho": 0,
  "nivelEnergia": 0,
  "nivelEstresse": 0,
  "observacaoRegistro": "",
  "usuarioId": 0
}
```

Exemplo: 

```json
{
  "dataRegistro": "2025-11-13T20:29:18.9479977Z",
  "humorRegistro": "Feliz",
  "horasSono": 9,
  "horasTrabalho": 6,
  "nivelEnergia": 8,
  "nivelEstresse": 4,
  "observacaoRegistro": "Finalizei as demandas no trabalho",
  "usuarioId": 1
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando um registro é criado com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Cria uma recomendação profissional através da procedure sp_inserir_recomendacao_profissional_completa

```http
  POST /api/v2/procedures/recomendacoes/profissional
```

Request Body:

```json
{
  "tituloRecomendacao": "",
  "descricaoRecomendacao": "",
  "promptUsado": "",
  "categoriaRecomendacao": "",
  "areaRecomendacao": "",
  "fonteRecomendacao": "",
  "usuarioId": 0
}
```

Exemplo: 

```json
{
  "tituloRecomendacao": "Vaga Front-end Pleno",
  "descricaoRecomendacao": "Oportunidade para desenvolvedor front-end com anos de experiência",
  "promptUsado": "IA me de uma vaga para um desenvolvedor com conhecimentos avançados em React, Tailwind e Mobile",
  "categoriaRecomendacao": "Vaga",
  "areaRecomendacao": "Front-end",
  "fonteRecomendacao": "LinkedIn",
  "usuarioId": 1
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando uma recomendação é criada com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Cria uma recomendação de saúde através da procedure sp_criar_recomendacao_saude_completa

```http
  POST /api/v2/procedures/recomendacoes/saude
```

Request Body:

```json
{
  "tituloRecomendacao": "",
  "descricaoRecomendacao": "",
  "promptUsado": "",
  "tipoSaude": "",
  "nivelAlerta": "",
  "mensagemSaude": "",
  "usuarioId": 0
}
```

Exemplo: 

```json
{
  "tituloRecomendacao": "Melhorar sono",
  "descricaoRecomendacao": "Optar por dormir em um horário antes da Meia-noite para uma melhor noite de sono.",
  "promptUsado": "IA me de uma sugestão de como ajustar meu horário de sono para melhorar minha energia e estresse durante o dia.",
  "tipoSaude": "Sono",
  "nivelAlerta": "Moderado",
  "mensagemSaude": "Estabeleça rotina de sono consistente",
  "usuarioId": 1
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 201 Created       | Recurso criado com sucesso      | Quando uma recomendação é criada com êxito |
| 400 Bad Request   | Requisição malformada           | Quando os dados enviados estão incorretos ou incompletos       |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found | Recurso não encontrado        |  Quando nenhum usuário foi encontrado com o ID especificado      |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Retorna dataset de usuários em JSON através da procedure sp_exportar_dataset_usuarios

```JSON
GET /api/v2/procedures/exportar/usuarios
```

Response Body
```
{
  "value": {
    "success": true,
    "totalUsuarios": 12,
    "data": [
      {
        "_id": 1,
        "id_usuario": 1,
        "nome_usuario": "maria.silva",
        "area_atual": "Suporte Técnico",
        "area_interesse": "DevOps",
        "objetivo_carreira": "Migrar para área de infraestrutura e automação",
        "nivel_experiencia": "Júnior",
        "competencias": [
          {
            "nome": "Docker",
            "categoria": "DevOps"
          },
          {
            "nome": "Git",
            "categoria": "DevOps"
          },
          {
            "nome": "Comunicação",
            "categoria": "Soft Skills"
          }
        ],
        "registros_bem_estar": [
          {
            "data": "2025-11-08",
            "humor": "Feliz",
            "horas_sono": 8,
            "horas_trabalho": 7,
            "nivel_energia": 8,
            "nivel_estresse": 4
          },
          {
            "data": "2025-11-07",
            "humor": "Calmo",
            "horas_sono": 7,
            "horas_trabalho": 8,
            "nivel_energia": 7,
            "nivel_estresse": 5
          },
          {
            "data": "2025-11-06",
            "humor": "Estressado",
            "horas_sono": 6,
            "horas_trabalho": 10,
            "nivel_energia": 5,
            "nivel_estresse": 8
          }
        ]
      },
      {
        "_id": 2,
        "id_usuario": 2,
        "nome_usuario": "joao.santos",
        "area_atual": "Analista de Sistemas",
        "area_interesse": "Data Science",
        "objetivo_carreira": "Tornar-me cientista de dados especializado em IA",
        "nivel_experiencia": "Pleno",
        "competencias": [
          {
            "nome": "Python",
            "categoria": "Back-end"
          },
          {
            "nome": "SQL",
            "categoria": "Banco de Dados"
          },
          {
            "nome": "Resolução de Problemas",
            "categoria": "Soft Skills"
          },
          {
            "nome": "Pandas",
            "categoria": "Data Science"
          }
        ],
        "registros_bem_estar": [
          {
            "data": "2025-11-11",
            "humor": "Estressado",
            "horas_sono": 5,
            "horas_trabalho": 11,
            "nivel_energia": 4,
            "nivel_estresse": 9
          },
          {
            "data": "2025-11-08",
            "humor": "Feliz",
            "horas_sono": 8,
            "horas_trabalho": 8,
            "nivel_energia": 8,
            "nivel_estresse": 4
          },
          {
            "data": "2025-11-03",
            "humor": "Calmo",
            "horas_sono": 7,
            "horas_trabalho": 9,
            "nivel_energia": 7,
            "nivel_estresse": 6
          }
        ]
      },
      {
        "_id": 3,
        "id_usuario": 3,
        "nome_usuario": "ana.costa",
        "area_atual": "Designer Gráfico",
        "area_interesse": "UX/UI",
        "objetivo_carreira": "Transição para design de experiência do usuário",
        "nivel_experiencia": "Júnior",
        "competencias": [
          {
            "nome": "Figma",
            "categoria": "UX/UI"
          },
          {
            "nome": "Comunicação",
            "categoria": "Soft Skills"
          },
          {
            "nome": "Trabalho em Equipe",
            "categoria": "Soft Skills"
          }
        ],
        "registros_bem_estar": [
          {
            "data": "2025-11-12",
            "humor": "Triste",
            "horas_sono": 6,
            "horas_trabalho": 8,
            "nivel_energia": 5,
            "nivel_estresse": 7
          },
          {
            "data": "2025-11-09",
            "humor": "Calmo",
            "horas_sono": 7,
            "horas_trabalho": 7,
            "nivel_energia": 7,
            "nivel_estresse": 5
          },
          {
            "data": "2025-11-05",
            "humor": "Feliz",
            "horas_sono": 8,
            "horas_trabalho": 6,
            "nivel_energia": 9,
            "nivel_estresse": 3
          }
        ]
      },
      {
        "_id": 4,
        "id_usuario": 4,
        "nome_usuario": "pedro.oliveira",
        "area_atual": "Desenvolvedor Junior",
        "area_interesse": "Back-end",
        "objetivo_carreira": "Crescer como desenvolvedor backend sênior",
        "nivel_experiencia": "Júnior",
        "competencias": [
          {
            "nome": "JavaScript",
            "categoria": "Front-end"
          },
          {
            "nome": "SQL",
            "categoria": "Banco de Dados"
          },
          {
            "nome": "Git",
            "categoria": "DevOps"
          },
          {
            "nome": "Node.js",
            "categoria": "Back-end"
          }
        ],
        "registros_bem_estar": [
          {
            "data": "2025-11-10",
            "humor": "Feliz",
            "horas_sono": 8,
            "horas_trabalho": 7,
            "nivel_energia": 8,
            "nivel_estresse": 4
          },
          {
            "data": "2025-11-07",
            "humor": "Calmo",
            "horas_sono": 7,
            "horas_trabalho": 8,
            "nivel_energia": 6,
            "nivel_estresse": 6
          },
          {
            "data": "2025-11-04",
            "humor": "Estressado",
            "horas_sono": 5,
            "horas_trabalho": 12,
            "nivel_energia": 3,
            "nivel_estresse": 9
          }
        ]
      },
      {
        "_id": 5,
        "id_usuario": 5,
        "nome_usuario": "julia.ferreira",
        "area_atual": "Estagiária TI",
        "area_interesse": "Front-end",
        "objetivo_carreira": "Desenvolver carreira em desenvolvimento web moderno",
        "nivel_experiencia": "Estagiário",
        "competencias": [
          {
            "nome": "JavaScript",
            "categoria": "Front-end"
          },
          {
            "nome": "React",
            "categoria": "Front-end"
          },
          {
            "nome": "TypeScript",
            "categoria": "Front-end"
          }
        ],
        "registros_bem_estar": [
          {
            "data": "2025-11-12",
            "humor": "Calmo",
            "horas_sono": 7,
            "horas_trabalho": 7,
            "nivel_energia": 7,
            "nivel_estresse": 5
          },
          {
            "data": "2025-11-09",
            "humor": "Estressado",
            "horas_sono": 6,
            "horas_trabalho": 9,
            "nivel_energia": 5,
            "nivel_estresse": 7
          },
          {
            "data": "2025-11-06",
            "humor": "Feliz",
            "horas_sono": 8,
            "horas_trabalho": 6,
            "nivel_energia": 8,
            "nivel_estresse": 3
          }
        ]
      },
      {
        "_id": 6,
        "id_usuario": 6,
        "nome_usuario": "carlos.mendes",
        "area_atual": "Nenhuma",
        "area_interesse": "Banco de Dados",
        "objetivo_carreira": "Iniciar carreira como DBA ou engenheiro de dados",
        "nivel_experiencia": "Nenhuma",
        "competencias": [
          {
            "nome": "SQL",
            "categoria": "Banco de Dados"
          },
          {
            "nome": "Resolução de Problemas",
            "categoria": "Soft Skills"
          }
        ],
        "registros_bem_estar": [
          {
            "data": "2025-11-08",
            "humor": "Feliz",
            "horas_sono": 8,
            "horas_trabalho": 0,
            "nivel_energia": 9,
            "nivel_estresse": 2
          },
          {
            "data": "2025-11-03",
            "humor": "Calmo",
            "horas_sono": 7,
            "horas_trabalho": 0,
            "nivel_energia": 8,
            "nivel_estresse": 2
          }
        ]
      },
      {
        "_id": 7,
        "id_usuario": 7,
        "nome_usuario": "fernanda.lima",
        "area_atual": "Gerente de Projetos",
        "area_interesse": "Governança de TI",
        "objetivo_carreira": "Especializar-me em governança e compliance de TI",
        "nivel_experiencia": "Sênior",
        "competencias": [
          {
            "nome": "Comunicação",
            "categoria": "Soft Skills"
          },
          {
            "nome": "Trabalho em Equipe",
            "categoria": "Soft Skills"
          },
          {
            "nome": "Resolução de Problemas",
            "categoria": "Soft Skills"
          }
        ],
        "registros_bem_estar": [
          {
            "data": "2025-11-10",
            "humor": "Calmo",
            "horas_sono": 7,
            "horas_trabalho": 9,
            "nivel_energia": 7,
            "nivel_estresse": 6
          },
          {
            "data": "2025-11-05",
            "humor": "Estressado",
            "horas_sono": 6,
            "horas_trabalho": 11,
            "nivel_energia": 5,
            "nivel_estresse": 8
          }
        ]
      },
      {
        "_id": 8,
        "id_usuario": 8,
        "nome_usuario": "ricardo.alves",
        "area_atual": "Desenvolvedor Full Stack",
        "area_interesse": "IA",
        "objetivo_carreira": "Migrar para desenvolvimento de soluções de inteligência artificial",
        "nivel_experiencia": "Pleno",
        "competencias": [
          {
            "nome": "Python",
            "categoria": "Back-end"
          },
          {
            "nome": "JavaScript",
            "categoria": "Front-end"
          },
          {
            "nome": "Machine Learning",
            "categoria": "IA"
          },
          {
            "nome": "TensorFlow",
            "categoria": "IA"
          }
        ],
        "registros_bem_estar": [
          {
            "data": "2025-11-11",
            "humor": "Estressado",
            "horas_sono": 5,
            "horas_trabalho": 10,
            "nivel_energia": 4,
            "nivel_estresse": 8
          },
          {
            "data": "2025-11-07",
            "humor": "Feliz",
            "horas_sono": 8,
            "horas_trabalho": 8,
            "nivel_energia": 8,
            "nivel_estresse": 4
          }
        ]
      },
      {
        "_id": 9,
        "id_usuario": 9,
        "nome_usuario": "beatriz.rocha",
        "area_atual": "QA Tester",
        "area_interesse": "DevOps",
        "objetivo_carreira": "Automatizar testes e trabalhar com CI/CD",
        "nivel_experiencia": "Júnior",
        "competencias": [
          {
            "nome": "Git",
            "categoria": "DevOps"
          },
          {
            "nome": "Trabalho em Equipe",
            "categoria": "Soft Skills"
          },
          {
            "nome": "Jenkins",
            "categoria": "DevOps"
          }
        ],
        "registros_bem_estar": [
          {
            "data": "2025-11-10",
            "humor": "Feliz",
            "horas_sono": 8,
            "horas_trabalho": 7,
            "nivel_energia": 8,
            "nivel_estresse": 4
          },
          {
            "data": "2025-11-06",
            "humor": "Calmo",
            "horas_sono": 7,
            "horas_trabalho": 8,
            "nivel_energia": 7,
            "nivel_estresse": 5
          }
        ]
      },
      {
        "_id": 10,
        "id_usuario": 10,
        "nome_usuario": "lucas.martins",
        "area_atual": "Analista de Negócios",
        "area_interesse": "Data Science",
        "objetivo_carreira": "Combinar análise de negócios com ciência de dados",
        "nivel_experiencia": "Pleno",
        "competencias": [
          {
            "nome": "SQL",
            "categoria": "Banco de Dados"
          },
          {
            "nome": "Resolução de Problemas",
            "categoria": "Soft Skills"
          },
          {
            "nome": "Power BI",
            "categoria": "Data Science"
          }
        ],
        "registros_bem_estar": [
          {
            "data": "2025-11-09",
            "humor": "Feliz",
            "horas_sono": 8,
            "horas_trabalho": 7,
            "nivel_energia": 9,
            "nivel_estresse": 3
          },
          {
            "data": "2025-11-04",
            "humor": "Calmo",
            "horas_sono": 7,
            "horas_trabalho": 8,
            "nivel_energia": 7,
            "nivel_estresse": 6
          }
        ]
      },
      {
        "_id": 11,
        "id_usuario": 11,
        "nome_usuario": "camila.souza",
        "area_atual": "Desenvolvedora Mobile",
        "area_interesse": "Back-end",
        "objetivo_carreira": "Expandir conhecimento para desenvolvimento backend",
        "nivel_experiencia": "Júnior",
        "competencias": [
          {
            "nome": "JavaScript",
            "categoria": "Front-end"
          },
          {
            "nome": "Git",
            "categoria": "DevOps"
          },
          {
            "nome": "Comunicação",
            "categoria": "Soft Skills"
          }
        ],
        "registros_bem_estar": []
      },
      {
        "_id": 12,
        "id_usuario": 12,
        "nome_usuario": "rafael.dias",
        "area_atual": "Freelancer Web",
        "area_interesse": "Front-end",
        "objetivo_carreira": "Profissionalizar carreira como desenvolvedor frontend",
        "nivel_experiencia": "Júnior",
        "competencias": [
          {
            "nome": "JavaScript",
            "categoria": "Front-end"
          },
          {
            "nome": "React",
            "categoria": "Front-end"
          },
          {
            "nome": "Git",
            "categoria": "DevOps"
          }
        ],
        "registros_bem_estar": []
      }
    ],
    "timestamp": "2025-11-13T23:23:42.3434677Z"
  },
  "statusCode": 200
}
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK      | Requisição bem-sucedida      | Quando o dataset é exportado com sucesso |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |



