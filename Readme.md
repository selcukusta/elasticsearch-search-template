# Use Search Template in Elasticsearch
> docker pull docker.elastic.co/elasticsearch/elasticsearch:5.4.3

> docker run --name elastic -d -p 9200:9200 -p 9300:9300 -e "http.host=0.0.0.0" -e "transport.host=127.0.0.1" -e "xpack.security.enabled=false" docker.elastic.co/elasticsearch/elasticsearch:5.4.3
