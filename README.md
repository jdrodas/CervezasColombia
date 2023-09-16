# Cervezas Artesanales Colombia

For the english version of this readme file, scroll past the spanish version! ;-)

Aplicaciones usando .NET framework para demostrar conectividad a SQLite, PostgreSQL y MongoDB.

Por favor tenga presente:

- Es un proyecto académico que pretende evolucionar el aprendizaje de conceptos relacionados con bases de datos. Esta no es una aplicación "lista para producción".

- Toda la información almacenada en las tablas y colecciones de las bases de datos es información pública disponible en los sitios web de las cervecerías. No es 100% confiable y no pretende serlo.

- Puede clonar el repositorio e inclusive proponer mejoras a través de issues, pero no necesariamente serán implementadas en el tiempo asignado para el curso. Siempre será un trabajo en constante modificación


## PoC
### [CervezasColombia_CS_PoC_Consola](https://github.com/jdrodas/CervezasColombia/tree/main/CervezasColombia_CS_PoC_Consola)
- Prueba de Concepto para validar funcionamiento del ORM Dapper, con base de datos y PostgreSQL. Aplicación de **consola** en C# con framework .NET 7.x

- Para cada base de datos, se realizan las operaciones CRUD básicas sobre una entidad específica.
  
- Para SQLite, se realizan invocaciones simples a sentencias de SQL.

- Para PostgeSQL, se realizan invocaciones utilizando lógica almacenada.

## API
### [CervezasColombia_CS_API_SQLite_Dapper](https://github.com/jdrodas/CervezasColombia/tree/main/CervezasColombia_CS_API_SQLite_Dapper)
- WebAPI en .NET 7.x implementando *Patrón Repositorio* con capa de persistencia de datos en SQLite a través de Dapper


# Craft Beers Colombia

Apps using .NET framework to demo database connectivity using SQLite, PostgreSQL and MongoDB.

Please keep in mind:

- This is an academic project that aims to evolve the learning of concepts related to databases. This is not a "production ready" application.

- All information stored in the tables and collections of the databases is public information available on the breweries' websites. It is not 100% reliable and it does not claim to be.

- You can clone the repository and even propose improvements through github issues, but they will not necessarily be implemented in the time allotted for the course. It will always be a Work in progress.

## PoC
### [CervezasColombia_CS_PoC_Consola](https://github.com/jdrodas/CervezasColombia/tree/main/CervezasColombia_CS_PoC_Consola)
- Proof of Concept to learn how the Dapper ORM works, using SQLite and PostgreSQL as Database. **Console** application using .NET 7.x framework and C#

- For each database, basic CRUD operations are performed on a specific entity.
  
- For SQLite, simple invocations to SQL statements are made.

- For PostgeSQL, invocations are made using stored procedures.


## API
### [CervezasColombia_CS_API_SQLite_Dapper](https://github.com/jdrodas/CervezasColombia/tree/main/CervezasColombia_CS_API_SQLite_Dapper)
- .NET 7.x WebAPI implementing *Repository Pattern* with infrastructure persistence layer in SQLite using Dapper