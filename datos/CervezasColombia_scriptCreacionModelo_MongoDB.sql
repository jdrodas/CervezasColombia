-- Scripts de clase - Octubre 5 de 2023
-- Curso de Tópicos Avanzados de base de datos - UPB 202320
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Cervezas Artesanales de Colombia
-- Motor de Base de datos: MongoDB - 7.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen -- https://hub.docker.com/_/mongo
docker pull mongo:latest

-- Crear el contenedor
docker run --name mongodb-cervezas -e “MONGO_INITDB_ROOT_USERNAME=mongoadmin” -e MONGO_INITDB_ROOT_PASSWORD=unaClav3 -p 27017:27017 -d mongo:latest

-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Con usuario mongoadmin:

-- crear la base de datos
use cervezas_db;

-- #########################################################
-- PENDIENTE: Crear el usuario con privilegios limitados
-- #########################################################

-- Creamos las collecciones ... Sin validación

-- Estilos
db.createCollection("estilos");
db.createCollection("envasados");
db.createCollection("ubicaciones");
db.createCollection("ingredientes");
db.createCollection("tipos_ingredientes");
db.createCollection("cervecerias");
db.createCollection("cervezas");
db.createCollection("unidades_volumen");
db.createCollection("rangos_abv");
db.createCollection("rangos_ibu");


-- Creamos las collecciones ... usando un json schema para validación
db.createCollection("estilos", {
   validator: {
      $jsonSchema: {
         bsonType: "object",
         title: "Estilos de cervezas",
         required: [ "nombre" ],
         properties: {
            nombre: {
               bsonType: "string",
               description: "'nombre' Debe ser una cadena de caracteres y no puede ser nulo"
            }
         }
      }
   }
} );

db.createCollection("envasados", {
   validator: {
      $jsonSchema: {
         bsonType: "object",
         title: "Recipientes para el envasado de las cervezas",
         required: [ "nombre" ],
         properties: {
            nombre: {
               bsonType: "string",
               description: "'nombre' Debe ser una cadena de caracteres y no puede ser nulo"
            }
         }
      }
   }
} );

db.createCollection("unidades_volumen", {
   validator: {
      $jsonSchema: {
         bsonType: "object",
         title: "Unidades de volumen para los envasados",
         required: [ "nombre","abreviatura" ],
         properties: {
            nombre: {
               bsonType: "string",
               description: "'nombre' Debe ser una cadena de caracteres y no puede ser nulo"
            },
            abreviatura: {
               bsonType: "string",
               description: "'abreviatura' Debe ser una cadena de caracteres y no puede ser nulo"
            }
         }
      }
   }
} );


db.createCollection("rangos_abv", {
   validator: {
      $jsonSchema: {
         bsonType: "object",
         title: "Rangos de Alcohol por volumen para las cervezas",
         required: [ "nombre","valor_inicial","valor_final" ],
         properties: {
            nombre: {
               bsonType: "string",
               description: "'nombre' Debe ser una cadena de caracteres y no puede ser nulo"
            },
            valor_inicial: {
               bsonType: "number",
               minimum: 0,
               description: "'valor_inicial' Debe ser numérico mínimo 0 y no puede ser nulo"
            },
            valor_final: {
               bsonType: "number",
               minimum: 0,
               description: "'valor_final' Debe ser numérico mínimo 0 y no puede ser nulo"
            }
         }
      }
   }
} );

-- ***************************************************
-- Consultas de apoyo para implementar el repositorio
-- ***************************************************

-- Estilos

-- Todos los estilos
db.estilos.find();

-- Estilo por Id
db.estilos.find({_id: ObjectId('651ea3cf5e56a5ee0e682443')});

-- Estilo por nombre
db.estilos.find({ nombre: "Amber Ale"});

-- Total Estilos
db.estilos.find().count();

-- ubicaciones

-- Todas las ubicaciones
db.ubicaciones.find();

-- Ubicación por Id
db.ubicaciones.find({_id: ObjectId('651ff74da87f7d3f845fd9d7')});

-- Ubicación por municipio
db.ubicaciones.find({municipio:"Neiva"});


-- Total Ubicaciones
db.ubicaciones.find().count();

-- ingredientes

-- Todos los ingredientes
db.ingredientes.find()

-- Total de ingredientes
db.ingredientes.find().count()

-- Ingrediente por tipo de ingredientes
db.ingredientes.find({tipo_ingrediente:"Saborizantes"});

-- envasados

--Insertar nuevo Envasado
db.envasados.insertOne({nombre:"Vasito Biodegradable"});