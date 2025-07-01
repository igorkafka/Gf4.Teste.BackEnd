"# Gf4.Teste.BackEnd" 
"# Gf4.Teste.BackEnd" 
Atenção! O projeto necessista do Redis instalado
## 🛠️ Como Rodar o Redis (Docker e WSL)

### 📦 Usando Docker

> Recomendado por ser rápido, isolado e não requer instalação nativa do Redis.

#### 🔹 Subindo o Redis com Docker

```bash
docker run --name redis-dev -p 6379:6379 -d redis
docker stop redis-dev
docker rm redis-dev
docker exec -it redis-dev redis-cli
