FROM mongo:4.4-bionic
COPY mflix.gz mflix.gz
CMD mongorestore --host=mflix-mongo --port=27017 --username=dbadmin --password=password --nsFrom="sample_mflix.*" --nsTo="mflix.*" --gzip --archive=mflix.gz