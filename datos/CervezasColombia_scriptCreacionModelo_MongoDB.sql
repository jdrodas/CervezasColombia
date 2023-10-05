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

-- Creamos las collecciones 

-- Estilos
db.createCollection("estilos");
db.createCollection("envasados");
db.createCollection("ubicaciones");
db.createCollection("ingredientes");
db.createCollection("cervecerias");
db.createCollection("cervezas");
db.createCollection("unidades_volumen");
db.createCollection("rangos_abv");
db.createCollection("rangos_ibu");

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
db.ubicaciones.find({_id: ObjectId('651ea44c5e56a5ee0e682484')});

-- Ubicación por municipio
db.ubicaciones.find({municipio:"Neiva"});


-- Total Ubicaciones
db.ubicaciones.find().count();

