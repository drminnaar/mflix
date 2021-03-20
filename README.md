# MFlix

A showcase of how to build API's (REST, gRPC, GraphQL) using C# .NET.

## The Name

I chose _MFlix_ as the name for this project because I am using one of the sample databases provided by MongoDB called MFlix. The MFlix database is composed of collections of movie related data. I provide more detail in the MongoDB section where I explain how to get a copy of the MFlix database. Therefore, because of the database name, and the fact that _MFlix_ is a short and catchy name, I decided to go with _MFlix_.

## Purpose

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

## Description

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