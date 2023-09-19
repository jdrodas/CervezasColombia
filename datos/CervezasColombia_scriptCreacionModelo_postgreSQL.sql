-- Scripts de clase - Septiembre 16 de 2023
-- Curso de Tópicos Avanzados de base de datos - UPB 202320
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Cervezas Artesanales de Colombia
-- Motor de Base de datos: PostgreSQL

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen
docker pull postgres:latest

-- Crear el contenedor
docker run --name postgres-cervezas -e POSTGRES_PASSWORD=unaClav3 -d -p 5432:5432 postgres:latest

-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Con usuario Root:

-- crear el esquema la base de datos
create database cervezas_db;

-- Conectarse a la base de datos
\c cervezas_db;

-- Creamos un esquema para almacenar todo el modelo de datos del dominio
create schema core;

-- crear el usuario con el que se floatizará la creación del modelo
create user cervezas_app with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database cervezas_db to cervezas_app;
grant create on database cervezas_db to cervezas_app;
grant create, usage on schema core to cervezas_app;
alter user cervezas_app set search_path to core;

set timezone='America/Bogota';

-- Script de creación de tablas y vistas

-- -----------------------
-- Tabla Rangos_ABV
-- -----------------------
create table core.rangos_abv (
  id                    integer generated always as identity constraint rangosAbv_pk primary key,
  nombre                varchar(50) not null constraint rangosAbv_nombre_uk unique,
  valor_inicial         float not null,
  valor_final           float not null,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp,
  constraint rangosAbv_uk unique (valor_inicial, valor_final) 
);

comment on table core.rangos_abv is 'Rangos de Alcohol por volumen para las cervezas';
comment on column core.rangos_abv.id is 'id del rango';
comment on column core.rangos_abv.nombre is 'nombre del rango';
comment on column core.rangos_abv.valor_inicial is 'valor inicial del rango';
comment on column core.rangos_abv.valor_final is 'valor final del rango';
comment on column core.rangos_abv.fecha_creacion is 'fecha de creación del registro';
comment on column core.rangos_abv.fecha_actualizacion is 'fecha de actualización del registro';

-- -----------------------
-- Tabla Rangos_IBU
-- -----------------------
create table core.rangos_ibu (
  id                    integer generated always as identity constraint rangosIbu_pk primary key,
  nombre                varchar(50) not null constraint rangosIbu_nombre_uk unique,
  valor_inicial         float not null,
  valor_final           float not null,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp,
  constraint rangosIbu_uk unique (valor_inicial, valor_final) 
);

comment on table core.rangos_ibu is 'Rangos de Unidades Internacionales de Amargor para las cervezas';
comment on column core.rangos_ibu.id is 'id del rango';
comment on column core.rangos_ibu.nombre is 'nombre del rango';
comment on column core.rangos_ibu.valor_inicial is 'valor inicial del rango';
comment on column core.rangos_ibu.valor_final is 'valor final del rango';
comment on column core.rangos_ibu.fecha_creacion is 'fecha de creación del registro';
comment on column core.rangos_ibu.fecha_actualizacion is 'fecha de actualización del registro';

-- -----------------------
-- Tabla Estilos
-- -----------------------
create table core.estilos
(
  id                    integer generated always as identity constraint estilos_pk primary key,
  nombre                varchar(100)  not null constraint estilo_nombre_uk unique,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp  
);

comment on table core.estilos is 'Estilos de cervezas';
comment on column core.estilos.id is 'id del estilo';
comment on column core.estilos.nombre is 'nombre del estilo';
comment on column core.estilos.fecha_creacion is 'fecha de creación del registro';
comment on column core.estilos.fecha_actualizacion is 'fecha de actualización del registro';

-- ------------------------------
-- Tabla envasados
-- ------------------------------
create table core.envasados (
  id                    integer generated always as identity constraint envasados_pk primary key,
  nombre                varchar(50)  not null constraint envasados_nombre_uk unique,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp  
);

comment on table core.envasados is 'Recipientes para el envasado de las cervezas';
comment on column core.envasados.id is 'id del envasado';
comment on column core.envasados.nombre is 'nombre del envasado';
comment on column core.envasados.fecha_creacion is 'fecha de creación del registro';
comment on column core.envasados.fecha_actualizacion is 'fecha de actualización del registro';

-- ------------------------------
-- Tabla unidades_volumen
-- ------------------------------
create table core.unidades_volumen (
  id                    integer generated always as identity constraint unidades_volumen_pk primary key,  
  nombre                varchar(100)  not null constraint unidades_volumen_nombre_uk unique,
  abreviatura           varchar(10)  not null constraint unidades_volumen_abreviatura_uk unique,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp, 
  constraint unidades_volumen_uk unique (nombre, abreviatura)
);

comment on table core.unidades_volumen is 'Unidades de volumen para los envasados';
comment on column core.unidades_volumen.id is 'id de la unidad de volumen';
comment on column core.unidades_volumen.nombre is 'nombre de la unidad de volumen';
comment on column core.unidades_volumen.abreviatura is 'abreviatura de la unidad de volumen';
comment on column core.unidades_volumen.fecha_creacion is 'fecha de creación del registro';
comment on column core.unidades_volumen.fecha_actualizacion is 'fecha de actualización del registro';

-- -----------------------
-- Tabla Ubicaciones
-- -----------------------
create table core.ubicaciones (
  id                    integer generated always as identity constraint ubicaciones_pk primary key,
  municipio             varchar(100) not null,
  departamento          varchar(100) not null,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp,
  constraint ubicaciones_uk unique (municipio, departamento)    
);

comment on table core.ubicaciones is 'Ubicaciones de las Cervecerias';
comment on column core.ubicaciones.id is 'id de la ubicación';
comment on column core.ubicaciones.municipio is 'municipio de la ubicación';
comment on column core.ubicaciones.departamento is 'departamento de la ubicacion';
comment on column core.unidades_volumen.fecha_creacion is 'fecha de creación del registro';
comment on column core.unidades_volumen.fecha_actualizacion is 'fecha de actualización del registro';

-- --------------------------
-- Tabla tipos_ingredientes
-- --------------------------
create table core.tipos_ingredientes
(
  id                    integer generated always as identity constraint tipos_ingredientes_pk primary key,
  nombre                varchar(100)  not null constraint tipos_ingredientes_nombre_uk unique,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp
);

comment on table core.tipos_ingredientes is 'Tipos de ingredientes de las cervezas';
comment on column core.tipos_ingredientes.id is 'id del tipo de ingrediente';
comment on column core.tipos_ingredientes.nombre is 'nombre del tipo de ingrediente';
comment on column core.tipos_ingredientes.fecha_creacion is 'fecha de creación del registro';
comment on column core.tipos_ingredientes.fecha_actualizacion is 'fecha de actualización del registro';

-- --------------------------
-- Tabla ingredientes
-- --------------------------
create table core.ingredientes (
  id                    integer generated always as identity constraint ingredientes_pk primary key,
  tipo_ingrediente_id   integer not null constraint ingrediente_tipo_ingrediente_fk references core.tipos_ingredientes,  
  nombre                varchar(100)  not null,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp,
  constraint ingrediente_tipo_ingrediente_uk unique (tipo_ingrediente_id, nombre)  
);

comment on table core.ingredientes is 'ingredientes que pueden tener de las cervezas';
comment on column core.ingredientes.id is 'id del ingrediente';
comment on column core.ingredientes.id is 'id del tipo de ingrediente';
comment on column core.ingredientes.nombre is 'nombre del ingrediente';
comment on column core.ingredientes.fecha_creacion is 'fecha de creación del registro';
comment on column core.ingredientes.fecha_actualizacion is 'fecha de actualización del registro';

-- -----------------------
-- Tabla Cervecerias
-- -----------------------
create table core.cervecerias (
  id                    integer generated always as identity constraint cervecerias_pk primary key,
  nombre                varchar(100) not null constraint cerveceria_nombre_uk unique,
  ubicacion_id          integer      not null constraint cerveceria_ubicacion_fk references core.ubicaciones,
  sitio_web             varchar(200) not null constraint cerveceria_sitio_web_uk unique,
  instagram             varchar(100) not null constraint cerveceria_instagram_uk unique,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp
);

comment on table core.cervecerias is 'Las Cervecerias';
comment on column core.cervecerias.id is 'id de la Cerveceria';
comment on column core.cervecerias.nombre is 'Nombre de la Cerveceria';
comment on column core.cervecerias.ubicacion_id is 'Id de la ubicación de la Cervecería';
comment on column core.cervecerias.sitio_web is 'Sitio Web de la Cervecería';
comment on column core.cervecerias.instagram is 'Instagram de la Cerveceria';
comment on column core.cervecerias.fecha_creacion is 'fecha de creación del registro';
comment on column core.cervecerias.fecha_actualizacion is 'fecha de actualización del registro';

-- -----------------------
-- Tabla Cervezas
-- -----------------------
create table core.cervezas (
  id                    integer generated always as identity constraint cervezas_pk primary key,
  nombre                varchar(100) not null,
  cerveceria_id         integer      not null constraint cerveza_cerveceria_fk references core.cervecerias,
  estilo_id             integer      not null constraint cerveza_estilo_fk references core.estilos,  
  ibu                   float        not null,
  abv                   float        not null,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp,
  constraint cervezas_uk unique (nombre,estilo_id,cerveceria_id),
  constraint cerveza_cerveceria_uk unique (nombre, cerveceria_id)
);

comment on table core.cervezas is 'Las Cervezas';
comment on column core.cervezas.id is 'id de la cerveza';
comment on column core.cervezas.nombre is 'nombre de la cerveza';
comment on column core.cervezas.cerveceria_id is 'Id de la cervecería';
comment on column core.cervezas.estilo_id is 'Id del estilo';
comment on column core.cervezas.ibu is 'valor del Ibu de la cerveza';
comment on column core.cervezas.abv is 'valor del abv de la cerveza';
comment on column core.cervezas.fecha_creacion is 'fecha de creación del registro';
comment on column core.cervezas.fecha_actualizacion is 'fecha de actualización del registro';


-- ------------------------------
-- Tabla envasados_cervezas
-- ------------------------------
create table core.envasados_cervezas (
  cerveza_id            integer not null constraint envasados_cervezas_cerveza_fk references core.cervezas,  
  envasado_id           integer not null constraint envasados_cervezas_envasado_fk references core.envasados,
  unidad_volumen_id     integer not null constraint envasados_cervezas_unidad_volumen_fk references core.unidades_volumen,
  volumen               float not null,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp,
  constraint envasados_cervezas_pk primary key (cerveza_id, envasado_id, unidad_volumen_id, volumen)
);

comment on table core.envasados_cervezas is 'Recipientes en los cuales se envasa las cervezas';
comment on column core.envasados_cervezas.cerveza_id is 'id de la cerveza';
comment on column core.envasados_cervezas.envasado_id is 'id del envasado';
comment on column core.envasados_cervezas.unidad_volumen_id is 'id de la unidad de volumen';
comment on column core.envasados_cervezas.volumen is 'volumen del envasado';
comment on column core.envasados_cervezas.fecha_creacion is 'fecha de creación del registro';
comment on column core.envasados_cervezas.fecha_actualizacion is 'fecha de actualización del registro';

-- ------------------------------
-- Tabla ingredientes_cervezas
-- ------------------------------

create table core.ingredientes_cervezas (
  cerveza_id            integer not null constraint ingredientes_cervezas_cerveza_fk references core.cervezas,  
  ingrediente_id        integer not null constraint ingredientes_cervezas_ingrediente_fk references core.ingredientes,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp,
  constraint ingredientes_cervezas_pk primary key (cerveza_id, ingrediente_id)
);

comment on table core.ingredientes_cervezas is 'ingredientes identificados de las cervezas';
comment on column core.ingredientes_cervezas.cerveza_id is 'id de la cerveza';
comment on column core.ingredientes_cervezas.ingrediente_id is 'id del ingrediente';
comment on column core.ingredientes_cervezas.fecha_creacion is 'fecha de creación del registro';
comment on column core.ingredientes_cervezas.fecha_actualizacion is 'fecha de actualización del registro';

-- Vistas:

-- ------------------------------
-- Vista: v_info_cervezas
-- ------------------------------
create view v_info_cervezas as
select
    cz.id cerveza_id,
    cz.nombre cerveza,
    c.id cerveceria_id,
    c.nombre cerveceria,
    e.id estilo_id,
    e.nombre estilo,
    cz.ibu,
    ri.nombre rango_ibu,
    cz.abv,
    ra.nombre rango_abv
from cervezas cz
    join cervecerias c on cz.cerveceria_id = c.id
    join estilos e on cz.estilo_id = e.id,
rangos_abv ra, rangos_ibu ri
where cz.ibu between ri.valor_inicial and ri.valor_final
    and cz.abv between ra.valor_inicial and ra.valor_final;

-- ------------------------------
-- Vista: v_info_ingredientes
-- ------------------------------
create view v_info_ingredientes as
select
    ti.id tipo_ingrediente_id,
    ti.nombre tipo_ingrediente,
    i.id ingrediente_id,
    i.nombre ingrediente
    from ingredientes i
    join tipos_ingredientes ti on i.tipo_ingrediente_id = ti.id;

-- --------------------------------------
-- Vista: v_info_ingredientes_cervezas
-- --------------------------------------
create view v_info_ingredientes_cervezas as
select
    c.id cerveceria_id,
    c.nombre cerveceria,
    cv.id cerveza_id,
    cv.nombre cerveza,
    ti.id tipo_ingrediente_id,
    ti.nombre tipo_ingrediente,
    i.id ingrediente_id,
    i.nombre ingrediente
from ingredientes_cervezas ic
    join cervezas cv on cv.id = ic.cerveza_id
    join cervecerias c on cv.cerveceria_id = c.id
    join ingredientes i on i.id = ic.ingrediente_id
    join tipos_ingredientes ti on i.tipo_ingrediente_id = ti.id;	  

-- --------------------------------------
-- Vista: v_info_envasados_cervezas
-- --------------------------------------
create view v_info_envasados_cervezas as
select
    cr.id cerveceria_id,
    cr.nombre cerveceria,
    ec.cerveza_id,
    c.nombre cerveza,
    ec.envasado_id,
    e.nombre envasado,
    ec.unidad_volumen_id,
    uv.nombre unidad_volumen,
    ec.volumen
from envasados_cervezas ec
    join cervezas c on ec.cerveza_id = c.id
    join cervecerias cr on c.cerveceria_id = cr.id
    join envasados e on ec.envasado_id = e.id
    join unidades_volumen uv on ec.unidad_volumen_id = uv.id;      

-- *****************************
-- Orden de cargue de datos
-- *****************************

/*
- rangos_abv
- rango_ibu
- estilos
- envasados
- unidades_volumen
- ubicaciones

- tipos_ingredientes
- ingredientes

- cervecerias
- cervezas

- envasados_cervezas
- ingredientes_cervezas
*/


-- Tablas temporales

-- ------------------------------
-- TMP_INGREDIENTES
-- ------------------------------
create table tmp_ingredientes (ingrediente varchar, tipo_ingrediente varchar);

insert into ingredientes (nombre, tipo_ingrediente_id)
select
    tmp.ingrediente,
    ti.id  tipo_ingrediente_id
from tmp_ingredientes tmp
    join tipos_ingredientes ti on tmp.tipo_ingrediente = ti.nombre;

-- ------------------------------
-- TMP_CERVECERIAS
-- ------------------------------
create table tmp_cervecerias
(nombre varchar, ubicacion varchar, sitio_web varchar, instagram varchar);

insert into cervecerias(nombre, ubicacion_id, sitio_web, instagram)
select tmp.nombre,
       u.id ubicacion_id,
       tmp.sitio_web,
       tmp.instagram
from tmp_cervecerias tmp
 join ubicaciones u on (u.municipio || ', ' ||u.departamento) = tmp.ubicacion;

-- ------------------------------
 -- TMP_CERVEZAS
-- ------------------------------
 create table tmp_cervezas
(nombre varchar, cerveceria varchar, estilo varchar, ibu float, abv float);

insert into cervezas (nombre, estilo_id, cerveceria_id, ibu, abv)
select
    tmp.nombre,
    e.id estilo_id,
    cr.id cerveceria_id,
    ibu,
    abv
from tmp_cervezas tmp
    join cervecerias cr on tmp.cerveceria = cr.nombre
    join estilos e on tmp.estilo = e.nombre;

-- ------------------------------
-- TMP_ENVASADOS_CERVEZAS
-- ------------------------------
create table tmp_envasados_cervezas
(cerveceria varchar, cerveza varchar, envasado varchar, unidad_volumen varchar, volumen float);

insert into envasados_cervezas (cerveza_id, envasado_id, unidad_volumen_id, volumen)
select
    c.id cerveza_id,
    e.id envasado_id,
    uv.id unidad_volumen_id,
    tmp.volumen
from tmp_envasados_cervezas tmp
 join cervecerias cr on cr.nombre = tmp.cerveceria
 join cervezas c on cr.id = c.cerveceria_id
        and c.nombre = tmp.cerveza
join envasados e on tmp.envasado = e.nombre
join unidades_volumen uv on tmp.unidad_volumen = uv.nombre;

-- ------------------------------
-- TMP_INGREDIENTES_CERVEZAS
-- ------------------------------
create table tmp_ingredientes_cervezas
(cerveceria varchar, cerveza varchar, tipo_ingrediente varchar, ingrediente varchar );

insert into ingredientes_cervezas (cerveza_id, ingrediente_id) 
select
    c.id cerveza_id,
    i.id ingrediente_id
from tmp_ingredientes_cervezas tmp
join cervecerias cr on tmp.cerveceria = cr.nombre
join cervezas c on cr.id = c.cerveceria_id
            and tmp.cerveza = c.nombre
join tipos_ingredientes ti on ti.nombre = tmp.tipo_ingrediente
join ingredientes i on ti.id = i.tipo_ingrediente_id
    and tmp.ingrediente = i.nombre;    


-- Procedimientos

-- -------------------------------------
-- Procedimiento: p_elimina_estilo
-- -------------------------------------

create or replace procedure core.p_elimina_estilo(p_id integer)
language plpgsql as
$$
    begin
        delete from estilos e where e.id = p_id;
    end;
$$;


-- privilegios

-- Sentencia para generar los privilegios base en las tablas del esquema core
select distinct
        table_schema,
        table_name,
        ('revoke select, insert, update, delete on ' || table_schema || '.' || table_name || ' from cervezas_usr;') sentencia_revoke,
        ('grant select, insert, update, delete on ' || table_schema || '.' || table_name || ' to cervezas_usr;') sentencia_grant
from information_schema.tables
where table_schema = 'core';


-- Para asignar

grant select, insert, update, delete on core.cervecerias to cervezas_usr;
grant select, insert, update, delete on core.cervezas to cervezas_usr;
grant select, insert, update, delete on core.estilos to cervezas_usr;
grant select, insert, update, delete on core.ingredientes to cervezas_usr;
grant select, insert, update, delete on core.rangos_abv to cervezas_usr;
grant select, insert, update, delete on core.rangos_ibu to cervezas_usr;
grant select, insert, update, delete on core.tipos_ingredientes to cervezas_usr;
grant select, insert, update, delete on core.ubicaciones to cervezas_usr;
grant select, insert, update, delete on core.ingredientes_cervezas to cervezas_usr;
grant select, insert, update, delete on core.envasados to cervezas_usr;
grant select, insert, update, delete on core.unidades_volumen to cervezas_usr;
grant select, insert, update, delete on core.envasados_cervezas to cervezas_usr;
grant select on core.v_info_cervezas to cervezas_usr;
grant select on core.v_info_ingredientes to cervezas_usr;
grant select on core.v_info_ingredientes_cervezas to cervezas_usr;
grant select on core.v_info_envasados_cervezas to cervezas_usr;

-- Para revocar

sentencia_revoke
revoke select, insert, update, delete on core.cervecerias from cervezas_usr;
revoke select, insert, update, delete on core.cervezas from cervezas_usr;
revoke select, insert, update, delete on core.estilos from cervezas_usr;
revoke select, insert, update, delete on core.ingredientes from cervezas_usr;
revoke select, insert, update, delete on core.rangos_abv from cervezas_usr;
revoke select, insert, update, delete on core.rangos_ibu from cervezas_usr;
revoke select, insert, update, delete on core.tipos_ingredientes from cervezas_usr;
revoke select, insert, update, delete on core.ubicaciones from cervezas_usr;
revoke select, insert, update, delete on core.ingredientes_cervezas from cervezas_usr;
revoke select, insert, update, delete on core.envasados from cervezas_usr;
revoke select, insert, update, delete on core.unidades_volumen from cervezas_usr;
revoke select, insert, update, delete on core.envasados_cervezas from cervezas_usr;
revoke select on core.v_info_cervezas from cervezas_usr;
revoke select on core.v_info_ingredientes from cervezas_usr;
revoke select on core.v_info_ingredientes_cervezas from cervezas_usr;
revoke select on core.v_info_envasados_cervezas from cervezas_usr;

-- Sentencia para identificar privilegios asignados:
select grantor, grantee, table_schema, table_name, privilege_type
from information_schema.table_privileges
where grantee = 'cervezas_usr'
and table_schema = 'core';





