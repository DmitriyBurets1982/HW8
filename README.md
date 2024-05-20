# При необходимости дополнительно установить Ingress-Nginx Controller
helm upgrade --install ingress-nginx ingress-nginx --repo https://kubernetes.github.io/ingress-nginx --namespace ingress-nginx --create-namespace

или

kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.10.1/deploy/static/provider/cloud/deploy.yaml


# Команды и результат
helm install hw8 k8s </br>
дождаться полной инициализации контейнера RabbitMQ и MassTransit в сервисах (~2 мин) </br>
newman run HW8.postman_collection </br>
helm uninstall hw8 </br>

Результаты выполнения приложены в файлах newman_1.png и newman_2.png


# Описание архитектурного решения
Механизм распределенной транзакции реализован на основе Саги (OrderStateMachine // MassTransit.RabbitMQ)
