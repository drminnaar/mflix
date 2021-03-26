# MFlix

A showcase of how to build API's (REST, gRPC, GraphQL) using C# .NET.

I chose _MFlix_ as the name for this project because I am using one of the sample databases provided by MongoDB called MFlix. The MFlix database is composed of collections of movie related data. I provide more detail in the MongoDB section where I explain how to get a copy of the MFlix database. Therefore, because of the database name, and the fact that _MFlix_ is a short and catchy name, I decided to go with _MFlix_.

- [1. Purpose](#1-purpose)
- [2. Description](#2-description)
- [3. Infrastructure](#3-infrastructure)
- [4. Getting Started](#4-getting-started)
- [5. Test MFlix gRPC services](#5-test-mflix-grpc-services)

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

Follow the following steps to get up and running:

- Get the code
- Manage infrastructure
- Start MFlix gRpc API

### 4.1 Get The Code

Use any of the following options to get a copy of the code:

```bash
# Download Zip File
wget https://github.com/drminnaar/mflix/archive/refs/heads/main.zip

# Clone
git clone https://github.com/drminnaar/mflix.git
```

### 4.2 Manage Infrastructure

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

#### 4.2.1 Start Stack

To start infrastructure services,

```bash
npm run up
```

The above command will __START__ the `docker-compose.yaml` stack and display a summary of the services afterwards. The summary should display a list of running services. The task is defined as follows:

```bash
docker-compose -f ./fabric/docker-compose.yaml up --build --detach && docker-compose -f ./fabric/docker-compose.yaml ps
```

![mflix-npm-up](https://user-images.githubusercontent.com/33935506/111884357-1de6fe00-8a26-11eb-96d2-48246141b4d5.png)

#### 4.2.2 Stop Stack

To stop infrastructure services,

```bash
npm run down
```

The above command will __STOP__ the `docker-compose.yaml` stack and display a summary of the services afterwards. The summary should be empty if all services were stopped and removed successfully. The task is defined as follows:

```bash
docker-compose -f ./fabric/docker-compose.yaml down --volumes && docker-compose -f ./fabric/docker-compose.yaml ps
```

![mflix-npm-down](https://user-images.githubusercontent.com/33935506/111884354-1cb5d100-8a26-11eb-95ea-f5182c20ceb4.png)

#### 4.2.3 Describe Stack

To describe infrastructure services,

```bash
npm run describe
```

The above command will use the `docker-compose.yaml` stack to display a summary of the stack services. The summary should be empty if all services were stopped and removed successfully. The task is defined as follows:

```bash
docker-compose -f ./fabric/docker-compose.yaml config --services
```

![mflix-npm-describe](https://user-images.githubusercontent.com/33935506/112548500-363d8b00-8e21-11eb-81a0-6e9db9a7d174.png)

#### 4.2.4 Get Stack Status

To get the current status of infrastructure services,

```bash
npm run status
```

The above command will use the `docker-compose.yaml` stack to display a detailed summary of the stack services with their corresponding status. The summary should be empty if all services were stopped and removed successfully. The task is defined as follows:

```bash
docker-compose -f ./fabric/docker-compose.yaml config ps
```

![mflix-npm-status](https://user-images.githubusercontent.com/33935506/112549384-95e86600-8e22-11eb-8c95-1416ff3f7bd3.png)

#### 4.2.5 Manage Stack Using NPM

Alternatively, one can install an _[npm extension](https://github.com/Microsoft/vscode-npm-scripts)_ and run the your _npm_ tasks as follows:

![npm-task-runner-720](https://user-images.githubusercontent.com/33935506/111884505-e9277680-8a26-11eb-82b4-54d7af16931b.gif)

#### 4.2.6 Test Stack

There are currently 3 services that will mostly be used. The services are listed as follows:

- MongoDB

  ```bash
  mongo --host localhost --port 27017 --username dbadmin --password password
  ```

  ![mflix-mongo-shell](https://user-images.githubusercontent.com/33935506/111885737-85a14700-8a2e-11eb-8e1d-a8793a5e9efc.png)

- MongoDB Express

  ```bash
  navigate to http://localhost:8081
  ```

  ![mflix-mongo-express](https://user-images.githubusercontent.com/33935506/111885741-86d27400-8a2e-11eb-823d-a6988f30f770.png)

- SEQ Log Server

  ```bash
  navigate to http://localhost:8082
  ```

  ![mflix-seq](https://user-images.githubusercontent.com/33935506/111885740-86d27400-8a2e-11eb-953b-a359816a2928.png)

### 4.3 Start MFlix gRpc API

Use any of the following options to start the MFlix gRpc API:

```bash
# Use dotnet cli command
dotnet watch run --project ./src/MFlix.GrpcApi/MFlix.GrpcApi.csproj
```

```bash
# Use NPM task runner
npm run start:grpc
```

---

## 5. Test MFlix gRPC Services

Before testing the gRPC services that are defined by the MFlix gRPC server, you will need the following:

- All infrastructure services must be started
- the MFlix gRPC API must be started

See the [Getting Started Section](#4-getting-started) to get more information.

I recommend the following tools for testing gRPC services:

- [gRPCurl](#51-grpcurl)
- [gRPCUI](#52-grpcui)
- [BloomRPC](#53-bloomrpc)
- [Insomnia](#54-insomnia)

### 5.1 gRPCurl

You will need to have [golang (go)](https://golang.org/) installed before installing `gRPCurl`. See the install instructions at the [official golang documentation](https://golang.org/doc/install).

All the following commands have been tested on the [cross-platform (Windows, Linux, and macOS) Powershell](https://github.com/PowerShell/PowerShell)

### 5.1.1 Install gRPCurl

```powershell
# install grpcurl using go
go get github.com/fullstorydev/grpcurl/...
go install github.com/fullstorydev/grpcurl/cmd/grpcurl

# after install
grpcurl -version

# OUTPUT:
grpcurl.exe v1.7.0
```

### 5.1.2 Use gRPCurl

#### Get Help

```powershell
# INPUT:
grpcurl -h
```

#### List Available Services

```powershell
# INPUT:
grpcurl localhost:5001 list

# OUTPUT:
grpc.reflection.v1alpha.ServerReflection
mflix.services.MovieService
```

#### Describe Available Services (Detailed List)

```powershell
# INPUT:
grpcurl localhost:5001 describe

# OUTPUT:
grpc.reflection.v1alpha.ServerReflection is a service:
service ServerReflection {
  rpc ServerReflectionInfo ( stream .grpc.reflection.v1alpha.ServerReflectionRequest ) returns ( stream .grpc.reflection.v1alpha.ServerReflectionResponse );
}
mflix.services.MovieService is a service:
service MovieService {
  rpc DeleteMovie ( .mflix.services.DeleteMovieRequest ) returns ( .mflix.services.DeleteMovieResponse );
  rpc GetMovieById ( .mflix.services.GetMovieByIdRequest ) returns ( .mflix.services.GetMovieByIdResponse );
  rpc GetMovieList ( .mflix.services.GetMovieListRequest ) returns ( .mflix.services.GetMovieListResponse );
  rpc SaveImdbRating ( .mflix.services.SaveImdbRatingRequest ) returns ( .mflix.services.SaveImdbRatingResponse );
  rpc SaveMetacriticRating ( .mflix.services.SaveMetacriticRatingRequest ) returns ( .mflix.services.SaveMetacriticRatingResponse );
  rpc SaveMovie ( .mflix.services.SaveMovieRequest ) returns ( .mflix.services.SaveMovieResponse );
  rpc SaveTomatoesRating ( .mflix.services.SaveTomatoesRatingRequest ) returns ( .mflix.services.SaveTomatoesRatingResponse );
}
```

#### Describe Movie Service

```powershell
#INPUT:
grpcurl localhost:5001 describe mflix.services.MovieService

# OUTPUT:
mflix.services.MovieService is a service:
service MovieService {
  rpc DeleteMovie ( .mflix.services.DeleteMovieRequest ) returns ( .mflix.services.DeleteMovieResponse );
  rpc GetMovieById ( .mflix.services.GetMovieByIdRequest ) returns ( .mflix.services.GetMovieByIdResponse );
  rpc GetMovieList ( .mflix.services.GetMovieListRequest ) returns ( .mflix.services.GetMovieListResponse );
  rpc SaveImdbRating ( .mflix.services.SaveImdbRatingRequest ) returns ( .mflix.services.SaveImdbRatingResponse );
  rpc SaveMetacriticRating ( .mflix.services.SaveMetacriticRatingRequest ) returns ( .mflix.services.SaveMetacriticRatingResponse );
  rpc SaveMovie ( .mflix.services.SaveMovieRequest ) returns ( .mflix.services.SaveMovieResponse );
  rpc SaveTomatoesRating ( .mflix.services.SaveTomatoesRatingRequest ) returns ( .mflix.services.SaveTomatoesRatingResponse );
}
```

#### List Movie Service Methods

```powershell
# INPUT: list methods for MovieService
grpcurl localhost:5001 list mflix.services.MovieService

# OUTPUT:
mflix.services.MovieService.DeleteMovie
mflix.services.MovieService.GetMovieById
mflix.services.MovieService.GetMovieList
mflix.services.MovieService.SaveImdbRating
mflix.services.MovieService.SaveMetacriticRating
mflix.services.MovieService.SaveMovie
mflix.services.MovieService.SaveTomatoesRating
```

#### Describe Requests

```powershell
# INPUT:
grpcurl localhost:5001 describe mflix.services.GetMovieListRequest

# OUTPUT
mflix.services.GetMovieListRequest is a message:
message GetMovieListRequest {
  .mflix.services.MovieOptions options = 1;
}


# INPUT:
grpcurl localhost:5001 describe mflix.services.MovieOptions

# OUTPUT
mflix.services.MovieOptions is a message:
message MovieOptions {
  int32 pageNumber = 1;
  int32 pageSize = 2;
  repeated string sortBy = 3;
  string title = 4;
  string rated = 5;
  string runtime = 6;
  string year = 7;
  string type = 8;
  repeated string cast = 9;
  repeated string genres = 10;
  repeated string directors = 11;
}
```

#### Get Movie By Id

```powershell
# INPUT
$request = @'
{
  \"movieId\": \"573a1397f29313caabce68f6\"
}
'@

grpcurl -d $request localhost:5001 mflix.services.MovieService/GetMovieById


# OUTPUT:
{
  "movie": {
    "id": "573a1397f29313caabce68f6",
    "title": "Star Wars: Episode IV - A New Hope",
    "runtime": 121,
    "rated": "PG",
    "year": 1977,
    "poster": "https://m.media-amazon.com/images/M/MV5BNzVlY2MwMjktM2E4OS00Y2Y3LWE3ZjctYzhkZGM3YzA1ZWM2XkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_SY1000_SX677_AL_.jpg",
    "imdb": {
      "rating": 8.7,
      "votes": 773938,
      "id": 76759
    },
    "tomatoes": {
      "consensus": "A legendarily expansive and ambitious start to the sci-fi saga, George Lucas opened our eyes to the possiblites of blockbuster filmmaking and things have never been the same.",
      "critic": {
        "rating": 8.5,
        "numReviews": 82,
        "meter": 94
      },
      "dvd": "2004-09-21T00:00:00Z",
      "fresh": 77,
      "lastUpdated": "2015-09-12T17:04:03Z",
      "production": "20th Century Fox",
      "rotten": 5,
      "viewer": {
        "rating": 4.1,
        "numReviews": 848179,
        "meter": 96
      },
      "website": "http://www.starwars.com/episode-iv/"
    },
    "cast": [
      "Mark Hamill",
      "Harrison Ford",
      "Carrie Fisher",
      "Peter Cushing"
    ],
    "genres": [
      "Action",
      "Adventure",
      "Fantasy"
    ],
    "directors": [
      "George Lucas"
    ]
  }
}
```

#### Get List Of Movies

```powershell
# INPUT
$request = @'
{
    \"options\": {
    \"pageNumber\": 1,
    \"pageSize\": 20,
    \"sortBy\": [\"-year\"],
    \"title\": \"batman\",
    \"rated\": \"\",
    \"runtime\": \"\",
    \"year\": \"gte:2000\",
    \"type\": \"\",
    \"cast\": [],
    \"genres\": [],
    \"directors\": []
  }
}
'@

grpcurl -d $request localhost:5001 mflix.services.MovieService/GetMovieList

# OUTPUT
{
  "movies": [ ... ],
  "pageInfo": {
    "currentPageNumber": 1,
    "nextPageNumber": 0,
    "previousPageNumber": 0,
    "lastPageNumber": 1,
    "itemCount": "11",
    "pageSize": 20,
    "pageCount": 1,
    "hasPrevious": false,
    "hasNext": false
  }
}
```

#### Save Movie

```powershell
# INPUT:
$request = @'
{
    \"movie\": {
        \"id\": \"\",
        \"title\": \"API Wars\",
        \"plot\": \"One APIs great struggle against all odds to be the number one API .... in the world!\",
        \"runtime\": 120,
        \"rated\": \"PG\",
        \"year\": 2021,
        \"poster\": \"https://picsum.photos/200/300\",
        \"released\": \"2021-03-26T00:00:00Z\",
        \"genres\": [\"Action\"],
        \"cast\": [\"gRPC\",\"REST\",\"GraphQL\"],
        \"directors\": [\"Douglas Minnaar\"]
    }
}
'@

grpcurl -d $request localhost:5001 mflix.services.MovieService/SaveMovie


# OUTPUT:
{
  "movieId": "605d5276f164eea829cdc9a0"
}
```

#### Save IMDB Rating

```powershell
# INPUT:
$request = @'
{
    \"movieId\": \"573a1397f29313caabce68f6\",
    \"imdb\": {
        \"rating\": 5.4,
        \"votes\": 104354,
        \"id\": 10384738
    }
}
'@

grpcurl -d $request localhost:5001 mflix.services.MovieService/SaveImdbRating


# OUTPUT:
{
  "imdb": {
    "rating": 5.4,
    "votes": 104354,
    "id": 10384738
  }
}
```

#### Save Tomatoes Rating

```powershell
# INPUT:
$request = @'
{
    \"movieId\": \"573a1397f29313caabce68f6\",
    \"tomatoes\": {
        \"boxOffice\": \"\",
        \"consensus\": \"A legendary movie about legends\",
        \"critic\": {
          \"rating\": 1.4,
          \"numReviews\": 10,
          \"meter\": 10
        },
        \"dvd\": \"2021-03-26T00:00:00Z\",
        \"fresh\": 65,
        \"lastUpdated\": \"2021-03-26T00:00:00Z\",
        \"production\": \"20th Century Fox\",
        \"rotten\": 10,
        \"viewer\": {
          \"rating\": 16.4,
          \"numReviews\": 130,
          \"meter\": 1055
        },
        \"website\": \"http://www.example.com\"
    }
}
'@

grpcurl -d $request localhost:5001 mflix.services.MovieService/SaveTomatoesRating


# OUTPUT:
{
  "tomatoes": {
    "consensus": "A legendary movie about legends",
    "critic": {
      "rating": 1.4,
      "numReviews": 10,
      "meter": 10
    },
    "dvd": "2021-03-26T00:00:00Z",
    "fresh": 65,
    "lastUpdated": "2021-03-26T00:00:00Z",
    "production": "20th Century Fox",
    "rotten": 10,
    "viewer": {
      "rating": 16.4,
      "numReviews": 130,
      "meter": 1055
    },
    "website": "http://www.example.com"
  }
}
```

#### Save Metacritic Rating

```powershell
# INPUT:
$request = @'
{
    \"movieId\": \"573a1397f29313caabce68f6\",
    \"metacriticRating\": 89
}
'@

grpcurl -d $request localhost:5001 mflix.services.MovieService/SaveMetacriticRating


# OUTPUT
{
  "metacriticRating": 89
}
```

#### Delete Movie

```powershell
# INPUT:
$request = @'
{
    \"movieId\": \"573a1397f29313caabce68f6\"
}
'@

grpcurl -d $request localhost:5001 mflix.services.MovieService/DeleteMovie


# OUTPUT
{
  "movieId": "573a1397f29313caabce68f6"
}
```

### 5.2 gRPCUI

[gRPCUI](https://github.com/fullstorydev/grpcui) is a command-line tool that lets you interact with gRPC servers via a browser.

You will need to have [golang (go)](https://golang.org/) installed before installing `gRPCurl`. See the install instructions at the [official golang documentation](https://golang.org/doc/install).

All the following commands have been tested on the [cross-platform (Windows, Linux, and macOS) Powershell](https://github.com/PowerShell/PowerShell)

### 5.2.1 Install gRPCUI

```powershell
# install grpcui using go
go get github.com/fullstorydev/grpcui/...
go install github.com/fullstorydev/grpcui/cmd/grpcui

# after install
grpcui -version
```

### 5.2.2 Run gRPCUI

```powershell
grpcui localhost:5001

# OUTPUT:
gRPC Web UI available at http://127.0.0.1:63382/
```

#### Build Form Request

![grpcui-request-form](https://user-images.githubusercontent.com/33935506/112590301-7a537e80-8e67-11eb-90a7-10456de60fd8.png)

#### Build JSON Request

![grpcui-request-json](https://user-images.githubusercontent.com/33935506/112590303-7b84ab80-8e67-11eb-8e96-16effb53f2fd.png)

#### Response

![grpcui-response](https://user-images.githubusercontent.com/33935506/112590305-7b84ab80-8e67-11eb-92e8-dfa6a2c4ea9d.png)

### 5.3 BloomRPC

[BloomRPC](https://github.com/uw-labs/bloomrpc) aims to provide the simplest and most efficient developer experience for exploring and querying your GRPC services.

- Supports all major operating systems (MacOS / Windows / Linux Deb - Arch Linux)
- Native GRPC calls
- Unary Calls and Server Side Streaming Support
- Client side and Bi-directional Streaming
- Automatic Input recognition
- Multi tabs operations
- Metadata support
- Persistent Workspace
- Request Cancellation

### 5.3.1 Install

- Mac Install

  ```bash
  brew install --cask bloomrpc
  ```

- Windows Install

  ```bash
  # install using chocolatey
  choco install bloomrpc
  ```

### 5.3.2 Run BloomRPC

![bloomrpc](https://user-images.githubusercontent.com/33935506/112591118-bd622180-8e68-11eb-943e-2a2f3a997872.png)

### 5.4 Insomnia

[Insomnia](https://insomnia.rest/) is an Open Source API client, and collaborative API design platform for REST, SOAP, GraphQL, and GRPC.

- Organize requests
- Manage multiple environments
- Supports multiple protocols (gRPC, GraphQL, REST, SOAP)
- Theme Support

[Find a list of downloads here](https://github.com/kong/insomnia/releases/tag/core@2021.2.1).

For Windows, you can also install with chocolatey

```powershell
choco install insomnia-rest-api-client
```

#### Get Movie By Id

![insomnia-getmoviebyid](https://user-images.githubusercontent.com/33935506/112592708-48441b80-8e6b-11eb-9149-d241c93b5fe0.png)

#### Save Movie

![insomnia-savemovie](https://user-images.githubusercontent.com/33935506/112594530-fb157900-8e6d-11eb-99e8-fa11d4b717c6.png)

---

[MongoDB Docs]: https://docs.mongodb.com
[Docker]: https://www.docker.com
[MongoDB Atlas]: https://www.mongodb.com/cloud/atlas
[MongoDB Documentation]: https://docs.atlas.mongodb.com
[Daas]: https://en.wikipedia.org/wiki/Data_as_a_service
[MongoDB Compass]: https://www.mongodb.com/products/compass
[SEQ]: https://datalust.co/seq
[NPM]: https://www.npmjs.com/
