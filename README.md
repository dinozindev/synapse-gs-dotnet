# Synapse - API - Global Solution

## Integrantes

- Giovanna Revito Roz - RM558981
- Kaian Gustavo de Oliveira Nascimento - RM558986
- Lucas Kenji Kikuchi - RM554424

## Descrição do Projeto



## Justificativa da Arquitetura

Optamos por utilizar **ASP.NET Core com Minimal APIs** pela simplicidade na definição de rotas e menor boilerplate em comparação com Controllers tradicionais.  

A separação em **camadas (Models, DTOs, Services, Examples e Endpoints)** garante melhor manutenção e testabilidade do código.  

A escolha do **Entity Framework Core** com banco Oracle se deu por facilitar o mapeamento objeto-relacional, reduzindo código de SQL manual.  

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
git clone https://github.com/dinozindev/synapse-gs-dotnet
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

- #### Busca todas as motos que possuem cliente atrelado

```http
 GET /api/v2/procedures/motos-com-cliente
```

Response Body:
```
[
  {
    "motoId": 1,
    "placaMoto": "ABC1234",
    "modeloMoto": "Mottu Pop",
    "situacaoMoto": "Em Trânsito",
    "chassiMoto": "CHS12345678901234",
    "nomeCliente": "João Silva",
    "telefoneCliente": "(11) 91234-5678"
  },
  {
    "motoId": 2,
    "placaMoto": "DEF5678",
    "modeloMoto": "Mottu Sport",
    "situacaoMoto": "Em Trânsito",
    "chassiMoto": "CHS22345678901234",
    "nomeCliente": "Maria Oliveira",
    "telefoneCliente": "(21) 99876-5432"
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK    | Requisição bem-sucedida        | Quando as motos são retornadas com sucesso   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado         | Quando as motos não são encontradas                           |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Gera relatório de movimentações por setor

```http
 GET /api/v2/procedures/relatorio-movimentacoes
```

Response Body:
```
[
  "Pátio          Setor                         Quantidade",
  "------------   --------------------------    ----------",
  "               Sub Total      0",
  "               Sub Total      0",
  "               Sub Total      0",
  "               Sub Total      0",
  "Pátio Norte    Agendada Para Manutenção      4",
  "Pátio Norte    Danos Estruturais Graves      4",
  "Pátio Norte    Minha Mottu                   5",
  "Pátio Norte    Motor Defeituoso              4",
  "Pátio Norte    Pendência                     4",
  "Pátio Norte    Pronta para Aluguel           4",
  "Pátio Norte    Reparos Simples               4",
  "Pátio Norte    Sem Placa                     4",
  "               Sub Total      33",
  "               Sub Total      0",
  "               Sub Total      0",
  "               Sub Total      0",
  "Total Geral    33"
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK    | Requisição bem-sucedida        | Quando o relatório é retornado com sucesso   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado         | Quando o relatório não é gerado                           |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Exporta movimentações em JSON

```http
 GET /api/v2/procedures/exportar/movimentacoes
```

Response Body:
```

  {
    "_id": "MOV_000032",
    "movimentacaoId": 32,
    "dtEntrada": "2025-02-18T00:00:00Z",
    "dtSaida": null,
    "descricao": "Uso interno Mottu",
    "status": "ativa",
    "diasPermanencia": 251,
    "moto": {
      "id": 32,
      "placa": "AAX8883",
      "modelo": "Mottu-E",
      "chassi": "CHS90000000000024",
      "situacao": "Ativa"
    },
    "cliente": {
      "id": 32,
      "nome": "Juliane Castro",
      "telefone": "1144444444",
      "cpf": "93456789002",
      "email": "juliane@email.com",
      "sexo": "F"
    },
    "localizacao": {
      "patio": {
        "id": 1,
        "nome": "Pátio Norte",
        "zona": "Zona Norte"
      },
      "setor": {
        "id": 8,
        "tipo": "Minha Mottu",
        "status": "Parcial"
      },
      "vaga": {
        "id": 32,
        "numero": "A8-V4",
        "ocupada": true
      }
    }
  },
  {
    "_id": "MOV_000033",
    "movimentacaoId": 33,
    "dtEntrada": "2025-10-15T08:30:00Z",
    "dtSaida": null,
    "descricao": "Manutenção preventiva",
    "status": "ativa",
    "diasPermanencia": 12,
    "moto": {
      "id": 45,
      "placa": "BCD4567",
      "modelo": "Mottu Pop",
      "chassi": "CHS11122233344455",
      "situacao": "Em Manutenção"
    },
    "cliente": {
      "id": 18,
      "nome": "Pedro Almeida",
      "telefone": "11955556666",
      "cpf": "12345678901",
      "email": "pedro@email.com",
      "sexo": "M"
    },
    "localizacao": {
      "patio": {
        "id": 8,
        "nome": "Pátio ABC",
        "zona": "ABC Paulista"
      },
      "setor": {
        "id": 61,
        "tipo": "Agendada Para Manutenção",
        "status": "Livre"
      },
      "vaga": {
        "id": 190,
        "numero": "H5-V1",
        "ocupada": true
      }
    }
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK    | Requisição bem-sucedida        | Quando as movimentações são retornadas com sucesso   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado         | Quando nenhuma movimentação é retornada                           |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |

- #### Exporta pátios em JSON

```http
 GET /api/v2/procedures/exportar/patios
```

Response Body:
```
[
  {
    "_id": "PATIO_008",
    "patioId": 8,
    "nome": "Pátio ABC",
    "localizacao": "Santo André",
    "descricao": "Área nova",
    "gerente": {
      "id": 8,
      "nome": "Luciana Prado",
      "telefone": "11900008888",
      "cpf": "22222222207"
    },
    "funcionarios": [
      {
        "id": 8,
        "nome": "Débora Mendes",
        "telefone": "11988889999",
        "cargo": {
          "nome": "Mecânico",
          "descricao": "Responsável por realizar reparos e manutenções em motos"
        }
      }
    ],
    "setores": [
      {
        "id": 61,
        "tipo": "Agendada Para Manutenção",
        "status": "Livre",
        "capacidade": {
          "totalVagas": 3,
          "vagasLivres": 3,
          "vagasOcupadas": 0,
          "taxaOcupacao": 0
        },
        "vagas": [
          {
            "id": 190,
            "numero": "H5-V1",
            "ocupada": false
          },
          {
            "id": 191,
            "numero": "H5-V2",
            "ocupada": false
          },
          {
            "id": 192,
            "numero": "H5-V3",
            "ocupada": false
          }
        ]
      },
      {
        "id": 59,
        "tipo": "Danos Estruturais Graves",
        "status": "Livre",
        "capacidade": {
          "totalVagas": 3,
          "vagasLivres": 3,
          "vagasOcupadas": 0,
          "taxaOcupacao": 0
        },
        "vagas": [
          {
            "id": 184,
            "numero": "H3-V1",
            "ocupada": false
          },
          {
            "id": 185,
            "numero": "H3-V2",
            "ocupada": false
          },
          {
            "id": 186,
            "numero": "H3-V3",
            "ocupada": false
          }
        ]
      },
      {
        "id": 64,
        "tipo": "Minha Mottu",
        "status": "Livre",
        "capacidade": {
          "totalVagas": 3,
          "vagasLivres": 3,
          "vagasOcupadas": 0,
          "taxaOcupacao": 0
        },
        "vagas": [
          {
            "id": 199,
            "numero": "H8-V1",
            "ocupada": false
          },
          {
            "id": 200,
            "numero": "H8-V2",
            "ocupada": false
          },
          {
            "id": 201,
            "numero": "H8-V3",
            "ocupada": false
          }
        ]
      },
      {
        "id": 60,
        "tipo": "Motor Defeituoso",
        "status": "Livre",
        "capacidade": {
          "totalVagas": 3,
          "vagasLivres": 3,
          "vagasOcupadas": 0,
          "taxaOcupacao": 0
        },
        "vagas": [
          {
            "id": 187,
            "numero": "H4-V1",
            "ocupada": false
          },
          {
            "id": 188,
            "numero": "H4-V2",
            "ocupada": false
          },
          {
            "id": 189,
            "numero": "H4-V3",
            "ocupada": false
          }
        ]
      },
      {
        "id": 57,
        "tipo": "Pendência",
        "status": "Livre",
        "capacidade": {
          "totalVagas": 3,
          "vagasLivres": 3,
          "vagasOcupadas": 0,
          "taxaOcupacao": 0
        },
        "vagas": [
          {
            "id": 178,
            "numero": "H1-V1",
            "ocupada": false
          },
          {
            "id": 179,
            "numero": "H1-V2",
            "ocupada": false
          },
          {
            "id": 180,
            "numero": "H1-V3",
            "ocupada": false
          }
        ]
      },
      {
        "id": 62,
        "tipo": "Pronta para Aluguel",
        "status": "Livre",
        "capacidade": {
          "totalVagas": 3,
          "vagasLivres": 3,
          "vagasOcupadas": 0,
          "taxaOcupacao": 0
        },
        "vagas": [
          {
            "id": 193,
            "numero": "H6-V1",
            "ocupada": false
          },
          {
            "id": 194,
            "numero": "H6-V2",
            "ocupada": false
          },
          {
            "id": 195,
            "numero": "H6-V3",
            "ocupada": false
          }
        ]
      },
      {
        "id": 58,
        "tipo": "Reparos Simples",
        "status": "Livre",
        "capacidade": {
          "totalVagas": 3,
          "vagasLivres": 3,
          "vagasOcupadas": 0,
          "taxaOcupacao": 0
        },
        "vagas": [
          {
            "id": 181,
            "numero": "H2-V1",
            "ocupada": false
          },
          {
            "id": 182,
            "numero": "H2-V2",
            "ocupada": false
          },
          {
            "id": 183,
            "numero": "H2-V3",
            "ocupada": false
          }
        ]
      },
      {
        "id": 63,
        "tipo": "Sem Placa",
        "status": "Livre",
        "capacidade": {
          "totalVagas": 3,
          "vagasLivres": 3,
          "vagasOcupadas": 0,
          "taxaOcupacao": 0
        },
        "vagas": [
          {
            "id": 196,
            "numero": "H7-V1",
            "ocupada": false
          },
          {
            "id": 197,
            "numero": "H7-V2",
            "ocupada": false
          },
          {
            "id": 198,
            "numero": "H7-V3",
            "ocupada": false
          }
        ]
      }
    ],
    "estatisticas": {
      "totalSetores": 8,
      "totalVagas": 24,
      "vagasOcupadas": 0,
      "vagasLivres": 24,
      "taxaOcupacaoGeral": 0,
      "movimentacoesAtivas": 0
    }
  }
]
```

Códigos de Resposta

| Código HTTP       | Significado                     | Quando ocorre                                                  |
|-------------------|----------------------------------|----------------------------------------------------------------|
| 200 OK    | Requisição bem-sucedida        | Quando os pátios são retornados com sucesso   |
| 401 Unauthorized      | Requisição sem autorização         | Quando o Token JWT não foi informado                            |
| 404 Not Found     | Recurso não encontrado         | Quando nenhum pátio é retornado                          |
| 500 Internal Server Error | Erro interno             | Quando ocorre uma falha inesperada no servidor                 |
