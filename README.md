# MFlix

A showcase of how to build API's (REST, gRPC, GraphQL) using C# .NET.

I chose _MFlix_ as the name for this project because I am using one of the sample databases provided by MongoDB called MFlix. The MFlix database is composed of collections of movie related data. I provide more detail in the MongoDB section where I explain how to get a copy of the MFlix database. Therefore, because of the database name, and the fact that _MFlix_ is a short and catchy name, I decided to go with _MFlix_.

---

## 1. Purpose

The primary purpose of _MFlix_ is to demonstrate how to build gRPC, REST (http), and GraphQL API's using C# and the .NET Framework. In order to do this, I have created a contrived use-case that will allow me to demonstrate how to build different API's for different needs. Each API demonstrates how to address the following requirements:

- Error handling
- Logging
- Caching
- Security
- Dependency Injection
- Configuration
- How to do paging, filtering, and sorting via the API

The secondary purpose of _MFlix_ is to demonstrate how to work with different technologies, frameworks, and libraries using the C# .NET Framework. The different topics that will be demonstrated are as follows:

- Developing with MongoDB and C#
- Preparing a local development environment using Docker and Docker-Compose
- Working with MongoDB database
- Working with SEQ as a log server

---

## 2. Description

We are going to pretend to be a fictitious company called MFlix. We are going to pretend to take on the role of the core API development team for _MFlix_. Our strategy is to provide secure and easy access to data to both internal and external applications. We want to abstract away all direct access to database systems and instead provide high performance API's that can be used by product development teams both internally and external to _MFlix_.

Based on research, it was determined that there are 3 primary groups of developers with very specific preference in terms of preferred API integration. The use-cases are listed as follows:

![mflix-usecases](https://user-images.githubusercontent.com/33935506/111857121-550ccf00-8994-11eb-99a1-49e052e6c355.png)

Therefore, at a high level, the following deductions have been made in terms of the API's that we need to develop:

- Build gRPC API's
  - to satisfy requirement from backoffice and microservice developers
  - to abstract away direct access to databases
  - to provide highly performant access to data
- Build HTTP API
  - to satisfy requirement from web development community
  - satisfy most of the REST guiding principles
- Build HTTP API
  - to satisfy requirement from mobile development community

Furthermore, we can deduce a high-level architectural diagram that highlights

- the type of API's being built
- the way in which different API's will be accessed by different types of applications
- the scope of API's (public or private)

![mflix-hla-1](https://user-images.githubusercontent.com/33935506/111857444-58a15580-8996-11eb-8d3f-c3adea46a661.png)

---

## 3. Infrastructure

All the infrastructure that is required for the development of this project will be hosted within `Docker` and managed with `docker-compose`. The full stack has been defined in the `docker-compose.yaml` file in the `fabric` folder.

![mflix-fabric-folder](https://user-images.githubusercontent.com/33935506/111860679-fe13f380-89ad-11eb-8cc8-21f5db8000df.png)

In the image above, you will notice a folder called `mongo-seed` that contains 2 files, namely `Dockerfile` and `mflix.gz`. These 2 files are used to seed the _MFlix_ MongoDB database. Let's examine these 2 files in more detail in the next section.

Initially, _MongoDB_ will be the primary database used to store data. If you are interested in how to get started with _MongoDB_, I have create a [getting started guide](https://github.com/drminnaar/guides/blob/master/mongodb-guide/1-getting-started.md) that covers hosting options (local, docker, cloud), tools, and some basic commands.

We will also be using [SEQ] as a central logging server for all the API's being built.

### 3.1 Mongo Seed

The following 2 files are used as part of seeding the _MFlix_ database:

- mflix.gz
- Dockerfile

#### 3.1.1 mflix.gz

The file `mflix.gz` is a compressed mongodb dump of the _MFlix_ database. The _MFlix_ database is a sample database that is provided as part of the [MongoDB Atlas]. [MongoDB Atlas] is a _Data as a Service (DaaS)_ offering, and is hosted in the cloud. There is no installation of MongoDB required and a free tier is available.

I decided to make it even easier to work with this project by obtaining the free _MFlix_ sample dataset and including it as part of the Docker stack. However, if you would like to obtain the original _MFlix_ database, you can do so by following the following steps:

- To get started, signup for free by registering for a free tier account [here](https://www.mongodb.com/cloud/atlas). The free tier entitles you to 512MB storage. Please review the [MongoDB Atlas Documentation] for more information.
- Once you have registered and setup your MongoDB instance on _Atlas_, you will be presented with a dashboard resembling the following image:

  ![atlas](https://user-images.githubusercontent.com/33935506/37251882-d19a3818-2520-11e8-97f6-7015435f3cbd.png)

- Select the ellipses next to the `Connect` button. Then select `Load Sample Dataset`

  ![mflix-atlast-setup-2](https://user-images.githubusercontent.com/33935506/111863748-40473000-89c2-11eb-8c5a-8706195d7048.png)

- Take note of sample dataset size. Select `Load Sample Dataset`.

  ![mflix-atlast-setup-3](https://user-images.githubusercontent.com/33935506/111863746-3f160300-89c2-11eb-8409-dc0465dfc3ed.png)

- Open your terminal (bash/powershell) and create a dump of the _MFlix_ database. Take note that in addition to creating the dump, I also compress the file.

  ```bash
  # dump the mflix database to a local file
  mongodump --uri="mongodb+srv://your_mongodb_atlas_cluster" --db=sample_mflix --username=mongo --gzip --archive=mflix.gz
  ```

- In the same terminal window, run the following command to restore the dump to a destination of your choice. Take note that in the command below I am renaming the `sample_mflix` database to just `mflix` database as part of restore.

  ```bash
  # restore mflix database from file
  mongorestore --host=localhost --port=27017 --username=dbadmin --nsFrom="sample_mflix.*" --nsTo="mflix.*" --gzip --archive=mflix.gz
  ```

#### 3.1.2 MongoDB Seed Dockerfile

This Dockerfile will be used as part of the `docker-compose` stack and is used to seed the MongoDB database. It runs the commands to be able to copy the `mflix.gz` file and restore it to the Docker hosted MongoDB database.

```docker
FROM mongo:4.4-bionic
COPY mflix.gz mflix.gz
CMD mongorestore --host=mflix-mongo --port=27017 --username=dbadmin --password=password --nsFrom="sample_mflix.*" --nsTo="mflix.*" --gzip --archive=mflix.gz
```

### 3.2 The docker-compose File

The `docker-compose.yaml` file defines the stack that will be used for all _MFlix_ API development. The stack is defined as follows:

- Define MongoDB Setup
  - The service is defined as follows:

    ```yaml
    # MongoDB Setup
    mflix-mongo:
      image: mongo:4.4-bionic
      container_name: mflix-mongo
      restart: unless-stopped
      ports:
        - 27017:27017
      networks:
        - default
      environment:
        - MONGO_INITDB_ROOT_USERNAME=dbadmin
        - MONGO_INITDB_ROOT_PASSWORD=password
      volumes:
        - type: volume
          source: mflix-mongo-data
          target: /data/db
    ```

- Define MongoDB Seed Setup
  - depends on MongoDB Setup to complete first
  - The service is defined as follows:

    ```yaml
    # Container to seed MongoDB with MFlix data
    mflix-mongo-seed:
      container_name: mflix-mongo-seed
      build: ./mongo-seed
      networks:
        - default
      depends_on:
        - mflix-mongo
    ```

- Define MongoDB Express Setup
  - a Web Graphical User Interface for managing MongoDB
  - depends on MongoDB Seed setup to complete first
  - take note of the setting `ME_CONFIG_MONGODB_SERVER: mflix-mongo`. The value `mflix-mongo` must be the same value as the name of the MongoDB service defined above
  - The service is defined as follows:

    ```yaml
    # MongoExpress Setup
    mongo-express:
      image: mongo-express
      container_name: mflix-mongo-express
      restart: unless-stopped
      ports:
        - 8081:8081
      environment:
        ME_CONFIG_MONGODB_ADMINUSERNAME: dbadmin
        ME_CONFIG_MONGODB_ADMINPASSWORD: password
        ME_CONFIG_MONGODB_SERVER: mflix-mongo
      networks:
        - default
      depends_on:
        - mflix-mongo-seed
    ```

- Define SEQ Setup
  - [SEQ] is a logging server and will be used a central logging server for all API's
  - The service is defined as follows:

    ```yaml
    # Seq Setup
    mflix-seq:
      image: datalust/seq:2021.1
      container_name: mflix-seq
      restart: unless-stopped
      ports:
        - 8082:80
        - 5341:5341
      networks:
        - default
      environment:
        - ACCEPT_EULA=Y
      volumes:
        - type: volume
          source: mflix-seq-data
          target: /data 
    ```

---

## 4. Getting Started

### 4.1 Infrastructure

The complete _MFlix_ infrastructure is defined in a `docker-compose.yaml` stack. One could use _docker-compose_ commands to manage that stack. However, I think the easiest way to manage the stack is to use a task runner to execute commands relating to the management of the stack. I've decided to use _[npm]_ for my task runner. Although _[npm]_ is a package manager for the JavaScript programming language, it happens to make a good simple task runner as well. The tasks are defined as follows:

```json
{
  "name": "mflix",
  "version": "1.0.0",
  "scripts": {
    "up": "docker-compose -f ./fabric/docker-compose.yaml up --build --detach && docker-compose -f ./fabric/docker-compose.yaml ps",
    "down": "docker-compose -f ./fabric/docker-compose.yaml down --volumes && docker-compose -f ./fabric/docker-compose.yaml ps",
    "status": "docker-compose -f ./fabric/docker-compose.yaml ps",
    "describe": "docker-compose -f ./fabric/docker-compose.yaml config --services"
  }
}
```

#### 4.1.1 Manage Stack

To start infrastructure services,

```bash
npm run up
```

The above command will __START__ the `docker-compose.yaml` stack and display a summary of the services afterwards. The summary should display a list of running services. The task is defined as follows:

```bash
docker-compose -f ./fabric/docker-compose.yaml up --build --detach && docker-compose -f ./fabric/docker-compose.yaml ps
```

![mflix-npm-up](https://user-images.githubusercontent.com/33935506/111884357-1de6fe00-8a26-11eb-96d2-48246141b4d5.png)

To stop infrastructure services,

```bash
npm run down
```

The above command will __STOP__ the `docker-compose.yaml` stack and display a summary of the services afterwards. The summary should be empty if all services were stopped and removed successfully. The task is defined as follows:

```bash
docker-compose -f ./fabric/docker-compose.yaml down --volumes && docker-compose -f ./fabric/docker-compose.yaml ps
```

![mflix-npm-down](https://user-images.githubusercontent.com/33935506/111884354-1cb5d100-8a26-11eb-95ea-f5182c20ceb4.png)

Alternatively, one can install an _[npm extension]((https://github.com/Microsoft/vscode-npm-scripts))_ and run the your _npm_ tasks as follows:

![npm-task-runner-720](https://user-images.githubusercontent.com/33935506/111884505-e9277680-8a26-11eb-82b4-54d7af16931b.gif)

#### 4.1.2 Test Stack

To check the status of stack, run the following command:

```bash
npm run status
```

The above command should display a summary that lists the services that are running.

![mflix-npm-status](https://user-images.githubusercontent.com/33935506/111885399-62759800-8a2c-11eb-8d4f-96b85e94413e.png)

Open MongoDB shell

```bash
mongo --host localhost --port 27017 --username dbadmin --password password
```

![mflix-mongo-shell](https://user-images.githubusercontent.com/33935506/111885737-85a14700-8a2e-11eb-8e1d-a8793a5e9efc.png)

Open Mongo Express

```bash
navigate to http://localhost:8081
```

![mflix-mongo-express](https://user-images.githubusercontent.com/33935506/111885741-86d27400-8a2e-11eb-823d-a6988f30f770.png)

Open SEQ log server

```bash
navigate to http://localhost:8082
```

![mflix-seq](https://user-images.githubusercontent.com/33935506/111885740-86d27400-8a2e-11eb-953b-a359816a2928.png)

---

[MongoDB Docs]: https://docs.mongodb.com
[Docker]: https://www.docker.com
[MongoDB Atlas]: https://www.mongodb.com/cloud/atlas
[MongoDB Documentation]: https://docs.atlas.mongodb.com
[Daas]: https://en.wikipedia.org/wiki/Data_as_a_service
[MongoDB Compass]: https://www.mongodb.com/products/compass
[SEQ]: https://datalust.co/seq
[NPM]: https://www.npmjs.com/