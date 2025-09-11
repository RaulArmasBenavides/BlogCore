# WebAppDemoNet7
WebAppDemoNet7


Pasos para levantar en docker compose 


 docker compose build
docker compose up -d

# desde la carpeta donde está docker-compose.yml
docker compose up -d --no-deps --build blogcore
docker compose build blogcore           # solo compila
docker compose up -d blogcore           # recrea si detecta imagen nueva
docker compose restart blogcore         # reinicia sin reconstruir


docker compose stop            # todos
docker compose stop blogcore   # solo la web


Detener y borrar también volúmenes (perderías la BD sql_data):

docker compose down -v



demo users : 

raul.armas@intelica.com
marceloPERU!3