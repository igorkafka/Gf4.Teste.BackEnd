"# Gf4.Teste.BackEnd" 
"# G4f.Avaliacao.PedidoStore" 
=======

Atenção é necessário rodar o redis para executar o projeto!
docker run --name redis-dev -p 6379:6379 -d redis
docker stop redis-dev
docker rm redis-dev
docker exec -it redis-dev redis-cli
=======

Depois disso é só configurar conexão do redis, mongodb e SQL Server no appsettings

