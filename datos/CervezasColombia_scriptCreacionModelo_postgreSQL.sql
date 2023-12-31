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
  latitud               float default 0,
  longitud              float default 0,
  fecha_creacion        timestamp with time zone default current_timestamp,
  fecha_actualizacion   timestamp with time zone default current_timestamp,
  constraint ubicaciones_uk unique (municipio, departamento)    
);

comment on table core.ubicaciones is 'Ubicaciones de las Cervecerias';
comment on column core.ubicaciones.id is 'id de la ubicación';
comment on column core.ubicaciones.municipio is 'municipio de la ubicación';
comment on column core.ubicaciones.departamento is 'departamento de la ubicacion';
comment on column core.ubicaciones.latitud is 'Componente de latitud de la coordenada geográfica';
comment on column core.ubicaciones.longitud is 'Componente de longitud de la coordenada geográfica';
comment on column core.ubicaciones.fecha_creacion is 'fecha de creación del registro';
comment on column core.ubicaciones.fecha_actualizacion is 'fecha de actualización del registro';

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

-- ----------------------------
-- Vista: v_info_cervecerias
-- ----------------------------
create view v_info_cervecerias as
select
    cv.id cerveceria_id,
    cv.nombre cerveceria,
    cv.sitio_web,
    cv.instagram,
    cv.ubicacion_id,
    (u.municipio || ', ' || u.departamento) ubicacion
from cervecerias cv join ubicaciones u on cv.ubicacion_id = u.id;



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
-- Procedimientos asociados al estilo
-- -------------------------------------

-- Inserción
create or replace procedure core.p_inserta_estilo(
                    in p_nombre varchar)
    language plpgsql
as
$$
    begin
        insert into estilos (nombre)
        values (p_nombre);
    end;
$$;

-- Actualización
create or replace procedure core.p_actualiza_estilo(
                    in p_id integer,
                    in p_nombre varchar)
    language plpgsql
as
$$
    begin
        update estilos
        set
            nombre              = p_nombre,
            fecha_actualizacion = current_timestamp
        where id = p_id;
    end;
$$;

-- Eliminación
create or replace procedure core.p_elimina_estilo(in p_id integer)
language plpgsql as
$$
    begin
        delete from estilos e where e.id = p_id;
    end;
$$;

-- -------------------------------------------
-- Procedimientos asociados a la ubicacion
-- -------------------------------------------
-- Inserción
create or replace procedure core.p_inserta_ubicacion(
                    in p_municipio varchar,
                    in p_departamento varchar,
                    in p_latitud float,
                    in p_longitud float)
    language plpgsql
as
$$
    begin
        insert into ubicaciones (municipio,departamento, latitud, longitud)
        values (p_municipio, p_departamento, p_latitud, p_longitud);
    end;
$$;

-- Actualización
create or replace procedure core.p_actualiza_ubicacion(
                    in p_id integer,
                    in p_municipio varchar,
                    in p_departamento varchar,
                    in p_latitud float,
                    in p_longitud float)
    language plpgsql
as
$$
    begin
        update ubicaciones
        set
            municipio           = p_municipio,
            departamento        = p_departamento,
            latitud             = p_latitud,
            longitud            = p_longitud,
            fecha_actualizacion = current_timestamp
        where id = p_id;
    end;
$$;

-- Eliminación
create or replace procedure core.p_elimina_ubicacion(in p_id integer)
    language plpgsql
as
$$
    begin
        delete from ubicaciones u where u.id = p_id;
    end;
$$;

-- -----------------------------------------
-- Procedimientos asociados a la cerveza
-- -----------------------------------------

-- Inserción
create or replace procedure core.p_inserta_cerveza(
                    in p_nombre varchar,
                    in p_cervceria_id integer,
                    in p_estilo_id integer,
                    in p_ibu float,
                    in p_abv float)
    language plpgsql
as
$$
    declare
    l_cerveza_id            integer;
    l_envasado_id           integer;
    l_ingrediente_id        integer;
    l_unidad_volumen_id     integer;

    begin
        -- Insertamos la cerveza
        insert into cervezas(nombre, cerveceria_id, estilo_id, ibu, abv)
        values (p_nombre,p_cervceria_id,p_estilo_id,p_ibu,p_abv);

        -- Obtenemos el id creado para la cerveza
        select c.id into l_cerveza_id from cervezas c
        where c.nombre      = p_nombre
        and c.cerveceria_id = p_cervceria_id
        and c.estilo_id     = p_estilo_id;

        -- Obtener el Id del envasado Botella
        select id into l_envasado_id
        from envasados
        where nombre = 'Botella';

        -- Obtener el Id de la unidad de volumen Mililitros
        select id into l_unidad_volumen_id
        from unidades_volumen
        where nombre = 'Mililitros';

        -- Obtener el Id del ingrediente Agua de Manantial
        select ingrediente_id into l_ingrediente_id
        from v_info_ingredientes
        where tipo_ingrediente  = 'Agua'
        and ingrediente         = 'Agua de Manantial';

        -- Insertamos el envasado predeterminado:
        insert into envasados_cervezas (cerveza_id, envasado_id, unidad_volumen_id, volumen)
        values (l_cerveza_id,l_envasado_id,l_unidad_volumen_id,330);

        -- Insertamos el ingrediente predeterminado: 
        insert into ingredientes_cervezas(cerveza_id, ingrediente_id)
        values (l_cerveza_id,l_ingrediente_id);
    end;
$$;

-- Actualización
create or replace procedure core.p_actualiza_cerveza(
                    in p_id integer,
                    in p_nombre varchar,
                    in p_cervceria_id integer,
                    in p_estilo_id integer,
                    in p_ibu float,
                    in p_abv float)
    language plpgsql
as
$$
    begin
        update cervezas
        set
            nombre                  = p_nombre,
            cerveceria_id           = p_cervceria_id,
            estilo_id               = p_estilo_id,
            ibu                     = p_ibu,
            abv                     = p_abv,
            fecha_actualizacion     = current_timestamp
        where id = p_id;
    end;
$$;

-- Eliminación
create or replace procedure core.p_elimina_cerveza(in p_id integer)
    language plpgsql
as
$$
    begin
        -- Eliminamos los ingredientes asociados
        delete from ingredientes_cervezas ic where ic.cerveza_id = p_id;

        -- Eliminamos los envasados asociados
        delete from envasados_cervezas ec where ec.cerveza_id = p_id;

        -- Eliminamos finalmente la cerveza registrada
        delete from cervezas c where c.id = p_id;
    end;
$$;

-- -------------------------------------------
-- Procedimientos asociados a la cerveceria
-- -------------------------------------------

-- Inserción
create or replace procedure core.p_inserta_cerveceria(
                    in p_nombre varchar,
                    in p_ubicacion_id integer,
                    in p_sitio_web varchar,
                    in p_instagram varchar)
    language plpgsql
as
$$
    begin
        -- Insertamos la cerveceria
        insert into cervecerias (nombre,ubicacion_id,sitio_web,instagram)
        values (p_nombre,p_ubicacion_id,p_sitio_web,p_instagram);
    end;
$$;

-- Actualización
create or replace procedure core.p_actualiza_cerveceria(
                    in p_id integer,
                    in p_nombre varchar,
                    in p_ubicacion_id integer,
                    in p_sitio_web varchar,
                    in p_instagram varchar)
    language plpgsql
as
$$
    begin
        update cervecerias
        set
            nombre              = p_nombre,
            ubicacion_id        = p_ubicacion_id,
            sitio_web           = p_sitio_web,
            instagram           = p_instagram,
            fecha_actualizacion = current_timestamp
        where id = p_id;
    end;
$$;

-- Eliminación
create or replace procedure core.p_elimina_cerveceria(in p_id integer)
    language plpgsql
as
$$
    begin
        delete from cervecerias c where c.id = p_id;
    end;
$$;

-- -------------------------------------------
-- Procedimientos asociados a los envasados
-- -------------------------------------------

-- Inserción
create or replace procedure core.p_inserta_envasado(
                    in p_nombre varchar)
    language plpgsql
as
$$
    begin
        insert into envasados (nombre)
        values (p_nombre);
    end;
$$;

-- Actualización
create or replace procedure core.p_actualiza_envasado(
                    in p_id integer,
                    in p_nombre varchar)
    language plpgsql
as
$$
    begin
        update envasados
        set
            nombre              = p_nombre,
            fecha_actualizacion = current_timestamp
        where id = p_id;
    end;
$$;

-- Eliminación
create or replace procedure core.p_elimina_envasado(in p_id integer)
language plpgsql as
$$
    begin
        -- Borrar de la tabla envasados cerveza
        delete from envasados_cervezas where envasado_id = p_id;

        -- Borrar de la tabla envasados
        delete from envasados e where e.id = p_id;
    end;
$$;

-- --------------------------------------------------
-- Procedimientos asociados a los envasados_cerveza
-- --------------------------------------------------

-- Insercion
create or replace procedure core.p_inserta_envasado_cerveza(
                        in p_cerveza_id integer,
                        in p_envasado_id integer,
                        in p_unidad_volumen_id integer,
                        in p_volumen float)
    language plpgsql
as
$$
    begin
        insert into envasados_cervezas (cerveza_id, envasado_id, unidad_volumen_id, volumen)
        values (p_cerveza_id, p_envasado_id, p_unidad_volumen_id, p_volumen);
    end;
$$;

-- Eliminacion
create or replace procedure core.p_elimina_envasado_cerveza(
                        in p_cerveza_id integer,
                        in p_envasado_id integer,
                        in p_unidad_volumen_id integer,
                        in p_volumen float)
    language plpgsql
as
$$
    begin
        delete from envasados_cervezas
        where cerveza_id        = p_cerveza_id
        and envasado_id         = p_envasado_id
        and unidad_volumen_id   = p_unidad_volumen_id
        and volumen             = p_volumen;
    end;
$$;


-- -------------------------------------------
-- Procedimientos asociados a los ingredientes
-- -------------------------------------------

-- Inserción
create or replace procedure core.p_inserta_ingrediente(
                    in p_nombre varchar,
                    in p_tipo_ingrediente_id integer)
    language plpgsql
as
$$
    begin
        insert into ingredientes (nombre, tipo_ingrediente_id)
        values (p_nombre,p_tipo_ingrediente_id);
    end;
$$;

-- Actualización
create or replace procedure core.p_actualiza_ingrediente(
                    in p_id integer,
                    in p_nombre varchar,
                    in p_tipo_ingrediente_id integer)
    language plpgsql
as
$$
    begin
        update ingredientes
        set
            nombre              = p_nombre,
            tipo_ingrediente_id = p_tipo_ingrediente_id,
            fecha_actualizacion = current_timestamp
        where id = p_id;
    end;
$$;

-- Eliminación
create or replace procedure core.p_elimina_ingrediente(in p_id integer)
language plpgsql as
$$
    begin
        -- Borrar de la tabla ingredientes cerveza
        delete from ingredientes_cervezas where ingrediente_id = p_id;

        -- Borrar de la tabla envasados
        delete from ingredientes i where i.id = p_id;
    end;
$$;

-- ------------------------------------------------------
-- Procedimientos asociados a los ingredientes_cervezas
-- ------------------------------------------------------

-- Insercion
create or replace procedure core.p_inserta_ingrediente_cerveza(
                        in p_cerveza_id integer,
                        in p_ingrediente_id integer)
    language plpgsql
as
$$
    begin
        insert into ingredientes_cervezas (cerveza_id, ingrediente_id)
        values (p_cerveza_id, p_ingrediente_id);
    end;
$$;


-- Eliminacion
create or replace procedure core.p_elimina_ingrediente_cerveza(
                        in p_cerveza_id integer,
                        in p_ingrediente_id integer)
    language plpgsql
as
$$
    begin
        delete from ingredientes_cervezas
        where ingrediente_id    = p_ingrediente_id
        and cerveza_id          = p_cerveza_id;
    end;
$$;

-- ------------- 
-- privilegios
-- -------------

-- Creacion del usuario de conexión a la aplicación

-- crear el usuario con el que se ejecutarán acciones CRUD
create user cervezas_usr with encrypted password 'otraClav3';

-- asignación de privilegios para el usuario
grant connect on database cervezas_db to cervezas_usr;
grant usage on schema core to cervezas_usr;

-- Solo le damos privilegios de lectura en las tablas y ejecución en los procedimientos
grant select, insert, update, delete on all tables in schema core to cervezas_usr;
grant execute on all routines in schema core to cervezas_usr;

alter user cervezas_usr set search_path to core; 

-- Sentencia para identificar privilegios asignados en tablas:
select grantor, grantee, table_schema, table_name, privilege_type
from information_schema.table_privileges
where grantee = 'cervezas_usr'
and table_schema = 'core';

-- Sentencia para identificar privilegios asignados en rutinas:
select grantor, grantee, routine_schema, routine_name, privilege_type
from information_schema.routine_privileges
where grantee = 'cervezas_usr'
and routine_schema = 'core';

-- Para revocar privilegios innecesarios sobre las vistas informativas
revoke insert, update, delete on v_info_envasados_cervezas from cervezas_usr;
revoke insert, update, delete on v_info_ingredientes_cervezas from cervezas_usr;
revoke insert, update, delete on v_info_ingredientes from cervezas_usr;
revoke insert, update, delete on v_info_cervezas from cervezas_usr;