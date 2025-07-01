"# Gf4.Teste.BackEnd" 
"# Gf4.Teste.BackEnd" 

Atenção é necessário rodar o redis para executar o projeto!
```bash
docker run --name redis-dev -p 6379:6379 -d redis
docker stop redis-dev
docker rm redis-dev
docker exec -it redis-dev redis-cli
=======
