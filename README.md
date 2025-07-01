"# Gf4.Teste.BackEnd" 
"# Gf4.Teste.BackEnd" 
<h1>Atenção!</h1>
É necessário rodar o Redis
🛠️ Como Rodar o Redis (Docker e WSL)
📦 Usando Docker
Recomendado por ser rápido, isolado e não requer instalação nativa do Redis.

🔹 Subindo o Redis com Docker
bash
Copiar
Editar
docker run --name redis-dev -p 6379:6379 -d redis
🔹 Explicação dos parâmetros:
--name redis-dev: nome do container.

-p 6379:6379: expõe a porta padrão do Redis.

-d: roda o container em segundo plano.

redis: imagem oficial do Redis (usa a versão latest por padrão).

🔹 Parar e remover o container:
bash
Copiar
Editar
docker stop redis-dev
docker rm redis-dev
🔹 Acessar o CLI do Redis:
bash
Copiar
Editar
docker exec -it redis-dev redis-cli
🐧 Usando WSL (Ubuntu, Debian, etc.)
Ideal se você já desenvolve dentro do WSL e quer manter tudo no mesmo ambiente.

🔹 Instalando o Redis:
bash
Copiar
Editar
sudo apt update
sudo apt install redis -y
🔹 Iniciando o Redis:
bash
Copiar
Editar
sudo service redis-server start
🔹 Verificando se o Redis está rodando:
bash
Copiar
Editar
redis-cli ping
Esperado: PONG

🔹 Parando o serviço:
bash
Copiar
Editar
sudo service redis-server stop
✅ Testando a conexão
Se o Redis estiver rodando localmente na porta 6379, você pode testar a conexão no seu código com:

csharp
Copiar
Editar
// Exemplo em C#
var redis = ConnectionMultiplexer.Connect("localhost:6379");
❗ Dica
Certifique-se de que sua aplicação está apontando para localhost:6379.

No Windows puro (fora do WSL), recomendamos usar Docker Desktop para rodar Redis com mais facilidade.

